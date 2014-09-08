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
                string type = fi.Type;
                switch (type)
                {
                    case "bool": o = inStream.ReadBoolean(); break;
                    case "byte": o = inStream.ReadByte(); break;
                    case "short": o = inStream.ReadInt16(); break;
                    case "int":
                        int i = inStream.ReadInt32();
                        if (i == -16843010) i = -1;             // Int32 -16843010 : FEFE FEFE (hex)
                        o = i;
                        break;
                    case "Int64":
                        long l = inStream.ReadInt64();
                        if (l == -72340172838076674) l = -1;             // Int64 -72340172838076674: FEFE FEFE FEFE FEFE (hex)
                        o = l;
                        break;
                    default:
                        throw new Exception("Unknown field type: " + type);
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
                string type = fi.Type;
                switch (type)
                {
                    case "bool": outStream.Write((bool)o); break;
                    case "byte": outStream.Write((byte)o); break;
                    case "short": outStream.Write((short)o); break;
                    case "int": outStream.Write((int)o); break;
                    case "Int64": outStream.Write((Int64)o); break;
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
            string type = field.Type;
            bool error = false;
            switch (type)
            {
                case "bool": if (!(value is bool)) error = true; break;
                case "byte": if (!(value is byte)) error = true; break;
                case "short": if (!(value is short)) error = true; break;
                case "int": if (!(value is int)) error = true; break;
                case "Int64": if (!(value is Int64)) error = true; break;
                default:
                    throw new Exception("Unknown field type: " + type);
            }
            if (error)
                throw new Exception("Can't save value of type " + value.GetType() + " into field of type " + field.Type);

            values[index] = value;
        }



    }
}
