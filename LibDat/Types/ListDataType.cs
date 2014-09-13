namespace LibDat.Types
{
    /// <summary>
    /// Represents a list of Int32 found in the resource section of a .dat file
    /// </summary>
    public class ListDataType : DataType
    {
        /// <summary>
        /// type of data contained in the list
        /// </summary>
        public DataType ListType { get; private set; }

        public ListDataType(string name, int width, int pointerWidth, DataType listType) 
            : base(name, width, pointerWidth)
        {
            ListType = listType;
        }
    }
}
