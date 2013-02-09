using System.IO;

namespace LibDat
{
	public class StringIndex : System.Attribute { }
	public class DataIndex : System.Attribute { }

	public abstract class BaseDat
	{
		/// <summary>
		/// Save this record to the specified stream. Stream position is not preserved.
		/// </summary>
		/// <param name="outStream">Stream to write contents to</param>
		public abstract void Save(BinaryWriter outStream);
		/// <summary>
		/// Represents the number of bytes this record will read or write to the DAT file
		/// </summary>
		/// <returns>Number of bytes this record will take in its native format</returns>
		public abstract int GetSize();
	}
}
