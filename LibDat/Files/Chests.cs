using System;
using System.IO;

namespace LibDat.Files
{
	public class Chests : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public bool Flag0 { get; set; }
		public int Unknown0 { get; set; }
		[UserStringIndex] // Translatable?
		public int Name { get; set; }
		[StringIndex]
		public int AnimatedObject { get; set; }
		public bool Flag1 { get; set; }
		public bool Flag2 { get; set; }
		public int Unknown1 { get; set; }
		public bool Flag3 { get; set; }
		public bool Flag4 { get; set; }
		public int Unknown2 { get; set; }
		public int Data0Length { get; set; }
		//[DataIndex]
		public int Data0 { get; set; }
		public int Data1Length { get; set; }
		//[DataIndex]
		public int Data1 { get; set; }
		public Int64 ItemKey { get; set; }
		public bool Flag5 { get; set; }

		public Chests(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown0 = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			AnimatedObject = inStream.ReadInt32();
			Flag1 = inStream.ReadBoolean();
			Flag2 = inStream.ReadBoolean();
			Unknown1 = inStream.ReadInt32();
			Flag3 = inStream.ReadBoolean();
			Flag4 = inStream.ReadBoolean();
			Unknown2 = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			ItemKey = inStream.ReadInt64();
			Flag5 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Flag0);
			outStream.Write(Unknown0);
			outStream.Write(Name);
			outStream.Write(AnimatedObject);
			outStream.Write(Flag1);
			outStream.Write(Flag2);
			outStream.Write(Unknown1);
			outStream.Write(Flag3);
			outStream.Write(Flag4);
			outStream.Write(Unknown2);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(ItemKey);
			outStream.Write(Flag5);
		}

		public override int GetSize()
		{
			return 0x36;
		}
	}
}