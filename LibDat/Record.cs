using System;
using System.Collections;
using System.Collections.Generic;
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
	/// <summary>
	/// Property or field represents an offset to uint64 list in the data section of the .dat file
	/// </summary>
	public class UInt64Index : System.Attribute { }
	/// <summary>
	/// Property or field represents an offset to uint32 list in the data section of the .dat file
	/// </summary>
	public class UInt32Index : System.Attribute { }
	/// <summary>
	/// Property or field represents an offset to int32 list in the data section of the .dat file
	/// </summary>
	public class Int32Index : System.Attribute { }

	public class UserStringIndex : StringIndex
	{
	}

    

	public class Record
	{
        private RecordInfo recordInfo;
        private List<Object> values;
        private int fieldsCount;

        public Record(RecordInfo ri, BinaryReader inStream)
        {
            // TODO Test summary length ?
            recordInfo = ri;
            values = new List<Object>();
            fieldsCount = ri.Fields.Count;

            foreach(RecordFieldInfo fi in ri.Fields)
            {
                string type = fi.Type;
                Object o = null;
                switch (type)
                {
                    case "bool"     : o = inStream.ReadBoolean();break;
                    case "byte"     : o = inStream.ReadByte();break;
                    case "short"    : o = inStream.ReadInt16();break;
                    case "int"      : o = inStream.ReadInt32();break;
                    case "Int64"    : o = inStream.ReadInt64();break;
                    default:
                        // TODO: die here with error
                        break;
                }
                values.Add(o);
            }
        }

        public object GetField(int index)
        {
            if (index < 0 || index >= fieldsCount)
            {
                throw new Exception("Field's index out of bounds: " + index + " not in [0," + fieldsCount + "]");
            }
            return values[index];
        }

		/// <summary>
		/// Save this record to the specified stream. Stream position is not preserved.
		/// </summary>
		/// <param name="outStream">Stream to write contents to</param>
        public void Save(BinaryWriter outStream)
        {
            IEnumerator<Object> iter = values.GetEnumerator();
            foreach (RecordFieldInfo fi in recordInfo.Fields)
            {
                iter.MoveNext();
                Object o = iter.Current;
                string type = fi.Type;
                switch (type)
                {
                    case "bool"     : outStream.Write((bool)o); break;
                    case "byte"     : outStream.Write((byte)o); break;
                    case "short"    : outStream.Write((short)o); break;
                    case "int"      : outStream.Write((int)o); break;
                    case "Int64"    : outStream.Write((Int64)o); break;
                }
                values.Add(o);
            }
        }
	}
}
