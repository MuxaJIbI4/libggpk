using System.IO;

namespace TestProject.Dat.Files
{
	public class BackendErrors : BaseDat
	{
		[StringIndex]
		public int Id;
		[StringIndex]
		public int Text;

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
			return 8;
		}
	}
}
