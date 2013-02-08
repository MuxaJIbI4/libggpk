using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject.Dat.Files
{
	class VoteState : BaseDat
	{
		[StringIndex]
		public int Id;
		[StringIndex]
		public int Text;

		public VoteState(BinaryReader inStream)
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
			return 0x8;
		}
	}
}
