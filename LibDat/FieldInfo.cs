namespace LibDat
{
    /// <summary>
    /// contains information about record field: visible name, description and type of data inside
    /// </summary>
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

        // returns true if fields contains user string (data)
        public bool IsUser { get; private set; }


        // index, fieldId, fieldDescription, fieldType, isPointer
        public FieldInfo(int index, string id, string description, FieldTypeInfo type, bool isUser = false)
        {
            Index = index;
            Id = id;
            Description = description;
            FieldType = type;
            IsPointer = type.IsPointer;
            IsUser = isUser;
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
            return Id + delimiter + (FieldType.IsPointer?"*":"") + FieldType.Name + delimiter + FieldType.Width + " byte";
        }
    }
}
