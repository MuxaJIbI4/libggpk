using System.IO;

namespace LibDat.Files
{
	public class Difficulties : BaseDat
	{
		[StringIndex] // User string?
		public int Id { get; set; }
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		[UserStringIndex] 
		public int Text { get; set; }
		[StringIndex] 
		public int Prefix { get; set; }

		public Difficulties(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Text = inStream.ReadInt32();
			Prefix = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Text);
			outStream.Write(Prefix);
		}

		public override int GetSize()
		{
			return 0x14;
		}
	}
}