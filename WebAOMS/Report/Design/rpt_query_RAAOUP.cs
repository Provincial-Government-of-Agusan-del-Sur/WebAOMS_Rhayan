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
    /// Summary description for Report1.
    /// </summary>
    public partial class Status_AAOU : Telerik.Reporting.Report
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public Status_AAOU(Int16 fundid, DateTime to)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            DataSet dt = new DataSet();
            string filename = "";
            Int16 reportid = 23;
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
                string cmdStr = "exec [Accounting].[usp_rpt_Query_RAAOUP] @userid ,@fundid ,@to";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@userid", USER.C_swipeID);
                    command.Parameters.AddWithValue("@fundid", fundid);
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
            this.DataSource = dt.Tables[1];
            table1.DataSource = dt.Tables[2];
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}