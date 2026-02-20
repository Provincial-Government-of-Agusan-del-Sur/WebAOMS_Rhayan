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
    using WebAOMS.Mod;
    /// <summary>
    /// Summary description for rpt_tracking_form.
    /// </summary>
    public partial class rpt_tracking_form : Telerik.Reporting.Report
    {   
        
        TrackingSoapClient ws = new TrackingSoapClient();
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_tracking_form(string refno)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            DataSet dt = new DataSet();

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute [Accounting].[usp_rpt_tracking_form] @refno";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@refno", SqlDbType.Char,10).Value = refno;
                connection.Open();
                SqlDataAdapter dr = new SqlDataAdapter(command);
                dr.Fill(dt);
                connection.Close();

            }

            DataTable header = dt.Tables[1];

            if (header.Rows.Count > 0)
            {
                ReportParameters["CompanyName"].Value = header.Rows[0]["CompanyName"].ToString();
                ReportParameters["CompanyAddress"].Value = header.Rows[0]["CompanyAddress"].ToString();
                ReportParameters["T1"].Value = header.Rows[0]["T1"].ToString();
                ReportParameters["T2"].Value = header.Rows[0]["T2"].ToString();
            }

            pictureBox2.Value = "Content/Signatory/333.png";
            pictureBox3.Value = "Content/office/1.png";
            pictureBox4.Value = ISfn.QRGen(Track.get_tracking_link(refno).ToString(),4);
            this.DataSource = dt.Tables[1];
            table1.DataSource = dt.Tables[0];
            brcode_UI.Value = refno;
            txt_qrcode.Value = refno;
        }
    }
}