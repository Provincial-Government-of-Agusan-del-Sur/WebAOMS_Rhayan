namespace WebAOMS.Report.Design.Document
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
    using WebAOMS.ws_tracking;
    using System.Web.Mvc;

    /// <summary>
    /// Summary description for rpt_document_training_design.
    /// </summary>
    public partial class rpt_document_training_design : Telerik.Reporting.Report
    {
        public rpt_document_training_design(int report_id,long doc_details_id)
        {
            //
            TrackingSoapClient ws = new TrackingSoapClient();
            string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            
            string refno = "";
            string cmdStr = "execute [Accounting].[usp_rpt_Doc_details_test_v1] @report_id, @doc_details_id,@userid";
            SqlConnection connection = new SqlConnection(fmisConn);
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@report_id", SqlDbType.Int).Value = report_id;
                command.Parameters.Add("@doc_details_id", SqlDbType.Int).Value = doc_details_id;
                command.Parameters.Add("@userid", SqlDbType.Int).Value = USER.C_eID;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows == true)
                {
                    while (reader.Read())
                    {
                       htmlTextBox1.Value = HttpUtility.HtmlDecode(reader["script"].ToString()).Replace("&nbsp;&nbsp;", "");
                        ReportParameters["CompanyName"].Value = reader["CompanyName"].ToString();
                        ReportParameters["CompanyAddress"].Value = reader["CompanyAddress"].ToString();
                        ReportParameters["T1"].Value = reader["T1"].ToString();
                        ReportParameters["T2"].Value = reader["T2"].ToString();
                        refno = reader["refno"].ToString();
                        txt_office.Value =reader["T1"].ToString();
                        txt_title.Value = reader["T2"].ToString();
                        pictureBox1.Value = reader["CompanyLogo"].ToString();
                        txt_user.Value = reader["preparedby"].ToString();
                        txt_user_position.Value = reader["preparedPosition"].ToString();
                        txt_head.Value = reader["head"].ToString();
                        txt_head_position.Value = reader["head_position"].ToString();
                        txt_approve.Value = reader["approve"].ToString();
                        txt_approve_position.Value = reader["approve_position"].ToString();
                        txt_title.Value = reader["reportname"].ToString();
                        pictureBox3.Value = reader["officeLogo"].ToString();

                        txt_label_approve.Value = reader["approve_label"].ToString();
                    }
                }
            }
            //pictureBox3.Value = "Content/office/1.png";
            brcode_UI.Value = refno;
            txt_qrcode.Value = refno;
            pictureBox4.Value = ISfn.QRGen(ws.get_tracking_link(refno).ToString(), 4);
            
            txt_qrcode.Value = refno;
        }
    }
}