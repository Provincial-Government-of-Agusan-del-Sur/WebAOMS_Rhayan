namespace IFMIS.Report.Design.Payroll
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using IFMIS.Base;
    using System.Configuration;
    using IFMIS.Report.Design;
    using System.Web.Mvc;
    using IFMIS.Classess;

    /// <summary>
    /// Summary description for rpt_Certification_Mandatory.
    /// </summary>
    public partial class rpt_Certification_Mandatory : Telerik.Reporting.Report
    {
        public rpt_Certification_Mandatory(int eid, int from, int to, int payreffid)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            pictureBox1.Value = "Content/Company Image/CompanyLogo.png";
            this.DataSource = ISfn.ExecuteDataset("execute Epay.[usp_report_parameter] 1,0,2018,1,"+ USER.C_eID +" ");
            this.table1.DataSource = ISfn.ExecuteDataset("execute epay.[usp_Get_indexofpayment_details] " + eid + "," + from + "," + to + "," + payreffid + "");

            DataTable rec = ISfn.ExecuteDataset("execute Epay.[usp_Get_indexofpayment_employee] " + eid + "," + payreffid + "");
            if (rec.Rows.Count > 0)
            {
                ReportParameters["name"].Value = rec.Rows[0]["name"].ToString();
                ReportParameters["description"].Value = rec.Rows[0]["description"].ToString();
                ReportParameters["position"].Value = rec.Rows[0]["position"].ToString();
                ReportParameters["employmentstatus"].Value = rec.Rows[0]["employmentstatus"].ToString();
                ReportParameters["fdos"].Value = rec.Rows[0]["fdos"].ToString();
            }

            string UI = ISfn.gen_GUID();
            brcode_UI.Value = UI;
            txtUser.Value = USER.C_NameFML;
            txtUserposition.Value = USER.C_Position.ToString();
            ISfn.ActionLog("Preview Report(rpt_Certification_mandatory) " + eid + "," + from + "," + to + "," + payreffid + "");
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}