using System.IO;

namespace LibDat.Files
{
	public class Realms : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Name { get; set; }
		[StringIndex]
		public int Server { get; set; }
		public bool Flag0 { get; set; }

		public Realms(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			Server = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Name);
			outStream.Write(Server);
			outStream.Write(Flag0);
		}

		public override int GetSize()
		{
			return 0xD;
		}
	}
}