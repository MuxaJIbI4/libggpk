using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using LibDat.Types;

namespace LibDat
{
    /// <summary>
    /// helper class for:
    /// 1) parsing XML definition file
    /// 2) storaing definitions of records formats for all .dat files
    /// </summary>
    public static class RecordFactory
    {
        private static Dictionary<string, RecordInfo> _records;

        /// <summary>
        /// Read XML file on first call to ReadFactory
        /// </summary>
        static RecordFactory()
        {
            UpdateRecordsInfo();
        }

        /// <summary>
        /// Reads and parses XML definitions file
        /// <exception cref="Exception">throw exceptions if couldn't readand parse XML file</exception>
        /// </summary>
        public static void UpdateRecordsInfo()
        {
            _records = new Dictionary<string, RecordInfo>();

            // load default value types
            TypeFactory.LoadValueTypes();
            TypeFactory64.LoadValueTypes();

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
                _records.Add(file, new RecordInfo(file, 0, null, false));
                _records.Add(file + ".dat64", new RecordInfo(file+".dat64", 0, null, true));
                return;
            }

            // process fields of record 
            var fields = new List<FieldInfo>();
            var fields64 = new List<FieldInfo>();
            var index = 0;
            var totalLength = 0;
            var totalLength64 = 0;
            foreach (XmlNode field in node.ChildNodes)
            {
                if (field.NodeType == XmlNodeType.Comment)
                    continue;

                var fieldName = field.LocalName;
                if (!fieldName.Equals("field"))
                    throw new Exception("Invalid XML: <record> contain wrong elemnr: " + fieldName);

                var fieldId = GetAttributeValue(field, "id");
                if (fieldId == null)
                    throw new Exception("Invalid XML: field has wrong attribute 'id' :" + fieldName);

                var fieldType = GetAttributeValue(field, "type");
                if (fieldType == null)
                    throw new Exception("Invalid XML: couldn't  find type for field :" + fieldName);
                var dataType = TypeFactory.ParseType(fieldType);
                var dataType64 = TypeFactory64.ParseType(fieldType);

                var fieldDescription = GetAttributeValue(field, "description");

                var isUserString = GetAttributeValue(field, "isUser");
                var isUser = !String.IsNullOrEmpty(isUserString);

                fields.Add(new FieldInfo(dataType, index, totalLength, fieldId, fieldDescription, isUser));
                fields64.Add(new FieldInfo(dataType64, index, totalLength64, fieldId, fieldDescription, isUser));
                index++;
                totalLength += dataType.Width;
                totalLength64 += dataType64.Width;
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

            _records.Add(file, new RecordInfo(file, totalLength, fields, false));
            _records.Add(file+".dat64", new RecordInfo(file+".dat64", totalLength64, fields64, true));
        }

        // returns true if record's info for file fileName is defined
        public static bool HasRecordInfo(string fileName)
        {
            return _records.ContainsKey(Path.GetFileNameWithoutExtension(fileName)) || _records.ContainsKey(Path.GetFileName(fileName));
        }

        public static RecordInfo GetRecordInfo(string datName)
        {
            if (!_records.ContainsKey(datName))
            {
                throw new Exception("Not defined parser for filename: " + datName);
            }
            return _records[datName];
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