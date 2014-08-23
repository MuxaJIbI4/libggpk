using System.IO;

namespace LibDat.Files
{
	public class BindableVirtualKeys : BaseDat
	{
		public int KeyCode { get; set; }
		[UserStringIndex]
		public int Name { get; set; }
		[StringIndex]
		public int Id { get; set; }

		public BindableVirtualKeys(BinaryReader inStream)
		{
			KeyCode = inStream.ReadInt32();
			Name = inStream.ReadInt32();
			Id = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(KeyCode);
			outStream.Write(Name);
			outStream.Write(Id);
		}

		public override int GetSize()
		{
			return 0x0C;
		}
	}
}