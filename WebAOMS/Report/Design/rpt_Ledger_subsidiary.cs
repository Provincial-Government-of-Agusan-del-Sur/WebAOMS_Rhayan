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
    /// <summary>
    /// Summary description for rpt_Ledger_subsidiary.
    /// </summary>
    public partial class rpt_Ledger_subsidiary : Telerik.Reporting.Report
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_Ledger_subsidiary(int fundid,DateTime from,DateTime to,string childcode)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            DataSet dt = new DataSet();

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "exec [Accounting].[usp_rpt_Financial_Ledger] @userid ,@fundid ,@from ,@to ,@childcode";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                command.Parameters.AddWithValue("@fundid", fundid);
                command.Parameters.AddWithValue("@from", from);
                command.Parameters.AddWithValue("@to", to);
                command.Parameters.AddWithValue("@childcode", childcode);
                connection.Open();

                SqlDataAdapter dr = new SqlDataAdapter(command);

                dr.Fill(dt);
                connection.Close();

            }
            DataTable header = dt.Tables[0];

            if (header.Rows.Count > 0)
            {
                ReportParameters["CompanyName"].Value = header.Rows[0]["CompanyName"].ToString();
                ReportParameters["CompanyAddress"].Value = header.Rows[0]["CompanyAddress"].ToString();
                ReportParameters["T1"].Value = header.Rows[0]["T1"].ToString();
                ReportParameters["T2"].Value = header.Rows[0]["T2"].ToString();
                ReportParameters["dateString"].Value = header.Rows[0]["dateString"].ToString();
                ReportParameters["fundtype"].Value = header.Rows[0]["fundtype"].ToString();
            }

            this.DataSource = dt.Tables[1];
            table1.DataSource = dt.Tables[0];
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}