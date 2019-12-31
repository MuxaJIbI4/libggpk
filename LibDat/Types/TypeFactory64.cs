using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using LibDat.Data;

namespace LibDat.Types
{
    /// <summary>
    /// helper class for:
    /// 1) parsing and storing types from XML
    /// 2) reading data of speific type
    /// 3) contain extension methods on BinaryWriter and BinaryReader to facilitate reading of data of C# value types
    /// </summary>
    public static class TypeFactory64
    {

        #region Extension methods on BinaryWriter and BinaryReader

        private static readonly Dictionary<Type, Func<BinaryReader, object>> ReadFuncs64 =
            new Dictionary<Type, Func<BinaryReader, object>>
        {
             {typeof (bool), s => s.ReadBoolean()},
             {typeof (byte), s => s.ReadByte()},
             {typeof (short), s => s.ReadInt16()},
             {typeof (int), s => s.ReadInt32()},
             {typeof (uint), s => s.ReadUInt32()},
             {typeof (float), s => s.ReadSingle()},
             {typeof (long), s =>
                {
                    var a = s.ReadInt64();
                    s.BaseStream.Seek(8, SeekOrigin.Current);
                    return a;
                }
             },
             {typeof (ulong), s =>
                {
                    var a = s.ReadUInt64();
                    s.BaseStream.Seek(8, SeekOrigin.Current);
                    return a;
                }
             },
             {typeof (string), s =>
                {
                     var sb = new StringBuilder();
                     char ch;
                     while ((ch = s.ReadChar()) != 0) { sb.Append(ch); }
                     ch = s.ReadChar();
                     if (ch != 0)    // string should end with int(0)
                         throw new Exception("Not found int(0) value at the end of the string");
                     return sb.ToString();
                 }
             }
        };

        private static readonly Dictionary<Type, Action<BinaryWriter, object>> WriteFuncs64 =
            new Dictionary<Type, Action<BinaryWriter, object>>
        {
            {typeof (bool), (bw, o) => bw.Write((bool)o)},
            {typeof (byte), (bw, o) => bw.Write((byte)o)},
            {typeof (short), (bw, o) => bw.Write((short)o)},
            {typeof (int), (bw, o) => bw.Write((int)o)},
            {typeof (uint), (bw, o) => bw.Write((uint)o)},
            {typeof (float), (bw, o) => bw.Write((float)o)},
            {typeof (long), (bw, o) =>
                {
                    bw.Write((long)o);
                    bw.BaseStream.Seek(8, SeekOrigin.Current);
                }
            },
            {typeof (ulong), (bw, o) =>
                {
                    bw.Write((ulong)o);
                    bw.BaseStream.Seek(8, SeekOrigin.Current);
                }
            },
            {typeof (string), (bw, o) =>
                {
                    foreach (var ch in (string)o)
                    {
                        bw.Write(ch);
                    }
                    bw.Write(0);
                }
            },
        };

        public static T Read64<T>(this BinaryReader reader)
        {
            if (ReadFuncs64.ContainsKey(typeof(T)))
                return (T)ReadFuncs64[typeof(T)](reader);
            throw new NotImplementedException();
        }

        public static void Write64<T>(this BinaryWriter writer, object obj)
        {
            if (WriteFuncs64.ContainsKey(typeof(T)))
                WriteFuncs64[typeof(T)](writer, (T)obj);
            throw new NotImplementedException();
        }

        #endregion

        private static Dictionary<string, BaseDataType> _types;

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
        public static BaseDataType ParseType(string fieldType)
        {
            if (HasTypeInfo(fieldType))
                return GetTypeInfo(fieldType);

            BaseDataType type;
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
                        type = new PointerDataType(fieldType, refType.PointerWidth, 8, refType);
                    }
                    else if (pointerString.Equals("list|")) // list of data
                    {
                        var listType = ParseType(refTypeString);
                        type = new ListDataType64(fieldType, -1, 16, listType);
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

        private static BaseDataType ParseValueType(string fieldType)
        {
            var match = Regex.Match(fieldType, @"^(\w+)$");
            if (match.Success)
            {
                return GetTypeInfo(match.Groups[0].Value);
            }
            throw new Exception(String.Format("Not a valid value type definition: \"{0}\"", fieldType));
        }

        public static void LoadValueTypes()
        {
            _types = new Dictionary<string, BaseDataType>
            {
                {"bool", new BaseDataType("bool", 1, 8)},
                {"byte", new BaseDataType("byte", 1, 8)},
                {"short", new BaseDataType("short", 2, 8)},
                {"int", new BaseDataType("int", 4, 8)},
                {"uint", new BaseDataType("uint", 4, 8)},
                {"float", new BaseDataType("float", 4, 8)},
                {"long", new BaseDataType("long", 16, 8)},
                {"ulong", new BaseDataType("ulong", 16, 8)},
                {"string", new BaseDataType("string", -1, 8)},
                {"ref|generic", new BaseDataType("ref|generic", 8, 8)}
            };
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

        private static BaseDataType GetTypeInfo(string type)
        {
            if (!HasTypeInfo(type))
                throw new Exception("Unknown data type: " + type);
            return _types[type];
        }

        /// <summary>
        /// creates new instance of AbstratData derived class from <c>inStream</c>
        /// inStream position should be in the beginning of data of pointer to data
        /// </summary>
        /// <param name="type">type to read</param>
        /// <param name="inStream">strem to read from</param>
        /// <param name="options">null or list of params required to read dat aof type <c>type</c></param>
        /// <returns></returns>
        public static AbstractData CreateData(BaseDataType type, BinaryReader inStream, Dictionary<string, object> options)
        {

            // check if list type
            var listDataType = type as ListDataType64;
            if (listDataType != null) // list type data
                return new ListData(listDataType, inStream, options);

            if (type.Name == "ref|generic")
            {
                return new Int64Data(type, inStream, options);
            }

            // check if pointer type
            var pointerDataType = type as PointerDataType;
            if (pointerDataType != null) // pointer type data
                return new PointerData(pointerDataType, inStream, options);

            // value type data
            AbstractData data;
            switch (type.Name)
            {
                case "bool":
                    data = new ValueData64<bool>(type, inStream, options);
                    break;
                case "byte":
                    data = new ValueData64<byte>(type, inStream, options);
                    break;
                case "short":
                    data = new ValueData64<short>(type, inStream, options);
                    break;
                case "int":
                    data = new Int32Data64(type, inStream, options);
                    break;
                case "uint":
                    data = new ValueData64<uint>(type, inStream, options);
                    break;
                case "float":
                    data = new ValueData64<float>(type, inStream, options);
                    break;
                case "long":
                    data = new Int64Data64(type, inStream, options);
                    break;
                case "ulong":
                    data = new ValueData64<ulong>(type, inStream, options);
                    break;
                case "string":
                    data = new StringData(type, inStream, options);
                    break;
                default:
                    throw new Exception("Unknown value type name: " + type.Name);
            }
            return data;
        }
    }
}