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
    /// Summary description for rpt_SLARATA.
    /// </summary>
    public partial class rpt_SLARATA : Telerik.Reporting.Report
    {
        public rpt_SLARATA(string year, string month, string officeID,string deductedFrom)
        {
            int reportID = 0;
            
            
            //
            // Required for telerik Reporting designer support
            //
           

            InitializeComponent();

            if (ISfn.getCompanyName == "MUNICIPALITY OF PROSPERIDAD")
            {
                textBox60.Visible = false;
                textBox61.Visible = false;
                textBox62.Visible = false;

                textBox112.Visible = false;
                textBox111.Visible = false;
                textBox28.Visible = false;
                textBox29.Visible = false;
                textBox58.Visible = false;
                textBox56.Value = "APPROVED FOR PAYMENT:";
                textBox53.Value = "CERTIFIED:";
            }
            if (deductedFrom == "2")
            {
                textBox36.Value = "Subsistence and Laundry Allowance";
                textBox40.Value = "Hazard Pay";
                reportID = 2;
            }

            else if (deductedFrom == "3")
            {
                textBox36.Value = "Representation Allowance";
                textBox40.Value = "Transportation Allowance";
                reportID = 1;
            }
            

            table1.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE  [dbo].[MPproc_preview_report] @year = " + year + ",@month = " + month + ",@deductedfrom = " + deductedFrom + ",@reportID = " + reportID + ",@officeArray = '" + officeID + "'").Tables[0];
            this.table4.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE Epay.usp_SpecialPayroll_reportDetails 0,0," + USER.C_eID + "").Tables[0];
            //this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_reportDetails] 1,'0'," + year + "," + month + "").Tables[0];
            this.DataSource = ISfn.ExecuteDataset("EXECUTE [epay].[usp_report_parameter] 1,'0',"+year+","+month+"," + USER.C_eID + "");
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}