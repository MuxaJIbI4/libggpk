using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LibDat
{
	public static class RecordFactory
	{

        // TODO this property should be initialized on application start from external XML file
        private static Dictionary<String, Object> parsers;

        public static bool HasParser(string fileName)
        {
            if (fileName.EndsWith(".dat") ) // is it necessary ??
            {
                fileName = Path.GetFileNameWithoutExtension(fileName);
            }

            return parsers.ContainsKey(fileName);
        }

		public static Record Create(string fileName, BinaryReader inStream)
		{
            // 1. check for existence of XML description of parser for file 'fileName.dat'
            // throw new Exception("Missing dat parser for type " + fileName);

            // 2. Read and save 1 record from inStream

            // TODO This is stub, delete later
            Type t = Type.GetType(string.Format("LibDat.Files.{0}, LibDat", fileName));
            return (Record)Activator.CreateInstance(t, new object[] { inStream });
		}
	}
}
