using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
namespace PSInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestAPI();
            // TestLoad("userlist.xml");
            //TestSave();
            //Test_Adduser();
            //Test_GetUserForPSId();
            // Test_UpdateUser();
            //Test_Init();
            Test_Encr();
            Test_Decr();

        }

        private static void Test_Decr()
        {
            throw new NotImplementedException();
        }

        private static void Test_Encr()
        {
            string str = "TEST";
            string strEnc;
            strEnc = Helper.EncryptString(str);
            string sDecr;
            sDecr = Helper.DecryptString(str);
            if (sDecr != str)
                Console.WriteLine("failed");
            else
                Console.WriteLine("passed");

        }

        private static void Test_Init()
        {
            UserList ul = new UserList(@"C:\Docs\Pers\PSInterface\PSITest\bin\Debug\");
            ul.Load();
        }

        private static void Test_Adduser()
        {
            PSUser u = new PSUser() { RUser = "testadd", RPwd = "pwd", PSUserIds = new List<string> { "22", "3333" } };
            UserList ul = new UserList(@"C:\Docs\Pers\PSInterface\PSITest\bin\Debug\");
            ul.Load();
            ul.AddUser(u);
            ul.Save();
        }
        private static void Test_UpdateUser()
        {
            UserList ul = new UserList(@"C:\Docs\Pers\PSInterface\PSITest\bin\Debug\");
            ul.Load();
            int ix = ul.GetUserIndex("111");
            if (ix >= 0)
            {
                ul.USrs[ix].PSUserIds.Add("44556");
                ul.USrs[ix].RPwd = "newpwd";
                ul.Save();
            }
        }
        private static void Test_GetUserForPSId()
        {
            UserList ul = new UserList(@"C:\Docs\Pers\PSInterface\PSITest\bin\Debug\");
            if (ul.Load() < 0)
                Console.WriteLine("error");
            else
                Console.WriteLine("ok");
            if (ul.GetUserIxForPSId("3333") >= 0)
                Console.WriteLine("Found 3333 ");
            if (ul.GetUserIxForPSId("99") < 0)
                Console.WriteLine("Not Found 99");
        }

        private static void TestAPI()
        {
            string resp = // "{\"response\":{\"result\":{\"access_token\":\"7b230466359743c6f2096d03ee403f9420327ce6\",\"expires_in\":7776000,\"token_type\":\"bearer\",\"scope\":null}}}";
            "{\"response\":{\"result\":{\"access_token\":\"a317bf95408ad0e226ee5b14baa91e7fe497d76f\",\"expires_in\":7776000,\"token_type\":\"bearer\",\"scope\":null}}}";
            //string resp =  OAuth2.GetToken("bbb");\
            JObject json = JObject.Parse(resp);
            if (json["response"]["error"] != null)
            {
                resp = json["response"]["error"].ToString();
            }
            if (json["response"] != null && json["response"]["result"] != null)
            {
                resp = json["response"]["result"]["access_token"].ToString();
            }



            //Match m = Regex.Match(resp,"\"access_token\":\"[A-Za-z0-9]+\"");
            // if (m.Success)
            // {
            //     string[] parts = m.Groups[0].Value.Split(':');
            //     Console.WriteLine(parts[1].Replace("\"",""));
            // }

            Console.WriteLine(resp);
            //string userinfo = OAuth2.GetUserInfo("rewerrrr");
            //Console.WriteLine(userinfo);
        }
        private static void TestLoad(string f)
        {
            UserList ul = new UserList(@"C:\Docs\Pers\PSInterface\PSITest\bin\Debug\");
            if (ul.Load() < 0)
                Console.WriteLine("error");
            else
                Console.WriteLine("ok");
        }
        private static void TestSave()
        {
            UserList ul = new UserList(@"C:\Docs\Pers\PSInterface\PSITest\bin\Debug\");
            if (ul.Load() < 0)
                Console.WriteLine("error");
            else
                Console.WriteLine("ok");
        }
    }

}
