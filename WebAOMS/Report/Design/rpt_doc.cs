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
    using WebAOMS.ws_tracking;
    using System.Web;

    public partial class rpt_doc : Telerik.Reporting.Report
    {
        string dbcon_fmis = ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString();
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_doc()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            DataSet dt = new DataSet();
           
            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_rpt_Doc_details]  7,33";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
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
                this.htmlTextBox2.Value = HttpUtility.HtmlDecode(header.Rows[0]["script"].ToString());
            }
        }
    }
}