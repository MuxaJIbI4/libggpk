namespace LibDat.Types
{
    public class PointerDataType : DataType
    {
        /// <summary>
        /// type of date referenced by this pointer
        /// </summary>
        public DataType RefType { get; private set; }

        public PointerDataType(string name, int width, int pointerWidth,  DataType refType) 
            : base(name, width, pointerWidth)
        {
            RefType = refType;
        }
    }
}