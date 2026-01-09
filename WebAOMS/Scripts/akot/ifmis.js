var ifmis = function () { }
var ISfn = new ifmis()
var mgsInfo = function () { }
var mgs = new mgsInfo()
var newmodal = function () { }
var mdl_popup = new newmodal()

    mgsInfo.prototype.success = function (caption, content) {
        $.Notify({
            caption: caption,
            content: content,
            style: { background: "#60A917" },
            position: 'bottom-right'
        });
    };
    
    mgsInfo.prototype.warning = function (caption, content) {
        $.Notify({
            caption: caption,
            content: content,
            style: { background: "#CE352C" },
            position: 'bottom-right'
        });
    };

mgsInfo.prototype.yesNo = function (msgs,yesclick) {
wndPopup.title('System Confirmation');
wndPopup.content("<div style=\"text-align:center; padding-top:15px\">" +
""+msgs+"" +
"<div style=\"padding-top:10px\"><button onclick=\"" + yesclick + "()\" class=\"k-button\"><i class=\"fa fa-check fa-lg \"></i> Yes</button><button onclick=\"noClick()\" class=\"k-button \"><i class=\"fa fa-close fa-lg \"></i> No</button> " +
"</div></div><script>function noClick(){wndPopup.close()}</script>")
wndPopup.center().open();
};

mgsInfo.prototype.bsyesNo = function (msgs,yesclick) {
    var html = "<div class=\"modal-dialog\"> " +
            "<div class=\"modal-content\" > " +
               " <div class=\"modal-header\"> " +
                "    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">x</button> " +
                "    <h4 class=\"modal-title\" id=\"simpleModalLabel\">System Confirmation</h4>" +
                "</div>" +
                "<div class=\"modal-body\" style=\"padding-left:20px;padding-right:20px;height:80px!important\" >" +
                "   <p>" + msgs + "</p>" +
                "</div>" +
                "<div class=\"modal-footer\">" +
                "    <button onclick=\"" + yesclick + "\" type=\"button\" class=\"btn btn-danger\">Delete</button>" +
                "    <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Cancel</button>" +
                "</div>" +
            "</div>" +
        "</div>";
    $('#modal_delete').html(html);
$('#modal_delete').modal('show');
};

newmodal.prototype.popmodal = function (size,title,content,btns,height) {
    var html = "<div class=\"modal-dialog "+size+"\">" +
 "           <div class=\"modal-content\">" +
 "              <div class=\"modal-header\">" +
 "                   <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">x</button>" +
 "                   <h4 id=\"g_modal_title\" class=\"modal-title\">"+title+"</h4>" +
 "               </div>" +
 "               <div id=\"g_body\" class=\"modal-body\" style=\"height:"+height+"px!important\">" +
 "                   <div id=\"g_content\" class=\"col-md-12 \" style=\"padding-top:10px\"> " + content + "" +
 "                   </div>" +
"                </div>" +
"                <div id=\"g_btns\" class=\"modal-footer\"> " + btns + "" +
"                   <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Cancel</button>" +
 "               </div>" +
 "           </div>" +
 "       </div>";

    $('#g_modal').html(html);
$('#g_modal').modal('show');
};


mgsInfo.prototype.bsyesNoWithRemarks = function (msgs, yesclick) {
    var html = "<div class=\"modal-dialog\"> " +
            "<div class=\"modal-content\" > " +
               " <div class=\"modal-header\"> " +
                "    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">x</button> " +
                "    <h4 class=\"modal-title\" id=\"simpleModalLabel\">System Confirmation</h4>" +
                "</div>" +
                "<div class=\"modal-body\" style=\"padding-left:20px;padding-right:20px;height:170px!important\" >" +
                "   <p>" + msgs + "</p>" +
               " <div class=\"form-group\">" + 
                    "<div class=\"col-lg-12 col-md-12 col-sm-12\">" +
                       " <textarea name=\"Particular\" id=\"txt_remarks\" class=\"form-control\" placeholder=\"Your Reason to delete this transaction\" rows=\"5\" style=\"resize: none;\" type=\"text\"></textarea>" +
                    "</div>" +
                "</div>" +
                "<div class=\"col-lg-12 col-md-12 col-sm-12\">" +
                "<div class=\"input-group no-padding\">" +
	                "<div data-toggle=\"buttons\">" +
		                "<label id=\"lblcheck_del\" class=\"btn checkbox-inline btn-checkbox-primary-inverse\">" +
			                "<input id=\"chck_del\" type=\"checkbox\">I Agree to the AOMS User Agreement" +
		                "</label>" +
	                "</div>" +
                "</div>" +
                "</div>" +
                "</div>" +
                "<div class=\"modal-footer\">" +
                "    <button id=\"id_del_btn\" onclick=\"" + yesclick + "\" type=\"button\" class=\"btn btn-danger\" disabled=\"disabled\">Yes to Delete</button>" +
                "    <button type=\"button\" onclick=\"zdex()\" class=\"btn btn-default\" data-dismiss=\"modal\">Cancel</button>" +
                "</div>" +
            "</div>" +
                "<script>$(function () {$('#chck_del').change(function () {if ($('#chck_del').is(':checked')) {$('#id_del_btn').removeAttr('disabled');}else {$('#id_del_btn').attr('disabled', 'disabled');}})})</script>"
        "</div>";
    $('#modal_delete').html(html);
    $('#modal_delete').modal('show');
};

mgsInfo.prototype.bsQuestion = function (msgs, yesclick, btnname) {
    var html = "<div class=\"modal-dialog\"> " +
            "<div class=\"modal-content\"> " +
               " <div class=\"modal-header\"> " +
                "    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">×</button> " +
                "    <h4 class=\"modal-title\" id=\"simpleModalLabel\">System Confirmation</h4>" +
                "</div>" +
                "<div class=\"modal-body\" style=\"padding-left:20px;padding-right:20px\">" +
                "   <p>" + msgs + "</p>" +
                "</div>" +
                "<div class=\"modal-footer\">" +
                "    <button onclick=\"" + yesclick + "\" type=\"button\" class=\"btn btn-success\">"+btnname+"</button>" +
                "    <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Cancel</button>" +
                "</div>" +
            "</div>" +
        "</div>";
    $('#modal_question').html(html);
    $('#modal_question').modal('show');
};

mgsInfo.prototype.warning = function (msgs) {
    var html = "<div class=\"modal-dialog\"> " +
            "<div class=\"modal-content\"> " +
               " <div class=\"modal-header\"> " +
                "    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">×</button> " +
                "    <h4 class=\"modal-title\">System Message</h4>" +
                "</div>" +
                "<div class=\"modal-body\" style=\"padding-left:20px;padding-right:20px\">" +
                "   <p style=\"height:80px\">" + msgs + "</p>" +
                "</div>" +
                "<div class=\"modal-footer\">" +
                "    <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Ok</button>" +
                "</div>" +
            "</div>" +
        "</div>";
    $('#modal_warning').html(html);
    $('#modal_warning').modal('show');
};

mgsInfo.prototype.bsSave = function (yesclick) {
    var html = "        <div class=\"modal-dialog\"> " +
            "<div class=\"modal-content\"> " +
               " <div class=\"modal-header\"> " +
                "    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">×</button> " +
                "    <h4 class=\"modal-title\" id=\"simpleModalLabel\">System Confirmation</h4>" +
                "</div>" +
                "<div class=\"modal-body\">" +
                "   <p>Do you want to save changes?</p>" +
                "</div>" +
                "<div class=\"modal-footer\">" +
                "    <button onclick=\"" + yesclick + "\" type=\"button\" class=\"btn btn-success\">Save Changes</button>" +
                "    <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Cancel</button>" +
                "</div>" +
            "</div>" +
        "</div>";
    $('#modal_saveChange').html(html)
    $('#modal_saveChange').modal('show');
};

mgsInfo.prototype.mgsbox = function (message,yesclick) {
    var html = "<div class=\"modal-dialog\ id=\"id_modal_mgsbox\"> " +
            "<div class=\"modal-content\"> " +
               " <div class=\"modal-header\"> " +
                "    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">×</button> " +
                "    <h4 class=\"modal-title\" id=\"simpleModalLabel\">System Confirmation</h4>" +
                "</div>" +
                "<div class=\"modal-body\">" +
                "   <p>"+message+"</p>" +
                "</div>" +
                "<div class=\"modal-footer\">" +
                "    <button onclick=\"" + yesclick + "\" type=\"button\" class=\"btn btn-success btn-loading-state\"><i class=\"fa fa-check\"></i>Yes</button>" +
                "    <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Cancel</button>" +
                "</div>" +
            "</div>" +
        "</div>";
    $('#modal_meesage').html(html)
    $('#modal_meesage').modal('show');
};
mgsInfo.prototype.boxYesNo = function (message,yesclick) {
    var html = "<div class=\"modal-dialog\ id=\"id_modal_mgsbox\"> " +
            "<div class=\"modal-content\"> " +
               " <div class=\"modal-header\"> " +
                "    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">×</button> " +
                "    <h4 class=\"modal-title\" id=\"simpleModalLabel\">System Confirmation</h4>" +
                "</div>" +
                "<div class=\"modal-body\">" +
                "   <p style='padding:10px'>"+message+"</p>" +
                "</div>" +
                "<div class=\"modal-footer\">" +
                "    <button onclick=\"" + yesclick + "\" type=\"button\" class=\"btn btn-success btn-loading-state\"><i class=\"fa fa-check\"></i>Yes</button>" +
                "    <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Cancel</button>" +
                "</div>" +
            "</div>" +
        "</div>";
    $('#modal_meesage').html(html)
    $('#modal_meesage').modal('show');
};
mgsInfo.prototype.mgsbox_close = function () {
    $('#modal_meesage').html("");
    $('#modal_meesage').modal('hide');
};

mgsInfo.prototype.mgsbox_danger = function (msgs, yesclick,yesButton) {
    var html = "<div class=\"modal-dialog\"> " +
            "<div class=\"modal-content\"> " +
               " <div class=\"modal-header\"> " +
                "    <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-hidden=\"true\">×</button> " +
                "    <h4 class=\"modal-title\" id=\"simpleModalLabel\">System Confirmation</h4>" +
                "</div>" +
                "<div class=\"modal-body\">" +
                "   <p>" + msgs + "</p>" +
                "</div>" +
                "<div class=\"modal-footer\">" +
                "    <button onclick=\"" + yesclick + "\" type=\"button\" class=\"btn btn-danger\">"+ yesButton +"</button>" +
                "    <button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Cancel</button>" +
                "</div>" +
            "</div>" +
        "</div>";
    $('#modal_meesage').html(html)
    $('#modal_meesage').modal('show');
};

mgsInfo.prototype.mgsbox_danger_close = function () {
    $('#modal_meesage').html("")
    $('#modal_meesage').modal('hide');;
};

 mgsInfo.prototype.yesNoPara = function (msgs, yesclick) {
     wndPopup.title('System Confirmation');
     wndPopup.content("<div style=\"text-align:center; padding-top:15px\">" +
                 "" + msgs + "" +
                 "<div style=\"padding-top:10px\"><button onclick=\"" + yesclick + "\" class=\"k-button\"><i class=\"fa fa-check fa-lg \"></i> Yes</button><button onclick=\"noClick()\" class=\"k-button \"><i class=\"fa fa-close fa-lg \"></i> No</button> " +
                 "</div></div><script>function noClick(){wndPopup.close()}</script>")
     wndPopup.center().open();
 };

 mgsInfo.prototype.information = function (msgs) {
     wndPopup.title('System Information');
     wndPopup.content("<div style=\"text-align:center; padding-top:15px\">" +
                 "" + msgs + "" +
                 "<div style=\"padding-top:10px\"> <button onclick=\"okClick()\" class=\"k-button \"><i class=\"fa fa-info fa-lg \"></i> Ok</button> " +
                 "</div></div><script>function okClick(){wndPopup.close()}</script>")
     wndPopup.center().open();
 };

 mgsInfo.prototype.popup_warning = function (title,msgs) {
     wndPopup.title(title);
     wndPopup.content("<div style=\"text-align:center; padding-top:15px\">" +
                 "" + msgs + "" +
                 "<div style=\"padding-top:10px\"> <button onclick=\"okClick()\" class=\"k-button \"><i class=\"fa fa-warning fa-lg \"></i> Ok</button> " +
                 "</div></div><script>function okClick(){wndPopup.close()}</script>")
     wndPopup.center().open();
 };


ifmis.prototype.Get_kendoDropDownList = function (name, Text_, Value_, dataS) {
    $("" + name + "").kendoDropDownList({
        dataTextField: "" + Text_ + "",
        dataValueField: "" + Value_ + "",
        dataSource: dataS
    });
};

    ifmis.prototype.replacehtmlcode = function (str) {
        return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/p_/g, '<br>').replace(/t_/g, '&nbsp;').replace(/4444/g, '&#x');
    }

    ifmis.prototype.removeBrtab = function (str) {
        return String(str).replace(/p_/g, '').replace(/t_/g, '').replace(/4444/g, '&#x');
    }

    ifmis.prototype.kloadingClose = function(t,elem) {
        setTimeout(function () {
            document.getElementById(elem).setAttribute('style', 'display:none');
            //loading.close()
        }, t)
    }

    ifmis.prototype.kloadingOpen = function (elem) {
            var Kloading = document.getElementById(elem);
            Kloading.setAttribute('style', 'width:500px;height:100%;float:right');
            Kloading.innerHTML = "<span class=\"k-loading-text\">Loading...</span>" +
                "<div class=\"k-loading-image\">" +
                    "<div class=\"k-loading-color\"> " +
                    "</div>" +
                "</div>"
    }

    ifmis.prototype.JSComboBox = function (dataSourceID, cboname,placeHolder) {
        $.post('../PrivateShared/LoadComboDatasource', { datasourceid: dataSourceID },
        function (data) {
            $("#"+ cboname +"").kendoComboBox({
                placeholder: placeHolder,
                dataTextField: "txt",
                dataValueField: "val",
                dataSource: data
            });
        }, "json");
    }

    ifmis.prototype.applyFilter = function (filterField, filterValue, gridname) {
        // get the kendoGrid element.
        var gridData = $("#"+gridname+"").data("kendoGrid");

        // get currently applied filters from the Grid.
        var currFilterObj = gridData.dataSource.filter();

        // get current set of filters, which is supposed to be array.
        // if the oject we obtained above is null/undefined, set this to an empty array
        var currentFilters = currFilterObj ? currFilterObj.filters : [];

        // iterate over current filters array. if a filter for "filterField" is already
        // defined, remove it from the array
        // once an entry is removed, we stop looking at the rest of the array.
        if (currentFilters && currentFilters.length > 0) {
            for (var i = 0; i < currentFilters.length; i++) {
                if (currentFilters[i].field == filterField) {
                    currentFilters.splice(i, 1);
                    break;
                }
            }
        }
        // if "filterValue" is "0", meaning "-- select --" option is selected, we don't 
        // do any further processing. That will be equivalent of removing the filter.
        // if a filterValue is selected, we add a new object to the currentFilters array.
        if (filterValue != "0") {
            currentFilters.push({
                field: filterField,
                operator: "contains",
                value: filterValue
            });
        }
        // finally, the currentFilters array is applied back to the Grid, using "and" logic.
        gridData.dataSource.filter({
            logic: "and",
            filters: currentFilters
        });
    }



    ifmis.prototype.applyFilterEqual = function (filterField, filterValue, gridname) {
        // get the kendoGrid element.
        var gridData = $("#" + gridname + "").data("kendoGrid");

        // get currently applied filters from the Grid.
        var currFilterObj = gridData.dataSource.filter();

        // get current set of filters, which is supposed to be array.
        // if the oject we obtained above is null/undefined, set this to an empty array
        var currentFilters = currFilterObj ? currFilterObj.filters : [];

        // iterate over current filters array. if a filter for "filterField" is already
        // defined, remove it from the array
        // once an entry is removed, we stop looking at the rest of the array.
        if (currentFilters && currentFilters.length > 0) {
            for (var i = 0; i < currentFilters.length; i++) {
                if (currentFilters[i].field == filterField) {
                    currentFilters.splice(i, 1);
                    break;
                }
            }
        }
        // if "filterValue" is "0", meaning "-- select --" option is selected, we don't 
        // do any further processing. That will be equivalent of removing the filter.
        // if a filterValue is selected, we add a new object to the currentFilters array.
        if (filterValue != "0") {
            currentFilters.push({
                field: filterField,
                operator: "is equal to",
                value: filterValue
            });
        }
        // finally, the currentFilters array is applied back to the Grid, using "and" logic.
        gridData.dataSource.filter({
            logic: "and",
            filters: currentFilters
        });
    }
