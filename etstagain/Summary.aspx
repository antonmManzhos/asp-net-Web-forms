<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="TestAspNet45.Summary" %>
<%@ Import Namespace="etstagain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <h2>Приглашения</h2>

    <h3>Люди которые были приглашены: </h3>
    <table>
        <thead>
            <tr>
                <th>Имя</th>
                <th>Email</th>
                <th>Телефон</th>
            </tr>
        </thead>
        <tbody>
                                <asp:ListView ID = "ProductDataList" runat="server" HorizontalAlign="Justify"
                        EnableViewState="true" CellPadding="10" RepeatLayout="Table" RepeatDirection="Horizontal">
                        <ItemTemplate>
                            <h1>Hello World!!!</h1>
                            </ItemTemplate>
                            </asp:ListView>

<%--            <% var yesData = ResponseRepository.GetRepository().GetAllResponses().Where(r => r.WillAttend.Value);
                foreach (var rsvp in yesData) {
                    string htmlString = String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td>", rsvp.Name, rsvp.Email, rsvp.Phone);
                    Response.Write(htmlString);
                } %>--%>
        </tbody>
    </table>
</body>
</html>