using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace WebAOMS.Models
{
    public class grid_reviewer_list
    {
        public Int32 doc_form_id { get; set; }
        public Decimal GAmount { get; set; }
        public string Particular { get; set; }
        public string Status_name { get; set; }
        public string InclusiveDate { get; set; }
        public string remarks { get; set; }
        public string refno { get; set; }
        public string OfficeName { get; set; }
        public string NameFML { get; set; }
        public string DTE { get; set; }
    }
    public class ChartOfAccounts
    {
        public Int32 ChartAccountChildID { get; set; }
        public Int32 code { get; set; }
        public Int32 AccountChildParentID { get; set; }
        public string AccountChildName { get; set; }
        public string ChildCode { get; set; }
        public string ChartAccountChildCode { get; set; }
        public Int16 levelNo { get; set; }
        public string isActive { get; set; }
        public string hasChild { get; set; }
        public DateTime ChangeDate { get; set; }
        public string ModifiedByID { get; set; }
        public Double EndingBalance { get; set; }
    }
    public class FilesUpload {
        public int FileId { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public int userID { get; set; }
        public DateTime DTE { get; set; }
    }
    public class l_StatusDescription
    {
        public Int32 status_code { get; set; }
        public Int32 IS_id { get; set; }
        public Int32 prerequisite_id { get; set; }
        public string Status_name { get; set; }
        public string Status_Description { get; set; }
        public string Status_location { get; set; }
        public Int32 MinutesDone { get; set; }
        public string is_active { get; set; }
        public Int32 user_eid { get; set; }
        public DateTime datecreated { get; set; }
        public Int32  orderby { get; set; }
    }
    public class l_Signatory
    {
        public string BudgetName { get; set; }
        public string BudgetPosition { get; set; }
        public string AccountingName { get; set; }
        public string AccountingPosition { get; set; }
        public string PTOName { get; set; }
        public string PTOPosition { get; set; }
        public string HRName { get; set; }
        public string HRPosition { get; set; }
        public string LCEName { get; set; }
        public string LCEPosition { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public Int32 UserID { get; set; }
    }
    public class l_Transaction_type
    {
        public Int16 Transtype_id { get; set; }
        public string TransactionName { get; set; }
        public Int16 isDebit { get; set; }
        public Int32 ChartAccountID { get; set; }
    }
    public class l_tables
    {
        public Int64 object_id { get; set; }
        public string tablename { get; set; }
        public string schemaname { get; set; }        
    }
    public class l_columns
    {
        public Int64 object_id { get; set; }
        public Int64 column_id { get; set; }
        public string name { get; set; }
        public Int64 max_length { get; set; }
    }
    public class usp_JEV_save_transaction_numbering
    {
        public Int64 jevid { get; set; }
        public DateTime Date_ { get; set; }
        public string RCI { get; set; }
        public string Checkno { get; set; }
        public string Particular { get; set; }
        public string JEVno { get; set; }
        public string Claimantcode { get; set; }
        public Decimal Gamount { get; set; }
        public Int32 Transtype { get; set; }
        public string FmisVoucherno { get; set; }
        public string Dvno { get; set; }
        public string Obrno { get; set; }
        public Int32 RCenter { get; set; }
        public string Rdono { get; set; }
        public DateTime Jevdate { get; set; }
        public string Ptvno { get; set; }
        public string JevSeriesNo { get; set; }
        public string PClosing { get; set; }
        public Int16 HaveDoc { get; set; }
        public Int16 isContinuing { get; set; }
        public Int16 isAdjustment { get; set; }
        public Int16 isCA { get; set; }
        public Int16 fundID { get; set; }
        public Int32 userid { get; set; }
    }
    public class l_ClaimantDetails
    {
        public Int64 trnno { get; set; }
        public string ClaimantCode { get; set; }
        public string Firstname { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }        
        public string TIN { get; set; }
        public int clamantType { get; set; }
         
    }
    public class Query_Criteria_raaou
    {
        public string OfficeName { get; set; }
        public string programName { get; set; }
        public string AccountName { get; set; }
        public Decimal Appropropriation { get; set; }
        public Decimal Allotment { get; set; }
        public Decimal Obligation { get; set; }
        public Decimal Utilization { get; set; }
        public Decimal PayableAmount { get; set; }
        public Decimal PayableUtilization { get; set; }

        public Query_Criteria_raaou() { }

        public Query_Criteria_raaou(DataRow row)
        {
            this.OfficeName = (string)row["OfficeName"];
            this.programName = (string)row["programName"];
            this.AccountName = (string)row["AccountName"];
            this.Appropropriation = (decimal)row["Appropropriation"];
            this.Allotment = (decimal)row["Allotment"];
            this.Obligation = (decimal)row["Obligation"];
            this.Utilization = (decimal)row["Utilization"];
            this.PayableAmount = (decimal)row["PayableAmount"];
            this.PayableUtilization = (decimal)row["PayableUtilization"];


        }
        

    }
    public class t_base_custom
    {
        public Int32 custom_eid { get; set; }
        public Nullable<decimal> PagIbigPS { get; set; }
        public Nullable<decimal> Wtax { get; set; }
        public Nullable<decimal> AdditionIncome { get; set; }
        public Nullable<decimal> UnionDues { get; set; }
        public Nullable<decimal> SRA { get; set; }
        public Nullable<decimal> STA { get; set; }
    }
    public class usp_TaxComputation
    {
        public Int32 NoOfMonths { get; set; }
        public Decimal mBasic { get; set; }
        public Decimal AnnualBasic { get; set; }
        public Decimal OtherIncome { get; set; }
        public Decimal Less { get; set; }
        public Decimal OtherIncomeTax { get; set; }
        public Decimal TotalPaidTAX { get; set; }
        public Decimal AdditionalOfExcess { get; set; }
        public Decimal CurrentTax { get; set; }
        public Decimal Bonus { get; set; }
        public Decimal TotalIncome { get; set; }
        public Decimal TotalPS { get; set; }
        public Decimal TaxableIncome { get; set; }
        public Decimal NetTaxableIncome { get; set; }
        public Decimal PercentageOfExcess { get; set; }
        public Decimal GrossTaxw { get; set; }
        public Decimal RemainingTAXDue { get; set; }
        public Decimal MonthlyTaxw { get; set; }
        public Int64 eid { get; set; }
        public string Empname { get; set; }
    }
    public class t_income_deduct
    {
        public Int32 ID { get; set; }
        public Int32 col_id { get; set; }
        public Int32 eid { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public Int32 col_type_id { get; set; }
         
    }

    public class dgsign_pdf
    {
        public string file { get; set; }
        public string code { get; set; }
    }
    public class Remittance_ExportGSIS
    {
        public Decimal? lch_dcs { get; set; }
        public Decimal? stock_purchase { get; set; }
        public Decimal? edu_child { get; set; }
        public Decimal? genspcl { get; set; }
        public Decimal? help { get; set; }
        public Decimal? basic { get; set; }
        public Decimal? ps { get; set; }
        public Decimal? gs { get; set; }
        public Decimal? ec { get; set; }
        public Decimal? consoloan { get; set; }
        public Decimal? ecardplus { get; set; }
        public Decimal? salaryloan { get; set; }
        public Decimal? cash_adv { get; set; }
        public Decimal? emrgyln { get; set; }
        public Decimal? educ_asst { get; set; }
        public Decimal? ela { get; set; }
        public Decimal? sos { get; set; }
        public Decimal? plreg { get; set; }
        public Decimal? plopt { get; set; }
        public Decimal? rel { get; set; }
        public Decimal? opt_life { get; set; }
        public Decimal? ceap { get; set; }
        public Decimal? genesis { get; set; }
        public Decimal? genplus { get; set; }
        public Decimal? genflexi { get; set; }
        public string prefix { get; set; }
        public string crn { get; set; }
        public string datehired { get; set; }
        public string bpno { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string mi { get; set; }
        public string appelation { get; set; }
        public string birthdate { get; set; }
    }
    public class fn_GetRegularActive_by_Amount
    {
        public Decimal Basic { get; set; }
        public Decimal Pera { get; set; }
        public Decimal SLA { get; set; }
        public Decimal Hazard { get; set; }
        public Decimal GSIS_PS { get; set; }
        public Decimal MED_PS { get; set; }
        public Decimal two { get; set; }
        public Decimal PAGIBIG_PS { get; set; }
        public Decimal TAXW { get; set; }
        public Decimal lwop { get; set; }
        public Decimal OTHERDEDUCTIONS { get; set; }
        public Decimal NETAMOUNT { get; set; }
        public Decimal one { get; set; }
        public Int64 eid { get; set; }
        public string EmpName { get; set; }
        public string BIRCategory { get; set; }
    }

    public class CashBook_report_noBeginning {
        public string RecordID { get; set; }
        public DateTime Date { get; set; }
        public string TransactionType { get; set; }
        public string PTVNO { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public string CheckNo { get; set; }
        public string ClaimantName { get; set; }
        public string Particular { get; set; }
        public string userid { get; set; }
        public string ATM { get; set; }
        public Int32 trnno { get; set; }
        public string IRA { get; set; }
        public string Reconciled { get; set; }
        public string ReconciledByNDate { get; set; }
        public string ReconcilingSeqNo { get; set; }
        public string RCINo { get; set; }
        public decimal DebitMonth { get; set; }
        public decimal CreditMonth { get; set; }
        public int status { get; set; }
    }
    public class DocDetailsInclusiveDateModel
    {
        public int doc_details_id_incl { get; set; }
        public int doc_details_id { get; set; }
        public DateTime InclusiveDate { get; set; }
    }
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public DateTime InclusiveDate { get; set; }
        public string Venue { get; set; }
        public string Rationale { get; set; }
        public string Objectives { get; set; }
        public string ExpectedOutput { get; set; }
        public string SourceOfFund { get; set; }
        public string Caterer { get; set; }

        public List<TargetParticipant> TargetParticipants { get; set; }
        public List<BudgetaryRequirement> BudgetaryRequirements { get; set; }
    }

    public class TargetParticipant
    {
        public int ParticipantId { get; set; }
        public int EventId { get; set; }

        [Required(ErrorMessage = "Participant is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Participant must be between 2 and 200 characters.")]
        public string Participant { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "No of pax must be greater than 0.")]
        public int NoOfPax { get; set; }
        
        [UIHint("ClientOffice")]
        public tbl_l_PMISOffice Office
        {
            get;
            set;
        }
        public int OfficeID { get; set; }
        public Event Event { get; set; }
    }
    public class OfficeModel
    {
        public int OfficeID { get; set; }
        public string OfficeAbbr { get; set; }
    }

    public class BudgetaryRequirement
    {
        public int BudgetId { get; set; }
        public int EventId { get; set; }

        [Required(ErrorMessage = "Particular is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Particular must be between 2 and 200 characters.")]
        public string Particular { get; set; }

        [Range(1.0, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        public Event Event { get; set; }
    }
    
    public class DocEvents_IPS
    {
        public Int32 IPS_Id { get; set; }
        public Int32 EventId { get; set; }

        [UIHint("ActivityColumn")]
        public string DTE { get; set; }

        [UIHint("ActivityColumn")]
        public string Activity { get; set; }

        [UIHint("ActivityColumn")]
        public string AssignPerson { get; set; }
        public string userid { get; set; }
        public string bold { get; set; }
        public int orderby { get; set; }

    }
    public class Attachment
    {
        public string attach_id { get; set; }
        public string attach_name { get; set; }
        public string attach_description { get; set; }
        public string IsPrerequisite { get; set; }
        public string orderby { get; set; }

    }
    public class t_DocEvents_process_design
    {
        public Int32 processD_Id { get; set; }
        public Int32 EventId { get; set; }

        [UIHint("ActivityColumn")]
        public string DTE { get; set; }

        [UIHint("ActivityColumn")]
        public string LearningObjectives { get; set; }

        [UIHint("ActivityColumn")]
        public string Activity { get; set; }

        [UIHint("ActivityColumn")]
        public string ExpectedOutput { get; set; }

        [UIHint("ActivityColumn")]
        public string TopicsHighLights { get; set; }
        [UIHint("ActivityColumn")]
        public string LearningMethodology { get; set; }

        [UIHint("ActivityColumn")]
        public string ResourcesNeeded { get; set; }

        [UIHint("ActivityColumn")]
        public string AssignPerson { get; set; }

        public string userid { get; set; }
        public bool bold { get; set; }
        public int orderby { get; set; }

    }
    public class AvailableOffice
    {
        public Int64 OfficeID { get; set; }
        public string Officename { get; set; }
    }
    public class ClientOffice
    {
        public Int64 OfficeID { get; set; }
        public string Officename { get; set; }
    }
    public class SpecificActivityInfo
    {
        public int SpecificActivity_id { get; set; }
        public string SpecificActivity { get; set; }
        public Boolean isplc { get; set; }
    }
    public class data_venue
    {
        public int venueid { get; set; }
        public string venue { get; set; }
        public Boolean isplc { get; set; }
    }
    public class activity_item_details
    {
        public int itemid { get; set; }
        public string itemDetails { get; set; }

    }
    public class activity_item
    {
        public int itemid { get; set; }
        public string item { get; set; }
        public Boolean isplc { get; set; }
        public string SpecificActivity { get; set; }
        public decimal price { get; set; }
        public int qty { get; set; }
        
    }
    public class grid_BudgetaryRequirement
    {
        public int BudgetId { get; set; }
        public int EventId { get; set; }

        [Required(ErrorMessage = "Particular is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Particular must be between 2 and 200 characters.")]
        public string Particular { get; set; }
        public int qty { get; set; }
        public int itemid { get; set; }
        [Range(1.0, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        public Event Event { get; set; }
    }

    public class grid_activitySchedule
    {
        public int accountdenominationid { get; set; }
        public int itemid { get; set; }
        public string item { get; set; }
        public int officeid { get; set; }
        public int office { get; set; }
        public int venueid { get; set; }
        public string venue { get; set; }
        public decimal price { get; set; }
        public int qty { get; set; }
        public string tentativedate { get; set; }
        public Boolean isplc { get; set; }
        public int eid { get; set; }
        public string dateentered { get; set; }
        public string SpecificActivity { get; set; }
        public int SpecificActivity_id { get; set; }
    }
    public class DocTitile {
        public int EventId { get; set; }
        public string Title { get; set; }
    }
   
    public class AspNetUserViewModel
    {
        public string Id { get; set; }
        public int? UserID { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }

    public class PayrollMapped
    {
        public int trnno { get; set; }
        public byte? PayrollType { get; set; }
        public int? DeductionID { get; set; }
        public string childcode { get; set; }
        public int? ChartOfAccountID { get; set; }
        public byte? isDebit { get; set; }
        public string PayrollOrRemittanceID { get; set; }
        public byte? PayrollEmpStatusID { get; set; }
    }

    public class vw_t_Incoming_ObligLiquidation
    {
        public Int64 trans_id { get; set; }
        public string AlobsNo { get; set; }
        public string obrno { get; set; }
        public Decimal OBRAmount { get; set; }
        public Decimal Liquidation { get; set; }
        public Decimal balance { get; set; }
        public string Particulars { get; set; }
        public string ClaimantCode { get; set; }
        public string FundType { get; set; }
        public Int32 functionID { get; set; }
        public Int32 FMISOfficeID { get; set; }
        public Int32 FundID { get; set; }
        public Int32 countperson { get; set; }
        public string OfficeMedium { get; set; }
        public string ModeOfExpenses_Code { get; set; }
    }
    public class vw_Incoming_Received
    {
        public Int64 trans_id { get; set; }
        public string DVNo { get; set; }
        public string DVNo_new { get; set; }
        public string ObrNo { get; set; }
        public string Name { get; set; }
        public Decimal GAmount { get; set; }
        public string Particular { get; set; }
        public string FundType { get; set; }
        public Int32 functionID { get; set; }
        public string OfficeMedium { get; set; }
        public Int32 FMISOfficeID { get; set; }
        public string OOE { get; set; }
        public string ClaimantCode { get; set; }
    }
    public class NoticeViewModel
    {
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int? SignatoryID { get; set; }

        public IEnumerable<SelectListItem> SignatoryList { get; set; }
    }


}