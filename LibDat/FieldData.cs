using System;
using System.Collections.Generic;
using System.IO;
using LibDat.Data;
using LibDat.Types;

namespace LibDat
{
    /// <summary>
    /// contains field data:
    /// 1) field info
    /// 2) actual data read from stream
    /// Basically it's a wrapper around data at field offset
    /// </summary>
    public class FieldData
    {
        public bool x64;

        public AbstractData Data { get; private set; }

        public FieldInfo FieldInfo { get; private set; }

        public FieldData(FieldInfo fieldInfo, BinaryReader reader, bool dat64 = false)
        {
            x64 = dat64;
            FieldInfo = fieldInfo;
            var offset = reader.GetDataSectionOffset();
            var dict = new Dictionary<string, object>();
            dict["offset"] = offset;
            if (x64) Data = TypeFactory64.CreateData(fieldInfo.FieldType, reader, dict);
            else Data = TypeFactory.CreateData(fieldInfo.FieldType, reader, dict);
        }

        /// <summary>
        /// Returns pointer prefix string in format:
        ///     ""                      if field is value type
        ///     "@ = "                  if field is pointer type
        ///     "[length]@offset = "    if field is list type
        /// </summary>
        /// <returns></returns>
        public string GetOffsetPrefix()
        {
            if (!FieldInfo.IsPointer) return String.Empty;

            var pData = Data as PointerData;
            if (pData == null)
                throw new Exception("FieldData of pointer type doesn't have data of PointerData class");
            if (!x64 && FieldInfo.FieldType.Width == 4) return String.Format("@{0}", pData.RefData.Offset);
            if (x64 && FieldInfo.FieldType.Width == 8) return String.Format("@{0}", pData.RefData.Offset);

            var lData = pData.RefData as ListData;
            if (lData == null)
                throw new Exception("Didn't find ListData data at offset of FieldData of pointer to list type");
            if (!x64 && FieldInfo.FieldType.Width == 8)  return String.Format("[{0}]@{1}", lData.Count, pData.RefData.Offset);
            if (x64 && FieldInfo.FieldType.Width == 16) return String.Format("[{0}]@{1}", lData.Count, pData.RefData.Offset);

            throw new Exception("Wrong FieldType.Width");
        }
    }
}