namespace IFMIS.Report.Design
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
    /// <summary>
    /// Summary description for rpt_GeneralPayroll.
    /// </summary>
    public partial class rpt_GeneralPayroll_diff : Telerik.Reporting.Report
    {
        public rpt_GeneralPayroll_diff(int year, int month, int officeID)
        {
            // Required for telerik Reporting designer support
            InitializeComponent();
            // TODO: Add any constructor code after InitializeComponent call
            string UI = ISfn.newguid();

            ISfn.LogReportGuid(ISfn.CreateRegularPayBatchno(officeID,year,month), UI,11);
            obj_otherdetails.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_reportDetails] 1," + officeID + "," + year + "," + month + ",3").Tables[0];
            table1.DataSource = ISfn.ExecuteDataset("EXECUTE [dbo].[sp_GridDatasource] @GridDatasourceID = 1069,@para1 = "+officeID+",@UserType = 0");
            brcode_UI.Value = UI;
        }
    }
}