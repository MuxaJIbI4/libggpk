using System.Collections.Generic;
using System.IO;

namespace LibDat.Types
{
    /// <summary>
    /// Type that represents "list" data: sequence of one ore more data of the same BaseDataType derived type
    /// </summary>
    public class ListDataType64 : BaseDataType
    {
        /// <summary>
        /// type of data in the list
        /// </summary>
        public BaseDataType ListType { get; private set; }

        public ListDataType64(string name, int width, int pointerWidth, BaseDataType listType) 
            : base(name, width, pointerWidth)
        {
            ListType = listType;
        }

        public override Dictionary<string, object> ReadPointer(BinaryReader reader)
        {
            var dict = new Dictionary<string, object>();
            var count = (int)reader.ReadInt64();
            var offset = (int)reader.ReadInt64();
            dict["count"] = count;
            dict["offset"] = offset;
            return dict;
        }
    }
}