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
    using WebAOMS.ws_tracking;

    /// <summary>
    /// Summary description for rpt_DV_rata.
    /// </summary>
    public partial class rpt_DV_rata : Telerik.Reporting.Report
    {
        TrackingSoapClient ws = new TrackingSoapClient();
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_DV_rata(Int32 dvid, string refno)
        {

            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";

            DataSet dt = new DataSet();

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute  [Accounting].[usp_rpt_DV_details_rata] @dvid";
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
            this.table6.DataSource = dt.Tables[0];
            table3.DataSource = dt.Tables[0];
            table2.DataSource = dt.Tables[1];
            this.DataSource = dt.Tables[1];
            
            pictureBox2.Value = ISfn.QRGen(ws.get_tracking_link(refno).ToString(), 4);
             
        }
    }
}