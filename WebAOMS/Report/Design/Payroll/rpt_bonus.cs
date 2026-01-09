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
    /// Summary description for rpt_bonus.
    /// </summary>
    public partial class rpt_bonus : Telerik.Reporting.Report
    {
        public rpt_bonus(int year, int month, int qtr, int payreffid, int reportID, int employmentstatusid, int officeid, int payrefftypeid, string batchno)
        {
            string UI = Guid.NewGuid().ToString().Substring(24, 12);
            ISfn.LogReportGuid(ISfn.CreateRegularPayBatchno(officeid, year, month), UI, 9);
            InitializeComponent();
            string emptype = "";
            
            if (employmentstatusid == 1)
            {
                emptype = "REGULAR";
            }
            else if (employmentstatusid == 5)
            {
                emptype = "CASUAL";
            }
            DataTable rec = ISfn.ExecuteDataset("execute sp_rpt_PageDatasource @reportID = " + reportID + ",@year = " + year + ",@month = " + @month + ",@qtr = 2 ");
            //batchno = ISfn.ExecScalar("select [BonusBatchno] from dbo.[tbl_Payroll_Bonus] where payreffID = 37 and employmentstatus_id = 5 and year_ = " + year + " and officeID = " + officeid + " and PayreffTypeID = 9 order by BonusBatchno desc").ToString();
            txt_batchno.Value = "Batch No.: " + batchno;
            textBox2.Value = "MID-YEAR BONUS " + year.ToString() +"(" + emptype + ")";
            htmlTextBox2.Value = "Office: <strong>" + ISfn.ExecScalar("SELECT [OfficeName] FROM [pmis].[dbo].[OfficeDescription] where officeid = " + officeid + " ") + "</strong>";
            this.DataSource = rec;
           // this.table1.DataSource = ISfn.ExecuteDataset("EXECUTE [dbo].[sp_GridDatasource] @GridDatasourceID = 1082,@para1 = '" + officeid + "',@para2 = " + employmentstatusid + ",@para3=" + year + ",@para4=" + payrefftypeid + ",@para5= " + batchno);
            this.table4.DataSource = rec;
            // TODO: Add any constructor code after InitializeComponent call
            brcode_UI.Value = UI;
            ISfn.ActionLog("Preview Report(rpt_bonus)," + ISfn.CreateRegularPayBatchno(officeid, year, month) + "," + officeid + "," + year + "," + month + "," + qtr + "," + employmentstatusid + "," + batchno + "");
        }
    }
}