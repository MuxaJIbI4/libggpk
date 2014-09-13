using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using LibDat.Data;
using LibDat.Types;

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
        private static Dictionary<string, DataType> _types;

        private static readonly Dictionary<Type, Func<BinaryReader, object>> ReadFuncs =
            new Dictionary<Type, Func<BinaryReader, object>>
        {
            {typeof (bool), s => s.ReadBoolean()},
            {typeof (byte), s => s.ReadByte()},
            {typeof (short), s => s.ReadInt16()},
            {typeof (int), s => s.ReadInt32()},
            {typeof (uint), s => s.ReadUInt32()},
            {typeof (long), s => s.ReadInt64()},
            {typeof (ulong), s => s.ReadUInt64()},
            {typeof (string), s => 
            {
                var sb = new StringBuilder();
                char ch;
                while ((ch = s.ReadChar()) != 0) { sb.Append(ch); }
                ch = s.ReadChar();
                if (ch != 0)    // string should with int(0)
                    throw new Exception("Not found int(0) value at the end of the string");
                return sb.ToString();
            }}
        };

        private static readonly Dictionary<Type, Action<BinaryWriter, object>> WriteFuncs =
            new Dictionary<Type, Action<BinaryWriter, object>>
        {
            {typeof (bool), (bw, o) => bw.Write((bool)o)},
            {typeof (byte), (bw, o) => bw.Write((byte)o) },
            {typeof (short), (bw, o) => bw.Write((short)o)},
            {typeof (int), (bw, o) => bw.Write((int)o)},
            {typeof (uint), (bw, o) => bw.Write((uint)o)},
            {typeof (long), (bw, o) => bw.Write((long)o)},
            {typeof (ulong), (bw, o) => bw.Write((ulong)o)},
            {typeof (string), (bw, o) =>
            {
                foreach (var ch in (string)o)
                {
                    bw.Write(ch);
                }
                bw.Write(0);
            }},
        };


        public static T Read<T>(this BinaryReader reader)
        {
            if (ReadFuncs.ContainsKey(typeof(T)))
                return (T)ReadFuncs[typeof(T)](reader);
            throw new NotImplementedException();
        }

        public static void Write<T>(this BinaryWriter reader, object obj)
        {
            if (WriteFuncs.ContainsKey(typeof(T)))
                WriteFuncs[typeof(T)](reader, (T)obj);
            throw new NotImplementedException();
        }

        static RecordFactory()
        {
            UpdateRecordsInfo();
        }

        public static void UpdateRecordsInfo()
        {
            _records = new Dictionary<string, RecordInfo>();
            _types = new Dictionary<string, DataType>();

            // load default value types
            LoadValueTypes();

            // load XML
            var doc = new XmlDocument();
            doc.Load("DatDefinitions.xml");

            // load records
            var nodes = doc.SelectNodes("//record");
            if (nodes == null)
                throw new Exception("Not found type record definitions");
            foreach (XmlNode node in nodes)
            {
                ProcessRecordDefinition(node);
            }
        }

        private static void ProcessRecordDefinition(XmlNode node)
        {
            var file = GetAttributeValue(node, "file");
            if (file == null)
                throw new Exception("Invalid XML: record has wrong attribute 'id' :" + node);
            var lengthString = GetAttributeValue(node, "length");
            if (lengthString == null)
                throw new Exception("Invalid XML: record has wrong attribute 'length' :" + node);
            var length = Convert.ToInt32(lengthString);
            if (length == 0)
            {
                _records.Add(file, new RecordInfo(file));
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
                if (!fieldName.Equals("field"))
                    throw new Exception("Invalid XML: <record> contain wrong elemnr: " + fieldName);

                var fieldId = GetAttributeValue(field, "id");
                if (fieldId == null)
                    throw new Exception("Invalid XML: field has wrong attribute 'id' :" + field);

                var fieldType = GetAttributeValue(field, "type");
                if (fieldType == null)
                    throw new Exception("Invalid XML: couldn't  find type for field :" + field);
                var dataType = ParseType(fieldType);

                var fieldDescription = GetAttributeValue(field, "description");

                var isUserString = GetAttributeValue(field, "isUser");
                var isUser = !String.IsNullOrEmpty(isUserString);

                fields.Add(new FieldInfo(dataType, index, fieldId, fieldDescription, isUser));
                index++;
                totalLength += dataType.Width;
            }

            // testing whether record's data is correct
            if (totalLength != length)
            {
                var error = "Total length of fields: " + totalLength + " not equal record length: " + length
                            + " for file: " + file;
                foreach (var field in fields)
                {
                    Console.WriteLine("{0} = {1}", field.FieldType.Name, field.FieldType.Width);
                }
                throw new Exception(error);
            }

            _records.Add(file, new RecordInfo(file, length, fields));
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
        /// parser field type and creates type hierarchy
        /// For example for like "ref|list|ref|string" it will create
        /// PointerDataType r1;
        /// r1.RefType = ListDataType l1
        /// l1.ListType= PointerDataType r2;
        /// r2.RefType = StringData
        /// </summary>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        private static DataType ParseType(string fieldType)
        {
            if (HasTypeInfo(fieldType))
                return GetTypeInfo(fieldType);

            DataType type;
            var match = Regex.Match(fieldType, @"(\w+\|)?(.+)");
            if (match.Success)
            {
                if (String.IsNullOrEmpty(match.Groups[1].Value)) // value type
                {
                    type = ParseValueType(match.Groups[2].Value);
                }
                else // pointer to other type
                {
                    var pointerString = match.Groups[1].Value;
                    var refTypeString = match.Groups[2].Value;

                    if (pointerString.Equals("ref|")) // pointer
                    {
                        var refType = ParseType(refTypeString);
                        type = new PointerDataType(fieldType, refType.PointerWidth, 4, refType);
                    }
                    else if (pointerString.Equals("list|")) // list of data
                    {
                        var listType = ParseType(refTypeString);
                        type = new ListDataType(fieldType, -1, 8, listType);
                    }
                    else
                    {
                        throw new Exception("Unknown complex type name:" + pointerString);
                    }
                }
            }
            else
            {
                throw new Exception(@"String is not a valid type definition: " + fieldType);
            }

            if (type != null)
                _types[fieldType] = type;
            return type;
        }

        private static DataType ParseValueType(string fieldType)
        {
            var match = Regex.Match(fieldType, @"^(\w+)$");
            if (match.Success)
            {
                return GetTypeInfo(match.Groups[0].Value);
            }
            throw new Exception(String.Format("Not a valid value type definition: \"{0}\"", fieldType));
        }

        private static void LoadValueTypes()
        {
            // value types
            _types.Add("bool", new DataType("bool", 1, 4));
            _types.Add("byte", new DataType("byte", 1, 4));
            _types.Add("short", new DataType("short", 2, 4));
            _types.Add("int", new DataType("int", 4, 4));
            _types.Add("uint", new DataType("uint", 4, 4));
            _types.Add("long", new DataType("long", 8, 4));
            _types.Add("ulong", new DataType("ulong", 8, 4));
            _types.Add("string", new DataType("string", -1, 4));
        }

        /// <summary>
        /// creates new instance of AbstratData derived class from <c>inStream</c>
        /// inStream position should be in the beginning of data of pointer to data
        /// </summary>
        /// <param name="type">type to read</param>
        /// <param name="inStream">strem to read from</param>
        /// <param name="isAtPointer">true is inStream positioned on pointer to <c>type</c> data </param>
        /// <returns></returns>
        public static AbstractData ReadType(DataType type, BinaryReader inStream, bool isAtPointer)
        {
//            Console.WriteLine("ReadType: "  + type.Name + " isAtPointer=" + isAtPointer);

            AbstractData data;
            var offset = GetOffset(inStream);

            // check if list type
            var listDataType = type as ListDataType;
            if (listDataType != null) // list type data
            {
//                Console.WriteLine("Found list data type " + listDataType.Name);
                if (!isAtPointer)
                    throw new Exception("List data should be referenced by pointer data");

                var count = inStream.ReadInt32();
                offset = inStream.ReadInt32();
                inStream.BaseStream.Seek(DatContainer.DataSectionOffset + offset, SeekOrigin.Begin);
                data = new ListData(listDataType, offset, count, inStream);
                DatContainer.DataEntries[offset] = data;
                return data;
            }

            // check if pointer type
            var pointerDataType = type as PointerDataType;
            if (pointerDataType != null) // pointer type data
            {
//                Console.WriteLine("Found pointer data type " + pointerDataType.Name);
                if (isAtPointer)
                {
                    offset = inStream.ReadInt32();
                    inStream.BaseStream.Seek(DatContainer.DataSectionOffset + offset, SeekOrigin.Begin);
                }
                data = new PointerData(pointerDataType, offset, inStream);
                return data;
            }

            // value type data

            if (isAtPointer)
            {
//                Console.WriteLine("Value is at pointer:");
//                PrintOffsets(inStream);
                offset = inStream.ReadInt32();
                inStream.BaseStream.Seek(DatContainer.DataSectionOffset + offset, SeekOrigin.Begin);
            }
            switch (type.Name)
            {
                case "bool":
                    data = new ValueData<bool>(type, offset, inStream);
                    break;
                case "byte":
                    data = new ValueData<byte>(type, offset, inStream);
                    break;
                case "short":
                    data = new ValueData<short>(type, offset, inStream);
                    break;
                case "int":
                    data = new Int32Data(type, offset, inStream);
                    break;
                case "uint":
                    data = new ValueData<uint>(type, offset, inStream);
                    break;
                case "long":
                    data = new Int64Data(type, offset, inStream);
                    break;
                case "ulong":
                    data = new ValueData<ulong>(type, offset, inStream);
                    break;
                case "string":
                    data = new StringData(type, offset, inStream);
                    DatContainer.DataEntries[offset] = data;
                    break;
                default:
                    throw new Exception("Unknown value type name: " + type.Name);

            }
            return data;
        }

        public static int GetOffset(BinaryReader reader)
        {
            return (int) reader.BaseStream.Position - DatContainer.DataSectionOffset;
        }
        public static void PrintOffsets(BinaryReader reader)
        {
            var offset1 = (int) reader.BaseStream.Position;
            var offset2 =  offset1 - DatContainer.DataSectionOffset;
            Console.WriteLine( offset1+ "\t" + offset2);
        }

        /// <summary>
        /// Returns true if info for type typeName is defined
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool HasTypeInfo(string type)
        {
            return _types.ContainsKey(type);
        }

        private static DataType GetTypeInfo(string type)
        {
            if (!HasTypeInfo(type))
                throw new Exception("Unknown data type: " + type);
            return _types[type];
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