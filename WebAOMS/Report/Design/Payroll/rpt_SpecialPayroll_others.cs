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
    /// Summary description for rpt_SpecialPayroll_others.
    /// </summary>
    public partial class rpt_SpecialPayroll_others : Telerik.Reporting.Report
    {
        public rpt_SpecialPayroll_others(int year, int month, int bonusbatchno, int userid, int OfficeID)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            string UI = "";
            string emptype = "";
            emptype = "REGULAR";
            DataSet rec;

            textBox2.Value = "PERA, RATA " + year.ToString() + "(" + emptype + ")";
            //htmlTextBox2.Value = "Office: <strong>" + ISfn.ExecScalar("SELECT [OfficeName] FROM [pmis].[dbo].[OfficeDescription] where officeid = " + officeid + " ") + "</strong>";
            txt_batchno.Value = "Batch No.: " + bonusbatchno.ToString();
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [epay].[usp_print_special_payroll_dynamic] " + bonusbatchno + "," + USER.C_eID + "," + OfficeID + "");
            // = ISfn.ExecuteDataset("execute sp_rpt_PageDatasource @reportID = 14,@year = " + year + ",@month = 5,@qtr = 2 ");
            this.DataSource = rec.Tables[1];

            this.table2.DataSource = rec.Tables[0];

            this.table4.DataSource = rec.Tables[1];
            brcode_UI.Value = ISfn.newguid();

            //
        }
    }
}