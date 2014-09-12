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
        public object Value { get; set; }

        /// <summary>
        /// Contains offset to AbstractData in data section (only for pointer type fields)
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Contains offset to value AbstractData in data section (only for pointer type fields).
        /// If AbstractData at <c>Offset</c> isn't PointerData then <c>>ValueOffset = Offset</c>
        /// </summary>
        public int ValueOffset { get; set; }

        public FieldInfo FieldInfo { get; private set; }

        public FieldData(FieldInfo field, object o)
        {
            FieldInfo = field;
            Value = o;

            // default incorrect offsets
            ValueOffset = 0;
            Offset = 0;
        }

        /// <summary>
        /// Returns pointer prefi string in format:
        ///     ""                      if field is value type
        ///     "@ = "                  if field's width is 4
        ///     "[length]@offset = "    if field's width is 8
        /// </summary>
        /// <returns></returns>
        public string GetTypePointerPrefix()
        {
            if (!FieldInfo.FieldType.IsPointer) return String.Empty;
            if (FieldInfo.FieldType.Width != 8) return String.Format("@{0}", Offset);

            int length;
            int offset;
            RecordFactory.GetListLengthAndOffset(this, out length, out offset);
            return String.Format("[{0}]@{1}", length, offset);
        }

        
        public override string ToString()
        {
            return String.Format("{0}{1}", GetTypePointerPrefix(), Value);
        }
    }
}
