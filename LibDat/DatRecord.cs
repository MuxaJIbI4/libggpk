using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace LibDat
{
    public class DatRecord
    {
        public DatRecordInfo RecordInfo { get; private set; }
        private List<Object> values;
        private int fieldsCount;

        public DatRecord(DatRecordInfo ri, BinaryReader inStream)
        {
            RecordInfo = ri;
            values = new List<Object>();
            fieldsCount = ri.Fields.Count;

            foreach (DatRecordFieldInfo fi in ri.Fields)
            {
                Object o = null;
                switch (fi.FieldType)
                {
                    case FieldTypes._01bit: o = inStream.ReadBoolean(); break;
                    case FieldTypes._08bit: o = inStream.ReadByte(); break;
                    case FieldTypes._16bit: o = inStream.ReadInt16(); break;
                    case FieldTypes._32bit:
                        int i = inStream.ReadInt32();

                        // Int32 -16843010 : FEFE FEFE (hex)
                        if (i == -16843010) i = -1;
                        o = i;
                        break;
                    case FieldTypes._64bit:
                        long l = inStream.ReadInt64();

                        // Int64 -72340172838076674: FEFE FEFE FEFE FEFE (hex)
                        if (l == -72340172838076674) l = -1;
                        o = l;
                        break;
                }
                values.Add(o);
            }
        }

        /// <summary>
        /// Save this record to the specified stream. Stream position is not preserved.
        /// </summary>
        /// <param name="outStream">Stream to write contents to</param>
        public void Save(BinaryWriter outStream)
        {
            IEnumerator<Object> iter = values.GetEnumerator();
            foreach (DatRecordFieldInfo fi in RecordInfo.Fields)
            {
                iter.MoveNext();
                Object o = iter.Current;
                switch (fi.FieldType)
                {
                    case FieldTypes._01bit: outStream.Write((bool)o); break;
                    case FieldTypes._08bit: outStream.Write((byte)o); break;
                    case FieldTypes._16bit: outStream.Write((short)o); break;
                    case FieldTypes._32bit: outStream.Write((int)o); break;
                    case FieldTypes._64bit: outStream.Write((Int64)o); break;
                }
                values.Add(o);
            }
        }

        // return value of field given by its RecordFieldInfo object
        public object GetFieldValue(DatRecordFieldInfo field)
        {
            int index = RecordInfo.Fields.IndexOf(field);
            if (index == -1)
            {
                throw new Exception("Searched record for field that doesn't exist in it");
            }

            return GetFieldValue(index);
        }

        // return value of field given by its index starting from 0
        public object GetFieldValue(int index)
        {
            if (index < 0 || index >= fieldsCount)
            {
                throw new Exception("Field's index out of bounds: " + index + " not in [0," + fieldsCount + "]");
            }
            return values[index];
        }

        // saved value of field given by its RecordFieldInfo object
        public void SetFieldValue(DatRecordFieldInfo field, object value)
        {
            int index = RecordInfo.Fields.IndexOf(field);

            // test for wrong field
            if (index == -1)
                throw new Exception("Searched record for field that doesn't exist in it");

            SetFieldValue(index, value);
        }

        // saved value of field given by its RecordFieldInfo object
        public void SetFieldValue(int index, object value)
        {
            // test for wrong index
            if (index < 0 || index >= fieldsCount)
            {
                throw new Exception("Field's index out of bounds: " + index + " not in [0," + fieldsCount + "]");
            }

            // test for correct value
            DatRecordFieldInfo field = RecordInfo.Fields[index];
            bool error = false;
            switch (field.FieldType)
            {
                case FieldTypes._01bit: if (!(value is bool)) error = true; break;
                case FieldTypes._08bit: if (!(value is byte)) error = true; break;
                case FieldTypes._16bit: if (!(value is short)) error = true; break;
                case FieldTypes._32bit: if (!(value is int)) error = true; break;
                case FieldTypes._64bit: if (!(value is Int64)) error = true; break;
            }
            if (error)
                throw new Exception("Can't save value of type " + value.GetType() 
                    + " into field of type " + Enum.GetName(typeof(FieldTypes), field.FieldType));
            values[index] = value;
        }
    }
}
