namespace WebAOMS.Report.Design
{
    using System;
    using System.Data;
    using WebAOMS.Base;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Web;
    /// <summary>
    /// Summary description for rpt_Query_saaocc.
    /// </summary>
    public partial class rpt_Query_saaocc : Telerik.Reporting.Report
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_Query_saaocc(Int16 fundid, DateTime to, int hideAccountname,int hideOOEname, int reportid)
        {

            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            DataSet dt = new DataSet();
            string filename = "";
            
            string refno = "";

            refno = reportid.ToString("000") + fundid.ToString("000") + to.ToString("MMddyyyy");
            filename = HttpContext.Current.Server.MapPath("~/xmlreport/") + refno;

            if (ISfn.checkIfRegenerateReport(refno) == 0)
            {
                dt.ReadXml(filename);
            }
            else
            {
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "exec [Accounting].[usp_rpt_Query_RAAOUP] @userid ,@fundid ,@to,@report_id";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                    command.Parameters.AddWithValue("@fundid", fundid);
                    command.Parameters.AddWithValue("@to", to);
                    command.Parameters.AddWithValue("@report_id", reportid);
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
            
            this.DataSource = dt.Tables[1];
            table1.DataSource = dt.Tables[2];


            if (hideAccountname == 1)
            {
                detail.Visible = false;
            }

            if (hideOOEname == 1)
                groupHeaderSection2.Visible = false;
            }
    }
}