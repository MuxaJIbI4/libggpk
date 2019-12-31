using LibDat.Types;

namespace LibDat
{
    /// <summary>
    /// contains information about record single field: 
    /// 1) index of field in the row, visible name, description, whether it's a user field
    /// 2) underlying type of data
    /// </summary>
    public class FieldInfo
    {
        // index of field in record starting from 0
        public int Index { get; private set; }

        /// <summary>
        /// returns field's offset from record start (in bytes)
        /// </summary>
        public int RecordOffset { get; private set; }

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

        public BaseDataType FieldType { get; private set; }

        public bool IsPointer { get; private set; }

        // index, fieldId, fieldDescription, fieldType, isPointer
        public FieldInfo(BaseDataType type, int index, int recordOffset, 
            string id, string description, bool isUser = false) 
        {
            Index = index;
            Id = id;
            RecordOffset = recordOffset;
            Description = description;
            IsUser = isUser;
            IsPointer = type is PointerDataType;

            FieldType = type;
        }

        public string GetFullName(string delimiter)
        {
            return    Id                + delimiter 
                    + FieldType.Name    + delimiter
                    + (FieldType.Width == 1 ? " byte" : " bytes");
        }
    }
}