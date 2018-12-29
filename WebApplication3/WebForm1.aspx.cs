using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace WebApplication3
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GenerateExcelData();
        }
        private void GenerateExcelData()
        {
            OleDbConnection oledbConn;
            try
            {
                // need to pass relative path after deploying on server
                string path = System.IO.Path.GetFullPath(Server.MapPath("~/Book1.xlsx"));
                /* connection string  to work with excel file. HDR=Yes - indicates 
                   that the first row contains columnnames, not data. HDR=No - indicates 
                   the opposite. "IMEX=1;" tells the driver to always read "intermixed" 
                   (numbers, dates, strings etc) data columns as text. 
                Note that this option might affect excel sheet write access negative. */


                oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Book1.xlsx; Extended Properties=Excel 12.0 Xml;HDR=YES;");
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand(); ;
                cmd.Connection = oledbConn;
                OleDbDataAdapter oda = new OleDbDataAdapter();
                DataTable dt = new DataTable();
                oledbConn.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT distinct([R.no]) FROM [Sheet1$]";
                oda.SelectCommand = cmd;
                oda.Fill(dt);
                oledbConn.Close();
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }// clos
            catch (Exception e)
            {

            }
        }
    }
}