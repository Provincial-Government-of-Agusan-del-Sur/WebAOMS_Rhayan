namespace WebAOMS.Report.Design
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using WebAOMS.Report.Design;
    using System.Data.SqlClient;
    using System.Configuration;
    using WebAOMS.Base;
    /// <summary>
    /// Summary description for rpt_log_printed.
    /// </summary>
    public partial class rpt_log_printed : Telerik.Reporting.Report
    {
        string fmisConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public rpt_log_printed(DateTime from, DateTime to ,int status_id)
        {
            
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            DataSet dt = new DataSet();
            SqlConnection connection = new SqlConnection(fmisConn);
            string cmdStr = "execute Accounting.usp_print_log @from,@to,@status_id ";
            using (SqlCommand command = new SqlCommand(cmdStr, connection))
            {
                command.Parameters.Add("@from", SqlDbType.Date).Value = from;
                command.Parameters.Add("@to", SqlDbType.Date).Value = to;
                command.Parameters.Add("@status_id", SqlDbType.Int).Value = status_id;
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
            this.DataSource = dt.Tables[1];
            table1.DataSource = dt.Tables[0];
            txt_user.Value = USER.C_NameFML;
            txt_user_position.Value = USER.C_Position;
                
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}