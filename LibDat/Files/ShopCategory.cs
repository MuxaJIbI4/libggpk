using System.IO;

namespace LibDat.Files
{
	public class ShopCategory : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Unknown1 { get; set; }
		[StringIndex]
		public int Unknown2 { get; set; }
		[StringIndex] // UserStringIndex?
		public int Unknown3 { get; set; }

		public ShopCategory()
		{
			
		}
		public ShopCategory(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Unknown2 = inStream.ReadInt32();
			Unknown3 = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Unknown1);
			outStream.Write(Unknown2);
			outStream.Write(Unknown3);
		}

		public override int GetSize()
		{
			return 0x10;
		}
	}
}