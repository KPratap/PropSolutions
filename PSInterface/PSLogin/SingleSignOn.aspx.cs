using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SingleSignOn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string name = Request.Form["UserName"];
        string pwd = Request.Form["Password"];
        Response.Write("got here: " + name + " " + pwd);

    }
}