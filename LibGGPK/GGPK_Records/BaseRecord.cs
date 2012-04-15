using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	public class BaseRecord
	{
		public uint Length;
		public long RecordBegin;

		public BaseRecord()
		{

		}

		public virtual void Read(BinaryReader br)
		{

		}
	}
}
