using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibDat.Types;

namespace LibDat.Data
{
    public class ListData : AbstractData
    {
        /// <summary>
        /// Number of elements in the list
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// list of objects
        /// </summary>
        public List<AbstractData> List { get; private set; }

        public DataType ListType { get; private set; }

        public ListData(ListDataType type, int offset, int count, BinaryReader inStream) : base(type, offset)
        {
//            Console.WriteLine(String.Format("ListData Constructor: type={0} offset={1} count={3} position={2}",
//                type.Name, offset, RecordFactory.GetOffset(inStream), count));

            ListType = type.ListType;
            Count = count;
            List = new List<AbstractData>(Count);
            Length = ListType.Width * Count;
            if (count == 0)
                return;

            var currentOffset = (int)inStream.BaseStream.Position;
            for (var i = 0; i < count; ++i)
            {
                // given fixed size of ListType
                inStream.BaseStream.Seek(currentOffset + i*ListType.Width, SeekOrigin.Begin);
                var data = RecordFactory.ReadType(ListType, inStream, false);
                List.Add(data);
            }
        }

        public override int Save(BinaryWriter outStream)
        {
            // TODO: recursive save 
            throw new NotImplementedException();
            
        }

        public override string GetValueString()
        {
            return String.Format("[{0}]", String.Join(", ", List.Select(s => s.GetValueString())));
        }
    }
}
