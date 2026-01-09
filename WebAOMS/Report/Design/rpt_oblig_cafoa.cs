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
    /// Summary description for rpt_oblig_cafoa.
    /// </summary>
    public partial class rpt_oblig_cafoa : Telerik.Reporting.Report
    {
        TrackingSoapClient ws = new TrackingSoapClient();
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_oblig_cafoa(Int32 obligid)
         {

            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";

            DataSet dt = new DataSet();

            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute  [Accounting].[usp_rpt_Oblig_cafoa_details] @obligid";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {

                command.Parameters.Add("@obligid", SqlDbType.Int).Value = obligid;
                connection.Open();

                SqlDataAdapter dr = new SqlDataAdapter(command);
                dr.Fill(dt);
                connection.Close();
            }
            //table1.DataSource = dt.Tables[0];
            //this.table1.DataSource = dt.Tables[0];
            //table2.DataSource = dt.Tables[1];
            tbl_account.DataSource = dt.Tables[2];
            table1.DataSource = dt.Tables[3];
            this.DataSource = dt.Tables[1];
            pictureBox2.Value = ISfn.QRGen(obligid.ToString(), 4);
        }
    }
    }