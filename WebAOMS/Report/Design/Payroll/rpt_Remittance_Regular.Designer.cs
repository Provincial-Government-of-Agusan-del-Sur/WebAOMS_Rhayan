namespace IFMIS.Report.Design.Payroll
{
    partial class rpt_Remittance_Regular
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Barcodes.Code128Encoder code128Encoder1 = new Telerik.Reporting.Barcodes.Code128Encoder();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox43 = new Telerik.Reporting.TextBox();
            this.textBox44 = new Telerik.Reporting.TextBox();
            this.textBox45 = new Telerik.Reporting.TextBox();
            this.textBox46 = new Telerik.Reporting.TextBox();
            this.txt_gs = new Telerik.Reporting.TextBox();
            this.textbox22 = new Telerik.Reporting.TextBox();
            this.textBox49 = new Telerik.Reporting.TextBox();
            this.textBox50 = new Telerik.Reporting.TextBox();
            this.textBox51 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.obj_remitlist = new Telerik.Reporting.ObjectDataSource();
            this.reportFooterSection1 = new Telerik.Reporting.ReportFooterSection();
            this.txt_Accountant_position = new Telerik.Reporting.TextBox();
            this.txt_Accountant = new Telerik.Reporting.TextBox();
            this.textBox24 = new Telerik.Reporting.TextBox();
            this.txtUserposition = new Telerik.Reporting.TextBox();
            this.txtUser = new Telerik.Reporting.TextBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox53 = new Telerik.Reporting.TextBox();
            this.textBox54 = new Telerik.Reporting.TextBox();
            this.textBox55 = new Telerik.Reporting.TextBox();
            this.textBox52 = new Telerik.Reporting.TextBox();
            this.textBox10 = new Telerik.Reporting.TextBox();
            this.html_header = new Telerik.Reporting.HtmlTextBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.textBox31 = new Telerik.Reporting.TextBox();
            this.textBox32 = new Telerik.Reporting.TextBox();
            this.txt_ps = new Telerik.Reporting.TextBox();
            this.textBox34 = new Telerik.Reporting.TextBox();
            this.textBox38 = new Telerik.Reporting.TextBox();
            this.textBox39 = new Telerik.Reporting.TextBox();
            this.textBox40 = new Telerik.Reporting.TextBox();
            this.textBox42 = new Telerik.Reporting.TextBox();
            this.textBox41 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.textBox37 = new Telerik.Reporting.TextBox();
            this.brcode_UI = new Telerik.Reporting.Barcode();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox12 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.1800394207239151D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox43,
            this.textBox44,
            this.textBox45,
            this.textBox46,
            this.txt_gs,
            this.textbox22,
            this.textBox49,
            this.textBox50,
            this.textBox51,
            this.textBox9});
            this.detail.Name = "detail";
            this.detail.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.detail.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.detail.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            // 
            // textBox43
            // 
            this.textBox43.Format = "{0:N2}";
            this.textBox43.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.5911960601806641D), Telerik.Reporting.Drawing.Unit.Inch(3.9333768654614687E-05D));
            this.textBox43.Name = "textBox43";
            this.textBox43.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.87806278467178345D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            this.textBox43.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox43.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox43.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox43.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox43.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox43.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox43.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox43.StyleName = "";
            this.textBox43.Value = "= Fields.GSAmount + Fields.PSAmount + Fields.StateInsAmount";
            // 
            // textBox44
            // 
            this.textBox44.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.9272799491882324D), Telerik.Reporting.Drawing.Unit.Inch(3.9333768654614687E-05D));
            this.textBox44.Name = "textBox44";
            this.textBox44.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.46875005960464478D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            this.textBox44.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox44.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox44.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox44.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox44.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox44.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox44.StyleName = "";
            this.textBox44.Value = "=Fields.suffix";
            // 
            // textBox45
            // 
            this.textBox45.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.4939858913421631D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox45.Name = "textBox45";
            this.textBox45.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.43321529030799866D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            this.textBox45.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox45.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox45.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox45.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox45.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox45.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox45.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox45.StyleName = "";
            this.textBox45.Value = "=Fields.mi";
            // 
            // textBox46
            // 
            this.textBox46.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.4730740785598755D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox46.Name = "textBox46";
            this.textBox46.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0208333730697632D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            this.textBox46.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox46.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox46.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox46.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox46.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox46.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox46.StyleName = "";
            this.textBox46.Value = "=Fields.fname";
            // 
            // txt_gs
            // 
            this.txt_gs.Format = "{0:N2}";
            this.txt_gs.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.799992561340332D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.txt_gs.Name = "txt_gs";
            this.txt_gs.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.79112571477890015D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            this.txt_gs.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_gs.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_gs.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_gs.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_gs.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.txt_gs.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.txt_gs.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_gs.Style.Visible = true;
            this.txt_gs.StyleName = "";
            this.txt_gs.Value = "= Fields.GSAmount";
            // 
            // textbox22
            // 
            this.textbox22.Format = "{0:N2}";
            this.textbox22.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textbox22.Name = "textbox22";
            this.textbox22.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.79991424083709717D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            this.textbox22.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textbox22.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textbox22.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textbox22.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textbox22.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textbox22.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textbox22.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textbox22.StyleName = "";
            this.textbox22.Value = "= Fields.PSAmount";
            // 
            // textBox49
            // 
            this.textBox49.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3961091041564941D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox49.Name = "textBox49";
            this.textBox49.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0038912296295166D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            this.textBox49.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox49.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox49.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox49.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox49.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox49.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox49.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox49.Value = "= Fields.MembershipNo";
            // 
            // textBox50
            // 
            this.textBox50.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.431329607963562D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox50.Name = "textBox50";
            this.textBox50.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0416653156280518D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            this.textBox50.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox50.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox50.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox50.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox50.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox50.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox50.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox50.Value = "= Fields.lname";
            // 
            // textBox51
            // 
            this.textBox51.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0041669211350381374D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox51.Name = "textBox51";
            this.textBox51.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.42708417773246765D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            this.textBox51.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox51.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox51.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox51.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox51.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox51.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox51.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox51.Value = "= RowNumber()";
            // 
            // textBox9
            // 
            this.textBox9.Format = "{0:N2}";
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.400078296661377D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.59984356164932251D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            this.textBox9.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox9.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox9.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox9.StyleName = "";
            this.textBox9.Value = "= Fields.StateInsAmount";
            // 
            // obj_remitlist
            // 
            this.obj_remitlist.Name = "obj_remitlist";
            // 
            // reportFooterSection1
            // 
            this.reportFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.6668899059295654D);
            this.reportFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txt_Accountant_position,
            this.txt_Accountant,
            this.textBox24,
            this.txtUserposition,
            this.txtUser,
            this.textBox19,
            this.textBox53,
            this.textBox54,
            this.textBox55,
            this.textBox52,
            this.textBox10});
            this.reportFooterSection1.Name = "reportFooterSection1";
            this.reportFooterSection1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // txt_Accountant_position
            // 
            this.txt_Accountant_position.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.208803653717041D), Telerik.Reporting.Drawing.Unit.Inch(1.3668899536132813D));
            this.txt_Accountant_position.Name = "txt_Accountant_position";
            this.txt_Accountant_position.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.26045560836792D), Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D));
            this.txt_Accountant_position.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_Accountant_position.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_Accountant_position.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txt_Accountant_position.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_Accountant_position.Value = "";
            // 
            // txt_Accountant
            // 
            this.txt_Accountant.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.208803653717041D), Telerik.Reporting.Drawing.Unit.Inch(1.0689733028411865D));
            this.txt_Accountant.Name = "txt_Accountant";
            this.txt_Accountant.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.26045560836792D), Telerik.Reporting.Drawing.Unit.Inch(0.29791674017906189D));
            this.txt_Accountant.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_Accountant.Style.Font.Bold = true;
            this.txt_Accountant.Style.Font.Underline = true;
            this.txt_Accountant.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txt_Accountant.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.txt_Accountant.Value = "";
            // 
            // textBox24
            // 
            this.textBox24.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.808882474899292D), Telerik.Reporting.Drawing.Unit.Inch(0.468972772359848D));
            this.textBox24.Name = "textBox24";
            this.textBox24.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.6603767871856689D), Telerik.Reporting.Drawing.Unit.Inch(0.47696530818939209D));
            this.textBox24.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox24.Style.Font.Bold = true;
            this.textBox24.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox24.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox24.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox24.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox24.StyleName = "";
            this.textBox24.Value = "Certified Correct:";
            // 
            // txtUserposition
            // 
            this.txtUserposition.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.00880330428481102D), Telerik.Reporting.Drawing.Unit.Inch(1.3918899297714233D));
            this.txtUserposition.Name = "txtUserposition";
            this.txtUserposition.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.1540877819061279D), Telerik.Reporting.Drawing.Unit.Inch(0.2749999463558197D));
            this.txtUserposition.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.txtUserposition.Style.Font.Bold = false;
            this.txtUserposition.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.txtUserposition.Style.Font.Underline = false;
            this.txtUserposition.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtUserposition.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtUserposition.StyleName = "";
            this.txtUserposition.Value = "";
            // 
            // txtUser
            // 
            this.txtUser.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.00880330428481102D), Telerik.Reporting.Drawing.Unit.Inch(1.0689733028411865D));
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.1540877819061279D), Telerik.Reporting.Drawing.Unit.Inch(0.322916716337204D));
            this.txtUser.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.txtUser.Style.Font.Bold = true;
            this.txtUser.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.txtUser.Style.Font.Underline = true;
            this.txtUser.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txtUser.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.txtUser.StyleName = "";
            this.txtUser.Value = "";
            // 
            // textBox19
            // 
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.01297047920525074D), Telerik.Reporting.Drawing.Unit.Inch(0.468972772359848D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5958330631256104D), Telerik.Reporting.Drawing.Unit.Inch(0.47696495056152344D));
            this.textBox19.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox19.Style.Font.Bold = true;
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox19.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox19.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox19.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox19.StyleName = "";
            this.textBox19.Value = "Prepared by:";
            // 
            // textBox53
            // 
            this.textBox53.Format = "{0:N2}";
            this.textBox53.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.799992561340332D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox53.Name = "textBox53";
            this.textBox53.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.79112571477890015D), Telerik.Reporting.Drawing.Unit.Inch(0.28125D));
            this.textBox53.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox53.Style.Font.Bold = true;
            this.textBox53.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox53.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox53.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox53.Style.Visible = true;
            this.textBox53.StyleName = "";
            this.textBox53.Value = "=sum(Fields.GSAmount)";
            // 
            // textBox54
            // 
            this.textBox54.Format = "{0:N2}";
            this.textBox54.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox54.Name = "textBox54";
            this.textBox54.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.79991424083709717D), Telerik.Reporting.Drawing.Unit.Inch(0.28125D));
            this.textBox54.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox54.Style.Font.Bold = true;
            this.textBox54.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox54.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox54.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox54.StyleName = "";
            this.textBox54.Value = "=sum(Fields.PSAmount)";
            // 
            // textBox55
            // 
            this.textBox55.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3961091041564941D), Telerik.Reporting.Drawing.Unit.Inch(7.832844858057797E-05D));
            this.textBox55.Name = "textBox55";
            this.textBox55.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0038912296295166D), Telerik.Reporting.Drawing.Unit.Inch(0.28125D));
            this.textBox55.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox55.Style.Font.Bold = true;
            this.textBox55.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox55.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox55.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox55.StyleName = "";
            this.textBox55.Value = "GRAND TOTAL:";
            // 
            // textBox52
            // 
            this.textBox52.Format = "{0:N2}";
            this.textBox52.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.5911960601806641D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox52.Name = "textBox52";
            this.textBox52.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.87806278467178345D), Telerik.Reporting.Drawing.Unit.Inch(0.28124997019767761D));
            this.textBox52.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox52.Style.Font.Bold = true;
            this.textBox52.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox52.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox52.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox52.StyleName = "";
            this.textBox52.Value = "=sum(Fields.StateInsAmount + Fields.GSAmount + Fields.PSAmount)";
            // 
            // textBox10
            // 
            this.textBox10.Format = "{0:N2}";
            this.textBox10.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.4000787734985352D), Telerik.Reporting.Drawing.Unit.Inch(7.8837074397597462E-05D));
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.59984302520751953D), Telerik.Reporting.Drawing.Unit.Inch(0.28125D));
            this.textBox10.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox10.Style.Font.Bold = true;
            this.textBox10.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox10.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox10.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox10.StyleName = "";
            this.textBox10.Value = "=sum(Fields.StateInsAmount)";
            // 
            // html_header
            // 
            this.html_header.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.html_header.Name = "html_header";
            this.html_header.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.4542031288146973D), Telerik.Reporting.Drawing.Unit.Inch(0.59996062517166138D));
            this.html_header.Style.Font.Name = "Arial Narrow";
            this.html_header.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.html_header.Value = "";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.89999997615814209D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.4582910537719727D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox1.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(3D);
            this.textBox1.Style.Font.Bold = false;
            this.textBox1.Style.Font.Italic = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox1.Value = "";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.60000002384185791D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.4582910537719727D), Telerik.Reporting.Drawing.Unit.Inch(0.19992129504680634D));
            this.textBox2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox2.Style.BorderWidth.Bottom = Telerik.Reporting.Drawing.Unit.Point(3D);
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.Value = "";
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.5809875726699829D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.html_header,
            this.textBox1,
            this.textBox2,
            this.textBox31,
            this.textBox32,
            this.txt_ps,
            this.textBox34,
            this.textBox38,
            this.textBox39,
            this.textBox40,
            this.textBox42,
            this.textBox41,
            this.textBox8});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            this.pageHeaderSection1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBox31
            // 
            this.textBox31.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.5911960601806641D), Telerik.Reporting.Drawing.Unit.Inch(1.100078821182251D));
            this.textBox31.Name = "textBox31";
            this.textBox31.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.87917411327362061D), Telerik.Reporting.Drawing.Unit.Inch(0.4791666567325592D));
            this.textBox31.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox31.Style.Font.Bold = true;
            this.textBox31.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox31.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox31.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox31.StyleName = "";
            this.textBox31.Value = "Total Amount";
            // 
            // textBox32
            // 
            this.textBox32.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.799992561340332D), Telerik.Reporting.Drawing.Unit.Inch(1.0999997854232788D));
            this.textBox32.Name = "textBox32";
            this.textBox32.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.79112571477890015D), Telerik.Reporting.Drawing.Unit.Inch(0.4791666567325592D));
            this.textBox32.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox32.Style.Font.Bold = true;
            this.textBox32.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox32.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox32.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox32.Style.Visible = true;
            this.textBox32.StyleName = "";
            this.textBox32.Value = "Government Share";
            // 
            // txt_ps
            // 
            this.txt_ps.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5D), Telerik.Reporting.Drawing.Unit.Inch(1.0999997854232788D));
            this.txt_ps.Name = "txt_ps";
            this.txt_ps.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.79991424083709717D), Telerik.Reporting.Drawing.Unit.Inch(0.47916662693023682D));
            this.txt_ps.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_ps.Style.Font.Bold = true;
            this.txt_ps.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.txt_ps.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.txt_ps.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_ps.StyleName = "";
            this.txt_ps.Value = "Personal Share";
            // 
            // textBox34
            // 
            this.textBox34.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3975358009338379D), Telerik.Reporting.Drawing.Unit.Inch(1.0999997854232788D));
            this.textBox34.Name = "textBox34";
            this.textBox34.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0024644136428833D), Telerik.Reporting.Drawing.Unit.Inch(0.47916662693023682D));
            this.textBox34.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox34.Style.Font.Bold = true;
            this.textBox34.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox34.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox34.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox34.Value = "Membershp No.";
            // 
            // textBox38
            // 
            this.textBox38.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.9287068843841553D), Telerik.Reporting.Drawing.Unit.Inch(1.100078821182251D));
            this.textBox38.Name = "textBox38";
            this.textBox38.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.46875005960464478D), Telerik.Reporting.Drawing.Unit.Inch(0.47916662693023682D));
            this.textBox38.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox38.Style.Font.Bold = true;
            this.textBox38.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox38.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox38.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox38.StyleName = "";
            this.textBox38.Value = "Suffix";
            // 
            // textBox39
            // 
            this.textBox39.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.4911279678344727D), Telerik.Reporting.Drawing.Unit.Inch(1.100078821182251D));
            this.textBox39.Name = "textBox39";
            this.textBox39.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.43749997019767761D), Telerik.Reporting.Drawing.Unit.Inch(0.47916659712791443D));
            this.textBox39.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox39.Style.Font.Bold = true;
            this.textBox39.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox39.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox39.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox39.StyleName = "";
            this.textBox39.Value = "M.I";
            // 
            // textBox40
            // 
            this.textBox40.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.474500298500061D), Telerik.Reporting.Drawing.Unit.Inch(1.1018208265304565D));
            this.textBox40.Name = "textBox40";
            this.textBox40.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0165489912033081D), Telerik.Reporting.Drawing.Unit.Inch(0.47916662693023682D));
            this.textBox40.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox40.Style.Font.Bold = true;
            this.textBox40.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox40.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox40.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox40.StyleName = "";
            this.textBox40.Value = "First Name";
            // 
            // textBox42
            // 
            this.textBox42.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0055933636613190174D), Telerik.Reporting.Drawing.Unit.Inch(1.0999997854232788D));
            this.textBox42.Name = "textBox42";
            this.textBox42.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.42708396911621094D), Telerik.Reporting.Drawing.Unit.Inch(0.47916659712791443D));
            this.textBox42.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox42.Style.Font.Bold = true;
            this.textBox42.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox42.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox42.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox42.Value = "No.";
            // 
            // textBox41
            // 
            this.textBox41.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.43275603652000427D), Telerik.Reporting.Drawing.Unit.Inch(1.0999997854232788D));
            this.textBox41.Name = "textBox41";
            this.textBox41.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0416653156280518D), Telerik.Reporting.Drawing.Unit.Inch(0.47916662693023682D));
            this.textBox41.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox41.Style.Font.Bold = true;
            this.textBox41.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox41.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox41.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox41.Value = "Last Name";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.4000787734985352D), Telerik.Reporting.Drawing.Unit.Inch(1.0999997854232788D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.59984302520751953D), Telerik.Reporting.Drawing.Unit.Inch(0.47916662693023682D));
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox8.Style.Font.Bold = true;
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.StyleName = "";
            this.textBox8.Value = "ECC";
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.60196441411972046D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox37,
            this.brcode_UI,
            this.textBox11,
            this.textBox3,
            this.textBox5,
            this.textBox6,
            this.textBox7,
            this.textBox4,
            this.textBox12});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // textBox37
            // 
            this.textBox37.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.5999999046325684D), Telerik.Reporting.Drawing.Unit.Inch(0.28105241060256958D));
            this.textBox37.Name = "textBox37";
            this.textBox37.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8677165508270264D), Telerik.Reporting.Drawing.Unit.Inch(0.32087293267250061D));
            this.textBox37.Style.Font.Italic = true;
            this.textBox37.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox37.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox37.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox37.Value = "This is a computer generated document.\r\n";
            // 
            // brcode_UI
            // 
            this.brcode_UI.BarAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.brcode_UI.Encoder = code128Encoder1;
            this.brcode_UI.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0041669211350381374D), Telerik.Reporting.Drawing.Unit.Inch(0.28105241060256958D));
            this.brcode_UI.Name = "brcode_UI";
            this.brcode_UI.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.9000003337860107D), Telerik.Reporting.Drawing.Unit.Inch(0.32087293267250061D));
            this.brcode_UI.Style.Font.Italic = false;
            this.brcode_UI.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(0.28105241060256958D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000001668930054D), Telerik.Reporting.Drawing.Unit.Inch(0.32087293267250061D));
            this.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Bottom;
            this.textBox11.Value = "= \"Page \" + PageNumber + \" of \" + PageCount";
            // 
            // textBox3
            // 
            this.textBox3.Format = "{0:N2}";
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.5911960601806641D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.87917411327362061D), Telerik.Reporting.Drawing.Unit.Inch(0.28124997019767761D));
            this.textBox3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.StyleName = "";
            this.textBox3.Value = "=PageExec(\"textBox50\",sum(Fields.GSAmount + Fields.PSAmount + Fields.StateInsAmou" +
    "nt))";
            // 
            // textBox5
            // 
            this.textBox5.Format = "{0:N2}";
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.79991424083709717D), Telerik.Reporting.Drawing.Unit.Inch(0.28125D));
            this.textBox5.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.StyleName = "";
            this.textBox5.Value = "=PageExec(\"textBox50\",sum(Fields.PSAmount))";
            // 
            // textBox6
            // 
            this.textBox6.Format = "{0:N2}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.799992561340332D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.79112571477890015D), Telerik.Reporting.Drawing.Unit.Inch(0.28125D));
            this.textBox6.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox6.Style.Visible = true;
            this.textBox6.StyleName = "";
            this.textBox6.Value = "=PageExec(\"textBox50\",sum(Fields.GSAmount))";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.1916670799255371D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2083325386047363D), Telerik.Reporting.Drawing.Unit.Inch(0.28125D));
            this.textBox7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.StyleName = "";
            this.textBox7.Value = "=\"Record: \" + PageExec(\"textBox50\",count(Fields.eid))";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.01297047920525074D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2083325386047363D), Telerik.Reporting.Drawing.Unit.Inch(0.28125D));
            this.textBox4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox4.StyleName = "";
            this.textBox4.Value = "PAGE TOTAL";
            // 
            // textBox12
            // 
            this.textBox12.Format = "{0:N2}";
            this.textBox12.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.400078296661377D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.59984356164932251D), Telerik.Reporting.Drawing.Unit.Inch(0.28125D));
            this.textBox12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox12.Style.Font.Bold = true;
            this.textBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox12.StyleName = "";
            this.textBox12.Value = "=PageExec(\"textBox50\",sum(Fields.StateInsAmount))";
            // 
            // rpt_Remittance_Regular
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail,
            this.pageFooterSection1,
            this.reportFooterSection1,
            this.pageHeaderSection1});
            this.Name = "rpt_Remittance";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.2700004577636719D), Telerik.Reporting.Drawing.Unit.Inch(11.6899995803833D));
            this.Style.Visible = true;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(7.5002360343933105D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.ObjectDataSource obj_remitlist;
        private Telerik.Reporting.ReportFooterSection reportFooterSection1;
        private Telerik.Reporting.TextBox txt_Accountant_position;
        private Telerik.Reporting.TextBox txt_Accountant;
        private Telerik.Reporting.TextBox textBox24;
        private Telerik.Reporting.TextBox txtUserposition;
        private Telerik.Reporting.TextBox txtUser;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox43;
        private Telerik.Reporting.TextBox textBox44;
        private Telerik.Reporting.TextBox textBox45;
        private Telerik.Reporting.TextBox textBox46;
        private Telerik.Reporting.TextBox txt_gs;
        private Telerik.Reporting.TextBox textbox22;
        private Telerik.Reporting.TextBox textBox49;
        private Telerik.Reporting.TextBox textBox50;
        private Telerik.Reporting.TextBox textBox51;
        private Telerik.Reporting.TextBox textBox53;
        private Telerik.Reporting.TextBox textBox54;
        private Telerik.Reporting.TextBox textBox55;
        private Telerik.Reporting.TextBox textBox52;
        private Telerik.Reporting.HtmlTextBox html_header;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.TextBox textBox37;
        private Telerik.Reporting.Barcode brcode_UI;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox31;
        private Telerik.Reporting.TextBox textBox32;
        private Telerik.Reporting.TextBox txt_ps;
        private Telerik.Reporting.TextBox textBox34;
        private Telerik.Reporting.TextBox textBox38;
        private Telerik.Reporting.TextBox textBox39;
        private Telerik.Reporting.TextBox textBox40;
        private Telerik.Reporting.TextBox textBox42;
        private Telerik.Reporting.TextBox textBox41;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox10;
        private Telerik.Reporting.TextBox textBox12;
    }
}