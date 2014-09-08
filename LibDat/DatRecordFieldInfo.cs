using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibDat
{
    // contains information about record's fields
    public class DatRecordFieldInfo
    {
        // index of field in record starting from 0
        public int Index { get; private set; }

        public string Description { get; private set; }

        public string Type { get; private set; }

        /// Field value represents an offset to a data in the data section of the .dat file with specific type:
        ///     StringIndex         unicode string
        ///     UserStringIndex     unicode string (shown to user)
        ///     DataIndex           data isn't explored yet and are probably incorrect.
        ///     UInt64Index         uint64 list
        ///     UInt32Index         uint32 list
        ///     Int32Index:         int32 list
        public string PointerType { get; private set; }

        // returns true if fields contains offset to data section of .dat
        public bool HasPointer { get; private set; }

        public DatRecordFieldInfo(int index, string description, string type, string pointerType)
        {
            Index = index;
            Description = description;
            Type = type;
            PointerType = pointerType;

            if (!String.IsNullOrEmpty(pointerType))
            {
                HasPointer = true;
            }
        }

        public bool IsString()
        {
            return PointerType.Equals("UserStringIndex") || PointerType.Equals("StringIndex");
        }


        public bool IsUserString()
        {
            return PointerType.Equals("UserStringIndex");
        }

        public string ToString(string delimiter)
        {
            string s = Description + delimiter;
            s += (HasPointer ? "*[" + PointerType + "]" : Type );
            return s;
        }
    }
}
