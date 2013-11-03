using System.IO;

namespace LibDat.Files
{
	public class GlobalAudioConfig : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		public int Value { get; set; }

		public GlobalAudioConfig()
		{
			
		}
		public GlobalAudioConfig(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Value = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Value);
		}

		public override int GetSize()
		{
			return 0x08;
		}
	}
}