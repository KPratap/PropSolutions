using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PSInterface;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

public partial class LoginTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void LogOn1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        LoginUserSSO(LogOn1.UserName, LogOn1.Password);
    }

    private void LoginUserSSO(string uname, string upwd)
    {
            Session["rrsuser"] = uname;
            Session["rrspwd"] = upwd;
            Server.Transfer("~/SSOLogin.aspx");
    }

}