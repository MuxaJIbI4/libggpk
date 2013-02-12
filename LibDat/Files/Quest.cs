using System.IO;

namespace LibDat.Files
{
        public class Quest : BaseDat
        {
                [StringIndex]
                public int Id { get; set; }
                public int Act { get; set; }
                [UserStringIndex]
                public int Title { get; set; }
                public int Unknown3 { get; set; }
                [StringIndex]
                public int QuestIcon { get; set; }

                public Quest(BinaryReader inStream)
                {
                        Id = inStream.ReadInt32();
                        Act = inStream.ReadInt32();
                        Title = inStream.ReadInt32();
                        Unknown3 = inStream.ReadInt32();
                        QuestIcon = inStream.ReadInt32();
                }

                public override void Save(BinaryWriter outStream)
                {
                        outStream.Write(Id);
                        outStream.Write(Act);
                        outStream.Write(Title);
                        outStream.Write(Unknown3);
                        outStream.Write(QuestIcon);
                }

                public override int GetSize()
                {
                        return 0x14;
                }
        }
}