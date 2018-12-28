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

        public BaseDataType ListType { get; private set; }

        public ListData(ListDataType type, BinaryReader reader, Dictionary<string, object> options)
            : base(type)
        {
            if (!options.ContainsKey("count") || !options.ContainsKey("offset"))
                throw new Exception("Wrong parameters for reading ListData");

            ListType = type.ListType;

            // moving to start of list
            Offset = (int)options["offset"];
            reader.BaseStream.Seek(DatContainer.DataSectionOffset + Offset, SeekOrigin.Begin);

            Count = (int)options["count"];
            List = new List<AbstractData>(Count);
            Length = ListType.Width * Count;
            if (Count == 0)
                return;

            // Count > 0
            var currentOffset = reader.GetDataSectionOffset();
            for (var i = 0; i < Count; ++i)
            {
                // given fixed size of ListType
                var listEntryOffset = currentOffset + i*ListType.Width; 
                var dict = new Dictionary<string, object>();
                dict["offset"] = listEntryOffset;
                var data = TypeFactory.CreateData(ListType, reader, dict);
                List.Add(data);
            }

            DatContainer.DataEntries[Offset] = this;
        }

        public override void WritePointer(BinaryWriter writer)
        {
            writer.Write(Count);
            writer.Write(Offset);
        }

        public override string GetValueString()
        {
            return String.Format("[{0}]", String.Join(", ", List.Select(s => s.GetValueString())));
        }
    }
}
