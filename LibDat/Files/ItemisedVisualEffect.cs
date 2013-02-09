using System;
using System.IO;

namespace LibDat.Files
{
        public class ItemisedVisualEffect : BaseDat
        {
                public Int64 Unknown0 { get; set; }
                public Int64 Unknown1 { get; set; }
                public Int64 Unknown2 { get; set; }
                public Int64 Unknown3 { get; set; }
                public Int64 Unknown4 { get; set; }
                public int Unknown5 { get; set; }
                public int Unknown6 { get; set; }
                [StringIndex]
                public int Index0 { get; set; }
                public Int64 Unknown7 { get; set; }

                public ItemisedVisualEffect(BinaryReader inStream)
                {
                        Unknown0 = inStream.ReadInt64();
                        Unknown1 = inStream.ReadInt64();
                        Unknown2 = inStream.ReadInt64();
                        Unknown3 = inStream.ReadInt64();
                        Unknown4 = inStream.ReadInt64();
                        Unknown5 = inStream.ReadInt32();
                        Unknown6 = inStream.ReadInt32();
                        Index0 = inStream.ReadInt32();
                        Unknown7 = inStream.ReadInt64();
                }

                public override void Save(BinaryWriter outStream)
                {
                        outStream.Write(Unknown0);
                        outStream.Write(Unknown1);
                        outStream.Write(Unknown2);
                        outStream.Write(Unknown3);
                        outStream.Write(Unknown4);
                        outStream.Write(Unknown5);
                        outStream.Write(Unknown6);
                        outStream.Write(Index0);
                        outStream.Write(Unknown7);
                }

                public override int GetSize()
                {
                        return 0x40;
                }
        }
}