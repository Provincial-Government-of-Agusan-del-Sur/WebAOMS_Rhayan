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
    /// <summary>
    /// Summary description for rpt_Cancelled_check_v2.
    /// </summary>
    public partial class rpt_Cancelled_check_v2 : Telerik.Reporting.Report
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_Cancelled_check_v2(Int32 tyear)
        {
            
            InitializeComponent();

            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            DataSet dt = new DataSet();
            //string filename = "";
            //Int16 reportid = 1;
            //string refno = "";


            //refno = IsPreClosing.ToString("0") + reportid.ToString("000") + fundid.ToString("000") + to.ToString("MMddyyyy");
            //filename = ISfn
            //if (ISfn.checkIfRegenerat.xmlpath(reportid) + refno;eReport(refno) == 0)
            //{
            //    dt.ReadXml(filename);
            //}
            //else
            //{
                SqlConnection connection = new SqlConnection(fmisConn);
                string cmdStr = "exec [Accounting].[usp_Cancelled_check] @tyear";
                using (SqlCommand command = new SqlCommand(cmdStr, connection))
                {
                    command.Parameters.AddWithValue("@tyear", tyear);
                    connection.Open();

                    SqlDataAdapter dr = new SqlDataAdapter(command);
                    dr.Fill(dt);
                    connection.Close();
                }

            //    dt.WriteXml(filename, XmlWriteMode.WriteSchema);
            //}

            DataTable header = dt.Tables[1];

            if (header.Rows.Count > 0)
            {
                ReportParameters["CompanyName"].Value = header.Rows[0]["CompanyName"].ToString();
                ReportParameters["CompanyAddress"].Value =  header.Rows[0]["CompanyAddress"].ToString();
                ReportParameters["Status"].Value = header.Rows[0]["Status"].ToString();
                
            }
            this.table1.DataSource = dt.Tables[0];
        }
    }
}