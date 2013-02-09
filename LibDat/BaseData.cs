using System.IO;

namespace LibDat
{
	public abstract class BaseData
	{
		/// <summary>
		/// Save this record to the specified stream. Stream position is not preserved.
		/// </summary>
		/// <param name="outStream">Stream to write contents to</param>
		public abstract void Save(BinaryWriter outStream);
	}
}