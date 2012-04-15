using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	/// <summary>
	/// Every record has a Length, just a common class for all the records
	/// </summary>
	public class BaseRecord
	{
		/// <summary>
		/// Length of the entire record in bytes
		/// </summary>
		public uint Length;
		/// <summary>
		/// Offset in pack file where record begins
		/// </summary>
		public long RecordBegin;

		public BaseRecord()
		{

		}

		public virtual void Read(BinaryReader br)
		{

		}
	}
}
