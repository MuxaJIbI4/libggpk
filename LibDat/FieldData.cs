using System;

namespace LibDat
{
    /// <summary>
    /// contains information about field data:
    /// * if field is value type it stores in the field value
    /// * if field is pointer type it stores field value plus offset to value type in data section
    ///   ( this data can't pointer to other data section )
    /// </summary>
    public class FieldData
    {
        /// <summary>
        /// contains data from record
        /// </summary>
        private object _value;
        public object Value {
            get { return _value; }
            set
            {
                var width = FieldInfo.FieldType.Width;
                var error = false;
                switch (width)
                {
                    case 1: if (!(value is bool || value is byte)) error = true; break;
                    case 2: if (!(value is short)) error = true; break;
                    case 4: if (!(value is int)) error = true; break;
                    case 8: if (!(value is Int64)) error = true; break;
                }
                if (error)
                    throw new Exception("Can't save value of type " + value.GetType()
                        + " into field of type " + FieldInfo.FieldType.Name);
                _value = value;
            }
        }
        
        /// <summary>
        /// Contains offset to AbstractData in data section (only for pointer type fields)
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Contains offset to value AbstractData in data section (only for pointer type fields).
        /// If AbstractData at <c>Offset</c> isn't PointerData then <c>>ValueOffset = Offset</c>
        /// </summary>
        public int ValueOffset { get; set; }

        public int Length { get; private set; }

        public FieldInfo FieldInfo { get; private set; }

        public FieldData(FieldInfo field, object o)
        {
            FieldInfo = field;
            _value = o;

            // init with incorrect value
            ValueOffset = 0;
            Length = -1;

            if (!FieldInfo.IsPointer)
                return;

            if (FieldInfo.FieldType.Width == 8)
            {
                int length;
                int offset;
                GetListLengthAndOffset(this, out length, out offset);
                Offset = offset;
                Length = length;
            }
            else if (FieldInfo.FieldType.Width == 4)
            {
                Offset = (int) Value;
            }
            else
                throw new Exception("Pointer type field width should 4 or 8, but found: " + FieldInfo.FieldType.Width);
        }

        /// <summary>
        /// Returns pointer prefi string in format:
        ///     ""                      if field is value type
        ///     "@ = "                  if field's width is 4
        ///     "[length]@offset = "    if field's width is 8
        /// </summary>
        /// <returns></returns>
        public string GetOffsetPrefix()
        {
            if (!FieldInfo.FieldType.IsPointer) return String.Empty;
            if (FieldInfo.FieldType.Width != 8) return String.Format("@{0}", Offset);
            
            return String.Format("[{0}]@{1}", Length, Offset);
        }

        private static void GetListLengthAndOffset(FieldData fieldData, out int length, out int offset)
        {
            if (fieldData.FieldInfo.FieldType.Width != 8)
                throw new Exception("Can't extract length and offset from this type: " +
                    fieldData.FieldInfo.FieldType.Name);
            var value = (Int64)fieldData.Value;
            var bytes = BitConverter.GetBytes(value);
            length = BitConverter.ToInt32(bytes, 0);
            offset = BitConverter.ToInt32(bytes, 4);
        }
    }
}
