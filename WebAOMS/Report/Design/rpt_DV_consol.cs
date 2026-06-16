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
            //pictureBox1.Value = "Content/Company Image/CompanyLogo.png";

            DataSet dt = new DataSet();


            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute  [Accounting].[usp_JEV_Get_DV_consolidated] '101-1011-26-02-2396'";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {

                command.Parameters.Add("@obrno", SqlDbType.Int).Value = 0;
                connection.Open();

                SqlDataAdapter dr = new SqlDataAdapter(command);
                dr.Fill(dt);
                connection.Close();
            }

            this.table1.DataSource = dt.Tables[0];

            //  pictureBox2.Value = ISfn.QRGen(Track.get_tracking_link(refno).ToString(), 4);
        }
    }
}