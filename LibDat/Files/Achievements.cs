using System.IO;

namespace LibDat.Files
{
	public class Achievements : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Description { get; set; }
		public int Unknown2 { get; set; }
        [UserStringIndex]
		public int Objective { get; set; }
		public int Unknown4 { get; set; }
		public bool Flag0 { get; set; }
		public bool Flag1 { get; set; }

		public Achievements(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Description = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Objective = inStream.ReadInt32();
			Unknown4 = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Flag1 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Description);
			outStream.Write(Unknown2);
			outStream.Write(Objective);
			outStream.Write(Unknown4);
			outStream.Write(Flag0);
			outStream.Write(Flag1);
		}

		public override int GetSize()
		{
			return 0x16;
		}
	}
}
