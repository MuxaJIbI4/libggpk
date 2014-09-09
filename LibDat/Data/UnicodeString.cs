using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace LibDat.Data
{
    /// <summary>
    /// Represents a unicode string found in the data section of a .dat file
    /// </summary>
    public class UnicodeString : AbstractData
    {
        /// <summary>
        /// Offset in the dat file with respect to the beginning of the data section
        /// </summary>
        public long Offset { get; private set; }

        /// <summary>
        /// The string
        /// </summary>
        public string Data { get; private set; }
        /// <summary>
        /// The replacement string. If this is set then it will replace the original string when it's saved.
        /// </summary>
        public string NewData { get; set; }
        /// <summary>
        /// Determins if this UnicodeString is a translatable string (eg: not used as an id, path, etc)
        /// </summary>
        public bool IsUserString { get; set; }
        /// <summary>
        /// Offset of the new string with respect to the beginning of the data section. This will be invalid until save is called.
        /// </summary>
        public long NewOffset;
        /// <summary>
        /// Offset of the data section in the .dat file (Starts with 0xbbbbbbbbbbbbbbbb)
        /// </summary>

        /// <summary>
        /// Offset of the data section in the .dat file (Starts with 0xbbbbbbbbbbbbbbbb)
        /// </summary>
        private readonly long dataTableOffset;

        public UnicodeString(long offset, long dataTableOffset, string data)
            : base (offset, dataTableOffset)
        {
            this.dataTableOffset = dataTableOffset;
            Offset = offset;
            NewData = null;
            IsUserString = false;

            Data = data;
        }

        public UnicodeString(long offset, long dataTableOffset, BinaryReader inStream, bool isUserString)
            : base(offset, dataTableOffset)
        {
            this.dataTableOffset = dataTableOffset;
            Offset = offset;

            NewData = null;
            IsUserString = isUserString;

            inStream.BaseStream.Seek(offset + dataTableOffset, SeekOrigin.Begin);
            ReadData(inStream);
            if (Data.Length == 1)
            {
                Console.WriteLine(Data);
                char ch = Data[0];
                int pointer = (int)ch;
                Console.WriteLine(pointer);
                // TODO: this is new pointer ?
            }
        }

        public override void ReadData(BinaryReader inStream)
        {
            StringBuilder sb = new StringBuilder();

            while (inStream.BaseStream.Position < inStream.BaseStream.Length)
            {
                char ch = inStream.ReadChar();
                if (ch == 0)
                {
                    // stream ends with 2 char(0) characters (4 '\0' bytes)
                    ch = inStream.ReadChar();
                    break;
                }

                sb.Append(ch);
            }

            this.Data = sb.ToString();
        }

        /// <summary>
        /// Saves the unicode string to the specified stream. 
        /// If 'NewData' has been filled out then it will be written instead of the original data.
        /// </summary>
        /// <param name="outStream"></param>
        public override void Save(BinaryWriter outStream)
        {
            NewOffset = (int)(outStream.BaseStream.Position - dataTableOffset);
            string dataToWrite = NewData ?? Data;

            for (int i = 0; i < dataToWrite.Length; i++)
            {
                outStream.Write(dataToWrite[i]);
            }
            outStream.Write((int)0);
        }

        public override string ToString()
        {
            return Data;
        }
    }
}