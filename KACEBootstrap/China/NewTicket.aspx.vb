Imports System.Net.Sockets
Imports System.Net.Mail

Public Class _default2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Function TestForServer(address As String, port As Integer) As Boolean
        Dim timeout As Integer = 100
        If ConfigurationManager.AppSettings("RemoteTestTimeout") IsNot Nothing Then
            timeout = Integer.Parse(ConfigurationManager.AppSettings("RemoteTestTimeout"))
        End If
        Dim result = False
        Try
            Using socket = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                Dim asyncResult As IAsyncResult = socket.BeginConnect(address, port, Nothing, Nothing)
                result = asyncResult.AsyncWaitHandle.WaitOne(timeout, True)
                socket.Close()
            End Using
            Return result
        Catch
            Return False
        End Try
    End Function

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim mailMessage As New Net.Mail.MailMessage
        Dim smtpClient As New Net.Mail.SmtpClient

        Dim strToken As String = ""

        smtpClient.Host = "mail.gorbel.com"

        mailMessage.To.Add("cbsupport@gorbel.com")
        mailMessage.Subject = text_Issue_Title.Text
        mailMessage.From = New MailAddress(text_from_email.Text)
        mailMessage.IsBodyHtml = False

        strToken = "@submitter=" + text_from_email.Text + vbNewLine + "@category=" + DropDownList1.SelectedValue + vbNewLine + text_Issue_description.Text

        mailMessage.Body = strToken

        If FileUpload1.HasFile Then
            mailMessage.Attachments.Add(New Net.Mail.Attachment(FileUpload1.PostedFile.ToString))
        End If

        MsgBox(strToken)

        'smtpClient.Send(mailMessage)
    End Sub
End Class