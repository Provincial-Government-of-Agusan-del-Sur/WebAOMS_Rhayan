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
    /// Summary description for rpt_PaySlip_byOffice.
    /// </summary>
    public partial class rpt_PaySlip_byOffice : Telerik.Reporting.Report
    {
        public rpt_PaySlip_byOffice(string eidarray,int year,int month,int userid)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            
            this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "execute [epay].[usp_payslip_all] '"+eidarray+"',"+year+","+month+"").Tables[0];
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [epay].[usp_report_parameter] 1,'0'," + year + "," + month + "," + USER.C_eID + "").Tables[0];
            if (rec.Rows.Count > 0)
            {
                txt_h2.Value = rec.Rows[0]["H2"].ToString();
                txt_period.Value = "Payslip for the month of " + rec.Rows[0]["monthYearString"].ToString();
                txt_h3.Value = rec.Rows[0]["H3"].ToString();
            }

            rec.Dispose();

            //
        }
    }
}