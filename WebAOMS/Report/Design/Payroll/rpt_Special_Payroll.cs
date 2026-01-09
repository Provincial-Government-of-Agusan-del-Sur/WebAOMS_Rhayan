namespace WebAOMS.Report.Design.Payroll
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using WebAOMS.Base;
    using WebAOMS.Report.Design;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Web;
    using System.IO;
    using WebAOMS.ws_tracking;

    /// <summary>
    /// Summary description for rpt_Special_Payroll.
    /// </summary>
    public partial class rpt_Special_Payroll : Telerik.Reporting.Report
    {
        public rpt_Special_Payroll(int batchno, int bankid, int officeid)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            string UI = Guid.NewGuid().ToString().Substring(24, 12);
            // ISfn.LogReportGuid(batchno, UI, 9);

            string bankname = "";
            if (bankid == 1)
            {
                bankname = "-LBP";
            }
            else if (bankid == 2)
            {
                bankname = "-DBP";
            }

            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "SELECT [t_compensation_batchID],a.[CompensationID],b.CompensatioName,[yearMonth],[employmentStatusArray],[employmentStatusText] FROM [pmis].[epay].[tbl_t_Special_Payroll_batch] as a inner join pmis.epay.tbl_l_compensation as b on b.CompensationID = a.CompensationID where t_compensation_batchID = " + batchno + "").Tables[0];
            if (rec.Rows.Count > 0)
            {
                textBox2.Value = rec.Rows[0]["CompensatioName"].ToString() + "(" + rec.Rows[0]["employmentStatusText"].ToString() + ")";
            }
            rec.Dispose();
            txt_batchno.Value = "Batch No.: " + batchno;

            this.DataSource = ISfn.ExecuteDataset("EXECUTE pmis.[epay].[usp_report_parameter] 1,'"+ USER.C_OfficeID +"',2019,8," + USER.C_eID + "");
            this.table1.DataSource = ISfn.ExecuteDataset("exec pmis.[epay].usp_print_special_payroll_dynamic " + batchno + "," + bankid + "," + USER.C_eID.ToString());
            this.table4.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE Epay.[usp_SpecialPayroll_reportDetails_byuserid] " + batchno + "," + officeid + "," + USER.C_eID.ToString() + "").Tables[0];
            // TODO: Add any constructor code after InitializeComponent call
            brcode_UI.Value = UI;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}