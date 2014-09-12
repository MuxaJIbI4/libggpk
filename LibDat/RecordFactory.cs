using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using LibDat.Data;

namespace LibDat
{
    /// <summary>
    /// this class reads and stores next informations from XML file:
    /// - definitions of records formats for all .dat files
    /// - types for pointer type fields
    /// </summary>
    public static class RecordFactory
    {
        private static Dictionary<string, RecordInfo> _records;
        private static Dictionary<string, FieldTypeInfo> _types;

        static RecordFactory()
        {
            UpdateRecordsInfo();
        }

        public static void UpdateRecordsInfo()
        {
            _records = new Dictionary<string, RecordInfo>();
            _types = new Dictionary<string, FieldTypeInfo>();

            // load default value types
            LoadValueTypes();

            // load XML
            var doc = new XmlDocument();
            doc.Load("DatDefinitions.xml");

            // load types
            var nodes = doc.SelectNodes("//typeDef");
            if (nodes == null)
                throw new Exception("Not found type definitions");
            foreach (XmlNode node in nodes)
            {
                ProcessPointerTypeDefinition(node);
            }

            // load records
            nodes = doc.SelectNodes("//record");
            if (nodes == null)
                throw new Exception("Not found type record definitions");
            foreach (XmlNode node in nodes)
            {
                ProcessRecordDefinition(node);
            }
        }

        private static void LoadValueTypes()
        {
            _types.Add("bool", new FieldTypeInfo("bool", 1, br => (object)br.ReadBoolean(), (bw, o) => bw.Write((bool)o)));
            _types.Add("byte", new FieldTypeInfo("byte", 1, br => (object)br.ReadByte(), (bw, o) => bw.Write((byte)o)));
            _types.Add("short", new FieldTypeInfo("short", 2, br => (object)br.ReadInt16(), (bw, o) => bw.Write((short)o)));
            _types.Add("int", new FieldTypeInfo("int", 4,
                br =>
                {
                    var result = br.ReadInt32();
                    // Int32 -16843010 : FEFE FEFE (hex)
                    if (result == -16843010) result = -1;
                    return (object)result;
                },
                (bw, o) => bw.Write((int)o)));
            _types.Add("Int64", new FieldTypeInfo("Int64", 8,
                br =>
                {
                    var result = br.ReadInt64();
                    // Int64 -72340172838076674: FEFE FEFE FEFE FEFE (hex)
                    if (result == -72340172838076674) result = -1;
                    return (object)result;
                },
                (bw, o) => bw.Write((int)o)));
        }

        /// <summary>
        /// Saves pointer type field's definitions found in XML file
        /// <typeDef name="StringOrUInt32List" width="8" pointerType="int,(*UInt32[]|*String)" /> 
        /// </summary>
        /// <param name="node"></param>
        private static void ProcessPointerTypeDefinition(XmlNode node)
        {
            if (node == null)
                throw new ArgumentNullException();

            // get name
            var name = GetAttributeValue(node, "name");
            if (String.IsNullOrEmpty(name))
                throw new Exception("Invalid XML: Type definition has wrong 'name' attribute");
            if (_types.ContainsKey(name))
                throw new Exception("Invalid XML: Type already defined: " + name);

            // get width
            var widthString = GetAttributeValue(node, "width");
            if (String.IsNullOrEmpty(widthString))
                throw new Exception("Invalid XML: Type definition has wrong 'width' attribute");
            var width = Convert.ToInt32(widthString);
            if (!(width == 4 || width == 8 ))
                throw new Exception("Invalid XML: pointer type width should be 4 or 8: " + name);

            // get 
            var pointerType = GetAttributeValue(node, "pointerType");
            if (String.IsNullOrEmpty(pointerType))
                throw new Exception("Invalid XML: Type definition has wrong 'pointerType' attribute");

            // create readers and writers
            TypeReader reader = br =>
            {
                if (width == 4)
                {
                    return (object)br.ReadInt32();
                }
                return (object)br.ReadInt64();
            };
            TypeWriter writer = (bw, o) =>
            {
                if (width == 4)
                {
                    bw.Write((int)o);
                }
                else
                {
                    bw.Write((Int64)o);
                }
            };
            

            // delegates to read data referenced by pointer field value
            PointerReaderDelegate pointerReader;
            // pointers with 1-byte width
            switch (name)
            {
                case "String":
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        var offset = fieldData.Offset;
                        var data = new UnicodeString(offset, dataTableBegin, inStream);
                        dataEntries[offset] = data;
                        fieldData.Offset = fieldData.ValueOffset = offset;
                    };
                    break;
                case "StringPointer":
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        var offset = fieldData.Offset;
                        var pointerData = new PointerData(offset, dataTableBegin, inStream);
                        dataEntries[offset] = pointerData;

                        var newOffset = pointerData.PointerOffset;
                        fieldData.ValueOffset = newOffset;

                        pointerData.Data = new UnicodeString(newOffset, dataTableBegin, inStream);
                        dataEntries[newOffset] = pointerData.Data;
                    };
                    break;
                case "Int32": // Unknown 32bit data
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        // (AbstractData)Activator.CreateInstance(Type, new object[] {..params..});
                        var offset = (int)fieldData.Value;
                        fieldData.Offset = offset;
                        dataEntries[offset] = new UnknownData(offset, dataTableBegin, inStream);
                    };
                    break;
                case "Int32List":
                case "UInt32List":
                case "UInt64List":
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        var length = fieldData.Length;
                        var offset = fieldData.Offset;
                        IDataList data = null;
                        if (name.Equals("Int32List"))
                            data = new ListInt32(offset, dataTableBegin, length, inStream);
                        if (name.Equals("UInt32List"))
                            data = new ListUInt32(offset, dataTableBegin, length, inStream);
                        if (name.Equals("UInt64List"))
                            data = new ListUInt64(offset, dataTableBegin, length, inStream);
                        fieldData.Offset = offset;
                        dataEntries[offset] = (AbstractData)data;
                    };
                    break;
                case "StringOrInt32List":
                case "StringOrUInt32List":
                case "StringOrUInt64List":
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        var length = fieldData.Length;
                        var offset = fieldData.Offset;
                        if (length > 0)
                        {
                            IDataList data = null;
                            if (name.Equals("StringOrInt32List"))
                                data = new ListInt32(offset, dataTableBegin, length, inStream);
                            if (name.Equals("StringOrUInt32List"))
                                data = new ListUInt32(offset, dataTableBegin, length, inStream);
                            if (name.Equals("StringOrUInt64List"))
                                data = new ListUInt64(offset, dataTableBegin, length, inStream);
                            dataEntries[offset] = (AbstractData)data;
                        }
                        else
                        {
                            dataEntries[offset] = new UnicodeString(offset, dataTableBegin, inStream);
                        }
                    };
                    break;
                case "StringOrStringPointer2Byte":
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        var length = fieldData.Length;
                        var offset = fieldData.Offset;

                        if (length > 0) // == 1  => field is pointer to pointer to string
                        {
                            var pointerData = new PointerData(offset, dataTableBegin, inStream);
                            dataEntries[offset] = pointerData;

                            var newOffset = pointerData.PointerOffset;
                            fieldData.ValueOffset = newOffset;

                            pointerData.Data =
                                dataEntries[newOffset] = new UnicodeString(newOffset, dataTableBegin, inStream);
                        }
                        else
                        {
                            dataEntries[offset] = new UnicodeString(offset, dataTableBegin, inStream);
                        }
                    };
                    break;
                default:
                    throw new Exception("Unknown pointer type: " + name);
            }

            // save data
            var type = new FieldTypeInfo(
                name, 
                Convert.ToInt32(width), 
                reader, 
                writer, 
                true, 
                pointerType, 
                pointerReader);
            _types[name] = type;
        }

        private static void ProcessRecordDefinition(XmlNode node)
        {
            var id = GetAttributeValue(node, "id");
            if (id == null)
                throw new Exception("Invalid XML: record has wrong attribute 'id' :" + node);
            var lengthString = GetAttributeValue(node, "length");
            if (lengthString == null)
                throw new Exception("Invalid XML: record has wrong attribute 'length' :" + node);
            var length = Convert.ToInt32(lengthString);
            if (length == 0)
            {
                _records.Add(id, new RecordInfo(id));
                return;
            }

            // process fields of record 
            var fields = new List<FieldInfo>();
            var index = 0;
            var totalLength = 0;
            foreach (XmlNode field in node.ChildNodes)
            {
                if (field.NodeType == XmlNodeType.Comment)
                    continue;
                var fieldName = field.LocalName;

                var fieldId = GetAttributeValue(field, "id");
                if (fieldId == null)
                    throw new Exception("Invalid XML: field has wrong attribute 'id' :" + field);

                var fieldDescription = GetAttributeValue(field, "description");

                var fieldType = GetAttributeValue(field, "type");
                if (fieldType == null)
                    throw new Exception("Invalid XML: field has wrong attribute 'type' :" + field);

                var isUserString = GetAttributeValue(field, "isUser");
                var isUser = !String.IsNullOrEmpty(isUserString);


                if (!HasTypeInfo(fieldType))
                    throw new Exception("Unknown field type: " + fieldType);
                var isPointer = fieldName.Equals("pointer");
                var typeInfo = GetTypeInfo(fieldType);
                if (typeInfo.IsPointer && !isPointer)
                    throw new Exception("Found pointer type in value field: " + id + " " + fieldId);
                if (!typeInfo.IsPointer && isPointer)
                    throw new Exception("Found value type in pointer field: " + id + " " + fieldId);

                fields.Add(new FieldInfo(index, fieldId, fieldDescription, typeInfo, isUser));
                index++;
                totalLength += typeInfo.Width;
            }

            // testing whether record's data is correct
            if (totalLength != length)
            {
                var error = "Total length of fields: " + totalLength + " not equal record length: " + length
                            + " for file: " + id;
                Console.WriteLine(error);
                throw new Exception(error);
            }

            _records.Add(id, new RecordInfo(id, length, fields));
        }

        // returns true if record's info for file fileName is defined
        public static bool HasRecordInfo(string fileName)
        {
            if (fileName.EndsWith(".dat")) // is it necessary ??
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);
            }
            return _records.ContainsKey(fileName);
        }

        public static RecordInfo GetRecordInfo(string datName)
        {
            if (!_records.ContainsKey(datName))
            {
                throw new Exception("Not defined parser for filename: " + datName);
            }
            return _records[datName];
        }

        /// <summary>
        /// Returns true if info for type typeName is defined
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool HasTypeInfo(string typeName)
        {
            return _types.ContainsKey(typeName);
        }

        public static FieldTypeInfo GetTypeInfo(string typeName)
        {
            return _types[typeName];
        }


        private static string GetAttributeValue(XmlNode node, string attributeName)
        {
            if (node == null)
                throw new ArgumentNullException();
            var attributes = node.Attributes;
            if (attributes == null)
                return null;

            var attribute = (XmlAttribute)(attributes.GetNamedItem(attributeName));
            return attribute == null ? null : attribute.Value;
        }
    }
}