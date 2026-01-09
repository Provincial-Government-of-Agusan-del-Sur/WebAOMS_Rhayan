namespace IFMIS.Report.Design.Payroll
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
    using IFMIS.Classess;
    /// <summary>
    /// Summary description for rpt_Payroll_JOContract.
    /// </summary>
    public partial class rpt_Payroll_JOContract : Telerik.Reporting.Report
    {
        public rpt_Payroll_JOContract(int batchno,int officeID,int year,int month,int qtr,int emptyp = 6)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            //batchno = 87;
            //ReportParameters["batchno"].Value = batchno;
            this.txtbatchno.Value = "Batch No.  " + batchno;
            if (emptyp == 6)
            {
                ReportParameters["emptyp"].Value = "JOB-ORDER";
            }
            else if (emptyp == 7)
            {
                ReportParameters["emptyp"].Value = "CONTRACT OF SERVICE";
            }
            this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_reportDetails] 2," + officeID + "," + year + "," + month + "," + qtr + "").Tables[0];
            obj_otherdetails.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_reportDetails] 2," + officeID + "," + year + "," + month + "," + qtr + "").Tables[0];
            obj_DSource.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "exec IFMIS.dbo.sp_Payroll_Printing_JOContract  " + batchno + "").Tables[0];
            //obj_otherdetails.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_reportDetails] 1," + officeID + "," + year + "," + month + "").Tables[0];
            // obj_entries.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "EXECUTE [dbo].[proc_Payroll_AccountingEntries] @PayrollOffice = " + officeID + ",@year = " + year + ",@month = " + month + "").Tables[0];
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}