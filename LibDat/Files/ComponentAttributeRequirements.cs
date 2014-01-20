using System.IO;

namespace LibDat.Files
{
	public class ComponentAttributeRequirements : BaseDat
	{
		[StringIndex]
		public int Index0 { get; set; }
		public int ReqStr { get; set; }
		public int ReqDex { get; set; }
		public int RegInt { get; set; }

		public ComponentAttributeRequirements(BinaryReader inStream)
		{
			Index0 = inStream.ReadInt32();
			ReqStr = inStream.ReadInt32();
			ReqDex = inStream.ReadInt32();
			RegInt = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Index0);
			outStream.Write(ReqStr);
			outStream.Write(ReqDex);
			outStream.Write(RegInt);
		}

		public override int GetSize()
		{
			return 0x10;
		}
	}
}