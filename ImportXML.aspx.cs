using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using HRModule;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;

public partial class TestProj_ImportXML : System.Web.UI.Page
{
    OfflineTraining objOffTrain = new OfflineTraining();
    DataTable dt = new DataTable();
    DataSet ds = new DataSet();
    string FileName = "";
    string FilePath = "";
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void cmdShow_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsPostBack)
            {
                if (fuFilePath.HasFile && fuFilePath.PostedFile.ContentLength > 0 && fuFilePath.PostedFile.FileName.Length > 0)
                {
                    FileName = Path.GetFileName(fuFilePath.PostedFile.FileName);
                    string Extension = Path.GetExtension(fuFilePath.PostedFile.FileName);
                    string FolderPath = ConfigurationManager.AppSettings["FolderPath"];
                    FilePath = Server.MapPath(FolderPath + FileName);
                    fuFilePath.SaveAs(FilePath);
                    Import_To_Grid(FilePath, Extension);
                }
            }
        }
        catch
        {
           ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('ERROR-MEDATA-01: Description - cmdShow_Click event problem.');", true);
        }
    }

    private void Import_To_Grid(string FilePath, string Extension)
    {
        string conStr = "";
        switch (Extension)
        {
            case ".xls": //Excel 97-03
                //conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;'";
                //conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=YES;'";
                ReadExcel(conStr, FilePath);
                break;
            case ".xlsx": //Excel 07
                //conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;'";
                ReadExcel(conStr, FilePath);
                break;
            case ".csv":
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='C:/Users/backup/Desktop/';Extended Properties='text;HDR=Yes;FMT=Delimited'";
                ReadCSV(conStr, FilePath);
                break;
            case ".xml":
                ReadXML();
                break;
        }
    }

    protected void ReadExcel(string conStr, string FilePath)
    {
        conStr = String.Format(conStr, FilePath, 1);
        OleDbConnection connExcel = new OleDbConnection(conStr);
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        System.Data.DataTable dt = new System.Data.DataTable();
        cmdExcel.Connection = connExcel;

        connExcel.Open();
        System.Data.DataTable dtExcelSchema;
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        connExcel.Close();

        //Read Data from First Sheet
        connExcel.Open();
        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";

        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);
        connExcel.Close();
        gvUpload2.DataSource = dt;
        gvUpload2.DataBind();
    }

    protected void ReadCSV(string conStr, string FilePath)
    {
        // CREATE INSTANCE FOR OLEDB CONNECTION CLASS
        OleDbConnection conCSV = new OleDbConnection();

        conCSV.ConnectionString = conStr;

        // CREATE INSTANCE FOR COMMAND OBJECT 
        OleDbCommand cmdCSV = new OleDbCommand();

        cmdCSV.Connection = conCSV;
        cmdCSV.CommandText = "SELECT * FROM [abc.csv]";

        conCSV.Open();
        dt.Load(cmdCSV.ExecuteReader());

        gvUpload2.DataSource = dt;
        gvUpload2.DataBind();

        conCSV.Close();
    }

    //protected void ReadXML()
    //{
    //    string fileName = Path.Combine(Server.MapPath("~/"), Guid.NewGuid().ToString() + ".xml");
    //    fuFilePath.PostedFile.SaveAs(fileName);

    //    XDocument xDoc = XDocument.Load(fileName);


    //    List<EmployeeMaster> emList = xDoc.Descendants("Employee").Select(d =>
    //                    new EmployeeMaster
    //                    {
    //                        EmployeeID = d.Element("EmployeeID").Value,
    //                        CompanyName = d.Element("CompanyName").Value,
    //                        ContactName = d.Element("ContactName").Value,
    //                        ContactTitle = d.Element("ContactTitle").Value,
    //                        EmployeeAddress = d.Element("EmployeeAddress").Value,
    //                        PostalCode = d.Element("PostalCode").Value
    //                    }).ToList();

    //    // Update Data Here
    //    using (MuDatabaseEntities dc = new MuDatabaseEntities())
    //    {
    //        foreach (var i in emList)
    //        {
    //            var v = dc.EmployeeMasters.Where(a => a.EmployeeID.Equals(i.EmployeeID)).FirstOrDefault();
    //            if (v != null)
    //            {
    //                //v.EmployeeID = i.EmployeeID;
    //                v.CompanyName = i.CompanyName;
    //                v.ContactName = i.ContactName;
    //                v.ContactTitle = i.ContactTitle;
    //                v.EmployeeAddress = i.EmployeeAddress;
    //                v.PostalCode = i.PostalCode;
    //            }
    //            else
    //            {
    //                dc.EmployeeMasters.Add(i);
    //            }
    //        }

    //        dc.SaveChanges();
    //    }

    //    // Populate update data
    //    PopulateData();
    //    lblMessage.Text = "Import Done successfully!";
    //}

    protected void ReadXML()
    {
        ds.ReadXml(Server.MapPath("~/TestProj/" + FileName));
        gvUpload2.DataSource = ds.Tables[0];
        gvUpload2.DataBind();
        //ReadNascaXml();

    }

    protected void ReadNascaXml()
    {
        XDocument loaded = XDocument.Parse(Server.MapPath("~/TestProj/" + FileName), LoadOptions.SetLineInfo);

        XDocument clone = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
            loaded.LastNode
            );

        Console.WriteLine(clone);
        System.Diagnostics.Stopwatch.GetTimestamp();

        // char[] pattern = {'<','#','#','N','A','S','C','A','D','R','M','F','I','L','E','-','V','E','R','1','.','0','0','#','#','>'};
        // string pattern = "<## .*? ##>";
        // <## NASCA DRM FILE - VER1.00 ##>+
        // string change = "";
        // XmlDocument xDoc = new XmlDocument();
        
        // xDoc.LoadXml(Server.MapPath("~/TestProj/" + FileName));
        //// xDoc.Load(Server.MapPath("~/TestProj/" + FileName));
        //// xDoc.InnerXml = Regex.Replace(xDoc.InnerXml, pattern, change, RegexOptions.Singleline);
        // xDoc.Save(Server.MapPath("~/TestProj/" + FileName));
        // ds.ReadXml(Server.MapPath("~/TestProj/" + FileName));
        // gvUpload2.DataSource = ds.Tables[0];
        // gvUpload2.DataBind();
     
        
        // Dim pattern As String = String.Empty
        // Dim xDoc As XmlDocument = New XmlDocument()
        // xDoc.Load(path)
        // Pattern of comments
        // pattern = "(<!--.*?--\>)"
        // xDoc.InnerXml = Regex.Replace(xDoc.InnerXml, pattern, String.Empty, RegexOptions.Singleline)
    }

    static IEnumerable<XElement> SimpleStreamAxis(string inputUrl, string elementName)
    {
        using (XmlReader reader = XmlReader.Create(inputUrl))
        {
            reader.MoveToContent();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == elementName)
                    {
                        XElement el = XNode.ReadFrom(reader) as XElement;
                        if (el != null)
                        {
                            yield return el;
                        }
                    }
                }
            }
        }
    }

}

