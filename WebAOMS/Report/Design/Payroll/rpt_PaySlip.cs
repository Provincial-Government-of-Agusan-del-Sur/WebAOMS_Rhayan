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
    /// Summary description for Report1.
    /// </summary>
    public partial class rpt_PaySlip : Telerik.Reporting.Report
    {
        public rpt_PaySlip(string eid, string year, string month)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            table1.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE  [epay].[sp_Payroll_GetIncomeDedcut] @eid = " + eid + ",@year = " + year + ",@month = " + month + ",@payType = 0").Tables[0];
            table2.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE  [epay].[sp_Payroll_GetIncomeDedcut] @eid = " + eid + ",@year = " + year + ",@month = " + month + ",@payType = 1").Tables[0];
            this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [epay].[usp_report_parameter] 1,'0'," + year + "," + month + "," + USER.C_eID + "").Tables[0];
            DataTable rec;
            rec =OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE  [epay].[sp_Payroll_GetIncomeDedcut] @eid = " + eid + ",@year = " + year + ",@month = " + month + ",@payType = 4").Tables[0];
            if (rec.Rows.Count > 0){
                txt_name.Value = rec.Rows[0]["name"].ToString();
                txt_Position.Value = rec.Rows[0]["position"].ToString();
                txt_address.Value = rec.Rows[0]["address"].ToString();
                txt_office.Value = rec.Rows[0]["office"].ToString();
                txt_deductAmount.Value = rec.Rows[0]["Deduct"].ToString();
                txt_incomeAmount.Value = rec.Rows[0]["Income"].ToString();
                txt_netAmount.Value =  rec.Rows[0]["NetAmount"].ToString();
               textBox1.Value = rec.Rows[0]["Payperiod"].ToString();
            }

            rec.Dispose();
           
            //
            txtUser.Value = USER.C_NameFML;
            
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}