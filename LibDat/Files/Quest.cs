using System.IO;

namespace LibDat.Files
{
        public class Quest : BaseDat
        {
                [StringIndex]
                public int Index0 { get; set; }
                public int Unknown1 { get; set; }
                [StringIndex]
                public int Index1 { get; set; }
                public int Unknown3 { get; set; }
                [StringIndex]
                public int Index2 { get; set; }

                public Quest(BinaryReader inStream)
                {
                        Index0 = inStream.ReadInt32();
                        Unknown1 = inStream.ReadInt32();
                        Index1 = inStream.ReadInt32();
                        Unknown3 = inStream.ReadInt32();
                        Index2 = inStream.ReadInt32();
                }

                public override void Save(BinaryWriter outStream)
                {
                        outStream.Write(Index0);
                        outStream.Write(Unknown1);
                        outStream.Write(Index1);
                        outStream.Write(Unknown3);
                        outStream.Write(Index2);
                }

                public override int GetSize()
                {
                        return 0x14;
                }
        }
}