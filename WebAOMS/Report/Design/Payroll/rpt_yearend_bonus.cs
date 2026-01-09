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
    /// Summary description for rpt_yearend_bonus.
    /// </summary>
    public partial class rpt_yearend_bonus : Telerik.Reporting.Report
    {
        public rpt_yearend_bonus(int bonusbatchno, int officeid, int year)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            string UI = "";
            string emptype = "";
            emptype = "REGULAR";
            DataSet rec;
            //batchno = ISfn.ExecScalar("select [BonusBatchno] from dbo.[tbl_Payroll_Bonus] where payreffID = 37 and employmentstatus_id = 5 and year_ = " + year + " and officeID = " + officeid + " and PayreffTypeID = 9 order by BonusBatchno desc").ToString();
            //txt_batchno.Value = "Batch No.: " + batchno;



            htmlTextBox2.Value = "Office: <strong>" + ISfn.ExecScalar("SELECT [OfficeName] FROM [pmis].[dbo].[OfficeDescription] where officeid = " + officeid + " ") + "</strong>";
            txt_batchno.Value = "Batch No.: " + bonusbatchno.ToString();
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [epay].[usp_print_special_payroll_yearend] " + bonusbatchno + "," + USER.C_eID + "," + officeid + "");
            // = ISfn.ExecuteDataset("execute sp_rpt_PageDatasource @reportID = 14,@year = " + year + ",@month = 5,@qtr = 2 ");
            this.DataSource = rec.Tables[1];

            this.table2.DataSource = rec.Tables[0];

            this.table4.DataSource = rec.Tables[1];

            
            if (rec.Tables[2].Rows.Count > 0)
            {
                textBox2.Value = "YEAR END BONUS " + rec.Tables[2].Rows[0]["year_"].ToString() + "(" + emptype + ")";
                //textBox39.Value = rec.Rows[0]["HRSignatory"].ToString();
            }
            // TODO: Add any constructor code after InitializeComponent call
            brcode_UI.Value = ISfn.newguid();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}