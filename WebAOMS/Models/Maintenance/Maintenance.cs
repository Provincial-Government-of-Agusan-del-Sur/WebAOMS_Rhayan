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

}