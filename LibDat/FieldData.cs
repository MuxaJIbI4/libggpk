using LibDat.Data;

namespace LibDat
{
    public class FieldData
    {
        /// <summary>
        /// contains underlying value or pointer data
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// for pointer fields contains pointed data section entry 
        /// </summary>
        public AbstractData Data { get; set; }

        public FieldInfo FieldInfo { get; private set; }

        public FieldData(FieldInfo field, object o)
        {
            FieldInfo = field;
            Value = o;
        }
    }
}
