Public Class Form1

    Dim dat As VisualGGPK.DatWrapper

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If (OpenFileDialog1.ShowDialog = DialogResult.OK) Then

            TextBox1.Text = OpenFileDialog1.FileName
            dat = New VisualGGPK.DatWrapper(TextBox1.Text)
            DataGridView1.DataSource = dat.Strings
            Button2.Enabled = True

        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        SaveFileDialog1.FileName = OpenFileDialog1.SafeFileName
        If (SaveFileDialog1.ShowDialog = DialogResult.OK) Then

            dat.Save(SaveFileDialog1.FileName)

        End If

    End Sub
End Class