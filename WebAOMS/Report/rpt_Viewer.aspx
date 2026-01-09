<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rpt_Viewer.aspx.cs" Inherits="WebAOMS.Report.rpt_Viewer" %>


<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=16.1.22.622, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/akot/jquery-1.11.0.min.js"></script>
</head>
    <body style="height:100%">
       
        <form id="form1" runat="server">
              <telerik:ReportViewer ID="rp" runat="server" Resources-PrintToolTip ="true"  ShowExportGroup="true"  style=" width:100%;height:2000px;margin:auto" ViewMode="Interactive"></telerik:ReportViewer>
        </form>
         <script type="text/javascript">
            $(document).ready(function () {
                document.body.style.overflow = "hidden";
                var viewportWidth = $(window).width();
                var viewportHeight = $(window).height();
                document.body.style.overflow = "";
                $("#rp").height(viewportHeight-20)
            })            

            rp.prototype.PrintReport = function () {

            switch (this.defaultPrintFormat) {

            case "Default":

            this.DefaultPrint();

            break;

            case "PDF":

            this.PrintAs("PDF");

            previewFrame = document.getElementById(this.previewFrameID);

            previewFrame.onload = function () { previewFrame.contentDocument.execCommand("print", true, null); }

            break;

            }

            };

</script>
    </body>
</html>
