using System.IO;

namespace LibDat.Files
{
	public class ItemClassesDisplay : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Name { get; set; }
		public int Unknown3 { get; set; }

		public ItemClassesDisplay(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Name);
			outStream.Write(Unknown3);
		}

		public override int GetSize()
		{
			return 0x0C;
		}
	}
}