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
    /// Summary description for rpt_remittance_loan.
    /// </summary>
    public partial class rpt_remittance_loan : Telerik.Reporting.Report
    {
        public rpt_remittance_loan(int year, int month, string officeArray, int deductionID, int remittanceType)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            //pictureBox1.Value = "Content/Company Image/CompanyLogo.png";

            this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "execute epay.[usp_Print_Remittance_optional] " + year + "," + month + "," + deductionID + "," + USER.C_eID + ",'" + officeArray.Replace("'", "").Replace("--", "") + "',"+remittanceType+"").Tables[0];
            DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_report_parameter] 1,'0'," + year + "," + month + "," + USER.C_eID + "").Tables[0];
            if (rec.Rows.Count > 0)
            {
                txt_Accountant.Value = rec.Rows[0]["AccountingName"].ToString();
                txt_Accountant_position.Value = rec.Rows[0]["AccountingPosition"].ToString();
                html_header.Value = rec.Rows[0]["H1"].ToString() + "<br /><b>" + rec.Rows[0]["CompanyName"].ToString() + "</b><br />" + rec.Rows[0]["CompanyAddress"].ToString();
                textBox1.Value = "For the month of " + rec.Rows[0]["monthYearString"].ToString();
            }
            //ISfn.ActionLog("Preview Report(rpt_Remittance)");
            // TODO: Add any constructor code after InitializeComponent call
            string contriName = "";
            contriName = ISfn.ExecScalar("SELECT  upper([Description]) FROM epay.tbl_l_deductions where deductionID = " + deductionID + "");
            if (month == 13) {
                textBox1.Value = "MID-YEAR 2017";
            }
            textBox2.Value = "REMITTANCE REPORT ON " + contriName;
            string UI = ISfn.gen_GUID();
            brcode_UI.Value = UI;
            txtUser.Value = USER.C_NameFML;
            txtUserposition.Value = USER.C_Position.ToString();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}