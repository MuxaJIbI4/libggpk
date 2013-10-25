using System.IO;

namespace LibDat.Files
{
        public class NPCTalk : BaseDat
        {
                public int Unknown0 { get; set; }
                public int Unknown1 { get; set; }
                public int Unknown2 { get; set; }
                [UserStringIndex]
                public int DialogueOption { get; set; }
                public int Unknown4 { get; set; }
                public int Unknown5 { get; set; }
                public int Unknown6 { get; set; }
                public int Unknown7 { get; set; }
                public int Unknown8 { get; set; }
                public int Unknown9 { get; set; }
                public int Unknown10 { get; set; }
                public int Unknown11 { get; set; }
                public int Unknown12 { get; set; }
                public int Unknown13 { get; set; }
                public int Unknown14 { get; set; }
                public int Unknown15 { get; set; }
                public int Unknown16 { get; set; }
                public int Unknown17 { get; set; }
                public int Unknown18 { get; set; }
                public int Unknown19 { get; set; }
                public int Unknown20 { get; set; }
                public bool Flag0 { get; set; }
                public bool Flag1 { get; set; }
                public bool Flag2 { get; set; }

                public NPCTalk(BinaryReader inStream)
                {
                        Unknown0 = inStream.ReadInt32();
                        Unknown1 = inStream.ReadInt32();
                        Unknown2 = inStream.ReadInt32();
                        DialogueOption = inStream.ReadInt32();
                        Unknown4 = inStream.ReadInt32();
                        Unknown5 = inStream.ReadInt32();
                        Unknown6 = inStream.ReadInt32();
                        Unknown7 = inStream.ReadInt32();
                        Unknown8 = inStream.ReadInt32();
                        Unknown9 = inStream.ReadInt32();
                        Unknown10 = inStream.ReadInt32();
                        Unknown11 = inStream.ReadInt32();
                        Unknown12 = inStream.ReadInt32();
                        Unknown13 = inStream.ReadInt32();
                        Unknown14 = inStream.ReadInt32();
                        Unknown15 = inStream.ReadInt32();
                        Unknown16 = inStream.ReadInt32();
                        Unknown17 = inStream.ReadInt32();
                        Unknown18 = inStream.ReadInt32();
                        Unknown19 = inStream.ReadInt32();
                        Unknown20 = inStream.ReadInt32();
                        Flag0 = inStream.ReadBoolean();
                        Flag1 = inStream.ReadBoolean();
                        Flag2 = inStream.ReadBoolean();
                }

                public override void Save(BinaryWriter outStream)
                {
                        outStream.Write(Unknown0);
                        outStream.Write(Unknown1);
                        outStream.Write(Unknown2);
                        outStream.Write(DialogueOption);
                        outStream.Write(Unknown4);
                        outStream.Write(Unknown5);
                        outStream.Write(Unknown6);
                        outStream.Write(Unknown7);
                        outStream.Write(Unknown8);
                        outStream.Write(Unknown9);
                        outStream.Write(Unknown10);
                        outStream.Write(Unknown11);
                        outStream.Write(Unknown12);
                        outStream.Write(Unknown13);
                        outStream.Write(Unknown14);
                        outStream.Write(Unknown15);
                        outStream.Write(Unknown16);
                        outStream.Write(Unknown17);
                        outStream.Write(Unknown18);
                        outStream.Write(Unknown19);
                        outStream.Write(Unknown20);
                        outStream.Write(Flag0);
                        outStream.Write(Flag1);
                        outStream.Write(Flag2);
                }

                public override int GetSize()
                {
                        return 0x57;
                }
        }
}