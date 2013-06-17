using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using FileHelpers;
using System.Security.Cryptography;



namespace PSInterface
{
    public class OAuth2
    {
        private static string _apiurl = "https://sync.propertysolutions.com/api/oauth";
        private static string _clientId = "dde8541e13ec4ed241f9.rent_rec";  // Supplied by property Solutions
        private static string _clientSecret = "57102b11720fd1c7cb9b08cd50e6374e"; // Supplied by property Solutions


        public static string APIUrl
        {
            get {return _apiurl;}
            set {_apiurl = value;}
        }
        public static string ClientId
        {
            get {return _clientId;}
            set {_clientId = value;}
        }
        public static string ClientSecret
        {
            get {return _clientSecret;}
            set {_clientSecret = value;}
        }

        public static string GetToken(string authCode)
        {
            string ret = string.Empty;
            string body = BuildJson_TokenReq(authCode, ClientId, ClientSecret);
            Log.Write("GetToken Body:" + body);
            try
            {
                ret = APIRequest(_apiurl, body);
                JObject json = JObject.Parse(ret);
                if (json["code"] != null)
                    return ret;

                if (json["response"]["error"] != null)
                {
                    ret = json["response"]["error"].ToString();
                }

                if (json["response"] != null && json["response"]["result"] != null)
                {
                    ret = json["response"]["result"]["access_token"].ToString();
                }
                return ret;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }

        }
        public static string GetUserInfo(string token)
        {
            string ret = string.Empty;
            string body = BuildJson_GetUser();
            Log.Write("GetUserInfo Body:" + body);

            try
            {
                ret = APIRequest(_apiurl, body, token);
                JObject json = JObject.Parse(ret);
                if (json["code"] != null)
                    return ret;

                if (json["response"]["error"] != null)
                {
                    ret = json["response"]["error"].ToString();
                }

                if (json["response"] != null && json["response"]["result"] != null)
                {
                    ret = json["response"]["result"]["user_id"].ToString();
                }
            }
            catch
            {
                throw new Exception("Get user info token " + token);
            }

            return ret;
        }
        private static string APIRequest(string url, string body, string token = "")
        {
            string resp = string.Empty;
            byte[] bodyByte = System.Text.Encoding.UTF8.GetBytes(body);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.Method = "POST";
            req.ContentLength = bodyByte.Length;
            req.ContentType = "application/json";
            if (token != string.Empty)
                req.Headers.Add("Authorization", "Bearer " + token);
            if (bodyByte.Length > 0)
            {
                using (Stream stream = req.GetRequestStream())
                    stream.Write(bodyByte, 0, bodyByte.Length);

                try
                {
                    HttpWebResponse webResp = (HttpWebResponse)req.GetResponse();
                    Stream respStream = webResp.GetResponseStream();
                    StreamReader sr = new StreamReader(respStream);
                    resp = sr.ReadToEnd();
                    webResp.Close();
                    Log.Write("Req:" + url);
                    Log.Write("Resp:" + resp);
                    return resp;

                }
                catch (WebException ex)
                {
                    HttpStatusCode scode = ((HttpWebResponse)ex.Response).StatusCode;
                    resp = "Status Code " + scode + "\r\n" + ex.ToString();
                    return resp;
                }
            }
            else Log.Write("Body is empty " + url);
            return resp;
        }
        private static string BuildJson_TokenReq(string authcode, string clientId, string clientSecret)
        {
            string ret = 
                  "{" +
                  "\"auth\": {" +
                  "\"type\": \"oauth\"," +
                  "\"code\":" + "\"" + authcode + "\"" +"," +
                  "\"grant_type\": \"authorization_code\"," +
                  "\"client_id\":" + "\"" + clientId +  "\"" +
                  ",\"client_secret\":" +  "\"" + ClientSecret + "\"" +
                  "},"  +
                  "\"method\": { " +
                  "\"name\": \"getAccessToken\"," +
                  "\"params\": {" +
                   "}" +
                  "}" +
                "}"; 
            return ret;
        }
        private static string BuildJson_GetUser()
        {

            string ret = 
                  "{" +
                  "\"auth\":"  +
                  "{\"type\": \"oauth\"},\"method\": {\"name\": \"getUserInfo\"," +
                  "\"params\": {}" +
                  "}" +
                "}"; 
        return ret;
        }
    }
    public class APIResp
    {
        public Result Resp;
    }
    public class Result
    {
        public string accessToken;
        public decimal expiresIn;
        public string tokenType;
        public string Scope;
    }
    public class UserData
    {
        private static string passkey = "AAAAB3NzaC1yc2EAAAABIwAAAQEAv01HbPkVdQ1H/m0qkT4rpDk84pzVdGIbAX+r5C9RYrjrfNZtAoYFQ3cSBciuAd8RTIvm5zWYYnyXwc5c8YGxgmnVKsPcQS+1iUW1EutBtm2v6+5GckLTvo84h8bqDSxMX0EHOsr0l3Dzf0uIzSGChhADcenFv79y7f76MF9phi2NYDXs5urdfAomXQWlZzdkTWdx6XN4JldPvXuQAEIeIf411ZWaCnjri+u3ZP/3BAOEgjhvZvoozurrtF9KdpcFDaaG9dPzC4o8j1ZTc4rbonKQRUwi3zM85zA1oDwRCQHW39qwHLsVAmENaaPPbBYQ3jxi3uIj2pJ7Q3YcCKUklQ";

        private string psUserID { get; set; }
        private string psToken { get; set; }
        private string rrsUserId { get; set; }
        private string rrsPassword { get; set; }
        private DateTime psAuthDate { get; set; }
        
    }

    [DelimitedRecord(",")] 
    public class RRSUser
    {
	    public string User; 
	    public string Pwd; 
    }
    public class PSUser
    {
        private string _RUser;
        private string _RPwd;
        private List<string> _PSUserIds;
        public string RUser
        {
            get
            {return _RUser;}
            set
            {_RUser = value;}
        }
        public string RPwd
        {
            get {return _RPwd;}
            set {_RPwd = value;}
        }
        public List<string> PSUserIds
        {
            get {return _PSUserIds;}
            set {_PSUserIds = value;}
        }
        public string PSUserIdsList
        {
            get {
                if (_PSUserIds == null || _PSUserIds.Count == 0)
                    return "No Property Solutions Ids";
                else
                    return String.Join(",", _PSUserIds.ToArray()).TrimEnd(',');
                }
        }

    }
    public class UserList
    {
        public List<PSUser> USrs;
        private string _uListFile = "userlist.xml";
        private string _path;
        private string _ulFullfile;
        public UserList(string path)
        {
            _path = path;
            if (!_path.EndsWith(@"\")) _path = _path + @"\";
            _ulFullfile = _path + _uListFile;
            
        }
        public int Load()
        {
            int rc = 0;
            try
            {
                if (!File.Exists(_ulFullfile))
                {
                    USrs = new List<PSUser>();
                    this.Save();
                }
                XmlSerializer serializer = new XmlSerializer(typeof(List<PSUser>));
                using (TextReader reader = new StreamReader(_ulFullfile))
                {
                    USrs = serializer.Deserialize(reader) as List<PSUser>;
                }
            }
            catch (Exception ex)
            {
                DumpException(ex);
                rc = -1;
            }
            return rc;
        }
        public int Save()
        {
            int rc = 0;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<PSUser>));
                using (TextWriter writer = new StreamWriter(_ulFullfile))
                {
                    serializer.Serialize(writer, USrs);
                }
            }
            catch (Exception ex)
            {
                DumpException(ex);
                rc = -1;
            }
            return rc;
        }

        public PSUser GetUserForPSId(string psid)
        {
            foreach (PSUser u in USrs)
            {
                if (u.PSUserIds.Contains(psid)) return u;
            }
            return null;
        }
        public int GetUserIxForPSId(string userId)
        {
            int x = -1;
            for (int i = 0; i < USrs.Count; i++)
                if (USrs[i].PSUserIds.Contains(userId)) return i;
            return x;
        }
        public int GetUserIndex(string username)
        {
            int x = -1;
            for (int i = 0; i < USrs.Count; i++)
                if (USrs[i].RUser == username) return i;
            return x;
        }
        public int AddUser(PSUser u)
        {
            int rc = -1;
            if (u == null) return rc;
            if (u.RUser == null || u.RUser == string.Empty) return rc;

            int uix = GetUserIndex(u.RUser);
            if (uix >= 0) return rc;

            if (uix < 0) // not found
            {
                PSUser n = new PSUser();
                string encPwd = Helper.EncryptString(u.RPwd);
                n.RUser = u.RUser;
                n.RPwd = encPwd; // u.RPwd;
                n.PSUserIds = u.PSUserIds;
                USrs.Add(n);
                rc = 0;
            }
            return rc;
        }
        public int UpdateUser(PSUser u)
        {
            int rc = -1;
            if (u == null) return rc;
            if (u.RUser == null || u.RUser == string.Empty) return rc;

            int uix = GetUserIndex(u.RUser);
            if (uix < 0) return rc;

            if (uix >= 0) //  found
            {
                string encPwd = Helper.EncryptString(u.RPwd);
                USrs[uix].RPwd = encPwd; // u.RPwd;
                rc = 0;
            }
            return rc;
        }
        public int DeleteUser(PSUser u)
        {
            int rc = -1;
            if (u == null) return rc;
            if (u.RUser == null || u.RUser == string.Empty) return rc;

            int uix = GetUserIndex(u.RUser);
            if (uix < 0) return rc;

            if (uix >= 0) //  found
            {
                USrs.RemoveAt(uix); 
                rc = 0;
            }
            return rc;
        }
        public static void DumpException(Exception ex)
        {
            //Console.WriteLine("--------- Outer Exception Data ---------");
            WriteExceptionInfo(ex);
            ex = ex.InnerException;
            if (null != ex)
            {
                //  Console.WriteLine("--------- Inner Exception Data ---------");
                WriteExceptionInfo(ex);
                ex = ex.InnerException;
            }
        }
        public static void WriteExceptionInfo(Exception ex)
        {
            //Console.WriteLine("Message: {0}", ex.Message);
            //Console.WriteLine("Exception Type: {0}", ex.GetType().FullName);
            //Console.WriteLine("Source: {0}", ex.Source);
            //Console.WriteLine("StrackTrace: {0}", ex.StackTrace);
            //Console.WriteLine("TargetSite: {0}", ex.TargetSite);
        }
    }
    public static class Log
    {
        private static string fpath = System.Web.HttpContext.Current.Server.MapPath("~/App_data/");
        public static void Write(string errorMessage)
        {
            try
            {
                string path = fpath + DateTime.Today.ToString("MM-dd-yyyy") + ".log";
                if (!File.Exists(path))
                {
                    File.Create(path).Close();
                    DeleteOld();
                }
                using (StreamWriter w = File.AppendText((path)))
                {
                    w.WriteLine("{0}", DateTime.Now.ToString() + " " + errorMessage);
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void DeleteOld()
        {
            string prev;
            string path = fpath + DateTime.Today.ToString("MM-dd-yyyy") + ".log";
            for (int i = 3; i < 5; i++)
            {
                prev = fpath + DateTime.Today.AddDays(-i).ToString("MM-dd-yyyy") + ".log";
                if (File.Exists(prev))
                {
                    File.Delete(prev);
                    Log.Write("Deleted " + prev);
                }
            }
        }
    }
    public class Helper
    {
        private static string passkey = "AAAAB3NzaC1yc2EAAAABIwAAAQEAv01HbPkVdQ1H/m0qkT4rpDk84pzVdGIbAX+r5C9RYrjrfNZtAoYFQ3cSBciuAd8RTIvm5zWYYnyXwc5c8YGxgmnVKsPcQS+1iUW1EutBtm2v6+5GckLTvo84h8bqDSxMX0EHOsr0l3Dzf0uIzSGChhADcenFv79y7f76MF9phi2NYDXs5urdfAomXQWlZzdkTWdx6XN4JldPvXuQAEIeIf411ZWaCnjri+u3ZP/3BAOEgjhvZvoozurrtF9KdpcFDaaG9dPzC4o8j1ZTc4rbonKQRUwi3zM85zA1oDwRCQHW39qwHLsVAmENaaPPbBYQ3jxi3uIj2pJ7Q3YcCKUklQ";

        public static string EncryptString(string InputText)
        {
            // "Password" string variable is nothing but the key(your secret key) value which is sent from the front end.
            // "InputText" string variable is the actual password sent from the login page.
            // We are now going to create an instance of the
            // Rihndael class.
            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            // First we need to turn the input strings into a byte array.
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
            // We are using Salt to make it harder to guess our key
            // using a dictionary attack.
            byte[] Salt = Encoding.ASCII.GetBytes(passkey.Length.ToString());
            // The (Secret Key) will be generated from the specified
            // password and Salt.
            //PasswordDeriveBytes -- It Derives a key from a password
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(passkey, Salt);
            // Create a encryptor from the existing SecretKey bytes.
            // We use 32 bytes for the secret key
            // (the default Rijndael key length is 256 bit = 32 bytes) and
            // then 16 bytes for the IV (initialization vector),
            // (the default Rijndael IV length is 128 bit = 16 bytes)
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(16), SecretKey.GetBytes(16));
            // Create a MemoryStream that is going to hold the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();
            // Create a CryptoStream through which we are going to be processing our data.
            // CryptoStreamMode.Write means that we are going to be writing data
            // to the stream and the output will be written in the MemoryStream
            // we have provided. (always use write mode for encryption)
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
            // Start the encryption process.
            cryptoStream.Write(PlainText, 0, PlainText.Length);
            // Finish encrypting.
            cryptoStream.FlushFinalBlock();
            // Convert our encrypted data from a memoryStream into a byte array.
            byte[] CipherBytes = memoryStream.ToArray();
            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();
            // Convert encrypted data into a base64-encoded string.
            // A common mistake would be to use an Encoding class for that.
            // It does not work, because not all byte values can be
            // represented by characters. We are going to be using Base64 encoding
            // That is designed exactly for what we are trying to do.
            string EncryptedData = Convert.ToBase64String(CipherBytes);
            // Return encrypted string.
            return EncryptedData;
        }
        public static string DecryptString(string InputText)
        {
            try
            {
                RijndaelManaged RijndaelCipher = new RijndaelManaged();
                byte[] EncryptedData = Convert.FromBase64String(InputText);
                byte[] Salt = Encoding.ASCII.GetBytes(passkey.Length.ToString());
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(passkey, Salt);
                // Create a decryptor from the existing SecretKey bytes.
                ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(16), SecretKey.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(EncryptedData);
                // Create a CryptoStream. (always use Read mode for decryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold EncryptedData;
                // DecryptedData is never longer than EncryptedData.
                byte[] PlainText = new byte[EncryptedData.Length];
                // Start decrypting.
                int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                memoryStream.Close();
                cryptoStream.Close();
                // Convert decrypted data into a string.
                string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
                // Return decrypted string.
                return DecryptedData;
            }
            catch (Exception exception)
            {
                return (exception.Message);
            }
        }
    }
}
