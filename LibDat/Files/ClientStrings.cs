using System.IO;

namespace LibDat.Files
{
	public class ClientStrings : BaseDat
	{
		[StringIndex]
		public int Id { get; set; }
		[UserStringIndex]
		public int Text { get; set; }

		public ClientStrings()
		{
			
		}
		public ClientStrings(BinaryReader inStream)
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