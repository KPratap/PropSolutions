using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SSOLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["rrsuser"] == null || Session["rrspwd"] == null)
            {
                Response.Write("Credentials are missing");
                return;
            }
            string psuser = string.Empty;
            if (Session["psuserid"] != null)
                psuser = Session["psuserid"].ToString();
            Response.Write("Logging in user " + Session["rrsuser"].ToString() + "/" + Session["rrspwd"]);

        }
    }
}