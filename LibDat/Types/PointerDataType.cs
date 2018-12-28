namespace LibDat.Types
{
    /// <summary>
    /// Represents "pointer": data that references another data
    /// </summary>
    public class PointerDataType : BaseDataType
    {
        /// <summary>
        /// type of date referenced data of this type
        /// </summary>
        public BaseDataType RefType { get; private set; }

        public PointerDataType(string name, int width, int pointerWidth,  BaseDataType refType) 
            : base(name, width, pointerWidth)
        {
            RefType = refType;
        }
    }
}