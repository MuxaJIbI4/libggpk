using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using LibDat.Data;

namespace LibDat
{
    // class read and saves descriptions of records for all .dat files
    public static class DatRecordInfoFactory
    {
        // TODO this property should be initialized on application start from external XML file
        private static Dictionary<string, DatRecordInfo> records;

        static DatRecordInfoFactory()
        {
            updateRecordsInfo();
            
        }

        public static void updateRecordsInfo () 
        {
            records = new Dictionary<string, DatRecordInfo>();

            // load XML
            XmlDocument doc = new XmlDocument();
            doc.Load("DatDefinitions.xml");

            XmlNodeList nodes = doc.SelectNodes("//definition");
            foreach (XmlNode node in nodes)
            {
                ProcessDatRecordDefinition(node);
            }
        }

        static private void ProcessDatRecordDefinition(XmlNode doc) 
        {
            XmlAttribute attr_id = (XmlAttribute)(doc.Attributes.GetNamedItem("id"));
            if (attr_id == null)
                throw new Exception("Attribute 'id' is missing in tag: " + doc.Name);
            string id = attr_id.Value;
            XmlAttribute attr_length = (XmlAttribute)(doc.Attributes.GetNamedItem("record_length"));
            int length = (attr_length == null ? 0 : Convert.ToInt32( attr_length.Value ) );
            
            // process record fields
            XmlNodeList nodes = doc.SelectNodes("field");
            var fields = new List<DatRecordFieldInfo>();
            int index = 0;
            foreach (XmlNode node in nodes) {
                string desc = node.SelectSingleNode("description").InnerText;
                string typeString = node.SelectSingleNode("type").InnerText;
                FieldTypes fieldType;
                switch (typeString)
                {
                    case "bool":    fieldType = FieldTypes._01bit; break;
                    case "byte":    fieldType = FieldTypes._08bit; break;
                    case "short":   fieldType = FieldTypes._16bit; break;
                    case "int":     fieldType = FieldTypes._32bit; break;
                    case "Int64":   fieldType = FieldTypes._64bit; break;
                    default:
                        throw new Exception("Unknown field type: " + typeString);
                }
                string pointer = node.SelectSingleNode("pointer").InnerText;
                
                if (!String.IsNullOrEmpty(pointer))
                {
                    PointerTypes pointerTypeString;
                    Type pointerType;
                    switch (pointer)
                    {
                        case "StringIndex": 
                            pointerTypeString = PointerTypes.StringIndex;
                            pointerType = typeof(UnicodeString);
                            break;
                        case "IndirectStringIndex": 
                            pointerTypeString = PointerTypes.IndirectStringIndex;
                            pointerType = typeof(UnicodeString); // TODO: this isn't right
                            break;
                        case "UserStringIndex": 
                            pointerTypeString = PointerTypes.UserStringIndex;
                            pointerType = typeof(UnicodeString);
                            break;
                        case "UInt64Index": 
                            pointerTypeString = PointerTypes.UInt64Index;
                            pointerType = typeof(UInt64List);
                            break;
                        case "UInt32Index": 
                            pointerTypeString = PointerTypes.UInt32Index;
                            pointerType = typeof(UInt32List);
                            break;
                        case "Int32Index": 
                            pointerTypeString = PointerTypes.Int32Index;
                            pointerType = typeof(Int32List);
                            break;
                        case "DataIndex": 
                            pointerTypeString = PointerTypes.DataIndex;
                            pointerType = typeof(UnknownData);
                            break;
                        default:
                            throw new Exception("Unknown pointer type: " + pointer);
                    }
                    fields.Add(new DatRecordFieldInfo(index, desc, fieldType, pointerTypeString, pointerType));
                }
                else
                {
                    fields.Add(new DatRecordFieldInfo(index, desc, fieldType));
                }
                
            }

            // TODO Test summary length ?
            int total_length = 0;
            foreach (var field in fields) 
            {
                switch(field.FieldType) 
                {
                    case FieldTypes._01bit:
                    case FieldTypes._08bit: total_length += 1; break;
                    case FieldTypes._16bit: total_length += 2; break;
                    case FieldTypes._32bit: total_length += 4; break;
                    case FieldTypes._64bit: total_length += 8; break;
                    default:
                        throw new Exception();
                }
            }
            if (total_length != length)
            {
                string error = "Total length of fields: " + total_length + " not equal record length: " + length 
                    + " for file: " + id;
                Console.WriteLine(error);
                throw new Exception(error);
            }

            records.Add(id, new DatRecordInfo(length, fields));
        }

        // returns true if record's info for file fileName is defined
        public static bool HasRecordInfo(string fileName)
        {
            if (fileName.EndsWith(".dat")) // is it necessary ??
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);
            }
            return records.ContainsKey(fileName);
        }

        public static DatRecordInfo GetRecordInfo(string DatName)
        {
            if (!records.ContainsKey(DatName))
            {
                throw new Exception("Not defined parser for filename: " + DatName);
            }
            return records[DatName];
        }
    }
}
