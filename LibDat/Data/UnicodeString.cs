using System.IO;
using System.Text;

namespace LibDat.Data
{
    /// <summary>
    /// Represents a unicode string found in the data section of a .dat file
    /// </summary>
    public sealed class UnicodeString : AbstractData
    {
        /// <summary>
        /// Offset in the dat file with respect to the beginning of the data section
        /// </summary>
        public new int Offset { get; private set; }

        /// <summary>
        /// Offset in the dat file with respect to the beginning of the data section 
        /// where entry is finished
        /// </summary>
        public int OffsetEnd { get; private set; }

        /// <summary>
        /// The string
        /// </summary>
        public string Data { get; private set; }
        /// <summary>
        /// The replacement string. If this is set then it will replace the original string when it's saved.
        /// </summary>
        public string NewData { get; set; }
        
//        /// <summary>
//        /// Determins if this UnicodeString is a translatable string (eg: not used as an id, path, etc)
//        /// </summary>
//        public bool IsUserString { get; private set; }
        
        /// <summary>
        /// Offset of the new string with respect to the beginning of the data section. This will be invalid until save is called.
        /// </summary>
        public int NewOffset;
        /// <summary>
        /// Offset of the data section in the .dat file (Starts with 0xbbbbbbbbbbbbbbbb)
        /// </summary>

        /// <summary>
        /// Offset of the data section in the .dat file (Starts with 0xbbbbbbbbbbbbbbbb)
        /// </summary>
        private readonly long _dataTableOffset;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="dataTableOffset"></param>
        /// <param name="data"></param>
        /// <param name="length">lengr of string in bytes</param>
        public UnicodeString(int offset, long dataTableOffset, string data, int length)
            : base(offset, dataTableOffset)
        {
            _dataTableOffset = dataTableOffset;
            Offset = offset;
            OffsetEnd = offset + length;
            NewData = null;

            Data = data;
        }

        public UnicodeString(int offset, int dataTableOffset, BinaryReader inStream)
            : base(offset, dataTableOffset)
        {
            _dataTableOffset = dataTableOffset;
            Offset = offset;

            NewData = null;

            inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
            
            ReadData(inStream);

            OffsetEnd = Offset + 2*Data.Length + 4;
        }

        protected override void ReadData(BinaryReader inStream)
        {
            var sb = new StringBuilder();

            while (inStream.BaseStream.Position < inStream.BaseStream.Length)
            {
                var ch = inStream.ReadChar();
                if (ch == 0)
                {
                    // stream ends with 2 char(0) characters (4 '\0' bytes)
                    if (inStream.BaseStream.Position < inStream.BaseStream.Length)
                        ch = inStream.ReadChar();
                    break;
                }
                sb.Append(ch);
            }
            Data = sb.ToString();
        }

        /// <summary>
        /// Saves the unicode string to the specified stream. 
        /// If 'NewData' has been filled out then it will be written instead of the original data.
        /// </summary>
        /// <param name="outStream"></param>
        public override void Save(BinaryWriter outStream)
        {
            NewOffset = (int)(outStream.BaseStream.Position - _dataTableOffset);
            var dataToWrite = NewData ?? Data;

            foreach (var ch in dataToWrite)
            {
                outStream.Write(ch);
            }
            outStream.Write(0);
        }

        public override string GetValueString()
        {
            return Data;
        }
    }
}