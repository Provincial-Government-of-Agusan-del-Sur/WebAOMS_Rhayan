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
    /// Summary description for rpt_Remittance.
    /// </summary>
    public partial class rpt_Remittance_Regular : Telerik.Reporting.Report
    {
        public rpt_Remittance_Regular(int year, int month, string officeArray, int deductionID, int remittanceType)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            //pictureBox1.Value = "Content/Company Image/CompanyLogo.png";

            this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "execute epay.[usp_Print_Remittance_optional] " + year + "," + month + "," + deductionID + "," + USER.C_eID + ",'" + officeArray.Replace("'", "").Replace("--", "") + "'," + remittanceType + "").Tables[0];
            DataTable rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_report_parameter] 1,'0'," + year + ","+month+"," + USER.C_eID + "").Tables[0];
            if (rec.Rows.Count > 0)
            {
                txt_Accountant.Value = rec.Rows[0]["AccountingName"].ToString();
                txt_Accountant_position.Value = rec.Rows[0]["AccountingPosition"].ToString();
                html_header.Value = rec.Rows[0]["H1"].ToString() + "<br /><b>" + rec.Rows[0]["CompanyName"].ToString() + "</b><br />" + rec.Rows[0]["CompanyAddress"].ToString();
                textBox1.Value = "For the month of " + rec.Rows[0]["monthYearString"].ToString();
            }
            //this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_report_parameter] 1,'0'," + year + "," + month + ","+USER.C_eID+"").Tables[0];
            //ISfn.ActionLog("Preview Report(rpt_Remittance)");
            // TODO: Add any constructor code after InitializeComponent call
            //textBox45.Width = Telerik.Reporting.Drawing.Unit.Inch(0.4300000715255737D);
            //textBox39.Width = Telerik.Reporting.Drawing.Unit.Inch(0.4300000715255737D);
            this.textBox50.Height = this.detail.Height;
            string contriName="";
                if (deductionID == 1) {
                    contriName = "State Insurance";
                    txt_ps.Value = "Amount";
                    this.txt_gs.Style.Visible = false;
                    textBox32.Visible = false;
                    textBox53.Visible = false;
                    textBox52.Visible = false;
                    textBox43.Visible = false;
                    textBox31.Visible = false;
                    textBox3.Visible = false;
                    textBox6.Visible = false;

                    textBox8.Visible = false;
                    textBox9.Visible = false;
                    textBox10.Visible = false;
                    textBox12.Visible = false;
                } 
                else if (deductionID == 2) {
                    contriName = "GSIS CONTRIBUTION";
                }
                else if (deductionID == 3)
                {
                    contriName = "PHILHEALTH CONTRIBUTION";
                    textBox8.Visible = false;
                    textBox9.Visible = false;
                    textBox10.Visible = false;
                    textBox12.Visible = false;
                  
                   // .Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.43321529030799866D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));

                }
                else if (deductionID == 4)
                {
                    contriName = "PAGIBIG CONTRIBUTION";
                    textBox8.Visible = false;
                    textBox9.Visible = false;
                    textBox10.Visible = false;
                    textBox12.Visible = false;
                    textBox45.Width = Telerik.Reporting.Drawing.Unit.Inch(1.0000000715255737D);
                    textBox39.Width = Telerik.Reporting.Drawing.Unit.Inch(1.000000715255737D);
                }
                else if (deductionID == 5)
                {
                    contriName = "WITHHOLDING TAX";
                    txt_ps.Value = "Amount";
                    this.txt_gs.Style.Visible = false;
                    textBox32.Visible = false;
                    textBox53.Visible = false;
                    textBox52.Visible = false;
                    textBox43.Visible = false;
                    textBox31.Visible = false;
                    textBox3.Visible = false;
                    textBox6.Visible = false;
                    textBox8.Visible = false;
                    textBox9.Visible = false;
                    textBox10.Visible = false;
                    textBox12.Visible = false;
                }
                else if (deductionID == 6)
                {
                    contriName = "UNION DUES";
                    txt_ps.Value = "Amount";
                    this.txt_gs.Style.Visible = false;
                    textBox32.Visible = false;
                    textBox53.Visible = false;
                    textBox52.Visible = false;
                    textBox43.Visible = false;
                    textBox31.Visible = false;
                    textBox3.Visible = false;
                    textBox6.Visible = false;
                    textBox8.Visible = false;
                    textBox9.Visible = false;
                    textBox10.Visible = false;
                    textBox12.Visible = false;
                }
               
                    if (deductionID > 6)
                    {
                            contriName = ISfn.ExecScalar("select [epay].[fn_get_CustomDeductionType](" + deductionID + ") as contriName");
                        textBox8.Visible = false;
                        textBox9.Visible = false;
                        textBox10.Visible = false;
                        textBox12.Visible = false;
                    }


            textBox2.Value = "REMITTANCE REPORT ON " + contriName;
            string UI = ISfn.gen_GUID();
            brcode_UI.Value = UI ;
            txtUser.Value = USER.C_NameFML;
            txtUserposition.Value = USER.C_Position.ToString();
            //ISfn.ActionLog("Preview Report(rpt_Remittance)," + year + "," + month + "," + qtr + "," + payreffid + "," + reportID + "," + employmentstatusid + "");
            //ISfn.LogReportGuid(ISfn.CreateRegularPayBatchno(payreffid,year,month), UI, 8);
        }
    }
}