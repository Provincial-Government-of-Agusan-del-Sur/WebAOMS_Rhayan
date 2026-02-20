namespace WebAOMS.Report.Design
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using WebAOMS.Base;
    using WebAOMS.Report.Design;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Web;
    using System.IO;
    /// <summary>
    /// Summary description for rpt_Financial_trialbalance_consolidated.
    /// </summary>
    public partial class Trial_Balance_Consolidated : Telerik.Reporting.Report
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public Trial_Balance_Consolidated(Int16 fundid, int IsPreClosing, DateTime to)
        {
            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            DataSet dt = new DataSet();
            string filename = "";
            Int16 reportid = 1;
            string refno = "";


            refno = IsPreClosing.ToString("0") + reportid.ToString("000") + fundid.ToString("000") + to.ToString("MMddyyyy");
            filename = HttpContext.Current.Server.MapPath("~/xmlreport/") + refno;

            if (ISfn.checkIfRegenerateReport(refno) == 0)
            {
                dt.ReadXml(filename);
            }
            else
            {
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "exec [Accounting].[usp_rpt_Financial_TrialBalance] @userid ,@fundid ,@to ,@IsPreClosing";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                    command.Parameters.AddWithValue("@fundid", fundid);
                    command.Parameters.AddWithValue("@IsPreClosing", IsPreClosing);
                    command.Parameters.AddWithValue("@to", to);
                    connection.Open();

                    SqlDataAdapter dr = new SqlDataAdapter(command);
                    dr.Fill(dt);
                    connection.Close();
                }
                dt.WriteXml(filename, XmlWriteMode.WriteSchema);
            }

            DataTable header = dt.Tables[0];

            if (header.Rows.Count > 0)
            {
                ReportParameters["CompanyName"].Value = header.Rows[0]["CompanyName"].ToString();
                ReportParameters["CompanyAddress"].Value = header.Rows[0]["CompanyAddress"].ToString();
                ReportParameters["T1"].Value = header.Rows[0]["T1"].ToString();
                ReportParameters["dateString"].Value = header.Rows[0]["dateString"].ToString();
                ReportParameters["fundtype"].Value = header.Rows[0]["fundtype"].ToString();
                txt_generated_text.Value = header.Rows[0]["generated_text"].ToString();
                txt_AccountantName.Value = header.Rows[0]["AccountantName"].ToString();
                txt_AccountantPosition.Value = header.Rows[0]["AccountantPosition"].ToString();
            }
            this.table1.DataSource = dt.Tables[1];
        }
    }
}