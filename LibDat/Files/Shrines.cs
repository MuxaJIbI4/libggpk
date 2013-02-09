using System.IO;

namespace LibDat.Files
{
	public class Shrines : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Charges { get; set; }
		public bool ChargesShared { get; set; }
		public int RegenTime { get; set; }
		public int Unknown2 { get; set; }
		public int Unknown3 { get; set; }
		[StringIndex]
		public int Name { get; set; }
		[StringIndex]
		public int Message { get; set; }
		[StringIndex]
		public int AoFile { get; set; }
		public int Magnitude1 { get; set; }
		public int Magnitude2 { get; set; }
		public int Magnitude3 { get; set; }
		public int Magnitude4 { get; set; }
		public int Unknown4 { get; set; }

		public Shrines(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Charges = inStream.ReadInt32();
			ChargesShared = inStream.ReadBoolean();
			RegenTime = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			Message = inStream.ReadInt32();
			AoFile = inStream.ReadInt32();
			Magnitude1 = inStream.ReadInt32();
			Magnitude2 = inStream.ReadInt32();
			Magnitude3 = inStream.ReadInt32();
			Magnitude4 = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
		}
		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Charges);
			outStream.Write(ChargesShared);
			outStream.Write(RegenTime);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
			outStream.Write(Name);
			outStream.Write(Message);
			outStream.Write(AoFile);
			outStream.Write(Magnitude1);
			outStream.Write(Magnitude2);
			outStream.Write(Magnitude3);
			outStream.Write(Magnitude4);
			outStream.Write(Unknown4);
		}

		public override int GetSize()
		{
			return 0x35;
		}
	}
}
