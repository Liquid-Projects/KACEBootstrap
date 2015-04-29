Public Class _Default3
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblQueueTitle.Text = "China CraneBrain Service Desk"
    End Sub

    Private Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If (String.IsNullOrEmpty(e.Row.Cells(3).Text) <> True) OrElse (e.Row.Cells(3).Text <> "&nbsp;") Then
                Dim spanPriority As Label = CType(e.Row.Cells(3).FindControl("Label1"), Label)
                If LCase(spanPriority.Text) = LCase("High") Then
                    spanPriority.CssClass = "label label-danger"
                ElseIf LCase(spanPriority.Text) = LCase("Medium") Then
                    spanPriority.CssClass = "label label-warning"
                Else
                    spanPriority.CssClass = "label label-default"
                End If
            End If
        End If
    End Sub
End Class