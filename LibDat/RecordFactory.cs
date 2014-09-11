using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using LibDat.Data;

namespace LibDat
{
    // class read and saves descriptions of records for all .dat files
    public static class RecordFactory
    {
        // TODO this property should be initialized on application start from external XML file
        private static Dictionary<string, RecordInfo> _records;
        private static Dictionary<string, FieldTypeInfo> _types;

        static RecordFactory()
        {
            LoadValueTypes();

            UpdateRecordsInfo();
        }

        public static void UpdateRecordsInfo()
        {
            _records = new Dictionary<string, RecordInfo>();
            _types = new Dictionary<string, FieldTypeInfo>();

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
            _types.Add("bool", new FieldTypeInfo("bool", 1, br => br.ReadBoolean(), (bw, o) => bw.Write((bool) o)));
            _types.Add("byte", new FieldTypeInfo("byte", 1, br => br.ReadByte(), (bw, o) => bw.Write((byte) o)));
            _types.Add("short", new FieldTypeInfo("short", 2, br => br.ReadInt16(), (bw, o) => bw.Write((short) o)));
            _types.Add("int", new FieldTypeInfo("int", 4,
                br =>
                {
                    var result = br.ReadInt32();
                    // Int32 -16843010 : FEFE FEFE (hex)
                    if (result == -16843010) result = -1;
                    return result;
                },
                (bw, o) => bw.Write((int) o)));
            _types.Add("Int64", new FieldTypeInfo("Int64", 8,
                br =>
                {
                    var result = br.ReadInt64();
                    // Int64 -72340172838076674: FEFE FEFE FEFE FEFE (hex)
                    if (result == -72340172838076674) result = -1;
                    return result;
                },
                (bw, o) => bw.Write((int) o)));
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
            // get width
            var widthString = GetAttributeValue(node, "width");
            if (String.IsNullOrEmpty(widthString))
                throw new Exception("Invalid XML: Type definition has wrong 'width' attribute");
            var width = Convert.ToInt32(widthString);

            // get 
            var pointerType = GetAttributeValue(node, "pointerType");
            if (String.IsNullOrEmpty(pointerType))
                throw new Exception("Invalid XML: Type definition has wrong 'pointerType' attribute");

            // create readers and writers
            TypeReader reader = br => width == 4 ? br.ReadInt32() : br.ReadInt64();
            TypeWriter writer = (bw, o) =>
            {
                if (width == 4)
                {
                    bw.Write((int) o);
                }
                else
                {
                    bw.Write((Int64) o);
                }
            };
            // TODO need to apply one or more AbstractData classes to pointerType
            PointerReaderDelegate pointerReader = null;

            // pointers with 1-byte width
            switch (name)
            {
                case "String":
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        var offset = (int) fieldData.Value;
                        var data = new UnicodeString(offset, dataTableBegin, inStream);
                        fieldData.Data = dataEntries[offset] = data;
                    };
                    break;
                case "StringPointer":
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        var offset = (int) fieldData.Value;
                        var pointerData = new PointerData(offset, dataTableBegin, inStream);
                        fieldData.Data = dataEntries[offset] = pointerData;

                        var newOffset = pointerData.PointerOffset;
                        pointerData.Data =
                            dataEntries[newOffset] = new UnicodeString(newOffset, dataTableBegin, inStream);
                    };
                    break;
                case "Int32": // Unknown 32bit data
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        var offset = (int) fieldData.Value;
                        // (AbstractData)Activator.CreateInstance(Type, new object[] { ... parameters ... });
                        fieldData.Data = dataEntries[offset] = new UnknownData(offset, dataTableBegin, inStream);
                    };
                    break;
                case "Int32List":
                case "UInt32List":
                case "UInt64List":
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        int length;
                        int offset;
                        GetListLengthAndOffset(fieldData, out length, out offset);
                        IDataList data = null;
                        if (name.Equals("Int32List"))
                            data = new Int32List(offset, dataTableBegin, length, inStream);
                        if (name.Equals("UInt32List"))
                            data = new UInt32List(offset, dataTableBegin, length, inStream);
                        if (name.Equals("UInt64List"))
                            data = new UInt64List(offset, dataTableBegin, length, inStream);
                        fieldData.Data = dataEntries[offset] = (AbstractData) data;
                    };
                    break;
                case "StringOrInt32List":
                case "StringOrUInt32List":
                case "StringOrUInt64List":
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        int length;
                        int offset;
                        GetListLengthAndOffset(fieldData, out length, out offset);
                        if (length > 0)
                        {
                            IDataList data = null;
                            if (name.Equals("StringOrInt32List"))
                                data = new Int32List(offset, dataTableBegin, length, inStream);
                            if (name.Equals("StringOrUInt32List"))
                                data = new UInt32List(offset, dataTableBegin, length, inStream);
                            if (name.Equals("StringOrUInt64List"))
                                data = new UInt64List(offset, dataTableBegin, length, inStream);
                            fieldData.Data = dataEntries[offset] = (AbstractData)data;
                        }
                        else
                        {
                            fieldData.Data = dataEntries[offset] = new UnicodeString(offset, dataTableBegin, inStream);
                        }
                    };
                    break;
                case "StringOrStringPointer2Byte":
                    pointerReader = (inStream, fieldData, dataTableBegin, dataEntries) =>
                    {
                        int length;
                        int offset;
                        GetListLengthAndOffset(fieldData, out length, out offset);
                        if (length > 0) // == 1 field is pointer to pointer to string
                        {
                            var pointerData = new PointerData(offset, dataTableBegin, inStream);
                            fieldData.Data = dataEntries[offset] = pointerData;

                            var newOffset = pointerData.PointerOffset;
                            pointerData.Data = 
                                dataEntries[newOffset] = new UnicodeString(newOffset, dataTableBegin, inStream);
                        }
                        else
                        {
                            fieldData.Data = dataEntries[offset] = new UnicodeString(offset, dataTableBegin, inStream);
                        }
                    };
                    break;
                default:
                    throw new Exception("Unknown pointer type: " + name);
            }

            // save data
            var type = new FieldTypeInfo(name, Convert.ToInt32(width), reader, writer, true, pointerType, pointerReader);
            _types[name] = type;
        }

        private static void GetListLengthAndOffset(FieldData fieldData, out int length, out int offset)
        {
            var value = (Int64) fieldData.Value;
            var bytes = BitConverter.GetBytes(value);
            length = BitConverter.ToInt32(bytes, 0);
            offset = BitConverter.ToInt32(bytes, 4);
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
                var fieldName = field.LocalName;

                var fieldId = GetAttributeValue(field, "id");
                if (fieldId == null)
                    throw new Exception("Invalid XML: field has wrong attribute 'id' :" + field);

                var fieldDescription = GetAttributeValue(field, "description");
                if (fieldDescription == null)
                    throw new Exception("Invalid XML: field has wrong attribute 'description' :" + field);

                var fieldType = GetAttributeValue(field, "type");
                if (fieldType == null)
                    throw new Exception("Invalid XML: field has wrong attribute 'type' :" + field);

                if (!HasTypeInfo(fieldType))
                    throw new Exception("Unknown field type: " + fieldType);
                var isPointer = fieldName.Equals("pointer");
                var typeInfo = GetTypeInfo(fieldType);
                if (typeInfo.IsPointer && !isPointer)
                    throw new Exception("Found pointer type in value field: " + field);
                if (!typeInfo.IsPointer && isPointer)
                    throw new Exception("Found value type in pointer field: " + field);

                fields.Add(new FieldInfo(index, fieldId, fieldDescription, typeInfo));
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

            var attribute = (XmlAttribute) (attributes.GetNamedItem(attributeName));
            return attribute == null ? null : attribute.Value;
        }
    }
}