<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PSUpload.aspx.cs" Inherits="PSUpload" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head2" runat="server">
    <title>Rent Recovery Solutions File Upload</title>
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
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
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
                <div>
                    <table style="width: 100%;" align="center">
                        <tr style="border-style: none; border-color: #6699FF">
                            <td class="style7">
                            </td>
                            <td class="style8">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <asp:FileUpload id="FileUploadControl" runat="server" Width="242px" size="60" /><br /><br />
                                    <asp:Button runat="server" id="Button1" text="Upload File" onclick="UploadButton_Click"  Visible="true"/>
                                    <br /><br />
                                    <asp:Label runat="server" id="StatusLabel" text="" Visible="true" />
                                </div>

                                <div>

                                    <asp:GridView ID="gvUsers" runat="server" CellPadding="4" 
                                        EnableModelValidation="True" ForeColor="#333333" GridLines="None" OnRowDataBound="gvUsers_RowDataBound" >
                                        <AlternatingRowStyle BackColor="White" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <EmptyDataTemplate>There are currently no users uploaded</EmptyDataTemplate>
                                    </asp:GridView>
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
            </div>
        </div>
        <div id="footer">
            <div style="height: 30px; padding: 0 10px; margin: 0 auto; color: #fff; line-height: 30px;
                letter-spacing: 1px; font-size: 14px; background-color: #6b8e00; text-align: center;">
                Rent Recovery Solutions - 2814 Spring Rd, Ste 301, Atlanta, GA 30339 - Toll free:
                800-335-0119</div>
        </div>
    </div>


        </div>
    </form>
</body>
</html>
