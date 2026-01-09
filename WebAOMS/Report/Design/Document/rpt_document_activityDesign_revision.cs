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
    using WebAOMS.Controllers;

    /// <summary>
    /// Summary description for rpt_document_activityDesign_revision.
    /// </summary>
    public partial class rpt_document_acivityDesign_revision : Telerik.Reporting.Report
    {
        public rpt_document_acivityDesign_revision(int report_id, long doc_details_id)
        {
            TrackingController ws = new TrackingController();
            string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            //if (report_id == 48)
            //{
            //    report_id = 53;
            //}
            //else if (report_id == 49)
            //{
            //    report_id = 54;
            //}
            DataSet dt = new DataSet();
            string refno = "";
            string cmdStr = "execute [Accounting].[usp_rpt_Doc_details_revision_v2] @report_id, @doc_details_id,@userid";
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
                        txt_activity_title.Value = reader["Title"].ToString();
                        txt_dte_from.Value = reader["DTE_from"].ToString();
                        txt_venue_from.Value = reader["venue_from"].ToString();
                        txt_dte_to.Value = reader["DTE_to"].ToString();
                        txt_venue_to.Value = reader["venue_to"].ToString();
                        txt_reason.Value = reader["reason"].ToString();

                        ReportParameters["CompanyName"].Value = reader["CompanyName"].ToString();
                        ReportParameters["CompanyAddress"].Value = reader["CompanyAddress"].ToString();
                        ReportParameters["T1"].Value = reader["T1"].ToString();
                        ReportParameters["T2"].Value = reader["T2"].ToString();
                        refno = reader["refno"].ToString();
                        txt_office.Value = reader["T1"].ToString();
                        txt_title.Value = reader["T2"].ToString();
                        pictureBox1.Value = reader["CompanyLogo"].ToString();
                        txt_user.Value = reader["preparedby"].ToString();

                        txt_user.Value = reader["preparedby"].ToString();

                        htmlTextBox1.Value = reader["FullrecommendingApproval"].ToString();

                        txt_approve.Value = reader["approve"].ToString();


                        txt_title.Value = reader["reportname"].ToString();
                        pictureBox3.Value = reader["officeLogo"].ToString();
                        txt_label_approve.Value = reader["approve_label"].ToString();
                        if (reader["venue_to"].ToString() == "Nothing Change")
                        {
                            panel_venue.Visible = false;
                        }
                        if (reader["DTE_to"].ToString() == "Nothing Change")
                        {
                            panel_dte.Visible = false;
                        }
                    }
                }

                connection.Close();
            }

            //pictureBox3.Value = "Content/office/1.png";
            brcode_UI.Value = refno;
            txt_qrcode.Value = refno;
            pictureBox4.Value = ISfn.QRGen(ws.get_tracking_link(refno).ToString(), 4);

            txt_qrcode.Value = refno;
        }
    }
}