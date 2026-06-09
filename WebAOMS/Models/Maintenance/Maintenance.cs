using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAOMS.Models.Maintenance
{
    public class grid_claimant
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
    public class grid_proc_item
    {
        public string itemid { get; set; }
        public string itemname { get; set; }
        public string subcategory { get; set; }
        public string chartcode { get; set; }
        
    }

    public class grid_Item_Category
    {
        public Int32 itemgroupid { get; set; }
        public string itemcategory { get; set; }
        public string accountcode { get; set; }

    }
    public class grid_menu
    {
        public int menu_id { get; set; }
        public string MenuName { get; set; }
    }
    public class grid_office
    {
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
    }
    public class grid_user_office
    {
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
    }
    public class grid_user_menu
    {
        public int t_menu_user_id { get; set; }
        public int menu_id { get; set; }
        public string MenuName { get; set; }
    }
    public class grid_claimant_eproc
    {
        public string supplierid { get; set; }
        public string supplier { get; set; }
        public string address { get; set; }
    }
    public class grid_accountantadvice
    {
        public long jevid { get; set; }
        public string AdviceNo { get; set; }
        public string ChkBankAccntNo { get; set; }
        public string Checkno { get; set; }
        public string CheckDate { get; set; }
        public string Name { get; set; }
        public double ChkAmount { get; set; }
        public string Filename { get; set; }
    }
    public class grid_monitoring_activitydesign
    {
        public long doc_form_id { get; set; }
        public string Office { get; set; }
        public string Name { get; set; }
        public string Particular { get; set; }
        public double Amount { get; set; }
        public string DTE { get; set; }
        public string inclusivedte { get; set; }
        public string refno  { get; set; }
        public string Status_name { get; set; }
        public int status_code { get; set; }
        public int UserID { get; set; }


    }
    public class grid_accountingentries
    {
        public string refno { get; set; } 
	    public int ChartAccountChildID { get; set; }
	    public int AccountChildParentID { get; set; }
	    public string code { get; set; }
        public string AccountChildName { get; set; }
        public string ChildCode { get; set; }
        public int hasChild { get; set; }
        public double? debit { get; set; }
        public double? credit { get; set; }
        public int GLChartAccountChildID { get; set; }
    }

    public class controltransaction
    {
        public string particular { get; set; }
        public double amount { get; set; }
        public string cafoano { get; set; }
    }

}