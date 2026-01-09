namespace IFMIS.Report.Design.Payroll
{
    partial class rpt_PaySlip_byOffice
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.groupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.txt_Total = new Telerik.Reporting.TextBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.textBox21 = new Telerik.Reporting.TextBox();
            this.textBox23 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.groupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.txt_period = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.txt_h2 = new Telerik.Reporting.TextBox();
            this.txt_h3 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.txt_id = new Telerik.Reporting.TextBox();
            this.txt_name = new Telerik.Reporting.TextBox();
            this.txt_sg = new Telerik.Reporting.TextBox();
            this.textBox11 = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.textBox18 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.txt_net1 = new Telerik.Reporting.TextBox();
            this.txt_net2 = new Telerik.Reporting.TextBox();
            this.textBox17 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // groupFooterSection
            // 
            this.groupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(2.6000001430511475D);
            this.groupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txt_Total,
            this.textBox19,
            this.textBox21,
            this.textBox23,
            this.textBox14,
            this.textBox16,
            this.textBox15});
            this.groupFooterSection.KeepTogether = true;
            this.groupFooterSection.Name = "groupFooterSection";
            this.groupFooterSection.PrintAtBottom = false;
            this.groupFooterSection.PrintOnEveryPage = false;
            this.groupFooterSection.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.groupFooterSection.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // txt_Total
            // 
            this.txt_Total.Format = "{0:N2}";
            this.txt_Total.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.23629811406135559D));
            this.txt_Total.Name = "txt_Total";
            this.txt_Total.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3983864784240723D), Telerik.Reporting.Drawing.Unit.Inch(0.30208337306976318D));
            this.txt_Total.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_Total.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_Total.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_Total.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_Total.Style.Font.Bold = true;
            this.txt_Total.Style.Font.Name = "Courier New";
            this.txt_Total.Style.Font.Underline = false;
            this.txt_Total.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.txt_Total.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_Total.StyleName = "";
            this.txt_Total.Value = "=Fields.TotalIncome";
            // 
            // textBox19
            // 
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00020089212921448052D), Telerik.Reporting.Drawing.Unit.Cm(9.9477132607717067E-05D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0917010307312012D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.textBox19.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox19.Style.Font.Name = "Courier New";
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox19.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox19.Value = "GROSS PAY:";
            // 
            // textBox21
            // 
            this.textBox21.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.0921030044555664D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox21.Name = "textBox21";
            this.textBox21.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.807896614074707D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.textBox21.Style.Font.Name = "Courier New";
            this.textBox21.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox21.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox21.Value = "TOTAL DEDUCTION: ";
            // 
            // textBox23
            // 
            this.textBox23.Format = "{0:N2}";
            this.textBox23.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.398465633392334D), Telerik.Reporting.Drawing.Unit.Inch(0.23625946044921875D));
            this.textBox23.Name = "textBox23";
            this.textBox23.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4676764011383057D), Telerik.Reporting.Drawing.Unit.Inch(0.26878869533538818D));
            this.textBox23.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox23.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox23.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox23.Style.Font.Bold = true;
            this.textBox23.Style.Font.Name = "Courier New";
            this.textBox23.Style.Font.Underline = false;
            this.textBox23.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox23.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox23.StyleName = "";
            this.textBox23.Value = "=Fields.TotalDeduction";
            // 
            // textBox14
            // 
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.900197982788086D), Telerik.Reporting.Drawing.Unit.Cm(0.00010076903708977625D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.1497020721435547D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.textBox14.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox14.Style.Font.Name = "Courier New";
            this.textBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox14.Value = "TOTAL NETPAY: ";
            // 
            // textBox16
            // 
            this.textBox16.Format = "{0:N2}";
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.8662204742431641D), Telerik.Reporting.Drawing.Unit.Inch(0.23629912734031677D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6337401866912842D), Telerik.Reporting.Drawing.Unit.Inch(0.30208337306976318D));
            this.textBox16.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox16.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox16.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox16.Style.Font.Bold = true;
            this.textBox16.Style.Font.Name = "Courier New";
            this.textBox16.Style.Font.Underline = false;
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox16.StyleName = "";
            this.textBox16.Value = "=Fields.NetTotal";
            // 
            // textBox15
            // 
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00020089212921448052D), Telerik.Reporting.Drawing.Unit.Cm(1.3676878213882446D));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(19.049697875976562D), Telerik.Reporting.Drawing.Unit.Cm(0.53231245279312134D));
            this.textBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox15.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox15.Style.Font.Italic = true;
            this.textBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox15.Value = "This is computer generated document";
            // 
            // groupHeaderSection
            // 
            this.groupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.4999998807907105D);
            this.groupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txt_period,
            this.textBox2,
            this.txt_h2,
            this.txt_h3});
            this.groupHeaderSection.Name = "groupHeaderSection";
            this.groupHeaderSection.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.groupHeaderSection.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // txt_period
            // 
            this.txt_period.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.946300506591797D), Telerik.Reporting.Drawing.Unit.Cm(9.9961594969499856E-05D));
            this.txt_period.Name = "txt_period";
            this.txt_period.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.1903936862945557D), Telerik.Reporting.Drawing.Unit.Inch(0.21390499174594879D));
            this.txt_period.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_period.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_period.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_period.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_period.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.txt_period.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_period.Value = "";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.24826717376709D), Telerik.Reporting.Drawing.Unit.Cm(0.54361861944198608D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.2841076850891113D), Telerik.Reporting.Drawing.Unit.Inch(0.37652808427810669D));
            this.textBox2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox2.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox2.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox2.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox2.Style.Font.Italic = true;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.Value = "Note: Please submit any corrections in your payslip before the 5th of the month";
            // 
            // txt_h2
            // 
            this.txt_h2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00020089212921448052D), Telerik.Reporting.Drawing.Unit.Cm(9.9961594969499856E-05D));
            this.txt_h2.Name = "txt_h2";
            this.txt_h2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4.3094096183776855D), Telerik.Reporting.Drawing.Unit.Inch(0.21390499174594879D));
            this.txt_h2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_h2.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_h2.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_h2.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_h2.Style.Font.Bold = true;
            this.txt_h2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.txt_h2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            // 
            // txt_h3
            // 
            this.txt_h3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.54361861944198608D));
            this.txt_h3.Name = "txt_h3";
            this.txt_h3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8346457481384277D), Telerik.Reporting.Drawing.Unit.Inch(0.37644937634468079D));
            this.txt_h3.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_h3.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_h3.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_h3.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_h3.Style.Font.Bold = false;
            this.txt_h3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.txt_h3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_h3.Value = "";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(3.4645669460296631D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox5,
            this.textBox7,
            this.txt_id,
            this.txt_name,
            this.txt_sg,
            this.textBox11,
            this.textBox13,
            this.textBox9,
            this.textBox18,
            this.textBox4,
            this.textBox6,
            this.textBox8,
            this.txt_net1,
            this.txt_net2,
            this.textBox17});
            this.detail.Name = "detail";
            this.detail.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.detail.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.detail.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // textBox5
            // 
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00010028457472799346D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9998998641967773D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox5.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox5.Value = "ID Number:";
            // 
            // textBox7
            // 
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.60019993782043457D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9998998641967773D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.Value = "EMPLOYEE NAME:";
            // 
            // txt_id
            // 
            this.txt_id.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.0002000331878662D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.9459009170532227D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.txt_id.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.txt_id.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_id.Value = "=Fields.ID";
            // 
            // txt_name
            // 
            this.txt_name.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.0001997947692871D), Telerik.Reporting.Drawing.Unit.Cm(0.60020029544830322D));
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.9459009170532227D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.txt_name.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.txt_name.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_name.Value = "=Fields.EmpName";
            // 
            // txt_sg
            // 
            this.txt_sg.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.04640007019043D), Telerik.Reporting.Drawing.Unit.Cm(0.00020008468709420413D));
            this.txt_sg.Name = "txt_sg";
            this.txt_sg.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.6453003883361816D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.txt_sg.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.txt_sg.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_sg.Value = "=Fields.sg";
            // 
            // textBox11
            // 
            this.textBox11.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(10.946300506591797D), Telerik.Reporting.Drawing.Unit.Cm(0.00020008468709420413D));
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0998997688293457D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.textBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox11.Value = "SG/Step:";
            // 
            // textBox13
            // 
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.900197982788086D), Telerik.Reporting.Drawing.Unit.Cm(1.2002993822097778D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6337406635284424D), Telerik.Reporting.Drawing.Unit.Inch(0.33333322405815125D));
            this.textBox13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox13.Style.Font.Bold = true;
            this.textBox13.Style.Font.Italic = true;
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox13.Value = "NET EARNINGS";
            // 
            // textBox9
            // 
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00020089212921448052D), Telerik.Reporting.Drawing.Unit.Cm(1.2002997398376465D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3983075618743896D), Telerik.Reporting.Drawing.Unit.Inch(0.33333325386047363D));
            this.textBox9.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox9.Style.Font.Bold = true;
            this.textBox9.Style.Font.Italic = true;
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox9.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox9.Value = "EARNINGS";
            // 
            // textBox18
            // 
            this.textBox18.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.09210205078125D), Telerik.Reporting.Drawing.Unit.Cm(1.2002993822097778D));
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.4676764011383057D), Telerik.Reporting.Drawing.Unit.Inch(0.33333322405815125D));
            this.textBox18.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox18.Style.Font.Bold = true;
            this.textBox18.Style.Font.Italic = true;
            this.textBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox18.Value = "DEDUCTIONS";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00020089212921448052D), Telerik.Reporting.Drawing.Unit.Cm(2.0471665859222412D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0917010307312012D), Telerik.Reporting.Drawing.Unit.Cm(6.7526354789733887D));
            this.textBox4.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox4.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox4.Style.Font.Name = "Courier New";
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBox4.Value = "=Fields.Income";
            // 
            // textBox6
            // 
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.09210205078125D), Telerik.Reporting.Drawing.Unit.Cm(2.047166109085083D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.8078975677490234D), Telerik.Reporting.Drawing.Unit.Cm(6.7526354789733887D));
            this.textBox6.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.textBox6.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox6.Style.Font.Name = "Courier New";
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox6.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBox6.Value = "=Fields.Deduction";
            // 
            // textBox8
            // 
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.091901779174805D), Telerik.Reporting.Drawing.Unit.Cm(5.7000007629394531D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.808098316192627D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.textBox8.Style.Font.Name = "Courier New";
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.Value = "SALARY(1st Half)";
            // 
            // txt_net1
            // 
            this.txt_net1.Format = "{0:N2}";
            this.txt_net1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9416937828063965D), Telerik.Reporting.Drawing.Unit.Inch(2.4803543090820312D));
            this.txt_net1.Name = "txt_net1";
            this.txt_net1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5582668781280518D), Telerik.Reporting.Drawing.Unit.Inch(0.30208337306976318D));
            this.txt_net1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_net1.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_net1.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_net1.Style.Font.Bold = true;
            this.txt_net1.Style.Font.Name = "Courier New";
            this.txt_net1.Style.Font.Underline = false;
            this.txt_net1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.txt_net1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_net1.StyleName = "";
            this.txt_net1.Value = "=Fields.Net1";
            // 
            // txt_net2
            // 
            this.txt_net2.Format = "{0:N2}";
            this.txt_net2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.9448423385620117D), Telerik.Reporting.Drawing.Unit.Inch(3.1496069431304932D));
            this.txt_net2.Name = "txt_net2";
            this.txt_net2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5582668781280518D), Telerik.Reporting.Drawing.Unit.Inch(0.30208337306976318D));
            this.txt_net2.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_net2.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_net2.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_net2.Style.Font.Bold = true;
            this.txt_net2.Style.Font.Name = "Courier New";
            this.txt_net2.Style.Font.Underline = false;
            this.txt_net2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.txt_net2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txt_net2.StyleName = "";
            this.txt_net2.Value = "=Fields.Net2";
            // 
            // textBox17
            // 
            this.textBox17.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.091901779174805D), Telerik.Reporting.Drawing.Unit.Cm(7.4000000953674316D));
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.808098316192627D), Telerik.Reporting.Drawing.Unit.Cm(0.599899411201477D));
            this.textBox17.Style.Font.Name = "Courier New";
            this.textBox17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox17.Value = "Salary(2nd Half)";
            // 
            // rpt_PaySlip_byOffice
            // 
            group1.GroupFooter = this.groupFooterSection;
            group1.GroupHeader = this.groupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("Fields.EmpName"));
            group1.Name = "group1";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.groupHeaderSection,
            this.groupFooterSection,
            this.detail});
            this.Name = "Report1";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.30000001192092896D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(7.5031490325927734D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox txt_Total;
        private Telerik.Reporting.TextBox txt_period;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox txt_id;
        private Telerik.Reporting.TextBox txt_name;
        private Telerik.Reporting.TextBox txt_sg;
        private Telerik.Reporting.TextBox textBox11;
        private Telerik.Reporting.GroupHeaderSection groupHeaderSection;
        private Telerik.Reporting.GroupFooterSection groupFooterSection;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox textBox18;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox textBox21;
        private Telerik.Reporting.TextBox textBox23;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox textBox16;
        private Telerik.Reporting.TextBox textBox15;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox txt_net1;
        private Telerik.Reporting.TextBox txt_net2;
        private Telerik.Reporting.TextBox textBox17;
        private Telerik.Reporting.TextBox txt_h2;
        private Telerik.Reporting.TextBox txt_h3;
    }
}