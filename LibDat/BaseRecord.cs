using System.IO;

namespace LibDat
{
	/// <summary>
	/// Property or field represents an offset to a unicode string in the data section of the .dat file
	/// </summary>
	public class StringIndex : System.Attribute { }
	/// <summary>
	/// Property or field represents an offset to unknown data in the data section of the .dat file. These entries are not yet explored and are probably incorrect.
	/// </summary>
	public class DataIndex : System.Attribute { }

	public class UserStringIndex : StringIndex
	{
	}


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
