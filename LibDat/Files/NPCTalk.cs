using System;
using System.IO;

namespace LibDat.Files
{
        public class NPCTalk : BaseDat
        {
                public Int64 NPCKey { get; set; }
                public int Unknown2 { get; set; }
                [UserStringIndex]
                public int DialogueOption { get; set; }
                public int Data0Length { get; set; }
				[DataIndex]
                public int Data0 { get; set; }
                public int Data1Length { get; set; }
				[DataIndex]
                public int Data1 { get; set; }
                public int Data2Length { get; set; }
				[DataIndex]
                public int Data2 { get; set; }
				[StringIndex]
                public int Unknown10 { get; set; }
                public int Unknown11 { get; set; }
                public int Unknown12 { get; set; }
                public int Unknown13 { get; set; }
                public int Unknown14 { get; set; }
                public int Unknown15 { get; set; }
                public int Data3Length { get; set; }
				[DataIndex]
                public int Data3 { get; set; }
                public int Data4Length { get; set; }
				[DataIndex]
                public int Data4 { get; set; }
				[StringIndex]
                public int Unknown20 { get; set; }
                public bool Flag0 { get; set; }
                public bool Flag1 { get; set; }
                public bool Flag2 { get; set; }

                public NPCTalk(BinaryReader inStream)
                {
                        NPCKey = inStream.ReadInt64();
                        Unknown2 = inStream.ReadInt32();
                        DialogueOption = inStream.ReadInt32();
                        Data0Length = inStream.ReadInt32();
                        Data0 = inStream.ReadInt32();
                        Data1Length = inStream.ReadInt32();
                        Data1 = inStream.ReadInt32();
                        Data2Length = inStream.ReadInt32();
                        Data2 = inStream.ReadInt32();
                        Unknown10 = inStream.ReadInt32();
                        Unknown11 = inStream.ReadInt32();
                        Unknown12 = inStream.ReadInt32();
                        Unknown13 = inStream.ReadInt32();
                        Unknown14 = inStream.ReadInt32();
                        Unknown15 = inStream.ReadInt32();
                        Data3Length = inStream.ReadInt32();
                        Data3 = inStream.ReadInt32();
                        Data4Length = inStream.ReadInt32();
                        Data4 = inStream.ReadInt32();
                        Unknown20 = inStream.ReadInt32();
                        Flag0 = inStream.ReadBoolean();
                        Flag1 = inStream.ReadBoolean();
                        Flag2 = inStream.ReadBoolean();
                }

                public override void Save(BinaryWriter outStream)
                {
                        outStream.Write(NPCKey);
                        outStream.Write(Unknown2);
                        outStream.Write(DialogueOption);
                        outStream.Write(Data0Length);
                        outStream.Write(Data0);
                        outStream.Write(Data1Length);
                        outStream.Write(Data1);
                        outStream.Write(Data2Length);
                        outStream.Write(Data2);
                        outStream.Write(Unknown10);
                        outStream.Write(Unknown11);
                        outStream.Write(Unknown12);
                        outStream.Write(Unknown13);
                        outStream.Write(Unknown14);
                        outStream.Write(Unknown15);
                        outStream.Write(Data3Length);
                        outStream.Write(Data3);
                        outStream.Write(Data4Length);
                        outStream.Write(Data4);
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