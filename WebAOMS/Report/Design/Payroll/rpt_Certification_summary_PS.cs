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
    /// Summary description for rpt_Certification_summary_PS.
    /// </summary>
    public partial class rpt_Certification_summary_PS : Telerik.Reporting.Report
    {
        public rpt_Certification_summary_PS(int eid,int year)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "select NameFML,pos_name from dbo.vw_employee_Concatname  as a inner join dbo.RefsPositions	 as b on b.PositionCode = a.position_id where eid = "+eid+"").Tables[0];
            if (rec.Rows.Count > 0)
            {
                textBox3.Value = textBox3.Value.Replace("[name]", rec.Rows[0]["NameFML"].ToString()).Replace("[position]", rec.Rows[0]["pos_name"].ToString());                
            }
            rec.Dispose();
            table1.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE  epay.usp_get_certification_summary_ps @eid = " + eid + ",@year = " + year + "").Tables[0];

            this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_report_parameter] 1,'0'," + year + ",1," + USER.C_eID + "").Tables[0];
            //
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}