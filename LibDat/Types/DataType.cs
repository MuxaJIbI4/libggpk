namespace LibDat.Types
{
    public class DataType
    {
        public string Name { get; private set; }

        public int Width { get; private set; }

        public int PointerWidth { get; private set; }

        public DataType(string name, int width, int pointerWidth)
        {
            Name = name;
            Width = width;
            PointerWidth = pointerWidth;
        }
    }
}