using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;

public partial class Test2 : System.Web.UI.Page
{
    protected string LOGIN_URL = "https://clients.rentrecoverysolutions.com/login.aspx";
    protected string _VS = "__VIEWSTATE";
    protected string _EV = "__EVENTVALIDATION";
    protected string _valDelim = "value=\""; 

    protected void Page_Load(object sender, EventArgs e)
    {
        // first, request the login form to get the viewstate value
        HttpWebRequest webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
        StreamReader responseReader = new StreamReader(
              webRequest.GetResponse().GetResponseStream()
           );
        string responseData = responseReader.ReadToEnd();
        responseReader.Close();
        System.Threading.Thread.Sleep(3000);
        // extract the viewstate value and build out POST data
        string viewState = ExtractViewState(responseData);
        string eventValidation = ExtractEventValidation(responseData);
        string postData =
              String.Format(
                 "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE={0}&__EVENTVALIDATION={1}&LogOn1$UserName={2}&LogOn1$Password={3}&LogOn1$LoginButton=Log In",
                 viewState,eventValidation, "CLT", "TEST"
              );

        // have a cookie container ready to receive the forms auth cookie
        CookieContainer cookies = new CookieContainer();
        // now post to the login form
        webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
        webRequest.Method = "POST";
        webRequest.ContentType = "application/x-www-form-urlencoded";
        webRequest.CookieContainer = cookies;
        webRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31";
        webRequest.KeepAlive = false;
        webRequest.Referer = LOGIN_URL;
        webRequest.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
        webRequest.Headers.Add("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
        webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
        // write the form values into the request message;
        StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
        requestWriter.Write(postData);
        requestWriter.Close();
        // we don't need the contents of the response, just the cookie it issues
        webRequest.GetResponse().Close();

        // now we can send out cookie along with a request for the protected page
        webRequest = WebRequest.Create("https://clients.rentrecoverysolutions.com/Home.aspx") as HttpWebRequest;
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

}