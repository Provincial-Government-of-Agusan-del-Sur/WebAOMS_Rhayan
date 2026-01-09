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
    /// <summary>
    /// Summary description for rpt_SD_casual.
    /// </summary>
    public partial class rpt_SD_casual : Telerik.Reporting.Report
    {
        public rpt_SD_casual(int year, int month, int officeID,int fundsource)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            string UI = ISfn.newguid();

            ISfn.LogReportGuid(ISfn.CreateRegularPayBatchno(officeID, year, month), UI, 12);
            obj_otherdetails.DataSource = ISfn.ExecuteDataset("EXECUTE pmis.[dbo].[sp_Payroll_reportDetails] 1," + officeID + "," + year + "," + month + ",3");
            table1.DataSource = ISfn.ExecuteDataset("EXECUTE [dbo].[sp_GridDatasource] @GridDatasourceID = 1075,@para1 = " + officeID + ",@para2 = "+ fundsource +",@UserType = 0");
            brcode_UI.Value = UI;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}