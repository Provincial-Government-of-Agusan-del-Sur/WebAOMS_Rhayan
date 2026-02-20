namespace WebAOMS.Report.Design
{
    using System.Data;
    using WebAOMS.Base;
    using System.Data.SqlClient;
    using System.Configuration;
    using WebAOMS.ws_tracking;
    using WebAOMS.Mod;
    /// <summary>
    /// Summary description for rpt_DV_2021.
    /// </summary>
    public partial class rpt_DV_2021 : Telerik.Reporting.Report
    {
        TrackingSoapClient ws = new TrackingSoapClient();
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_DV_2021(System.Int32 dvid, string refno)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";

            DataSet dt = new DataSet();

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute  [Accounting].[usp_rpt_DV_details] @dvid";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {

                command.Parameters.Add("@dvid", SqlDbType.Int).Value = dvid;
                connection.Open();

                SqlDataAdapter dr = new SqlDataAdapter(command);
                dr.Fill(dt);
                connection.Close();
            }

            this.table1.DataSource = dt.Tables[0];
            this.table4.DataSource = dt.Tables[0];
            
            table2.DataSource = dt.Tables[1];
            this.DataSource = dt.Tables[1];
            pictureBox2.Value = ISfn.QRGen(Track.get_tracking_link(refno).ToString(), 4);
            brcode_UI.Value = refno;
        }
    }
}