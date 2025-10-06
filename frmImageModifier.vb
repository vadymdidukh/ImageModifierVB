Public Class frmImageModifier
    Dim myImage As Bitmap

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click
        Dim open As New OpenFileDialog
        open.Title = "Image Location"
        open.Filter = "JPeg Image|*.jpg|All files (*.*)|*.*"

        If open.ShowDialog() = Windows.Forms.DialogResult.OK Then
            myImage = New Bitmap(open.FileName, True)
            picOriginal.BorderStyle = BorderStyle.None
            picOriginal.Image = myImage
            picConverted.Image = Nothing
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If picConverted.Image IsNot Nothing Then
            Dim save As New SaveFileDialog
            save.Title = "Save Directory"
            save.Filter = "JPeg Image|*.jpg|All files (*.*)|*.*"

            If save.ShowDialog() = Windows.Forms.DialogResult.OK Then
                picConverted.Image.Save(save.FileName)
                MessageBox.Show("The converted image has been successfully saved!")
            End If
        Else MessageBox.Show("There is no image to be saved.", "Error")
        End If
    End Sub

    Private Sub UseMonochrome(imgToProcess As Bitmap)
        Dim r, g, b, average As Integer
        Dim y, x As Integer
        Dim newColor As Color

        For y = 0 To imgToProcess.Height - 1
            For x = 0 To imgToProcess.Width - 1
                Dim pixelColor As Color = imgToProcess.GetPixel(x, y)
                r = CInt(pixelColor.R)
                g = CInt(pixelColor.G)
                b = CInt(pixelColor.B)
                average = CInt((r + g + b) / 3)

                If average > 128 Then
                    newColor = Color.FromArgb(255, 255, 255)
                Else newColor = Color.FromArgb(0, 0, 0)
                End If

                imgToProcess.SetPixel(x, y, newColor)
            Next
        Next
    End Sub

    Private Sub UseGrayAveraging(imgToProcess As Bitmap)
        Dim r, g, b As Integer
        Dim y, x As Integer
        Dim newColor As Color
        Dim grey As Integer

        For y = 0 To imgToProcess.Height - 1
            For x = 0 To imgToProcess.Width - 1
                Dim pixelColor As Color = imgToProcess.GetPixel(x, y)
                r = CInt(pixelColor.R)
                g = CInt(pixelColor.G)
                b = CInt(pixelColor.B)

                grey = CInt((r + (g + b)) / 3)

                newColor = Color.FromArgb(grey, grey, grey)
                imgToProcess.SetPixel(x, y, newColor)
            Next
        Next
    End Sub

    Private Sub UseGrayLuma(imgToProcess As Bitmap)
        Dim r, g, b As Integer
        Dim y, x As Integer
        Dim newColor As Color
        Dim grey As Integer

        For y = 0 To imgToProcess.Height - 1
            For x = 0 To imgToProcess.Width - 1
                Dim pixelColor As Color = imgToProcess.GetPixel(x, y)
                r = CInt(pixelColor.R)
                g = CInt(pixelColor.G)
                b = CInt(pixelColor.B)

                grey = CInt(r * 0.2126 + g * 0.7152 + b * 0.0722)

                newColor = Color.FromArgb(grey, grey, grey)
                imgToProcess.SetPixel(x, y, newColor)
            Next
        Next
    End Sub

    Private Sub UseDesaturation(imgToProcess As Bitmap)
        Dim r, g, b, gray As Integer
        Dim y, x As Integer
        Dim newColor As Color

        For y = 0 To imgToProcess.Height - 1
            For x = 0 To imgToProcess.Width - 1
                Dim pixelColor As Color = imgToProcess.GetPixel(x, y)
                r = CInt(pixelColor.R)
                g = CInt(pixelColor.G)
                b = CInt(pixelColor.B)

                gray = CInt((Math.Max(r, Math.Max(g, b)) + Math.Min(r, Math.Min(g, b))) / 2)

                newColor = Color.FromArgb(gray, gray, gray)
                imgToProcess.SetPixel(x, y, newColor)
            Next
        Next
    End Sub

    Private Sub UseDecomposition(imgToProcess As Bitmap, useMax As Boolean)
        Dim r, g, b, gray As Integer
        Dim y, x As Integer
        Dim newColor As Color

        For y = 0 To imgToProcess.Height - 1
            For x = 0 To imgToProcess.Width - 1
                Dim pixelColor As Color = imgToProcess.GetPixel(x, y)
                r = CInt(pixelColor.R)
                g = CInt(pixelColor.G)
                b = CInt(pixelColor.B)

                If useMax Then
                    gray = Math.Max(r, Math.Max(g, b))
                Else
                    gray = Math.Min(r, Math.Min(g, b))
                End If

                newColor = Color.FromArgb(gray, gray, gray)
                imgToProcess.SetPixel(x, y, newColor)
            Next
        Next
    End Sub

    Private Sub UseSingleChannel(imgToProcess As Bitmap, channel As Integer)
        Dim r, g, b, gray As Integer
        Dim y, x As Integer
        Dim newColor As Color

        For y = 0 To imgToProcess.Height - 1
            For x = 0 To imgToProcess.Width - 1
                Dim pixelColor As Color = imgToProcess.GetPixel(x, y)
                r = CInt(pixelColor.R)
                g = CInt(pixelColor.G)
                b = CInt(pixelColor.B)

                Select Case channel
                    Case 0
                        gray = r
                    Case 1
                        gray = g
                    Case 2
                        gray = b
                End Select

                newColor = Color.FromArgb(gray, gray, gray)
                imgToProcess.SetPixel(x, y, newColor)
            Next
        Next
    End Sub

    Private Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click
        If picOriginal.Image IsNot Nothing Then
            If cboFilter.SelectedItem IsNot Nothing Then
                myImage = New Bitmap(picOriginal.Image)
                Select Case cboFilter.SelectedIndex
                    Case 0
                        UseMonochrome(myImage)
                    Case 1
                        UseGrayAveraging(myImage)
                    Case 2
                        UseGrayLuma(myImage)
                    Case 3
                        UseDesaturation(myImage)
                    Case 4
                        UseDecomposition(myImage, True)
                    Case 5
                        UseDecomposition(myImage, False)
                    Case 6
                        UseSingleChannel(myImage, 0)
                    Case 7
                        UseSingleChannel(myImage, 1)
                    Case 8
                        UseSingleChannel(myImage, 2)
                End Select

                picConverted.Image = myImage
                picConverted.BorderStyle = BorderStyle.None
            Else
                MessageBox.Show("Please select a filter to work with.", "Error")
            End If
        Else
            MessageBox.Show("Please select a picture to convert.", "Error")
        End If
    End Sub
End Class
