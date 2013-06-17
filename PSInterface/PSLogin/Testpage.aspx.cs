using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

public partial class Testpage : System.Web.UI.Page
{
    protected string _rrsLogin = "https://clients.rentrecoverysolutions.com/login.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        txtEventTarget.Text = "";
        txtEventArgument.Text = "";
        txtViewState.Text = "";
        txtEventValidation.Text = "";

        string url = _rrsLogin;
        string html = "";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        html = GetResponse(url, ref request);

        txtResponse1.Text = html;

        string[] data = ParseHTML(html);

        txtEventTarget.Text = data[0];
        txtEventArgument.Text = data[1];

        txtResponse2.Text = PostRequest(url, data, ref request);
    }

    private string PostRequest(string url, string[] args, ref HttpWebRequest request)
    {
        ASCIIEncoding encoding = new ASCIIEncoding();

        string postData = "__EVENTTARGET=" + args[0] + "&__EVENTARGUMENT=" + args[1];
        postData += "&__VIEWSTATE=" + args[2] + "&__EVENTVALIDATION=" + args[3];
        postData += "&LogOn1$UserName=CLT" +
                "&LogOn1$Password=TEST";
               // "&LogOn1$LoginButton=" + Uri.EscapeDataString("Log In");

        //"&__EVENTARGUMENT=" + string.Empty +
        //"&__EVENTTARGET=" + string.Empty;
        txtPostBack.Text = postData;

        byte[] data = encoding.GetBytes(postData);

        request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;
        request.Referer = url;

        Stream newStream = request.GetRequestStream();
        // Send the data.
        try
        {
            newStream.Write(data, 0, data.Length);
            newStream.Close();
        }
        catch (Exception ex)
        {
            Response.Write(ex.StackTrace);
        }
        finally
        {
            newStream.Close();
        }

        return GetResponse(url, ref request);
    }

    private string GetResponse(string url, ref HttpWebRequest request)
    {
        StringBuilder sb = new StringBuilder();
        Stream resStream = null;
        HttpWebResponse response = null;
        byte[] buf = new byte[8192];

        try
        {
            // execute the request
            response = (HttpWebResponse)request.GetResponse();

            // we will read data via the response stream
            resStream = response.GetResponseStream();
            string tempString = null;
            int count = 0;
            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);
                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.ASCII.GetString(buf, 0, count);
                    // continue building the string
                    sb.Append(tempString);
                }
            }
            while (count > 0); // any more data to read?
        }
        catch (Exception err)
        {
            String exc = err.Message;
        }
        finally
        {
            response.Close();
            resStream.Close();
        }

        return sb.ToString();
    }

    private string[] ParseHTML(string html)
    {
        string[] data = new string[4];
        string value = "";
        string temp = "";
        Match match;

        //Set the EVENTTARGET control
        data[0] = "";

        //Set the EVENTARGUMENT, should be an empty string
        data[1] = "";

        //get the ViewState data
        Regex regex = new Regex("id=\"__VIEWSTATE\" value=\"/[a-zA-Z0-9\\W]+\"\\s/>");
        match = regex.Match(html);
        value = match.Value;
        temp = value.Remove(value.IndexOf("id"), 24);
        temp = temp.Remove(temp.LastIndexOf("\""), 4);
        txtViewState.Text = temp;

        temp = temp.Replace("/", "%2F");
        temp = temp.Replace("+", "%2B");
        temp = temp.Replace("=", "%3D");
        data[2] = temp;

        //get the EVENTVALIDATION data
        regex = new Regex("id=\"__EVENTVALIDATION\" value=\"/[a-zA-Z0-9\\W]+\"\\s/>");
        match = regex.Match(html);
        value = match.Value;
        temp = value.Remove(value.IndexOf("id"), 30);
        temp = temp.Remove(temp.LastIndexOf("\""), 4);
        txtEventValidation.Text = temp;

        temp = temp.Replace("/", "%2F");
        temp = temp.Replace("+", "%2B");
        temp = temp.Replace("=", "%3D");
        data[3] = temp;

        return data;
    }

}
