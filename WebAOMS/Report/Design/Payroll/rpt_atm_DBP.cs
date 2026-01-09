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

    /// <summary>
    /// Summary description for rpt_atm_DBP.
    /// </summary>
    public partial class rpt_atm_DBP : Telerik.Reporting.Report
    {
        public rpt_atm_DBP(int period,int year,int month,string officearray,int typeid)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_report_parameter] 1,'0'," + year + "," + month + "," + USER.C_eID + "").Tables[0];
            if (rec.Rows.Count > 0)
            {
                txt_CompanyName.Value = rec.Rows[0]["CompanyName"].ToString();
                txt_period.Value = "Payroll Prooflist for " + rec.Rows[0]["nowdate"].ToString();
            }
            rec.Dispose();
            this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "exec [epay].usp_get_atm_employee_list 2," + period + "," + year + "," + month + ",'" + officearray + "',"+typeid+"").Tables[0];
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}