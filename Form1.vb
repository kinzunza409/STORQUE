﻿Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToParent()
        BtnCon.Enabled = False
        BtnCon.BringToFront()

        BtnDiscon.Enabled = False
        BtnDiscon.SendToBack()

        CmbBaud.SelectedItem = "9600"

        Dim aw As String
        aw = 300
        Chart1.Series("Potentiometer").Points.AddY(aw)
        Chart1.Series("Potentiometer").IsValueShownAsLabel = False
        Chart1.ChartAreas("ChartArea1").AxisX.Enabled = DataVisualization.Charting.AxisEnabled.False
    End Sub

    Private Sub BtnScanPort_Click(sender As Object, e As EventArgs) Handles BtnScanPort.Click
        CmbPort.Items.Clear()
        Dim myPort As Array
        Dim i As Integer
        myPort = IO.Ports.SerialPort.GetPortNames()
        CmbPort.Items.AddRange(myPort)
        i = CmbPort.Items.Count
        i = i - i
        Try
            CmbPort.SelectedIndex = i
        Catch ex As Exception
            Dim result As DialogResult
            result = MessageBox.Show("com port not detected", "Warning !!!", MessageBoxButtons.OK)
            CmbPort.Text = ""
            CmbPort.Items.Clear()
            Call Form1_Load(Me, e)
        End Try
        BtnCon.Enabled = True
        BtnCon.BringToFront()
        CmbPort.DroppedDown = True
    End Sub

    Private Sub BtnCon_Click(sender As Object, e As EventArgs) Handles BtnCon.Click
        BtnCon.Enabled = False
        BtnCon.SendToBack()

        SerialPort1.BaudRate = CmbBaud.SelectedItem
        SerialPort1.PortName = CmbPort.SelectedItem
        SerialPort1.Open()
        Timer1.Start()

        BtnDiscon.Enabled = True
        BtnDiscon.BringToFront()
    End Sub

    Private Sub BtnDiscon_Click(sender As Object, e As EventArgs) Handles BtnDiscon.Click
        BtnDiscon.Enabled = False
        BtnDiscon.SendToBack()

        Timer1.Stop()
        SerialPort1.Close()

        BtnCon.Enabled = True
        BtnCon.BringToFront()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            Dim i As Single = SerialPort1.ReadExisting
            LblPotentiometer.Text = "Potentiometer Value : " & i.ToString

            Chart1.Series("Potentiometer").Points.AddY(i.ToString)
            If Chart1.Series(0).Points.Count = 20 Then
                Chart1.Series(0).Points.RemoveAt(0)
            End If
            Chart1.ChartAreas(0).AxisY.Maximum = 300
        Catch ex As Exception

        End Try
    End Sub
End Class
