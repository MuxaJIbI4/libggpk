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
    }
}