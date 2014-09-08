using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace LibDat
{
    // class read and saves descriptions of records for all .dat files
    public static class DatRecordInfoFactory
    {
        // TODO this property should be initialized on application start from external XML file
        private static Dictionary<string, DatRecordInfo> parsers;

        static DatRecordInfoFactory()
        {
            parsers = new Dictionary<string, DatRecordInfo>();

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
            // TODO Test summary length ?
            // TODO Test for not null pointer type should be consistent (int or Int64)
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
                string pointerTypeString = node.SelectSingleNode("pointer").InnerText;
                if (!String.IsNullOrEmpty(pointerTypeString))
                {
                    PointerTypes pointerType;
                    switch (pointerTypeString)
                    {
                        case "StringIndex": pointerType = PointerTypes.StringIndex; break;
                        case "IndirectStringIndex": pointerType = PointerTypes.IndirectStringIndex; break;
                        case "UserStringIndex": pointerType = PointerTypes.UserStringIndex; break;
                        case "UInt64Index": pointerType = PointerTypes.UInt64Index; break;
                        case "UInt32Index": pointerType = PointerTypes.UInt32Index; break;
                        case "Int32Index": pointerType = PointerTypes.Int32Index; break;
                        case "DataIndex": pointerType = PointerTypes.DataIndex; break;
                        default:
                            throw new Exception("Unknown pointer type: " + pointerTypeString);
                    }
                    fields.Add(new DatRecordFieldInfo(index, desc, fieldType, pointerType));
                }
                else
                {
                    fields.Add(new DatRecordFieldInfo(index, desc, fieldType));
                }
                
            }

            parsers.Add(id, new DatRecordInfo(length, fields));
        }

        // returns true if record's info for file fileName is defined
        public static bool HasParser(string fileName)
        {
            if (fileName.EndsWith(".dat")) // is it necessary ??
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);
            }
            return parsers.ContainsKey(fileName);
        }

        public static DatRecordInfo GetRecordInfo(string DatName)
        {
            if (!parsers.ContainsKey(DatName))
            {
                throw new Exception("Not defined parser for filename: " + DatName);
            }
            return parsers[DatName];
        }
    }
}
