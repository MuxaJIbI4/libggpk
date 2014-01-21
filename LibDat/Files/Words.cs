using System.IO;

namespace LibDat.Files
{
	public class Words : BaseDat
	{
		public int Unknown0 { get; set; }
		[UserStringIndex]
		public int Text { get; set; }
		public int Data0Length { get; set; }
		[UInt64Index]
		public int Data0 { get; set; }
		public int Data1Length { get; set; }
		[UInt32Index]
		public int Data1 { get; set; }
		public int Unknown6 { get; set; }

		public Words(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt32();
			Text = inStream.ReadInt32();
			Data0Length = inStream.ReadInt32();
			Data0 = inStream.ReadInt32();
			Data1Length = inStream.ReadInt32();
			Data1 = inStream.ReadInt32();
			Unknown6 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Text);
			outStream.Write(Data0Length);
			outStream.Write(Data0);
			outStream.Write(Data1Length);
			outStream.Write(Data1);
			outStream.Write(Unknown6);
		}

		public override int GetSize()
		{
			return 0x1C;
		}
	}
}