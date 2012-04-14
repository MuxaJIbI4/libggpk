using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	public class FreeRecord : BaseRecord
	{
		public const string Tag = "FREE";
		public long DataBegin;
		public long DataLength;

		public FreeRecord(uint length, BinaryReader br)
		{
			Length = length;
			Read(br);
		}

		public override void Read(BinaryReader br)
		{
			base.Read(br);

			DataBegin = br.BaseStream.Position;
			DataLength = Length - 8;
			br.BaseStream.Seek(Length - 8, SeekOrigin.Current);
		}

		public override string ToString()
		{
			return Tag;
		}


		public byte[] ReadData(string ggpkPath)
		{
			byte[] buffer = new byte[DataLength];

			using (var fs = File.Open(ggpkPath, FileMode.Open))
			{
				fs.Seek(DataBegin, SeekOrigin.Begin);
				fs.Read(buffer, 0, buffer.Length);
			}

			return buffer;
		}
	}
}
