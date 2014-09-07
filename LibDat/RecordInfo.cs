using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;

namespace LibDat
{
    // contains info about single record in *.dat file
    public class RecordInfo
    {
        /// <summary>
        /// Represents the number of bytes this record will read or write to the DAT file
        /// </summary>
        /// <returns>Number of bytes this record will take in its native format</returns>
        public int Length { get; private set; }

        // record fields
        private List<RecordFieldInfo> fields;

        public ReadOnlyCollection<RecordFieldInfo> Fields
        {
            get { return fields.AsReadOnly(); }
            set {  }
        }

        public RecordInfo(XmlDocument doc)
        {

            Length = Convert.ToInt32( ((XmlAttribute)(doc.Attributes.GetNamedItem("length"))).Value );
            XmlNodeList nodes = doc.SelectNodes("field");

            fields = new List<RecordFieldInfo>();
            int index = 0;
            foreach (XmlNode node in nodes) {
                string desc = node.SelectSingleNode("description").Value; // or die
                string type = node.SelectSingleNode("type").Value;
                string pointerType = node.SelectSingleNode("pointer").Value;
                fields.Add(new RecordFieldInfo(index, desc, type, pointerType));
            }

        }
    }
}
