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
    /// Summary description for rpt_tax_computation_summary.
    /// </summary>
    public partial class rpt_tax_computation_summary : Telerik.Reporting.Report
    {
        public rpt_tax_computation_summary(int eid, int year, int remainingMonths)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            DataTable rec;
            rec = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "select NameFML,pos_name from dbo.vw_employee_Concatname  as a inner join dbo.RefsPositions	 as b on b.PositionCode = a.position_id where eid = " + eid + "").Tables[0];
            if (rec.Rows.Count > 0)
            {
                textBox3.Value = textBox3.Value.Replace("[name]", rec.Rows[0]["NameFML"].ToString()).Replace("[position]", rec.Rows[0]["pos_name"].ToString());
            }

            rec.Dispose();

            DataTable rect;
            rect = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "execute Epay.usp_taxComputation_Individual " + eid + ","+USER.C_eID+"," + year + "," + remainingMonths + "").Tables[0];
            if (rect.Rows.Count > 0)
            {
                string ntax, nettax, paidBasic,taxmonth;
                txt_midyear.Value =rect.Rows[0]["midyear"].formatNumber();
          
                txt_yearend.Value = rect.Rows[0]["yearend"].formatNumber();
                txt_midyear_yearend.Value = string.Format("{0:0,000.00}", (Convert.ToDecimal((object)rect.Rows[0]["midyear"] ?? 0.00) + Convert.ToDecimal((object)rect.Rows[0]["yearend"] ?? 0.00)));
                txt_ceiling.Value = rect.Rows[0]["Taxceiling"].formatNumber();
                txt_btax.Value = rect.Rows[0]["Bonus"].formatNumber();
                paidBasic= (Convert.ToDecimal(rect.Rows[0]["paidBasic"]) + Convert.ToDecimal(rect.Rows[0]["OtherIncome"])).formatNumber();


                txt_paidBasic.Value = paidBasic;
                txt_rbasic.Value = rect.Rows[0]["rbasic"].formatNumber();
                txt_gtax.Value = (Convert.ToDecimal(rect.Rows[0]["rbasic"]) + Convert.ToDecimal(paidBasic)  + Convert.ToDecimal( rect.Rows[0]["Bonus"])).formatNumber();
                txt_total_ps.Value = rect.Rows[0]["TotalPS"].formatNumber();
                ntax = ((Convert.ToDecimal(rect.Rows[0]["rbasic"]) + Convert.ToDecimal(paidBasic) + Convert.ToDecimal(rect.Rows[0]["Bonus"])) - Convert.ToDecimal(rect.Rows[0]["TotalPS"])).formatNumber();
                txt_ntax.Value = ntax;
                txt_taxRate_excess.Value = "Your Tax Rate is " + rect.Rows[0]["AdditionalOfExcess"].formatNumber() + " + " + rect.Rows[0]["PercentageOfExcess"].ToString() + "% of excess over " + rect.Rows[0]["Less"].formatNumber();
                txt_excess_of.Value = "" + rect.Rows[0]["AdditionalOfExcess"].formatNumber() + " + (." + rect.Rows[0]["PercentageOfExcess"].ToString() + " (" + ntax + " - " + rect.Rows[0]["Less"].formatNumber() + "))= ";
                nettax = (Convert.ToDecimal(rect.Rows[0]["AdditionalOfExcess"]) + ((Convert.ToDecimal(rect.Rows[0]["PercentageOfExcess"]) / 100) * (Convert.ToDecimal(ntax) - Convert.ToDecimal(rect.Rows[0]["Less"])))).formatNumber();
                txt_netexcess.Value = nettax;
                txt_total_tax_paid.Value = (Convert.ToDecimal(rect.Rows[0]["TotalPaidTAX"]) + Convert.ToDecimal(rect.Rows[0]["OtherIncomeTax"])).formatNumber();
                textBox31.Value = getpmonths(12 - remainingMonths) + "(BASIC + OTHER INCOME):";
                textBox33.Value = "Remaining months(" + getrmonths(remainingMonths) + "):";
                textBox35.Value = "Total Tax Paid(" + getpmonths(12-remainingMonths) + ") + Other Income Tax:";
                textBox42.Value = "Remaining months(" + getrmonths(remainingMonths) + "):";
                txt_rmonths.Value = remainingMonths.ToString();
                taxmonth = (Convert.ToDecimal(nettax) - (Convert.ToDecimal(rect.Rows[0]["TotalPaidTAX"]) + Convert.ToDecimal(rect.Rows[0]["OtherIncomeTax"]))).formatNumber();
                txt_nettax.Value = Convert.ToDecimal( taxmonth) > 0 ?taxmonth:"0.00";
                txt_taxmonth.Value =Convert.ToDecimal( taxmonth) > 0 ?((Convert.ToDecimal(nettax) - (Convert.ToDecimal(rect.Rows[0]["TotalPaidTAX"]) + Convert.ToDecimal(rect.Rows[0]["OtherIncomeTax"]))) / ((remainingMonths==0)?1:remainingMonths)).formatNumber():"0.00";
            }

            rect.Dispose();


            table1.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE  epay.[usp_get_TAX_computation_summary] @eid = " + eid + ",@year = " + year + ",@remainingMonths="+ (12-remainingMonths) +"").Tables[0];


            this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_report_parameter] 1,'0'," + year + ",1,"+USER.C_eID+"").Tables[0];
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        private static string getrmonths(int remainingmonths){
            string r="";
            switch (remainingmonths){

                case 12:

                    r = "January-December";
                    break;
                case 11:
                    r = "February-December";
                    break;
                case 10:
                    r = "March-December";
                    break;
                case 9:
                    r = "April-December";
                    break;
                case 8:
                    r = "May-December";
                    break;
                case 7:
                    r = "June-December";
                    break;
                case 6:
                    r = "July-December";
                    break;
                case 5:
                    r = "August-December";
                    break;
                case 4:
                    r = "September-December";
                    break;
                case 3:
                    r = "October-December";
                    break;
                case 2:
                    r = "November-December";
                    break;
                case 1:
                    r = "December";
                    break;
            }
            return r;
        }
        private static string getpmonths(int remainingmonths)
        {
            string r = "";
            switch (remainingmonths)
            {

                case 1:

                    r = "January";
                    break;
                case 2:
                    r = "January-February";
                    break;
                case 3:
                    r = "January-March";
                    break;
                case 4:
                    r = "January-April";
                    break;
                case 5:
                    r = "January-May";
                    break;
                case 6:
                    r = "January-June";
                    break;
                case 7:
                    r = "January-July";
                    break;
                case 8:
                    r = "January-August";
                    break;
                case 9:
                    r = "January-September";
                    break;
                case 10:
                    r = "January-October";
                    break;
                case 11:
                    r = "January-November";
                    break;
                case 12:
                    r = "January-December";
                    break;
            }
            return r;
        }
    }
}