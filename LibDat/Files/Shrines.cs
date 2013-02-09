using System.IO;

namespace LibDat.Files
{
	class Shrines : BaseDat
	{
		[StringIndex]
		public int Id;
		public int Charges;
		public bool ChargesShared;
		public int RegenTime;
		public int Unknown2;
		public int Unknown3;
		[StringIndex]
		public int Name;
		[StringIndex]
		public int Message;
		[StringIndex]
		public int AoFile;
		public int Magnitude1;
		public int Magnitude2;
		public int Magnitude3;
		public int Magnitude4;
		public int Unknown4;

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
			return 0x19;
		}
	}
}
