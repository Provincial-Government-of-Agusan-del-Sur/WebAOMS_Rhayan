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
    /// Summary description for rpt_Remittance.
    /// </summary>
    public partial class rpt_Remittance_Regular : Telerik.Reporting.Report
    {
        public rpt_Remittance_Regular(int year, int month, int qtr, int payreffid, int reportID,int employmentstatusid)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            this.DataSource = ISfn.ExecuteDataset("execute sp_rpt_PageDatasource @reportID = "+reportID+",@year = "+year+",@month = " + @month + ",@qtr = 2 ");
            this.table1.DataSource = ISfn.ExecuteDataset("EXECUTE [dbo].[sp_GridDatasource] @GridDatasourceID = 1040,@para1 = '" + employmentstatusid + "',@para2 = " + payreffid + ",@para3 = " + year + ",@para4 = " + month + "");
            ISfn.ActionLog("Preview Report(rpt_Remittance)");
            // TODO: Add any constructor code after InitializeComponent call
            //            
            textBox2.Value = "REMITTANCE REPORT ON " + ISfn.ExecScalar("select upper(name) from [dbo].[tbl_Payroll_Refference] where payreffid = " + payreffid + "");
            string UI = ISfn.gen_GUID();
            brcode_UI.Value = UI ;
            txtUser.Value = USER.C_NameFML;
            txtUserposition.Value = USER.C_Position.ToString();
            ISfn.ActionLog("Preview Report(rpt_Remittance)," + year + "," + month + "," + qtr + "," + payreffid + "," + reportID + "," + employmentstatusid + "");
            ISfn.LogReportGuid(ISfn.CreateRegularPayBatchno(payreffid,year,month), UI, 8);
        }
    }
}