using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
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
                string type = node.SelectSingleNode("type").InnerText;
                string pointerType = node.SelectSingleNode("pointer").InnerText;
                fields.Add(new DatRecordFieldInfo(index, desc, type, pointerType));
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
