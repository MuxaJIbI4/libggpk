using System;
using System.IO;

namespace LibDat.Files
{
	public class HideoutInteractable : BaseDat
	{
		public int Unknown0 { get; set; }
		public int Unknown1 { get; set; }
		[StringIndex]
		public int Object { get; set; }

		public HideoutInteractable()
		{

		}

		public HideoutInteractable(BinaryReader inStream)
		{
			Unknown0 = inStream.ReadInt32();
			Unknown1 = inStream.ReadInt32();
			Object = inStream.ReadInt32();
		}

		public override void Save(BinaryWriter outStream)
		{
			outStream.Write(Unknown0);
			outStream.Write(Unknown1);
			outStream.Write(Object);
		}

		public override int GetSize()
		{
			return 0xC;
		}
	}
}