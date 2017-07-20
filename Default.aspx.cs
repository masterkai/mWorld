using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
public partial class soeasy_activity_1040227_point_Default : System.Web.UI.Page
{
    string connStr = WebConfigurationManager.ConnectionStrings["sqlcn"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Charset = "utf-8";
        Response.ContentType = "application/json";
        string func = "";
        if (Request.HttpMethod == "POST")
        {
            func = Request.Form["func"] == null ? Request.Form["func"] : Request.Form["func"];
            switch (func)
            {
                case "dgaetaaraa":
                    GetDeptAddr();
                    break;
                case "sdeanntodaw":
                    SendData();
                    break;
                default:
                    /* 做追蹤 */
                    break;
            }

        }
    }

    protected void GetDeptAddr()
    {
        string callback = Request.QueryString["callback"];
        SqlConnection aSqlConnection = new SqlConnection();
        SqlCommand aSqlCommand = new SqlCommand();
        SqlDataAdapter aSqlDataAdapter = new SqlDataAdapter();
        string sql = "";
        DataTable DT = new DataTable();
        aSqlConnection.ConnectionString = "Data Source=10.0.0.126;Initial Catalog=soeasydb;Persist Security Info=True;User ID=so_user;Password=pcschoolmis";
        aSqlCommand.Connection = aSqlConnection;

        sql += "select org_no,org_desc" + "\n";
        sql += "from gmhqfindb.humandb.dbo.sys_org" + "\n";
        sql += "where upper_org in ('AH','AH1','A3')  and org_no <>'K11' " + "\n";

        aSqlCommand.CommandText = sql;

        aSqlDataAdapter.SelectCommand = aSqlCommand;
        try
        {
            aSqlConnection.Open();
            aSqlDataAdapter.Fill(DT);

            string result = "";
            string data = "";
            string org_no = "";
            string org_desc = "";
            foreach (DataRow aRow in DT.Rows)
            {
                org_no = org_no == "" ? aRow["org_no"].ToString().Trim() : org_no + ",;" + aRow["org_no"].ToString().Trim();
                org_desc = org_desc == "" ? aRow["org_desc"].ToString().Trim() : org_desc + ",;" + aRow["org_desc"].ToString().Trim();
            }
            data = "{\"org_no\":\"" + org_no + "\",\"org_desc\":\"" + org_desc + "\"}";
            result = string.Format("{0}({1})", callback, "[" + data + "]");
            Response.Write(result);
            Response.Flush();
        }
        catch (Exception ex)
        {
            //Response.Write(ex.Message);
        }
        finally
        {
            aSqlConnection.Close();
            aSqlConnection.Dispose();
            aSqlCommand.Dispose();
        }
    }

    protected void SendData()
    {
        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "save", "alert('SendData！');", true);            

        string callback = Request.QueryString["callback"];
        string data = "";
        string json = "";
        string result = "";
        string errMsg = "";

        string bumber = Request.Form["stdno"] == null ? Request.QueryString["stdno"] : Request.Form["stdno"];
        string name = Request.Form["name"] == null ? Request.QueryString["name"] : Request.Form["name"];
        string mobile = Request.Form["mobile"] == null ? Request.QueryString["mobile"] : Request.Form["mobile"];
        string ddl_school_no = Request.Form["school"] == null ? Request.QueryString["school"] : Request.Form["school"];
        string numbers = Request.Form["numbers"] == null ? Request.QueryString["numbers"] : Request.Form["numbers"];
        SqlTransaction Trans;
        SqlConnection aSqlConnection = new SqlConnection();
        SqlCommand aSqlCommand = new SqlCommand();


        DataTable dt = new DataTable();
        string sql = "";

        aSqlConnection.ConnectionString = "Data Source=10.0.0.126;Initial Catalog=soeasydb;Persist Security Info=True;User ID=so_user;Password=pcschoolmis";
        aSqlConnection.Open();

        Trans = aSqlConnection.BeginTransaction();
        aSqlCommand.Connection = aSqlConnection;
        aSqlCommand.Transaction = Trans;

        try
        {
            string msg = CheckNumbers(numbers.Trim());
            msg = msg + CheckStdNo(bumber.Trim());
            if (msg != "")
            {
                throw new Exception(msg);
            }

            sql += "insert into PointActivity(member_id, name, phone, dept, numbers, cre_dtime)" + "\n";
            sql += "values(@bumber" + "\n";
            sql += "     ,@name" + "\n";
            sql += "     ,@mobile" + "\n";
            sql += "     ,@ddl_school_no" + "\n";
            sql += "     ,@numbers" + "\n";
            sql += "     ,getdate())" + "\n";
            aSqlCommand.Parameters.Clear();
            aSqlCommand.Parameters.Add(new SqlParameter("@bumber", bumber.ToString().Trim().ToUpper()));
            aSqlCommand.Parameters.Add(new SqlParameter("@name", name.ToString().Trim()));
            aSqlCommand.Parameters.Add(new SqlParameter("@mobile", mobile.ToString().Trim()));
            aSqlCommand.Parameters.Add(new SqlParameter("@ddl_school_no", ddl_school_no.ToString().Trim()));
            aSqlCommand.Parameters.Add(new SqlParameter("@numbers", numbers.ToString().Trim()));     
            aSqlCommand.CommandText = sql;
            aSqlCommand.ExecuteNonQuery();


            Trans.Commit();

            data = "1";

            json = "{'result':'" + data + "','errMsg':'" + errMsg + "'}";
            result = string.Format("{0}({1})", callback, "[" + json + "]");
        }
        catch (Exception ex)
        {
            Trans.Rollback();

            data = "0";
            json = "{'result':'" + data + "','errMsg':'" + ex.Message.ToString()+ "'}";
            result = string.Format("{0}({1})", callback, "[" + json + "]");

            
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "save", "alert('" + ex .Message.ToString()+ "！');", true);            
            //return false;
        }
        finally
        {
            Response.Write(result);
            Response.Flush();

            aSqlConnection.Close();
            aSqlConnection.Dispose();
            aSqlCommand.Dispose();
        }
    }


    protected string CheckNumbers(string numbers)
    {        
        SqlConnection aSqlConnection = new SqlConnection();
        SqlCommand aSqlCommand = new SqlCommand();
        SqlDataAdapter aSqlDataAdapter = new SqlDataAdapter();
        string sql = "";
        DataTable DT = new DataTable();
        aSqlConnection.ConnectionString = "Data Source=10.0.0.126;Initial Catalog=soeasydb;Persist Security Info=True;User ID=so_user;Password=pcschoolmis";
        aSqlCommand.Connection = aSqlConnection;

        sql += "select numbers" + "\n";
        sql += "from PointActivity_numbers" + "\n";
        sql += "where project_id='2' and numbers = @numbers" + "\n";
        aSqlCommand.Parameters.Clear();
        aSqlCommand.Parameters.Add(new SqlParameter("@numbers", numbers.Trim()));
        aSqlCommand.CommandText = sql;

        aSqlDataAdapter.SelectCommand = aSqlCommand;
        try
        {
            aSqlConnection.Open();
            aSqlDataAdapter.Fill(DT);
            if (DT.Rows.Count == 0)
            {
                throw new Exception("抽獎序號錯誤");
            }
            else
            {
                sql = "select numbers from PointActivity where numbers =@numbers";
                aSqlCommand.Parameters.Clear();
                aSqlCommand.Parameters.Add(new SqlParameter("@numbers", numbers.Trim()));
                aSqlCommand.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable DT1 = new DataTable();
                da.SelectCommand = aSqlCommand;
                da.Fill(DT1);
                if (DT1.Rows.Count > 0)
                {
                    throw new Exception("此抽獎序號已使用");
                }
                else
                {
                    return "";
                }
                return "";
            }

        }
        catch (Exception ex)
        {
            return ex.Message;
            //Response.Write(ex.Message);
        }
        finally
        {
            aSqlConnection.Close();
            aSqlConnection.Dispose();
            aSqlCommand.Dispose();
        }
    }

    protected string CheckStdNo(string memberid) {
        SqlConnection aSqlConnection = new SqlConnection();
        SqlCommand aSqlCommand = new SqlCommand();
        SqlDataAdapter aSqlDataAdapter = new SqlDataAdapter();
        string sql = "";
        DataTable DT = new DataTable();
        aSqlConnection.ConnectionString = "Data Source=10.0.0.126;Initial Catalog=soeasydb;Persist Security Info=True;User ID=so_user;Password=pcschoolmis";
        aSqlCommand.Connection = aSqlConnection;

        sql += "select * " + "\n";
        sql += "from W_MEMBER" + "\n";
        sql += "where Member_id=@memberid" + "\n";
        aSqlCommand.Parameters.Clear();
        aSqlCommand.Parameters.Add(new SqlParameter("@memberid", memberid.Trim()));
        aSqlCommand.CommandText = sql;

        aSqlDataAdapter.SelectCommand = aSqlCommand;
        try
        {
            aSqlConnection.Open();
            aSqlDataAdapter.Fill(DT);
            if (DT.Rows.Count == 0)
            {
                throw new Exception("學員編號錯誤");
            }
            else
            {
                return "";
            }

        }
        catch (Exception ex)
        {
            return ex.Message;
            //Response.Write(ex.Message);
        }
        finally
        {
            aSqlConnection.Close();
            aSqlConnection.Dispose();
            aSqlCommand.Dispose();
        }
  
    }
}