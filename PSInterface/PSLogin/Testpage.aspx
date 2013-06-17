<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Testpage.aspx.cs" Inherits="Testpage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
<%--            txtEventTarget.Text = "";
        txtEventArgument.Text = "";
        txtViewState.Text = "";
        txtEventValidation.Text = "";--%>
        Target <asp:TextBox ID="txtEventTarget" runat="server" Width="694px"></asp:TextBox><br /><br />
        Argument <asp:TextBox ID="txtEventArgument" runat="server" 
            Width="730px"></asp:TextBox><br /><br />
        ViewState: <asp:TextBox ID="txtViewState" runat="server" 
            Width="720px"></asp:TextBox><br /><br />
        EvtVal: <asp:TextBox ID="txtEventValidation" runat="server" 
            Width="734px"></asp:TextBox><br /><br />
        Resp1 :<asp:TextBox ID="txtResponse1" runat="server" Width="738px"></asp:TextBox><br /><br />
        Resp2 :<asp:TextBox ID="txtResponse2" runat="server" Width="749px"></asp:TextBox><br /><br />
        Postback: <asp:TextBox ID="txtPostBack" runat="server" Width="725px"></asp:TextBox><br /><br />
        <asp:Button ID="btnSubmit" runat="server" Text="Button" OnClick="btnSubmit_Click" />
    </div>
    </form>
</body>
</html>
