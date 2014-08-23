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
		[StringIndex]
		public int Unknown2 { get; set; } // Duplicate data of 'Command' ?

		public Commands()
		{
			
		}
		public Commands(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Command = inStream.ReadInt32();
			Flag0 = inStream.ReadBoolean();
			Unknown2 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Command);
			outStream.Write(Flag0);
			outStream.Write(Unknown2);
		}

		public override int GetSize()
		{
			return 0x0D;
		}
	}
}