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
using System.Web.Security;

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
    public string RedirTo = "/Home.aspx";
    public string isLoggedIn = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!ValidRequest())
            {
                //Response.Write("Invalid Request");
                litMsg.Text = "Invalid Request";
                return;
            }
            litMsg.Text += "<br/>QS:" + Request.Url.Query;
            litMsg.Text += "<br/>AuthCode:" + auth_code;
            string token = OAuth2.GetToken(auth_code);
            if (token !=string.Empty)
            {
                //Response.Write("Unable to retrieve token");
                litMsg.Text +=  "<br/>Token:" +  token; // "Unable to retrieve token";
                string userid = OAuth2.GetUserInfo(token);
                litMsg.Text += "<br/>UserId:" + userid; // "Unable to retrieve userid";
                //if (userid.StartsWith("{") && userid.EndsWith("}"))
                //{
                //    //Response.Write("Unable to retrieve userid");
                //    litMsg.Text += "<br/>" + userid; // "Unable to retrieve userid";
                //    return;
                //}
            }
            //LogOn1.UserName = "CLT";
            //LogOn1.Password = "TEST";
        }
    }
    protected bool ValidRequest()
    {
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
    //protected void UploadButton_Click(object sender, EventArgs e)
    //{
    //    if (FileUploadControl.HasFile)
    //    {
    //        try
    //        {
    //            string filename = Path.GetFileName(FileUploadControl.FileName);
    //            FileUploadControl.SaveAs(Server.MapPath("~/App_Data/") + filename);
    //            StatusLabel.Text = "Upload status: File uploaded!";
    //        }
    //        catch (Exception ex)
    //        {
    //            StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
    //        }
    //    }
    //}
    protected void LogOn1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        LoginUserSSO(LogOn1.UserName, LogOn1.Password);
 
    }
    private void LoginUserNew(string uname, string upwd)
    {
        Response.Write("Loginusernew");
        if (uname == "CLT" && upwd == "TEST")
        {
            FormsAuthentication.SetAuthCookie(uname, true); //AutoLoginUserName is the user name you want to be loged in when this feature is enabled
            Server.Transfer("~/Home.aspx");
        }
    }

    private void LoginUserSSO(string uname, string upwd)
    {
        Response.Write("LoginuserSSO");
        Session["rrsuser"] = uname;
        Session["rrspwd"] = upwd;
        Server.Transfer("~/SSOLogin.aspx");
    }
    private void LoginUser(string uname, string upwd)
    {
        isLoggedIn = "0";
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

        // Read the response to thelogin post and load it to the page - does not work right
        //HttpWebResponse r = (HttpWebResponse) webRequest.GetResponse();
        responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
        responseData = responseReader.ReadToEnd();
        responseReader.Close();
        if (responseData.Contains("<title>CollectOne Web Portal Home Page</title>"))
            isLoggedIn = "1";

        // we don't need the contents of the response, just the cookie it issues
        //webRequest.GetResponse().Close();
        
        // now we can send out cookie along with a request for the protected page
        webRequest = WebRequest.Create(HOME_URL) as HttpWebRequest;
        webRequest.CookieContainer = cookies;
        responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());
        // and read the response
        responseData = responseReader.ReadToEnd();
        responseReader.Close();
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
    private string ExtractHidden(string s, string nameDelim, string valDelim)
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
            resp.Close();
            http.Method = "POST";
            ASCIIEncoding encoding = new ASCIIEncoding();
            http.ContentType = "application/x-www-form-urlencoded";
            //http.Referer = url;
            string postData = "LogOn1$UserName=" + LogOn1.UserName +
                            "&LogOn1$Password=" + LogOn1.Password +
                            "&LogOn1$LoginButton=" + "Log In" +
                            "&__EVENTVALIDATION=" +  eventValidation +
                            "&__VIEWSTATE=" +  viewState  +
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
            using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
            {
                strResponse = reader.ReadToEnd();
            }
            httpResponse.Close();
            //StatusLabel.Text = strResponse;
        }
        catch (WebException wex)
        {
            //StatusLabel.Text = wex.ToString();
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
            //StatusLabel.Text = httpResponse.ToString();
            // Probably want to inspect the http.Headers here first
            //http = WebRequest.Create(url2) as HttpWebRequest;
            //http.CookieContainer = new CookieContainer();
            //http.CookieContainer.Add(httpResponse.Cookies);
            //HttpWebResponse httpResponse2 = http.GetResponse() as HttpWebResponse;
        }
        catch (WebException wex)
        {
            //StatusLabel.Text = wex.ToString();
        }

    }
}