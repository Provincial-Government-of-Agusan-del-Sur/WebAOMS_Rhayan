namespace IFMIS.Report.Design
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using IFMIS.Base;
    using System.Configuration;
    using IFMIS.Report.Design;
    using System.Web.Mvc;
    /// <summary>
    /// Summary description for rpt_GeneralPayroll.
    /// </summary>
    public partial class rpt_GeneralPayroll : Telerik.Reporting.Report
    {
        public rpt_GeneralPayroll(string year,string month,string officeID)
        {
            // Required for telerik Reporting designer support
            InitializeComponent();
            // TODO: Add any constructor code after InitializeComponent call
            string UI = ISfn.newguid();
            ReportParameters["year"].Value = year;
            ReportParameters["month"].Value = month;
            ReportParameters["officeID"].Value = officeID;

            //ISfn.LogReportGuid(ISfn.CreateRegularPayBatchno(officeID,year,month), UI, 7);

            obj_DSource.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "execute MPproc_ViewPayrollAndDeductions " + month + "," + year + ",'" + officeID + "',9,0").Tables[0];
            obj_Deduct.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "execute MPproc_ViewPayrollAndDeductions " + month + "," + year + ",'" + officeID + "',3,0").Tables[0];
            obj_otherdetails.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [dbo].[MPproc_reportDetails] 1,'" + officeID + "'," + year + "," + month + "").Tables[0];
            obj_entries.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [dbo].[proc_Payroll_AccountingEntries] @PayrollOffice = '" + officeID + "',@year = " + year + ",@month = " + month + "").Tables[0];
            
            brcode_UI.Value = UI;
        }
    }
}