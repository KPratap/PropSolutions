<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PSLogin.aspx.cs" Inherits="PSLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rent Recovery Solutions PreLogin</title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <link href="css/ClientLogo.css" rel="Stylesheet" type="text/css" />
    <meta name="keywords" content="test" />
    <script type="text/javascript">

        function pageLoad() {
        }
        function CheckCapsLock(e) {
            var myKeyCode = 0;
            var myShiftKey = false;
            // Internet Explorer 4+
            if (document.all) {
                myKeyCode = e.keyCode;
                myShiftKey = e.shiftKey;
                document.getElementById('<%= lblCapsLock.ClientID %>').style.display = 'none';
                // Netscape 4
            } else if (document.layers) {
                myKeyCode = e.which;
                myShiftKey = (myKeyCode == 16) ? true : false;
                document.getElementById('<%= lblCapsLock.ClientID %>').style.display = 'none';
                // Netscape 6
            } else if (document.getElementById) {
                myKeyCode = e.which;
                myShiftKey = (myKeyCode == 16) ? true : false;
                document.getElementById('<%= lblCapsLock.ClientID %>').style.display = 'none';
            }
            // Upper case letters are seen without depressing the Shift key, therefore Caps Lock is on
            if ((myKeyCode >= 65 && myKeyCode <= 90) && !myShiftKey) {
                document.getElementById('<%= lblCapsLock.ClientID %>').style.display = 'inherit';
                // Lower case letters are seen while depressing the Shift key, therefore Caps Lock is on
            } else if ((myKeyCode >= 97 && myKeyCode <= 122) && myShiftKey) {
                document.getElementById('<%= lblCapsLock.ClientID %>').style.display = 'inherit';
            }
        }
    </script>
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
        #header
        {
            margin-left: auto;
            margin-right: auto;
            width: 900px;
        }
        #right-column
        {
            margin-left: auto;
            margin-right: auto;
            width: 900px;
        }
        #footer
        {
            clear: both;
            margin: 0 auto;
            width: 900px;
        }
        #wrapper
        {
            margin-left: auto;
            margin-right: auto;
            width: 900px;
        }
        ul li a
        {
            text-decoration: none;
            color: White;
            width: 9em;
        }
        ul
        {
            list-style-type: none;
            margin: 0;
            padding: 0;
        }
        li
        {
            display: inline;
        }
        img
        {
            border: none;
        }
        .textBox
        {
            background-color:#6b8e00;
            border-style: none;
            height: 23px;
        }
        .buttonStyle
        {
            background-color: #6b8e00;
            height: 23px;
            font-family: Verdana;
            font-size: 1em;
            color: #000;
        }
    </style>
</head>
<body onkeypress="CheckCapsLock(event)">
    <div id="wrapper" runat="server">
        <div id="header">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <img src="http://rentrecoverysolutions.com/img/RecoveryImages/MainLogo/RRS_Logo5.png" alt="Rent Recovery Solutions" />
                    </td>
                    <td style="width: 140px;">
                    </td>
                    <td>
                        <h2>
                            Multi-Family Collection Specialists
                        </h2>
                    </td>
                </tr>
            </table>
            <div style="height: 5px">
            </div>
            <div style="height: 30px; padding: 0 10px; margin: 0 auto; color: #fff; line-height: 30px;
                word-spacing: 50px; font-size: 16px; background-color: #000;">
                <ul>
                    <li><a href="http://www.rentrecoverysolutions.com">Website</a></li>
                    <li><a href="http://www.rentrecoverysolutions.com/articleSets/AboutUs/philosophy.aspx">
                        AboutUs</a></li>
                    <li><a href="http://www.rentrecoverysolutions.com/articleSets/Solutions/contingencycollections.aspx">
                        Solutions</a></li>
                    <li><a href="http://www.rentrecoverysolutions.com/articleSets/Resources/industry_links.aspx">
                        Resources</a></li>
                    <li><a href="http://www.rentrecoverysolutions.com/news.aspx">News</a></li>
                    <li><a href="http://www.rentrecoverysolutions.com/articleSets/Contacts/Contacts.aspx">
                        Contacts</a></li>
                </ul>
            </div>
        </div>
        <br />
        <div id="right-column">
            <div id="container">
                <form id="form1" runat="server">
                <div>
                    <table style="width: 100%;" align="center">
                        <tr style="border-style: solid; border-color: #6699FF">
                            <td class="style7">
                            </td>
                            <td class="style8">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" valign="middle" colspan="2">
                                <div style="border: 2px solid #E1E1E1; width: 400px; margin-top: 5px;">
                                    <br />
                                    <span>Enter in ALL <strong>CAPS</strong></span>
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
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Literal runat="server" ID="litMsg"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">
                            </td>
                            <td class="style8">
                                <u>
                                    <asp:Label ID="lblCapsLock" runat="server" Style="display: none;" Text="Caps lock is On" /></u>
                            </td>
                        </tr>
                        <tr style="border-style: solid; border-color: #6699FF">
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
                </form>
            </div>
        </div>
        <div id="footer">
            <div style="height: 30px; padding: 0 10px; margin: 0 auto; color: #fff; line-height: 30px;
                letter-spacing: 1px; font-size: 14px; background-color: #6b8e00; text-align: center;">
                Rent Recovery Solutions - 2814 Spring Rd, Ste 301, Atlanta, GA 30339 - Toll free:
                800-335-0119</div>
        </div>
    </div>
</body>
</html>
