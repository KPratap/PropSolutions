<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PSLogin.aspx.cs" Inherits="PSLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Rent Recovery Solutions PreLogin</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/ClientLogo.css" rel="Stylesheet" type="text/css" />
    <meta name="keywords" content="test" />
    <style type="text/css">
        .style2
        {
            width: 501px;
        }
        .style7
        {
            width: 501px;
            height: 21px;
        }
        .style8
        {
            height: 21px;
        }
        </style>
</head>
<body>
    <div style="margin:0 auto; width:130px; height:117px; background:url(http://rentrecoverysolutions.com/img/RecoveryImages/MainLogo/RRS_Logo5.png) no-repeat;"></div>
    <form id="form1" runat="server">
    <div>
        <table style="width:100%;" align="center">
            <tr style="border-style: solid; border-color: #6699FF">
                <td class="style7">
                </td>
                <td class="style8">
                </td>
            </tr>
            <tr>
                <td align="center" valign="middle" colspan="2">
                    <asp:Login ID="LogOn1" runat="server" BackColor="#F7F6F3" BorderColor="#E6E2D8" 
                        BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
                        Font-Size="0.8em" ForeColor="#333333" Height="161px" Width="316px" 
                        onauthenticate="LogOn1_Authenticate" 
                        DisplayRememberMe="False" 
                        InstructionTextStyle-CssClass="WebPortalLogin" >
                        <TextBoxStyle Font-Size="0.8em" />
                        <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" 
                            BorderWidth="1px" Font-Names="Verdana" Font-Size="1em" ForeColor="#284775" />
                        <TitleTextStyle BackColor="#6b8e00" Font-Bold="True" Font-Size="1.2em" 
                            ForeColor="White" />
                    </asp:Login>
                    <br />
                </td>
            </tr>
            <tr>
                <td></td>
                <td> <asp:Literal runat="server" ID="litMsg"></asp:Literal></td>
            </tr>
            <tr style="border-style: solid; border-color: #6699FF">
                <td class="style2">&nbsp;
                    </td>
                <td>&nbsp;
                    </td>
            </tr>
        </table>
    </div>
    </form>
    <div style="height:30px; padding:0 10px; margin:0 auto; color:#fff; line-height:30px; letter-spacing:1px; font-size:14px; background-color:#6b8e00; text-align:center;">Rent Recovery Solutions - 2814 Spring Rd, Ste 301, Atlanta, GA 30339 - Toll free: 800-335-0119</div>
</body>
</html>
