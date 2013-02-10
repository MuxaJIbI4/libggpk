using System.IO;

namespace LibDat.Files
{
	public class BackendErrors : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[StringIndex]
		public int Text { get; set; }

		public BackendErrors()
		{
			
		}
		public BackendErrors(BinaryReader inStream)
		{
			Id = inStream.ReadInt32();
			Text = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Id);
			outStream.Write(Text);
		}

		public override int GetSize()
		{
			return 0x08;
		}
	}
}
