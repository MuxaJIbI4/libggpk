using System;
using System.Collections.Generic;
using System.IO;
using LibDat.Data;

namespace LibDat
{
    /// <summary>
    /// read data of specific type from <c>reader</c>
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    public delegate object TypeReader(BinaryReader reader);

    /// <summary>
    /// save <c>o</c> to stream <c>writer</c>
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="o"></param>
    public delegate void TypeWriter(BinaryWriter writer, object o);

//    /// <summary>
//    /// converts field value to actual data
//    /// if field's type is value return field's value
//    /// if field's type is pointer returns value type found after dereferencing all pointers (including nested pointers)
//    /// </summary>
//    public delegate string FieldStringConverter(object fieldValue, IDictionary<int, AbstractData> dataEntries);

    /// <summary>
    /// For pointer type field read data at offset and add all found entries to <c>dataEntries</c>
    /// </summary>
    /// <param name="fieldData"></param>
    /// <param name="dataEntries"></param>
    public delegate void PointerReaderDelegate(
        BinaryReader reader, 
        FieldData fieldData, 
        int dataTableBegin,
        Dictionary<int, AbstractData> dataEntries);


    public struct FieldTypeInfo
    {
        private readonly string _name;
        public string Name { get { return _name; } }

        private readonly int _width;
        public int Width { get { return _width; } }

        private readonly bool _isPointer;
        public bool IsPointer { get { return _isPointer; } }

        private readonly string _pointerType;
        public string PointerType { get { return (_isPointer ? _pointerType : null); } }

        private readonly TypeReader _reader;
        public TypeReader Reader { get { return _reader; } }

        private readonly TypeWriter _writer;
        public TypeWriter Writer { get { return _writer; } }

        private readonly PointerReaderDelegate _pointerReader;
        public PointerReaderDelegate PointerReader {
            get { return _pointerReader; }
        }

        public FieldTypeInfo(string name, int width,
            TypeReader reader,
            TypeWriter writer,
            bool isPointer = false, 
            string pointerType = null, 
            PointerReaderDelegate pointerReader = null)
        {
            if (writer == null || reader == null)
                throw new ArgumentNullException("Reader/writer is null");
            _name = name;
            _width = width;
            _reader = reader;
            _writer = writer;
            _isPointer = isPointer;
            _pointerType = pointerType;
            _pointerReader = pointerReader;
        }
    }
}