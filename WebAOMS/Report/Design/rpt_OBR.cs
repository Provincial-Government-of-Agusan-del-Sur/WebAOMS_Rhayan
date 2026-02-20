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
    using WebAOMS.Mod;
    /// <summary>
    /// Summary description for rpt_OBR.
    /// </summary>
    public partial class rpt_OBR : Telerik.Reporting.Report
    {
        
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_OBR(Int32 dvid,string refno)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";

            DataSet dt = new DataSet();

                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "execute  [Accounting].[usp_rpt_OBR_details] @dvid";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    
                    command.Parameters.Add("@dvid",SqlDbType.Int).Value = dvid;
                    connection.Open();

                    SqlDataAdapter dr = new SqlDataAdapter(command);
                    dr.Fill(dt);
                    connection.Close();
                }

            //table1.DataSource = dt.Tables[0];
            this.table1.DataSource = dt.Tables[0];
            table2.DataSource = dt.Tables[1];
            this.DataSource = dt.Tables[1];
            pictureBox2.Value = ISfn.QRGen(Track.get_tracking_link(refno).ToString(), 4);
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}