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
    /// Summary description for rpt_Journal_Recap.
    /// </summary>
    public partial class rpt_Journal_Recap : Telerik.Reporting.Report
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_Journal_Recap(Int16 fundid, DateTime to,int transtype,int isConso)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            DataSet dset = new DataSet();
            DataTable recap_sig;
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "exec [Accounting].[usp_rpt_Journal_Recap] @userid,@fundid,@to,@Transtype,@isConso";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                command.Parameters.AddWithValue("@fundid", fundid);
                command.Parameters.AddWithValue("@to", to);
                command.Parameters.AddWithValue("@Transtype", transtype);
                command.Parameters.AddWithValue("@isConso", isConso);
                connection.Open();

                SqlDataAdapter dr = new SqlDataAdapter(command);

                dr.Fill(dset);

                connection.Close();
            }

            recap_sig = dset.Tables[0];

            //this.DataSource = recapdetails;
            if (recap_sig.Rows.Count > 0)
            {
                ReportParameters["CompanyName"].Value = recap_sig.Rows[0]["CompanyName"].ToString();
                ReportParameters["CompanyAddress"].Value = recap_sig.Rows[0]["CompanyAddress"].ToString();
                ReportParameters["T1"].Value = recap_sig.Rows[0]["T1"].ToString();
                ReportParameters["dateString"].Value = recap_sig.Rows[0]["dateString"].ToString();
                ReportParameters["fundtype"].Value = recap_sig.Rows[0]["fundtype"].ToString();
                txt_generated_text.Value = recap_sig.Rows[0]["generated_text"].ToString();
                txt_AccountantName.Value = recap_sig.Rows[0]["AccountantName"].ToString();
                txt_AccountantPosition.Value = recap_sig.Rows[0]["AccountantPosition"].ToString();
            }

            table1.DataSource = dset.Tables[1];
            //this.DataSource = recapdetails;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}