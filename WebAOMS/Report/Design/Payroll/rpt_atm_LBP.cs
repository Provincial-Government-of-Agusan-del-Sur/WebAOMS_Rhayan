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
    /// Summary description for rpt_atm_LBP.
    /// </summary>
    public partial class rpt_atm_LBP : Telerik.Reporting.Report
    {
        public rpt_atm_LBP(int period, int year, int month, string officearray,int atmbatchno,int typeid)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            //this.PageSettings.Landscape = false;
            //this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0D));
           

            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "SELECT * FROM [pmis].[dbo].[tbl_Payroll_report_Bank] where userid = "+USER.C_eID+"").Tables[0];
            if (rec.Rows.Count > 0)
            {
                txt_period.Value = "LBP BRANCH :" + rec.Rows[0]["lbp_branch"].ToString() + " CODE : " + rec.Rows[0]["Code"].ToString();

                txt_CompanyName.Value = ISfn.getCompanyName;
               // txt_period.Value = "Payroll Prooflist for " + rec.Rows[0]["nowdate"].ToString();
                txt_batchno.Value = "BATCH " + atmbatchno.ToString("00000");
                txtUser.Value = USER.C_NameFML;
                txtUserposition.Value = USER.C_Position;

                txtcertified.Value = rec.Rows[0]["notedBy"].ToString();
                txtcertifiedpos.Value = rec.Rows[0]["notedDesignation"].ToString();

                txt_accountant.Value = rec.Rows[0]["checkedBy"].ToString();
                txtAccountantpos.Value = rec.Rows[0]["checkedDesignation"].ToString();
            }
            rec.Dispose();
            this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "exec [epay].usp_get_atm_employee_list 1," + period + "," + year + "," + month + ",'" + officearray + "',"+typeid+"").Tables[0];
            
            if (ISfn.get_PayrollReportVersion(4) == "1")
            {
                this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.5), Telerik.Reporting.Drawing.Unit.Inch(13));
            }
            
            
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}