namespace WebAOMS.Report.Design.Document
{
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
    /// Summary description for rpt_document_trainingDesign2025.
    /// </summary>
    public partial class rpt_document_trainingDesign2025 : Telerik.Reporting.Report
    {
        public rpt_document_trainingDesign2025(int report_id, long doc_details_id)
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
            string cmdStr = "execute [Accounting].[usp_rpt_Doc_details_v2_test] @report_id, @doc_details_id,@userid";
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
                        txt_inclusive_date.Value = reader["InclusiveDate"].ToString();
                        txt_venue.Value = reader["Venue"].ToString();
                        txt_rationale.Value = HttpUtility.HtmlDecode(reader["Rationale"].ToString()).Replace("&nbsp;&nbsp;", "");
                        txt_objective.Value = HttpUtility.HtmlDecode(reader["Objectives"].ToString()).Replace("&nbsp;&nbsp;", "");
                        txt_WBAobjectives.Value = HttpUtility.HtmlDecode(reader["WAObjectives"].ToString()).Replace("&nbsp;&nbsp;", "");
                        txt_RObjectives.Value = HttpUtility.HtmlDecode(reader["RObjectives"].ToString()).Replace("&nbsp;&nbsp;", "");
                        txt_Evaluation.Value = HttpUtility.HtmlDecode(reader["Evaluation"].ToString()).Replace("&nbsp;&nbsp;", "");
                        txt_Lobjectives.Value = HttpUtility.HtmlDecode(reader["LObjectives"].ToString()).Replace("&nbsp;&nbsp;", "");
                        textBox4.Value = HttpUtility.HtmlDecode(reader["ExpectedOutput"].ToString()).Replace("&nbsp;&nbsp;", "");


                        txt_fund.Value = reader["SourceOfFund"].ToString();
                        txt_caterer.Value = reader["Caterer"].ToString();

                        ReportParameters["CompanyName"].Value = reader["CompanyName"].ToString();
                        ReportParameters["CompanyAddress"].Value = reader["CompanyAddress"].ToString();
                        ReportParameters["T1"].Value = reader["T1"].ToString();
                        ReportParameters["T2"].Value = reader["T2"].ToString();
                        refno = reader["refno"].ToString();
                        txt_office.Value = reader["T1"].ToString();
                        txt_title.Value = reader["T2"].ToString();
                        pictureBox1.Value = reader["CompanyLogo"].ToString();


                        txt_user.Value = reader["preparedby"].ToString();

                        htmlTextBox1.Value = reader["FullrecommendingApproval"].ToString();

                        txt_approve.Value = reader["approve"].ToString();

                        txt_title.Value = reader["reportname"].ToString();
                        pictureBox3.Value = reader["officeLogo"].ToString();
                        txt_label_approve.Value = reader["approve_label"].ToString();
                    }
                }

                connection.Close();
            }


            string cmdtbl = "exec [Accounting].[usp_rpt_Doc_Event_082025] @eventID";
            using (SqlCommand cmd = new SqlCommand(cmdtbl, connection))
            {
                cmd.Parameters.AddWithValue("@eventID", doc_details_id);
                connection.Open();
                SqlDataAdapter dr = new SqlDataAdapter(cmd);
                dr.Fill(dt);
                connection.Close();
            }

            tbl_targetpar.DataSource = dt.Tables[0];//target participant
            tbl_budget.DataSource = dt.Tables[1];//budgetary requirments



            tbl_ips.DataSource = dt.Tables[2];//IPS

            //pictureBox3.Value = "Content/office/1.png";
            brcode_UI.Value = refno;
            txt_qrcode.Value = refno;
            pictureBox4.Value = ISfn.QRGen(ws.get_tracking_link(refno).ToString(), 4);

            txt_qrcode.Value = refno;
        }
    }
}