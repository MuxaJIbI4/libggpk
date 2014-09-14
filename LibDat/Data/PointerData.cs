using System.IO;
using LibDat.Types;

namespace LibDat.Data
{
    /// <summary>
    /// Value f
    /// </summary>
    public class PointerData : AbstractData
    {
        /// <summary>
        /// referenced data
        /// </summary>
        public AbstractData RefData { get; set; }

        /// <summary>
        /// referenced data's type
        /// </summary>
        public BaseDataType RefType { get; private set; }

        public PointerData(PointerDataType dataType, int offset, BinaryReader inStream) : base(dataType, offset)
        {
//            Console.WriteLine(String.Format("PointerData Constructor: type={0} offset={1} position={2}",
//                dataType.Name, offset, RecordFactory.GetOffset(inStream)));
            RefType = dataType.RefType;
            Length = RefType.PointerWidth;
            RefData = TypeFactory.ReadType(RefType, inStream, true);
        }

        public override int Save(BinaryWriter outStream)
        {
            // TODO write Data instead of offset to Data
            // TODO: look for changed offset of data section entry which initially was at PointerOffset
            
            var newOffset = (int)outStream.BaseStream.Position;
            outStream.Write(RefData.Offset);
            var listData = RefData as ListData;
            if (listData != null)
                outStream.Write(listData.Count);
            
            return newOffset;
        }

        public override string GetValueString()
        {
            return RefData == null ? "[Error: Pointed Data Not Initialized]" : RefData.GetValueString();
        }
    }
}
