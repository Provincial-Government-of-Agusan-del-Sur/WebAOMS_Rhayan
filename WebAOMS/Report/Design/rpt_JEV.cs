namespace WebAOMS.Report.Design
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using Base;
    using System.Data;
    /// <summary>
    /// Summary description for rpt_JEV.
    /// </summary>
    public partial class rpt_JEV : Telerik.Reporting.Report
    {
        public rpt_JEV(Int64 jevid)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            DataSet dset;
            DataTable jevdetails;
            DataTable jeventries;
            DataTable jevSignatories;
            int transtype;
            dset = ISfn.ToDataset("execute Accounting.usp_JEV_print_Entries "+jevid+"");
            //
            jevdetails = dset.Tables[0];
            jeventries = dset.Tables[1];

            jevSignatories = dset.Tables[2];

            this.DataSource = jevdetails;
            if (jevdetails.Rows.Count > 0)
            {
                transtype = Convert.ToInt16(jevdetails.Rows[0]["transtype"]);
                txt_particular.Value = jevdetails.Rows[0]["particular"].ToString();
                txt_rc.Value = jevdetails.Rows[0]["officename"].ToString();
                checkBox1.Value = false;
                checkBox2.Value = false;
                checkBox3.Value = false;
                checkBox4.Value = false;
                checkBox5.Value = false;
                checkBox6.Value = false;
                if (transtype == 1)
                {
                    checkBox1.Value = true;
                }
                else if (transtype == 2)
                {
                    checkBox2.Value = true;
                }
                else if (transtype == 3)
                {
                    checkBox3.Value = true;
                }
                else if (transtype == 4)
                {
                    checkBox4.Value = true;
                }
                else if (transtype == 5)
                {
                    checkBox5.Value = true;
                }
                else if (transtype == 6)
                {
                    checkBox6.Value = true;
                }
            }
            if (jevSignatories.Rows.Count > 0)
            {
                txt_accountant.Value =jevSignatories.Rows[0]["AccountingName"].ToString();
                txt_Accountant_pos.Value = jevSignatories.Rows[0]["AccountingPosition"].ToString();
                textBox1.Value = jevSignatories.Rows[0]["CompanyName"].ToString();
                
            }

            table1.DataSource = jeventries;
            
            txt_userid.Value = USER.C_NameFML;
            txt_userPosition.Value = USER.C_Position;
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
    }
}