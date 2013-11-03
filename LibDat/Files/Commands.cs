using System.IO;

namespace LibDat.Files
{
	public class Commands : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Command { get; set; }
		public bool Flag0 { get; set; }

		public Commands()
		{
			
		}
		public Commands(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Command = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Command);
			outStream.Write(Flag0);
		}

		public override int GetSize()
		{
			return 0x09;
		}
	}
}