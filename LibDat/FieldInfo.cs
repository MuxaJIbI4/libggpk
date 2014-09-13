using LibDat.Types;

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
        
        // returns true if fields contains user string (data)
        public bool IsUser { get; private set; }

        public DataType FieldType { get; private set; }

        public bool IsPointer { get; private set; }

        // index, fieldId, fieldDescription, fieldType, isPointer
        public FieldInfo(DataType type, int index, string id, string description, bool isUser = false) 
        {
            Index = index;
            Id = id;
            Description = description;
            IsUser = isUser;
            IsPointer = type is PointerDataType;

            FieldType = type;
        }

        public bool IsString()
        {
            return FieldType.Name.Equals("String");
        }

        public string ToString(string delimiter)
        {
            return    Id                + delimiter 
                    + FieldType.Name    + delimiter 
                    + FieldType.Width   + " byte";
        }
    }
}
