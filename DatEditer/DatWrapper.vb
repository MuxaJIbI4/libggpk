Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports LibDat
Imports LibDat.Data

Namespace VisualGGPK
    Public Class DatString
        Private _data As AbstractData

        Public ReadOnly Property DataType As String
            Get
                Return _data.GetType().ToString().Replace("LibDat.Data.", "")
            End Get
        End Property

        Public ReadOnly Property OffsetStart As Integer
            Get
                Return _data.Offset
            End Get
        End Property

        Public ReadOnly Property OffsetEnd As Integer
            Get
                Return _data.Offset + _data.Length
            End Get
        End Property

        Public ReadOnly Property Length As Integer
            Get
                Return _data.Length
            End Get
        End Property

        Public ReadOnly Property IsUser As Boolean

        Public Property NewValue As String
            Get
                Dim str = TryCast(_data, StringData)
                If Not IsNothing(str) Then
                    Return str.NewValue
                Else
                    Return ""
                End If
            End Get
            Set(value As String)
                DirectCast(_data, StringData).NewValue = value
            End Set
        End Property

        Public ReadOnly Property Value As String
            Get
                Return _data.GetValueString()
            End Get
        End Property

        Public Sub New(Data As AbstractData, isUser As Boolean)
            _data = Data
            Me.IsUser = isUser
        End Sub
    End Class

    Public Class DatWrapper

        Private _datName As String
        Private _dat As DatContainer

        Public ReadOnly Property RecordInfo As RecordInfo
            Get
                Return _dat.RecordInfo
            End Get
        End Property

        Public ReadOnly Property DataSectionffset As Long
            Get
                Return DatContainer.DataSectionOffset
            End Get
        End Property

        Public ReadOnly Property DataSectionDataLength As Long
            Get
                Return _dat.DataSectionDataLength
            End Get
        End Property

        Public ReadOnly Property Records As List(Of RecordData)
            Get
                Return _dat.Records
            End Get
        End Property

        Public ReadOnly Property DataEntries As Dictionary(Of Integer, AbstractData)
            Get
                Return DatContainer.DataEntries
            End Get
        End Property

        Private _dataStrings As New List(Of DatString)
        Public ReadOnly Property Strings As List(Of DatString)
            Get
                Return _dataStrings
            End Get
        End Property

        Public Sub New(fileName As String)
            _datName = Path.GetFileNameWithoutExtension(fileName)
            Dim fileBytes = File.ReadAllBytes(fileName)
            ParseDatFile(New MemoryStream(fileBytes))
        End Sub

        Public Sub New(inStream As Stream, fileName As String)
            _datName = Path.GetFileNameWithoutExtension(fileName)
            ParseDatFile(inStream)
        End Sub

        Private Sub ParseDatFile(inStream As Stream)
            Try
                _dat = New DatContainer(inStream, _datName)
                Dim containerData = DataEntries.ToList()
                Dim userStringOffsets = _dat.GetUserStringOffsets()
                For Each keyValuePair In containerData
                    Dim data = keyValuePair.Value
                    Dim isUser = userStringOffsets.Contains(keyValuePair.Key)
                    Strings.Add(New DatString(data, isUser))
                Next
            Catch ex As Exception
                Throw New Exception(String.Format("Failed to read dat: {0}", ex.Message), ex)
            End Try
        End Sub

        Public Sub Save(savePath As String)
            Try
                _dat.Save(savePath)
            Catch ex As Exception
                Dim errorString As New StringBuilder()
                Dim temp = ex
                While (temp IsNot Nothing)
                    errorString.AppendLine(temp.Message)
                    temp = temp.InnerException
                End While
                MessageBox.Show(String.Format("Failed to save: {0}", errorString), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            MessageBox.Show(String.Format("Saved '{0}'", savePath), "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        Public Function GetCSV() As String
            Return _dat.GetCsvFormat()
        End Function

    End Class
End Namespace