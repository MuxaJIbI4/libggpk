using System.Collections.Generic;
using System.IO;

namespace LibDat.Types
{
    /// <summary>
    /// represents type of any data found in .dat file
    /// </summary>
    public class BaseDataType
    {
        /// <summary>
        /// Type name for identification
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Size of type data in bytes. Equals -1 if data has variable length (string, list of data)
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Size in bytes of pointer to data of this type. Can be 4 or 8
        /// </summary>
        public int PointerWidth { get; private set; }

        public BaseDataType(string name, int width, int pointerWidth)
        {
            Name = name;
            Width = width;
            PointerWidth = pointerWidth;
        }

        /// <summary>
        /// reads offset parameters to data of this type
        /// this method is called from constructor of PointerData instance
        /// </summary>
        /// <param name="reader">stream to read from</param>
        /// <returns>List of parameters required to read data of this type</returns>
        public virtual Dictionary<string, object> ReadPointer(BinaryReader reader)
        {
            var dict = new Dictionary<string, object>();
            var offset = reader.ReadInt32();
            dict["offset"] = offset;
            return dict;
        }
    }
}