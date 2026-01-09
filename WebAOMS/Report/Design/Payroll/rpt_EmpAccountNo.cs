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
    /// Summary description for rpt_EmpAccountNo.
    /// </summary>
    public partial class rpt_EmpAccountNo : Telerik.Reporting.Report
    {
        public rpt_EmpAccountNo(int officeID,int emptype,int payreffID)
        {
            //
            // Required for telerik Reporting designer support
            //
            var emptypeName = ISfn.Get_EmptypeByEmpStatusID(emptype);
            
            InitializeComponent();
            txtUser.Value = ISfn.ExecScalar("SELECT NameFML FROM [IFMIS].[dbo].[vw_employee_Concatname] where eid = " + USER.C_eID + "").ToString();
            txtUserposition.Value = USER.C_Position.ToString();
            txtemptype.Value = emptypeName.ToString();
            this.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["sqldb"].ToString(), CommandType.Text, "EXECUTE [dbo].[sp_Payroll_reportDetails] 3,"+officeID+",2015,5,3,@reportid = 4").Tables[0];
            obj_details.DataSource = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["ifmisConnString"].ToString(), CommandType.Text,
            "EXECUTE [dbo].[sp_GridDatasource] @GridDatasourceID = 1029,@para2 = '"+emptype+"',@para1="+payreffID+",@para3 ="+officeID+"").Tables[0];
            string UI = Guid.NewGuid().ToString().Substring(24, 12);
            brcode_UI.Value = UI;
        }
    }
}