Imports System.IO
Imports CefSharp
Imports CefSharp.WinForms

Public Class Form1
    Dim WithEvents browser As ChromiumWebBrowser
    Dim qator As Integer
    Dim qatori As Integer

    Dim KerakQatorlar(1000) As String
    Dim textSource, allText, lines(), KerakLines(), AvariaLines(500) As String
    Dim Otuda, Dosuda As Integer

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        RichTextBox1.Text = ""
        RichTextBox2.Text = ""
        qatori = 0
    End Sub

    Public Sub New()

        ' Этот вызов является обязательным для конструктора.
        InitializeComponent()

        ' Добавить код инициализации после вызова InitializeComponent().

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim setting As New CefSettings
        setting.RemoteDebuggingPort = 8088
        setting.IgnoreCertificateErrors = True
        setting.JavascriptFlags = True






        CefSharp.Cef.Initialize(setting)
        'browser = New ChromiumWebBrowser("http://google.com") With {
        '.Dock = DockStyle.Fill
        '}

        browser = New ChromiumWebBrowser("https://172.30.45.231:31943/") With {
        .Dock = DockStyle.Fill
        }
        'browser.SetZoomLevel(-2.0)
        Panel1.Controls.Add(browser)





    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        getSource()
        'Dim text = My.Computer.FileSystem.ReadAllText("новый 1.txt")
        textSource = My.Computer.FileSystem.ReadAllText("currentSource.txt")
        'RichTextBox1.Text = text
        'textSource = getSource()
        qator = 1
        lines = Split(textSource, "<tr")  'IO.File.ReadLines("c:\111.txt").ToArray
        For i = 0 To lines.Length - 1
            RichTextBox1.Text = lines(i)
            If lines(i).Contains("NodeB Name") Or lines(i).Contains("Site Name") Then
                KerakQatorlar(qator) = lines(i)
                qator = qator + 1
                'RichTextBox2.Text = RichTextBox2.Text & vbCrLf & qator
            End If
        Next
        'MsgBox(qator)
        For q = 1 To qator - 1
            allText = allText & KerakQatorlar(q)
        Next
        KerakLines = Split(allText, "<tr")

        'MsgBox(KerakLines.Length)
        qatori = 0
        For w = 0 To KerakLines.Length - 1
            'If KerakLines(w).Contains("OML Fault") Or KerakLines(w).Contains("NodeB Unavailable") Then
            If KerakLines(w).Contains("Site Name=") Or KerakLines(w).Contains("NodeB Name") Then
                qatori = qatori + 1
                If InStr(KerakLines(w), "Site Name=") Then
                    Otuda = InStr(KerakLines(w), "Site Name=") + 10
                    Dosuda = InStr(Otuda, KerakLines(w), ",")
                ElseIf InStr(KerakLines(w), "NodeB Name=") Then
                    Otuda = InStr(KerakLines(w), "NodeB Name=") + 11
                    Dosuda = InStr(Otuda, KerakLines(w), Chr(34))
                End If

                'qatori = qatori + 1
                AvariaLines(qatori) = Mid(KerakLines(w), Otuda, Dosuda - Otuda)
                'If AvariaLines(w).Contains("tabindex") Then
                'AvariaLines(w) = Mid(AvariaLines(w), 1, InStr(AvariaLines(w), "tabindex") - 1)
                'End If

                'RichTextBox2.Text = RichTextBox2.Text & vbCrLf & AvariaLines(w)
                'RichTextBox2.Text = RichTextBox2.Text & vbCrLf & Mid(KerakLines(w), Otuda, Dosuda - Otuda)
            End If
        Next
        For r = 1 To qatori
            If AvariaLines(r).Contains("tabindex") Then
                AvariaLines(r) = Mid(AvariaLines(r), 1, InStr(AvariaLines(r), "tabindex") - 3)
            End If
            RichTextBox2.Text = RichTextBox2.Text & vbCrLf & AvariaLines(r)

        Next


        MsgBox(qatori)
    End Sub

    Private Async Function getSource() As Task
        'Dim source As String = Await browser.GetBrowser().MainFrame.GetSourceAsync()
        Dim source As String = Await browser.GetBrowser.GetSourceAsync()
        Dim f As String = Application.StartupPath + "/currentSource.txt"
        Dim wr As StreamWriter = New StreamWriter(f, False, System.Text.Encoding.Default)
        wr.Write(source)
        wr.Close()
        System.Diagnostics.Process.Start(f)
        RichTextBox1.Text = source




    End Function

End Class
