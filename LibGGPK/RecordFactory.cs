using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LibGGPK
{
	/// <summary>
	/// Simplifies the task of creating records from data read in from the pack file
	/// </summary>
	internal static class RecordFactory
	{
		/// <summary>
		/// Reads a single record from the specified stream and creates the appropriate record type.
		/// </summary>
		/// <param name="br">Stream pointing at a record</param>
		/// <returns></returns>
		public static BaseRecord ReadRecord(BinaryReader br)
		{
			uint Length = br.ReadUInt32();
			string Tag = ASCIIEncoding.ASCII.GetString(br.ReadBytes(4));

			switch (Tag)
			{
				case FileRecord.Tag:
					return new FileRecord(Length, br);
				case GGPKRecord.Tag:
					return new GGPKRecord(Length, br);
				case FreeRecord.Tag:
					return new FreeRecord(Length, br);
				case DirectoryRecord.Tag:
					return new DirectoryRecord(Length, br);
			}

			throw new Exception("Invalid tag");
		}
	}
}
