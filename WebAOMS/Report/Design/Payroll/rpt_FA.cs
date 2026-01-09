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
    /// Summary description for rpt_PEI.
    /// </summary>
    public partial class rpt_FA : Telerik.Reporting.Report
    {
        public rpt_FA(int year, int month, int qtr, int payreffid, int reportID, int employmentstatusid, int officeid, int payrefftypeid)
        {
            //
            // Required for telerik Reporting designer support
            //
            string UI = Guid.NewGuid().ToString().Substring(24, 12);
            ISfn.LogReportGuid(ISfn.CreateRegularPayBatchno(officeid, year, month), UI, 9);
            InitializeComponent();
            string emptype = "";
            if (employmentstatusid == 1){
                emptype = "REGULAR";
            }
            else if (employmentstatusid == 5){
                emptype = "CASUAL";
            }
            else if (employmentstatusid == 6)
            {
                emptype = "JOB-ORDER";
            }
            else if (employmentstatusid == 7)
            {
                emptype = "CONTRACT OF SERVICES";
            }


            txt_batchno.Value = "Batch No.: " + ISfn.ExecScalar("select [BonusBatchno] from dbo.[tbl_Payroll_Bonus] where payreffID = 46 and employmentstatus_id = " + employmentstatusid + " and year_ = " + year + " and officeID = " + officeid + " and PayreffTypeID = " + payrefftypeid + " order by BonusBatchno desc").ToString();
            textBox2.Value = "FINANCIAL ASSISTANCE(" + emptype + ")";
            htmlTextBox2.Value = "Office: <strong>" + ISfn.ExecScalar("SELECT [OfficeName] FROM [pmis].[dbo].[OfficeDescription] where officeid = " + officeid + " ") + "</strong>";
            this.DataSource = ISfn.ExecuteDataset("execute sp_rpt_PageDatasource @reportID = " + reportID + ",@year = " + year + ",@month = " + @month + ",@qtr = 2 ");
            this.table1.DataSource = ISfn.ExecuteDataset("EXECUTE [dbo].[sp_GridDatasource] @GridDatasourceID = 1059,@para1 = '" + officeid + "',@para2 = " + employmentstatusid + ",@para3="+year+",@para4=" + payrefftypeid);
            this.table4.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_reportDetails] 1," + officeid + "," + year + "," + month + ",3").Tables[0];
            // TODO: Add any constructor code after InitializeComponent call
            brcode_UI.Value = UI;
            ISfn.ActionLog("Preview Report(rpt_PEI)," + ISfn.CreateRegularPayBatchno(officeid, year, month) + "," + officeid + "," + year + "," + month + "," + qtr + "," + employmentstatusid + "");
        }
    }
}