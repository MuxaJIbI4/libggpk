using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibDat
{

    public enum FieldTypes
    {
        _01bit,
        _08bit,
        _16bit,
        _32bit,
        _64bit,
    };

    ///     StringIndex         unicode string
    ///     UserStringIndex     unicode string (visible to user)
    ///     UInt64Index         uint64 list
    ///     UInt32Index         uint32 list
    ///     Int32Index:         int32 list
    ///     DataIndex           data isn't explored yet and are probably incorrect.
    public enum PointerTypes
    {
        StringIndex,
        IndirectStringIndex, // ???
        UserStringIndex,
        UInt64Index,
        UInt32Index,
        Int32Index,
        DataIndex,
    };

    // contains information about record's fields
    public class DatRecordFieldInfo
    {
        // index of field in record starting from 0
        public int Index { get; private set; }

        public string Description { get; private set; }

        public FieldTypes FieldType { get; private set; }

        /// Field value represents an offset to a data in the data section of the .dat file with specific type
        public PointerTypes PointerType { get; private set; }

        // returns true if fields contains offset to data section of .dat
        public bool HasPointer { get; private set; }

        public DatRecordFieldInfo(int index, string description, FieldTypes type)
        {
            Index = index;
            Description = description;
            FieldType = type;
            HasPointer = false;
        }

        public DatRecordFieldInfo(int index, string description, FieldTypes type, PointerTypes pointerType)
            : this(index, description, type)
        {
            PointerType = pointerType;
            HasPointer = true;
        }


        public bool IsString()
        {
            return PointerType == PointerTypes.StringIndex
                || PointerType == PointerTypes.UserStringIndex;
        }


        public bool IsUserString()
        {
            return PointerType == PointerTypes.UserStringIndex;
        }

        public string ToString(string delimiter)
        {
            string s = Description + delimiter;
            s += (HasPointer 
                ? "*[" + Enum.GetName(typeof(PointerTypes), PointerType) + "]"
                : Enum.GetName(typeof(FieldTypes), FieldType));
            return s;
        }
    }
}
