namespace WebAOMS.Report.Design.Payroll
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using WebAOMS.Base;
    using WebAOMS.Mod;
    using WebAOMS.Report.Design;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Web;
    using System.IO;
    using WebAOMS.ws_tracking;
    using WebAOMS.Controllers;
    /// <summary>
    /// Summary description for rpt_Special_Payroll_dynamic.
    /// </summary>
    public partial class rpt_Special_Payroll_dynamic : Telerik.Reporting.Report
    {
        
        public rpt_Special_Payroll_dynamic(int batchno, int bankid, int officeid)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            string UI = Guid.NewGuid().ToString().Substring(24, 12);
            // ISfn.LogReportGuid(batchno, UI, 9);
            TrackingController Tracks = new TrackingController();
            DataSet dset;

            dset = OleDbHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["pmisDBconnstring"].ToString(), CommandType.Text, "exec [epay].usp_print_special_payroll_dynamic_v2 " + batchno + "," + USER.C_eID.ToString() + "," + officeid);

            Telerik.Reporting.Drawing.FormattingRule formattingRule1 = new Telerik.Reporting.Drawing.FormattingRule();
            DataTable rec;

            rec = dset.Tables[3];
            
            //create a blank Table item
            Telerik.Reporting.Table table1 = new Telerik.Reporting.Table();
            table1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0), Telerik.Reporting.Drawing.Unit.Inch(0.0));
            table1.Name = "Table1";
            table1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4), Telerik.Reporting.Drawing.Unit.Inch(1));

            //get the data for the table
            DataTable data = rec;
            //table1.DataSource = data;

            //create a dynamic row group
            Telerik.Reporting.TableGroup DetailRowGroup = new Telerik.Reporting.TableGroup();
            DetailRowGroup.Groupings.Add(new Telerik.Reporting.Grouping(null));
            DetailRowGroup.Name = "DetailRowGroup";
            table1.RowGroups.Add(DetailRowGroup);
            //add a row container
            table1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.5)));

            int i = 0;
            //add columns
            foreach (DataRow rw in data.Rows)
            {
                //add a column container

                table1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(Convert.ToDouble(rw["width"]))));
                //add a static column group per data field
                Telerik.Reporting.TableGroup columnGroup = new Telerik.Reporting.TableGroup();
                table1.ColumnGroups.Add(columnGroup);

                //header textbox
                Telerik.Reporting.TextBox headerTextBox = new Telerik.Reporting.TextBox();
                headerTextBox.Name = "headerTextBox" + i.ToString();
                headerTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(Convert.ToDouble(rw["width"])), Telerik.Reporting.Drawing.Unit.Inch(0.25));  
                headerTextBox.Value = rw["ColDesc"].ToString();

                headerTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                headerTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                headerTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                headerTextBox.Style.Font.Bold = true;
                headerTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
                //  headerTextBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1);
                columnGroup.ReportItem = headerTextBox;

                //field that will be displayed
                Telerik.Reporting.TextBox detailRowTextBox = new Telerik.Reporting.TextBox();
                detailRowTextBox.Name = "detailRowTextBox" + i.ToString();
                detailRowTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(Convert.ToDouble(rw["width"])), Telerik.Reporting.Drawing.Unit.Inch(0.25));
                detailRowTextBox.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
                detailRowTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                detailRowTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
                detailRowTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                detailRowTextBox.StyleName = "";


                if (rw["datatype"].ToString() == "1" || rw["datatype"].ToString() == "3")
                {
                    detailRowTextBox.Format = "{0:N2}";
                    detailRowTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
                }
                else
                {
                    detailRowTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                }
                if (rw["ColName"].ToString() == "rowno")
                {
                    detailRowTextBox.Value = "= RowNumber()";
                    detailRowTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                }
                else
                {
                    detailRowTextBox.Value = "= Fields." + rw["ColName"].ToString();
                }
                table1.Body.SetCellContent(0, i, detailRowTextBox);
                i++;
                //add the nested items in the Table.Items collection
                table1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                  headerTextBox,
                  detailRowTextBox
              });
            }
            int x = 0;
            //add total group - static group out of the detail row group
            table1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.5)));
            Telerik.Reporting.TableGroup totalRowGroup = new Telerik.Reporting.TableGroup();
            totalRowGroup.Name = "totalRowGroup";
            table1.RowGroups.Add(totalRowGroup);
            foreach (DataRow rw in data.Rows)
            {
                Telerik.Reporting.TextBox totalRowTextBox = new Telerik.Reporting.TextBox();
                totalRowTextBox.Name = "detailRowTextBox" + x.ToString();
                totalRowTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(Convert.ToDouble(rw["width"])), Telerik.Reporting.Drawing.Unit.Inch(0.25));

                //this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3175086975097656D), Telerik.Reporting.Drawing.Unit.Inch(0.24999994039535523D));

                totalRowTextBox.Style.Font.Bold = true;
                totalRowTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
                totalRowTextBox.Style.Font.Underline = true;
                totalRowTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Double;
                totalRowTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                totalRowTextBox.Style.Visible = true;
                if (rw["datatype"].ToString() == "1")
                {
                    totalRowTextBox.Format = "{0:N2}";
                    totalRowTextBox.Value = "= Sum(Fields.[" + rw["ColName"].ToString() + "])";
                    totalRowTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;

                }
                else
                {
                    if (x == 1)
                    {
                        totalRowTextBox.Value = "Total:";
                    }
                    else
                    {
                        totalRowTextBox.Value = "";
                    }

                    totalRowTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                }
                table1.Body.SetCellContent(1, x, totalRowTextBox);
                x++;
            }
            //add the table in the detail section's Items collection
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { table1 });

            table1.ColumnHeadersPrintOnEveryPage = true;
            DataTable recp;
            recp = dset.Tables[4];
            if (recp.Rows.Count > 0)
            {
               //// txt_paymentperiod.Value = recp.Rows[0]["CompensatioName"].ToString() + " " + recp.Rows[0]["year"].ToString() + "(" + recp.Rows[0]["employmentStatusText"].ToString() + ")";
                this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(Convert.ToDouble(recp.Rows[0]["width"])), Telerik.Reporting.Drawing.Unit.Inch(Convert.ToDouble(recp.Rows[0]["height"])));
            }
            recp.Dispose();
            txt_batchno.Value = "Batch No.: " + batchno;

            this.DataSource = dset.Tables[1];
            table1.DataSource = dset.Tables[0];
            this.table4.DataSource = dset.Tables[1];
            brcode_UI.Value = UI;
            string refno = Tracks.get_unique_refno(batchno.ToString(), 50, "E3G7", "Special Payroll with batchno " + batchno.ToString(), ""); ;
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            pictureBox2.Value = ISfn.QRGen(Tracks.get_tracking_link(refno).ToString(), 4);
        }
    }
}