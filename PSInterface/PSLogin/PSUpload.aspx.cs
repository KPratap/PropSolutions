using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using FileHelpers;
using PSInterface;
using System.Xml.Serialization;
using System.Drawing;

public partial class PSUpload : System.Web.UI.Page
{
    protected int addCnt=0;
    protected int updCnt= 0;
    protected int delCnt = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        UserList ul = new UserList(Server.MapPath("~/App_Data/"));
        
        if (!Page.IsPostBack)
        {
            GetUsers(ul);
            StatusLabel.Text = "Users Loaded:" + ul.USrs.Count.ToString();
        }
    }
    protected void UploadButton_Click(object sender, EventArgs e)
    {
        if (FileUploadControl.HasFile)
        {
            try
            {
                string filename = Path.GetFileName(FileUploadControl.FileName);
                if (!filename.EndsWith(".csv"))
                {
                    StatusLabel.Text = "Please upload csv files only";
                    return;
                }
                string fullname = Server.MapPath("~/App_Data/") + filename;
                FileUploadControl.SaveAs(Server.MapPath("~/App_Data/") + filename);
                ProcessUploadFile(fullname);
                StatusLabel.Text = "Users Added: " + addCnt.ToString() +
                                    ", Updated: " + updCnt.ToString() +
                                    ", Deleted: " + delCnt.ToString();
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "The file could not be uploaded. The following error occured: " + ex.Message;
            }
        }
    }

    private void ProcessUploadFile(string fullname)
    {
        string msg = "ProcessUpload";
        try
        {
            addCnt = 0; updCnt = 0; delCnt = 0;
            FileHelperEngine engine = new FileHelperEngine(typeof(RRSUser));
            RRSUser[] users = engine.ReadFile(fullname) as RRSUser[];
            UserList ul = new UserList(Server.MapPath("~/App_Data/"));
            ul.Load();
            foreach (var u in users)
            {
                if (!u.User.StartsWith("-"))
                    AddUser(ul, u);
                else
                    DeleteUser(ul, u);
            }
            ul.Save();
            File.Delete(fullname);
            GetUsers(ul);
        }
        catch(Exception ex)
        {
            throw new Exception(msg + ":" + ex.Message);
        }
    }


    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int n = e.Row.Cells[1].Text.Length;
            e.Row.Cells[1].Text = new string('*', 10);
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Text = "User";
            e.Row.Cells[1].Text = "Password";
            e.Row.Cells[2].Text = "Property Solutions Ids";
            e.Row.BackColor = Color.FromArgb(107,142,0);
        }
    }
    //private void ProcessUploadFile(string fullname)
    //{
    //    string msg = "ProcessUpload";
    //    try
    //    {
    //        FileHelperEngine engine = new FileHelperEngine(typeof(RRSUser));
    //        RRSUser[] users = engine.ReadFile(fullname) as RRSUser[];
    //        string ul = Server.MapPath("~/App_Data/") + _Userlist;
    //        if (!File.Exists(ul))
    //        {
    //            List<PSUser> empty = new List<PSUser>();
    //            Serialize(empty, ul);
    //        }
    //        List<PSUser> usrs = Deserialize(ul);
    //        foreach (var u in users)
    //            AddUser(usrs, u);
    //        }
    //        Serialize(usrs, ul);
    //        File.Delete(fullname);
    //        GetUsers(ul);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception(msg + ":" + ex.Message);
    //    }
    //}
    private void GetUsers(UserList ul)
    {
        ul.Load();
        gvUsers.DataSource = ul.USrs;
        gvUsers.DataBind();
    }

    private void AddUser(UserList usrs, RRSUser u)
    {
        string encPwd =     Helper.EncryptString(u.Pwd);
        string decPwd = Helper.DecryptString(encPwd);
        int uix =  usrs.GetUserIndex(u.User);
        PSUser n = new PSUser();
        n.RUser = u.User; n.RPwd = u.Pwd; n.PSUserIds = null;
        if (uix < 0) // not found
            { usrs.AddUser(n); addCnt++; }
        else
            { usrs.UpdateUser(n); updCnt++; }
    }

    private void DeleteUser(UserList usrs, RRSUser u)
    {
        string user = u.User.TrimStart('-');
        PSUser n = new PSUser(); n.RUser = user; n.RPwd =u.Pwd;
        if (usrs.DeleteUser(n) == 0)
            delCnt++;
    }
     //public class Users
    //{
    //    public List<PSUser> RRSUsers;
    //}
    //private static void Serialize(List<PSUser> list, string fn)
    //{
    //    try
    //    {
    //        XmlSerializer serializer = new XmlSerializer(typeof(List<PSUser>));
    //        using (TextWriter writer = new StreamWriter(fn))
    //        {
    //            serializer.Serialize(writer, list);
    //        }
    //    }
    //    catch
    //    {
    //        throw new Exception("Error Writing users - please try again");

    //    }
    //}
    //private static List<PSUser> Deserialize(string f)
    //{
    //    List<PSUser> lists = null;
    //    TextReader reader = new StreamReader(f);
    //    try
    //    {
    //        XmlSerializer serializer = new XmlSerializer(typeof(List<PSUser>));
    //        lists = serializer.Deserialize(reader) as List<PSUser>;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Error Reading users - please try again " + ex.Message);
    //    }
    //    finally { if (reader != null) reader.Dispose(); }
    //    return lists;
    //}
    //private static int GetUserIxForUserName(List<PSUser> usrs, string p)
    //{
    //    int x = -1;
    //    try
    //    {
    //        x = usrs.FindIndex(s => s.RUser.Equals(p));
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //    return x;
    //}
}