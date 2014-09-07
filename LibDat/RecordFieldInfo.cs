using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibDat
{
    // contains information about record's fields
    public class RecordFieldInfo
    {
        // index of field in record starting from 0
        public int Index { get; private set; }

        public string Description { get; private set; }

        public string Type { get; private set; }

        public string PointerType { get; private set; }

        public RecordFieldInfo(int index, string description, string type, string pointerType)
        {
            Index = index;
            Description = description;
            Type = type;
            PointerType = pointerType;
        }

        public bool IsUserString()
        {
            return PointerType.Equals("UserStringIndex");
        }
    }
}
