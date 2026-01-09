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
    public partial class rpt_Payroll_JO : Telerik.Reporting.Report
    {
        
        public rpt_Payroll_JO(int batchno,int officeID,int year,int month,int qtr,int emptyp = 6)
        {
            InitializeComponent();
            string UI = Guid.NewGuid().ToString().Substring(24, 12);
            ISfn.LogReportGuid(batchno, UI, 2);
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
                this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.92310357093811035D), Telerik.Reporting.Drawing.Unit.Inch(0.35156214237213135D));
            }
            obj_otherdetails.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_reportDetails] 3," + officeID + "," + year + "," + month + "," + qtr + ",@reportid = 2,@batchno = " + batchno + "").Tables[0];
            obj_DSource.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "exec IFMIS.dbo.sp_Payroll_Printing_JOContract_v1  " + batchno + "").Tables[0];
            //obj_otherdetails.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_reportDetails] 1," + officeID + "," + year + "," + month + "").Tables[0];
            //obj_entries.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "EXECUTE [dbo].[proc_Payroll_AccountingEntries] @PayrollOffice = " + officeID + ",@year = " + year + ",@month = " + month + "").Tables[0];
            this.DataSource = obj_otherdetails.DataSource;
            brcode_UI.Value = UI;
            ISfn.ActionLog("Preview Report(rpt_Payroll_JO)," + batchno + "," + officeID + "," + year + "," + month + "," + qtr + "," + emptyp + "");
        }
    }
}