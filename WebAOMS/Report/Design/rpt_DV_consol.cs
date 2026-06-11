namespace WebAOMS.Report.Design
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using WebAOMS.Report.Design;
    using System.Data;
    using WebAOMS.Base;
    using System.Data.SqlClient;
    using System.Configuration;
    using WebAOMS.Mod;
    using System.Web.Mvc;
    /// <summary>
    /// Summary description for rpt_DV_consol.
    /// </summary>
    public partial class rpt_DV_consol : Telerik.Reporting.Report
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_DV_consol()
        {
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
            table3.DataSource = dt.Tables[0];
            table2.DataSource = dt.Tables[1];
            this.DataSource = dt.Tables[1];
            pictureBox2.Value = ISfn.QRGen(Track.get_tracking_link(refno).ToString(), 4);
        }
    }
}