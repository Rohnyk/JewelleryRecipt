using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.OleDb;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using System.Web.Configuration;

namespace WebApplication3
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
       public class getData
        {
            public string Rno;
            public string Name;
            public string Member_No;
            public string GroupName;
            public string Amount;
            public string installmentno;
            public string Address;
            public string status;
         }
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
     

        //[WebMethod]
        //public string exceldata(string gid)
        //{

        //    getData g = new getData();
        //    string d="";
        //    string jsondata="";
        //  try
        //  {
        //      Excel.Application xlApp=new Excel.Application();
        //      xlApp.DisplayAlerts=false;
        //      Excel.Workbook xlWorkBook=xlApp.Workbooks.Open(@"D:\VC#\WebApplication3\WebApplication3\0011311D.xls");
        //      Excel.Worksheet xlWorkSheet=xlWorkBook.Sheets[1];
        //      Excel.Range xlRange = xlWorkSheet.UsedRange;
        //      int rowCount = xlRange.Rows.Count;
        //      int colCount = xlRange.Columns.Count;
        //      for (int i = 1; i <= rowCount; i++)
        //      {
                

        //              //write the value to the console
        //          if (xlRange.Cells[i, 12] != null && xlRange.Cells[i, 12].Value2 == gid)
        //          {
        //              g.Name= xlRange.Cells[i, 4].Value2.ToString();
        //              g.Member_No= xlRange.Cells[i, 3].Value2.ToString();
        //              g.GroupName = xlRange.Cells[i, 2].Value2.ToString();
        //              g.Amount = xlRange.Cells[i, 6].Value2.ToString();
        //              g.installmentno = xlRange.Cells[i, 10].Value2.ToString();
        //              g.Address = xlRange.Cells[i, 1].Value2.ToString();
        //              JavaScriptSerializer js = new JavaScriptSerializer();
        //              jsondata = js.Serialize(g);
        //              break;
        //          }
        //          else
        //              d = "No R.No";
        //      }
     
            
        //      GC.Collect();
        //      GC.WaitForPendingFinalizers();

        //      //rule of thumb for releasing com objects:
        //      //  never use two dots, all COM objects must be referenced and released individually
        //      //  ex: [somthing].[something].[something] is bad
               
        //      //release com objects to fully kill excel process from running in the background
        //      Marshal.ReleaseComObject(xlRange);
        //      Marshal.ReleaseComObject(xlWorkSheet);

        //      //close and release
        //      xlWorkBook.Close();
        //      Marshal.ReleaseComObject(xlWorkBook);

        //      //quit and release
        //      xlApp.Quit();
        //      Marshal.ReleaseComObject(xlApp);
        //      if(jsondata.Equals(""))
        //      return d;
        //      else
        //      return jsondata;
        //  }
        //  catch(Exception ex)
        //  {
        //      return ex.ToString();
        //  }
        //}
        [WebMethod]
        public string exceldata(string gid)
        {
            try
            {
                string queryString = "SELECT  [Member Name],[Member No],[Group Name],[Agreed Amt],[Insall No],[Branch Name] from [LineA1$] where [GroupID]='"+gid+"'";
                getData g = new getData();
                string d = "";
                string jsondata = "";
                string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\VC#\WebApplication3\WebApplication3\0011311D.xls;Extended Properties='Excel 8.0;HDR=YES;IMEX=1;';";
                using (OleDbConnection conn = new OleDbConnection(ConnectionString))
                {

                    OleDbCommand command = new OleDbCommand(queryString, conn);
                    conn.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            g.Name = reader[0].ToString();
                            g.Member_No = reader[1].ToString();
                            g.GroupName = reader[2].ToString();
                            g.Amount = reader[3].ToString();
                            g.installmentno = reader[4].ToString();
                            g.Address = reader[5].ToString();
                            JavaScriptSerializer js = new JavaScriptSerializer();
                            jsondata = js.Serialize(g);
                            break;                  
                        }
                    }
                    conn.Close();

                }
                if (jsondata.Equals(""))
                    return d;
                else
                    return jsondata;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        [WebMethod]
        public string reciptno(string getdate) {
            try
            {
                getdate=getdate.Replace("/", "");
                if (!File.Exists(@"D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx"))
                {
                    Excel.Application app = new Excel.Application();
                    app.Visible = false;
                    app.DisplayAlerts = false;
                    Excel.Workbook wb = app.Workbooks.Add(1);
                    Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];

                    ws.Cells[1, 1] = "Rno";
                    ws.Cells[1, 2] = "GroupId";
                    ws.Cells[1, 3] = "Name";
                    ws.Cells[1, 4] = "Inst No";
                    ws.Cells[1, 5] = "Amount";
                    ws.Cells[1, 6] = "RSGST";
                    ws.Cells[1, 7] = "RCGST";
                    ws.Cells[1, 8] = "RIGST";
                    ws.Cells[1, 9] = "RRAMT";
                    ws.Cells[1, 10] = "Status";
                    FileInfo excelFile = new FileInfo(@"D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx");
                    wb.SaveAs(excelFile);
                    wb.Close();
                    app.Quit();
                }
                string queryString = "Select max([Rno]) from [Sheet1$]";
                string d = "";
                string ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx;Extended Properties='Excel 12.0;HDR=YES;';";
                using (OleDbConnection conn = new OleDbConnection(ConnectionString))
                {

                    OleDbCommand command = new OleDbCommand(queryString, conn);
                    conn.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return reader[0].ToString();    
                        }
                    }
                    conn.Close();

                }
                return "0";
            }
            catch(Exception e) {
                return "0";
            }
        }
        [WebMethod]
        public string addentry(string getdate,string Rno,string Gid,string Name,string Inst,string amt,string RSGST,string RCGST,string RIGST,string RRAMT,string Status) 
        {
            try
            {
                getdate=getdate.Replace("/","");
                if (!File.Exists(@"D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx"))
                {
                    Excel.Application app = new Excel.Application();
                    app.Visible = false;
                    app.DisplayAlerts = false;
                    Excel.Workbook wb = app.Workbooks.Add(1);
                    Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];

                    ws.Cells[1, 1] = "Rno";
                    ws.Cells[1, 2] = "GroupId";
                    ws.Cells[1, 3] = "Name";
                    ws.Cells[1, 4] = "Inst No";
                    ws.Cells[1, 5] = "Amount";
                    ws.Cells[1, 6] = "RSGST";
                    ws.Cells[1, 7] = "RCGST";
                    ws.Cells[1, 8] = "RIGST";
                    ws.Cells[1, 9] = "RRAMT";
                    ws.Cells[1, 10] = "Status";
                    FileInfo excelFile = new FileInfo(@"D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx");
                    wb.SaveAs(excelFile);
                    wb.Close();
                    app.Quit();
                }

                string queryString = "insert into [Sheet1$] ([Rno],[GroupId],[Name],[Inst No],[Amount],[RSGST],[RCGST],[RIGST],[RRAMT],[Status]) values('" + Rno + "','" + Gid + "','" + Name + "','" + Inst + "','" + amt + "','" + RSGST + "','" + RCGST + "','" + RIGST + "','" + RRAMT + "','" + Status + "')";
                OleDbConnection conn;
                string ConnectionString;
                ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx;Extended Properties='Excel 12.0;HDR=YES;READONLY=NO'";
                conn = new OleDbConnection(ConnectionString);
                OleDbCommand command = new OleDbCommand(queryString, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                return "Record Inserted";
            }
            catch(Exception e){
                return e.ToString();
            }
           }
        [WebMethod]
        public string delentry(string getdate, string Rno, string Gid, string Name, string Inst, string amt, string RSGST, string RCGST, string RIGST, string RRAMT, string Status)
        {
            try
            {
                getdate = getdate.Replace("/", "");
                if (!File.Exists(@"D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx"))
                {
                    Excel.Application app = new Excel.Application();
                    app.Visible = false;
                    app.DisplayAlerts = false;
                    Excel.Workbook wb = app.Workbooks.Add(1);
                    Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];

                    ws.Cells[1, 1] = "Rno";
                    ws.Cells[1, 2] = "GroupId";
                    ws.Cells[1, 3] = "Name";
                    ws.Cells[1, 4] = "Inst No";
                    ws.Cells[1, 5] = "Amount";
                    ws.Cells[1, 6] = "RSGST";
                    ws.Cells[1, 7] = "RCGST";
                    ws.Cells[1, 8] = "RIGST";
                    ws.Cells[1, 9] = "RRAMT";
                    ws.Cells[1, 10] = "Status";
                    FileInfo excelFile = new FileInfo(@"D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx");
                    wb.SaveAs(excelFile);
                    wb.Close();
                    app.Quit();
                    return "No Data";
                }
                Excel.Application xlApp = new Excel.Application();
                xlApp.DisplayAlerts=false;
                xlApp.Visible = false;
                object _missingValue = Type.Missing;
                Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(@"D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx", _missingValue,
                                                        false,
                                                        _missingValue,
                                                        _missingValue,
                                                        _missingValue,
                                                        true,
                                                        _missingValue,
                                                        _missingValue,
                                                        true,
                                                        _missingValue,
                                                        _missingValue,
                                                        _missingValue);
                Excel.Worksheet xlWorkSheet=xlWorkBook.Sheets[1];
                Excel.Range xlRange = xlWorkSheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                for (int i = 1; i <= rowCount; i++)
                    {           
                    if (xlRange.Cells[i, 2] != null && xlRange.Cells[i, 2].Value2 == Gid)
                              {
                                  xlWorkSheet.Rows[i].Delete();
                                  break;
                              }
                              
                          }


                FileInfo fileExcel = new FileInfo(@"D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx");
                xlWorkBook.SaveAs(fileExcel);
                xlWorkBook.Close();
                xlApp.Quit();
                return "Deleted";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        [WebMethod]
        public string exceldata2(string gid,string getdate)
        {
            List<getData> getData1 = new List<getData>();
            getdate = getdate.Replace("/", "");
          try
            {
                string queryString = "SELECT  [Rno],[Inst No],[Amount],[RSGST],[RCGST],[RIGST],[RRAMT],[Status] from [Sheet1$] where [GroupID]='"+gid+"'";
                string d = "";
                string jsondata = "";
                string ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx;Extended Properties='Excel 8.0;HDR=YES;';";
                using (OleDbConnection conn = new OleDbConnection(ConnectionString))
                {

                    OleDbCommand command = new OleDbCommand(queryString, conn);
                    conn.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            getData g = new getData();
                            g.Rno = reader[0].ToString();
                            g.installmentno = reader[1].ToString();
                            g.Amount = reader[2].ToString();
                            g.status = reader[7].ToString();
                            getData1.Add(g);
                            
                        }
                    }
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    jsondata = js.Serialize(getData1);                  
                    conn.Close();

                }
                if (jsondata.Equals(""))
                    return d;
                else
                    return jsondata;
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        [WebMethod]
        public string instno(string gid, string getdate)
        {
            getdate = getdate.Replace("/", "");
         
            try
            {
                string queryString = "SELECT  max([Inst No]) from [Sheet1$] where [GroupID]='" + gid + "'";
                string ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\VC#\WebApplication3\WebApplication3\ExcelOutput\d" + getdate + ".xlsx;Extended Properties='Excel 8.0;HDR=YES;';";
                using (OleDbConnection conn = new OleDbConnection(ConnectionString))
                {

                    OleDbCommand command = new OleDbCommand(queryString, conn);
                    conn.Open();
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return reader[0].ToString();
                        }
                    }
                    conn.Close();

                }
             
                    return "No Data";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        }
     
}
