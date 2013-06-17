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
using System.Collections.Specialized;
using System.Configuration;
using PSInterface;

public partial class PSLogin : System.Web.UI.Page
{
    protected string auth_code = string.Empty;
    protected string _rrsLogin = "https://clients.rentrecoverysolutions.com/login.aspx";
    protected string _evtVal = "/wEWBALT3OHJDwLEjIzrDwLxl6vJAgKio6smqlFN7tNMibRvRf8d6gZJ+3BxEhQ=";
    protected string _viewState = "/wEPDwUJNjY1MTg3NTc3D2QWAgIDD2QWAgIBDzwrAAoBAA8WAh4PSW5zdHJ1Y3Rpb25UZXh0BYoBVXNlcm5hbWUgYW5kIHBhc3N3b3JkcyBtdXN0IGJlIGVudGVyZWQgaW4gYWxsIENBUFMuIElmIHlvdXIgYWNjb3VudCBpcyBsb2NrZWQsIHBsZWFzZSBlbWFpbCBzZXJ2aWNlQHJlbnRyZWNvdmVyeXNvbHV0aW9ucy5jb20gZm9yIGEgcmVzZXQuZGQYAQUeX19Db250cm9sc1JlcXVpcmVQb3N0QmFja0tleV9fFgEFF0xvZ09uMSRMb2dpbkltYWdlQnV0dG9uYVLuBF42aCqlpeLuQieu8Nugi3E=";
    protected string viewState = string.Empty;
    protected string evtArg = string.Empty;
    protected string evtTarget = string.Empty;
    protected string eventValidation = string.Empty;
    //
    protected string LOGIN_URL = "https://clients.rentrecoverysolutions.com/login.aspx";
    protected string HOME_URL = "https://clients.rentrecoverysolutions.com/home.aspx";
    protected string _VS = "__VIEWSTATE";
    protected string _EV = "__EVENTVALIDATION";
    protected string _valDelim = "value=\"";

    protected string _psUserId = string.Empty;
    protected UserList ul;
    protected PSUser cuser;
    protected int cuser_ix;
    protected string ssoUrl = "/SingleSignOn.aspx";

    protected string logPath; 
    protected void Page_Load(object sender, EventArgs e)
    {
        logPath = Server.MapPath("~/App_Data");
        if (!Page.IsPostBack)
        {
            Log.Write("--New Request--");
            Session["psuserid"] = null;
            if (!ValidRequest())
            {
                litMsg.Text = "Invalid Request";
                return;
            }
            string token = OAuth2.GetToken(auth_code);
            if (!token.Contains("\"message\""))   // !=string.Empty)
            {
                string _psUserId = OAuth2.GetUserInfo(token);
                if (!_psUserId.Contains("\"message\"")) // != string.Empty)
                {
                    if (CheckUserId(_psUserId))
                    {
                        //LoginUserSSO(cuser.RUser, cuser.RPwd);
                        wrapper.Visible = false;
                        LoginSingleSignOn(cuser.RUser, Helper.DecryptString(cuser.RPwd));
                        return;
                    }
                    wrapper.Visible = true;
                    ViewState["psuserid"] = _psUserId;
                    litMsg.Text = "PS UserId " + _psUserId;
                }
            }
            else
                litMsg.Text = "Error Getting Token " + token;
        }
    }
    protected bool ValidRequest()
    {
        Log.Write(Request.Url.ToString());
        string referrer = "propertysolutions";
        if (Request.QueryString["referrer"] != null)
        {
            if (Request.QueryString["referrer"] != referrer)
                return false;
        }
        if (Request.QueryString["auth_code"] != null)
        {
            auth_code = Request.QueryString["auth_code"];
            return true;
        }
        return false;
    }
   
    protected void LogOn1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        //        LoginUserSSO(LogOn1.UserName, LogOn1.Password);
        _psUserId = string.Empty;
        if (ViewState["psuserid"] == null)
        {
            Log.Write("Property Solutions user id not found for session");
        }
        else
        {
            _psUserId = ViewState["psuserid"].ToString();
            Log.Write("Found PS ID in session " + _psUserId);
        }
        if (CheckUser(LogOn1.UserName, LogOn1.Password)) // id checked out
        {
            Log.Write("Local Auth ok");
            if (_psUserId != string.Empty && !ul.USrs[cuser_ix].PSUserIds.Contains(_psUserId))
            {
                ul.USrs[cuser_ix].PSUserIds.Add(_psUserId);
                ul.Save();
            }
            wrapper.Visible = false;
            LoginSingleSignOn(cuser.RUser, Helper.DecryptString(cuser.RPwd));
        }
        else
            litMsg.Text = "User name/password do not match";
    }

    private void LoginUserSSO(string uname, string upwd)
    {
        if (CheckUser(uname, upwd))
        {
            cuser.PSUserIds.Add(_psUserId);
            ul.Save();
            Response.Write("LoginuserSSO");
            Session["rrsuser"] = uname;
            Session["rrspwd"] = upwd;
            Server.Transfer("~/SSOLogin.aspx");
        }
        else
            litMsg.Text = "Username/password invalid";
    }
    private bool CheckUser(string uname, string upwd)
    {
        bool rc = false;
        cuser = null; cuser_ix = -1;
        ul = new UserList(Server.MapPath("~/App_Data/"));
        if (ul.Load() == 0)
        {
            int uix = ul.GetUserIndex(uname);
            //Log.Write("CheckUser:uix " + uix.ToString());
            if (uix >= 0)
            {
                Log.Write(Helper.DecryptString(ul.USrs[uix].RPwd));
                if (uname == ul.USrs[uix].RUser && upwd == Helper.DecryptString(ul.USrs[uix].RPwd))
                {
                    rc = true;
                    cuser = ul.USrs[uix];
                    cuser_ix = uix;
                    Log.Write("Creds match");
                }
            }
        }
        else
            litMsg.Text = "Error loading Users";
        return rc;
    }
    private bool CheckUserId(string psuser)
    {
        bool rc = false;
        cuser = null; cuser_ix = -1;
        ul = new UserList(Server.MapPath("~/App_Data/"));
        if (ul.Load() == 0)
        {
            int uix = ul.GetUserIxForPSId(psuser);
            if (uix >= 0)
            {
                rc = true;
                cuser = ul.USrs[uix];
                cuser_ix = uix;
            }
        }
        else
            litMsg.Text = "Error loading Users";
        return rc;
    }

// CDS Suggested Code
    protected void LoginSingleSignOn(string user, string pwd)
    {
        try
        {
            Log.Write("LoginSingleSignOn:" + user);
            NameValueCollection data = new NameValueCollection();
            //string uname = Session["rrsuser"].ToString();
            byte[] encUName_byte = new byte[user.Length];
            encUName_byte = System.Text.Encoding.UTF8.GetBytes(user);
            string encodedUName = Convert.ToBase64String(encUName_byte);
            //string pwd = Session["rrspwd"].ToString();
            byte[] encPwd_byte = new byte[pwd.Length];
            encPwd_byte = System.Text.Encoding.UTF8.GetBytes(pwd);
            string encodedPwd = Convert.ToBase64String(encPwd_byte);
            data.Add("UserName", encodedUName);
            data.Add("Password", encodedPwd);
            RedirectAndPOST(this.Page, ssoUrl , data);
        }
        catch (Exception ex)
        {
        }
    }

    protected void RedirectAndPOST(Page page, string destinationUrl, NameValueCollection data)
    {
        //Prepare the Posting form
        string strForm = PreparePOSTForm(destinationUrl, data);
        //Add a literal control the specified page holding 
        //the Post Form, this is to submit the Posting form with the request.
        page.Controls.Add(new LiteralControl(strForm));
    }

        /// <param name="url">The destination Url to which the post and redirection will occur, the Url can be in the same App or outside the App.</param>
        /// <param name="data">A collection of data that will be posted to the destination Url.</param>
        /// <returns>Returns a string representation of the Posting form.</returns>
    private static String PreparePOSTForm(string url, NameValueCollection data)
    {
        //Set a name for the form
        string formID = "PostForm";
        //Build the form using the specified data to be posted.
        StringBuilder strForm = new StringBuilder();
        strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
        foreach (string key in data)
        {
            strForm.Append("<input type=\"hidden\" name=\"" + key + "\" value=\"" + data[key] + "\">");
        }
        strForm.Append("</form>");
        //Build the JavaScript which will do the Posting operation.
        StringBuilder strScript = new StringBuilder();
        strScript.Append("<script language='javascript'>");
        strScript.Append("var v" + formID + " = document." + formID + ";");
        strScript.Append("v" + formID + ".submit();");
        strScript.Append("</script>");
        //Return the form and the script concatenated. (The order is important, Form then JavaScript)
        return strForm.ToString() + strScript.ToString();
    }
    // End
    #region Futile Attempts
    private void LoginUser(string uname, string upwd)
    {
        // first, request the login form to get the viewstate value
        HttpWebRequest webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
        StreamReader responseReader = new StreamReader(
              webRequest.GetResponse().GetResponseStream()
           );
        string responseData = responseReader.ReadToEnd();
        responseReader.Close();
        /*      http://odetocode.com/articles/162.aspx - Code based on this
                http://bytes.com/topic/asp-net/answers/323587-screen-scrape-login
                Last year I had a site that would occasionaly reject my web request
                from a screen scraping program. It was in a loop moving through a
                paged result set, and I couldn't figure out the random failures. On a
                whim I put in a few Thread.Sleep calls to slow the scraper down
                between requests and it never failed. I'm not sure if they monitored
                requests by IP to only allow so many per second or minute or what,
                though it was definitely timing related.
         * */
        System.Threading.Thread.Sleep(3000);

        // extract the viewstate value and build out POST data
        string viewState = ExtractViewState(responseData);
        string eventValidation = ExtractEventValidation(responseData);
        string postData =
              String.Format(
                 "__VIEWSTATE={0}&__EVENTVALIDATION={1}&LogOn1$UserName={2}&LogOn1$Password={3}&LogOn1$LoginButton=Log In",
                 viewState, eventValidation, uname, upwd
              );

        // have a cookie container ready to receive the forms auth cookie
        CookieContainer cookies = new CookieContainer();
        // now post to the login form
        webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
        webRequest.Method = "POST";
        webRequest.ContentType = "application/x-www-form-urlencoded";
        webRequest.CookieContainer = cookies;
        // write the form values into the request message
        StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
        requestWriter.Write(postData);
        requestWriter.Close();
        // we don't need the contents of the response, just the cookie it issues
        webRequest.GetResponse().Close();

        // now we can send out cookie along with a request for the protected page
        webRequest = WebRequest.Create(HOME_URL) as HttpWebRequest;
        webRequest.CookieContainer = cookies;
        responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
        // and read the response
        responseData = responseReader.ReadToEnd();
        responseReader.Close();
        //Response.Redirect("Home.aspx");
        Response.Write(responseData);
    }
    private string ExtractViewState(string s)
    {
        string viewStateNameDelimiter = "__VIEWSTATE";
        string valueDelimiter = "value=\"";

        int viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
        int viewStateValuePosition = s.IndexOf(
              valueDelimiter, viewStateNamePosition
           );

        int viewStateStartPosition = viewStateValuePosition +
                                     valueDelimiter.Length;
        int viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

        return HttpUtility.UrlEncodeUnicode(
                 s.Substring(
                    viewStateStartPosition,
                    viewStateEndPosition - viewStateStartPosition
                 )
              );
    }
    private string ExtractEventValidation(string s)
    {
        string viewStateNameDelimiter = "__EVENTVALIDATION";
        string valueDelimiter = "value=\"";

        int viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
        int viewStateValuePosition = s.IndexOf(
              valueDelimiter, viewStateNamePosition
           );

        int viewStateStartPosition = viewStateValuePosition +
                                     valueDelimiter.Length;
        int viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

        return HttpUtility.UrlEncodeUnicode(
                 s.Substring(
                    viewStateStartPosition,
                    viewStateEndPosition - viewStateStartPosition
                 )
              );
    }
    //private string ExtractHidden(string s, string nameDelim, string valDelim)
    //{
    //    string viewStateNameDelimiter = "__VIEWSTATE";
    //    string valueDelimiter = "value=\"";

    //    int viewStateNamePosition = s.IndexOf(viewStateNameDelimiter);
    //    int viewStateValuePosition = s.IndexOf(
    //          valueDelimiter, viewStateNamePosition
    //       );

    //    int viewStateStartPosition = viewStateValuePosition +
    //                                 valueDelimiter.Length;
    //    int viewStateEndPosition = s.IndexOf("\"", viewStateStartPosition);

    //    return HttpUtility.UrlEncodeUnicode(
    //             s.Substring(
    //                viewStateStartPosition,
    //                viewStateEndPosition - viewStateStartPosition
    //             )
    //          );
    //}
    private void PostToLogin()
    {
        PostWebRequest("POST", _rrsLogin);
    }
    private void GetLoginScreen()
    {
        GetWebRequest("GET", _rrsLogin);
    }
    protected void GetWebRequest(string method, string url)
    {
        string strResponse;
        StringBuilder stringBuilder = new StringBuilder();
        // Get login page
        HttpWebResponse resp;
        HttpWebRequest http = HttpWebRequest.Create(url) as HttpWebRequest;
        try
        {
            http = (HttpWebRequest)WebRequest.Create(url);
            http.Method = "GET";

            resp = (HttpWebResponse)http.GetResponse();
            // get login response and extract hidden fields
            using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
            {
                strResponse = reader.ReadToEnd();
                string[] data = ParseHTML(strResponse);
                viewState = HttpUtility.UrlEncode(data[2]);
                evtArg = HttpUtility.UrlEncode(data[1]);
                evtTarget = HttpUtility.UrlEncode(data[0]);
                eventValidation = HttpUtility.UrlEncode(data[3]);
            }
            // 
            http = (HttpWebRequest)WebRequest.Create(url);
            http.CookieContainer = new CookieContainer();
            http.CookieContainer.Add(resp.Cookies);
            http.Method = "POST";
            http.AllowAutoRedirect = false;
            ASCIIEncoding encoding = new ASCIIEncoding();
            http.ContentType = "application/x-www-form-urlencoded";
            //http.Referer = url;
            string postData = "LogOn1$UserName=" + LogOn1.UserName +
                            "&LogOn1$Password=" + LogOn1.Password +
                            "&LogOn1$LoginButton=" + Uri.EscapeDataString("Log In") +
                            "&__EVENTVALIDATION=" +  Uri.EscapeDataString(eventValidation) +
                            "&__VIEWSTATE=" +  Uri.EscapeDataString(viewState) +
                            "&__EVENTARGUMENT=" + string.Empty +
                            "&__EVENTTARGET=" + string.Empty;

            byte[] dataBytes = encoding.GetBytes(postData);
            http.ContentLength = dataBytes.Length;
            Stream postStream = http.GetRequestStream();
            try
            {
                postStream.Write(dataBytes, 0, dataBytes.Length);
            }
            catch (Exception ex)
            {
                Response.Write(ex.StackTrace);
            }
            finally
            {
                postStream.Close();
            }
            // get login response.
            HttpWebResponse httpResponse = http.GetResponse() as HttpWebResponse;
            using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
            {
                strResponse = reader.ReadToEnd();
            }
        }
        catch (WebException wex)
        {
        }
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
	       //txtViewState.Text = temp;
	 
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
	       //txtEventValidation.Text = temp;
	 
	       temp = temp.Replace("/", "%2F");
	       temp = temp.Replace("+", "%2B");
	       temp = temp.Replace("=", "%3D");
	       data[3] = temp;
	 
	       return data;
	   }
    private byte[] GetTagValue(string strResponse, string p)
    {
        throw new NotImplementedException();
    }
    protected void PostWebRequest(string method, string url)
    {
        
        HttpWebRequest http = HttpWebRequest.Create(url) as HttpWebRequest;
        try
        {
            http.KeepAlive = true;
            http.Method = method;
            http.AllowAutoRedirect = false;    
            ASCIIEncoding encoding = new ASCIIEncoding();
            http.ContentType = "application/x-www-form-urlencoded";
            string postData = "LogOn1$UserName=" + LogOn1.UserName +
                            "&LogOn1$Password=" + LogOn1.Password +
                            "&LogOn1$LoginButton=" + "Log In" +
                            "&__EVENTVALIDATION=" + _evtVal +
                            "&__VIEWSTATE=" + _viewState +
                            "&__EVENTARGUMENT=" + string.Empty +
                            "&__EVENTTARGET=" + string.Empty;

            byte[] dataBytes = UTF8Encoding.UTF8.GetBytes(postData);
            http.ContentLength = dataBytes.Length;
            using (Stream postStream = http.GetRequestStream())
            {
                postStream.Write(dataBytes, 0, dataBytes.Length);
            }
            HttpWebResponse httpResponse = http.GetResponse() as HttpWebResponse;
        }
        catch (WebException wex)
        {
        }

    }
    #endregion
}