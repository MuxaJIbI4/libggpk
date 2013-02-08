using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestProject.Dat.Files
{
	class ArmourTypes : BaseDat
	{
		public Int64 Unkown0;

		public ArmourTypes(BinaryReader inStream)
		{
			Unkown0 = inStream.ReadInt64();
		}

		public override void Save(System.IO.BinaryWriter outStream)
		{
			outStream.Write(Unkown0);
		}

		public override int GetSize()
		{
			return 8;
		}

		public override string ToString()
		{
			return Unkown0.ToString();
		}
	}
}
