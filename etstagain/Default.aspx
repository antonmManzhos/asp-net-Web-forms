<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="etstagain.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="rsvpform" runat="server">
     <asp:Button runat="server" ID="myButton" Text="hello" />
    </form>


     <asp:Panel ID="PnlSpEvent" runat="server">
             <asp:DataList ID="ProductDataList" runat="server" Width="100%" HorizontalAlign="Justify" RepeatColumns="4"
                    EnableViewState="true" CellPadding="10" RepeatLayout="Table" RepeatDirection="Horizontal"
                    BorderColor="White">
                 <ItemTemplate>
                     </ItemTemplate>
                     </asp:DataList>
         </asp:Panel>
    <div>
    <%= getHtmlTires()
         %>
        </div>
</body>
</html>