namespace LibDat
{

    //    public enum FieldTypes
    //    {
    //        _01bit,
    //        _08bit,
    //        _16bit,
    //        _32bit,
    //        _64bit,
    //    };
    //
    //    ///     StringIndex         unicode string
    //    ///     UserStringIndex     unicode string (visible to user)
    //    ///     UInt64Index         uint64 list
    //    ///     UInt32Index         uint32 list
    //    ///     Int32Index:         int32 list
    //    ///     DataIndex           data isn't explored yet and are probably incorrect.
    //    public enum PointerTypes
    //    {
    //        StringIndex,
    //        IndirectStringIndex, // ???
    //        UserStringIndex,
    //        UInt64Index,
    //        UInt32Index,
    //        Int32Index,
    //        DataIndex,
    //    };

    // contains information about record's fields
    public class FieldInfo
    {
        // index of field in record starting from 0
        public int Index { get; private set; }

        /// <summary>
        /// Field name
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Custom text for field
        /// </summary>
        public string Description { get; private set; }

        public FieldTypeInfo FieldType { get; private set; }

        // returns true if fields contains offset to data section of .dat
        public bool IsPointer { get; private set; }

        /// Field value represents an offset to a data in the data section of the .dat file with specific type
        //        public PointerTypes PointerTypeString { get; private set; }

        //        public Type PointerType { get; private set; }

        // index, fieldId, fieldDescription, fieldType, isPointer
        public FieldInfo(int index, string id, string description, FieldTypeInfo type)
        {
            Index = index;
            Id = id;
            Description = description;
            FieldType = type;
            IsPointer = type.IsPointer;
        }

        public bool IsString()
        {
            return FieldType.Name.Equals("UserString") || FieldType.Name.Equals("String");
        }

        public bool IsUserString()
        {
            return FieldType.Name.Equals("UserString");
        }

        public string ToString(string delimiter)
        {
            string s = Description + delimiter;
            s += (IsPointer ? FieldType.PointerType : FieldType.Name);
            return s;
        }
    }
}
