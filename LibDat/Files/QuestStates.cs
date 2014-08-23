using System;
using System.IO;

namespace LibDat.Files
{
	public class QuestStates : BaseDat
	{
		public Int64 Group { get; set; }
		public int Unknown2 { get; set; }
		public int Data0Length { get; set; }
		[UInt32Index]
		public int Data0 { get; set; }
		public int Data1Length { get; set; }
		[UInt32Index]
		public int Data1 { get; set; }
		[UserStringIndex]
		public int Text { get; set; }
		public bool Flag0 { get; set; }
		[UserStringIndex]
		public int Message { get; set; }
		public int Data2Length { get; set; }
		[UInt64Index]
		public int Data2 { get; set; }
		public int Unknown9 { get; set; }
		public int Unknown10 { get; set; }
		public int Unknown11 { get; set; }
		public int Unknown12 { get; set; }
		public int Unknown13 { get; set; }


		public QuestStates(BinaryReader inStream)
		{
			Group = inStream.ReadInt64();
			Unknown2 = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Text = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Message = inStream.ReadInt32();
			Data2Length = inStream.ReadInt32();
			Data2 = inStream.ReadInt32();
			Unknown9 = inStream.ReadInt32();
			Unknown10 = inStream.ReadInt32();
			Unknown11 = inStream.ReadInt32();
			Unknown12 = inStream.ReadInt32();
			Unknown13 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Group);
			outStream.Write(Unknown2);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Text);
			outStream.Write(Flag0);
			outStream.Write(Message);
			outStream.Write(Data2Length);
			outStream.Write(Data2);
			outStream.Write(Unknown9);
			outStream.Write(Unknown10);
			outStream.Write(Unknown11);
			outStream.Write(Unknown12);
			outStream.Write(Unknown13);
		}

		public override int GetSize()
		{
			return 0x41;
		}
	}
}
