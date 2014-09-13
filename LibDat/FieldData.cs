using System;
using System.IO;
using LibDat.Data;

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
        public AbstractData Data { get; private set; }
        
        public FieldInfo FieldInfo { get; private set; }

        public FieldData(FieldInfo fieldInfo, BinaryReader reader)
        {
            FieldInfo = fieldInfo;
            Data = RecordFactory.ReadType(fieldInfo.FieldType, reader, false);
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
            if (!FieldInfo.IsPointer) return String.Empty;

            var pData = Data as PointerData;
            if (FieldInfo.FieldType.Width != 8) return String.Format("@{0}", pData.RefData.Offset);

            return String.Format("[{0}]@{1}", (pData.RefData as ListData).Count, pData.RefData.Offset);
        }
    }
}
