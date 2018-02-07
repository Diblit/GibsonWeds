var mainJson = null;
var dtGuestList = null;
var userID;
$(document).ready(function () {
   
    mainJson = jQuery.parseJSON($('#jsonData').html());
    InitGuestListTable();
    Select2CoupleList();    

    
    $('#cmbGuestGroupCoupleEdit').select2({
        placeholder: "Please select",
        width: "100%",
        allowClear: true
    });
});

function InitGuestListTable() {

    dtGuestList = $('#tblGuestList').dataTable({

        "sSearch": true,
        "dom": '<"top"fl>rt<"bottom"p><"clear">', // "dom": '<"top"fl>rt<"bottom"ip><"clear">',
        "sPaginationType": "full_numbers",//IMPORTANT
        "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [6] }],
        "oLanguage": { "sEmptyTable": "No data to display" },
        "sSearch": true,
        "bJQueryUI": false,
        "bAutoWidth": false,
        "sAjaxSource": base_url + '/api/admin/guest/list',
        "aaData": mainJson.List,
        "aaSorting": [],
        "bProcessing": true,
        "bServerSide": true,
        "orderClasses": false,
        "iDisplayLength": 10,
        "aLengthMenu": [
            [5, 10, 25, 50, 100],
            [5, 10, 25, 50, 100]
        ],

        "bSort": true,
        "bRetrieve": true,
        'bSortable': false,
        'aTargets': [-1],
        "aoColumns": [{ "sTitle": "First Name", "mDataProp": "FirstName", "sWidth": "45%" },
            { "sTitle": "Last Name", "mDataProp": "LastName" },
            { "sTitle": "Email", "mDataProp": "Email" },
            { "sTitle": "Cell", "mDataProp": "Cell" },
            { "sTitle": "Has RSVP'd", "mDataProp": "hasRSVPd" },
            { "sTitle": "Is Attending", "mDataProp": "isAttending" },            
            { "sTitle": "Actions", "mDataProp": "userID", "sClass": "text-center w100 minw100" }
            

        ],
        "fnServerData": function (sSource, aoData, fnCallback, oSettings) {
            doDataTablePostAjaxCalling(this, sSource, aoData, fnCallback, oSettings);
        },

        "iDeferLoading": [mainJson.Count, mainJson.Count],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

            //Action Buttons
            var actionHtml = "";
            actionHtml += ' <a href="javascript:void(0);" onclick="OpenEditModal(' + nRow["_DT_RowIndex"] + ')" data-toggle="tooltip" title="Edit" data-placement="left" data-original-title="Edit" class="btn btn-default btn-sm btn-primary fa fa-pencil"></a>';
            //if (aData["isDeletable"]) {
                actionHtml += ' <a href="javascript:void(0);" onclick="DeleteGuest(' + aData["userID"] + ')" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times"></a>';
           // }
            //else {
            //    actionHtml += ' <a href="javascript:void(0);" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times" disabled></a>'
           // }



            $('td:eq(6)', nRow).html(actionHtml);

        },
        "fnDrawCallback": function (settings, json) {
            //Tooltips
            $('[data-toggle="tooltip"]').tooltip()
        }
    })

}

function SaveGuestAdd() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var GuestFirstNameAddO = $('#tbGuestNameAdd');
    var GuestFirstNameAdd = GuestFirstNameAddO.val();
    var GuestLastNameAddO = $('#tbGuestSurnameAdd');
    var GuestLastNameAdd = GuestLastNameAddO.val();
    var GuestEmailAddO = $('#tbGuestEmailAdd');
    var GuestEmailAdd = GuestEmailAddO.val();
    var GuestCellAddO = $('#tbGuestCellAdd');
    var GuestCellAdd = GuestCellAddO.val();
    
    var GuestAllowPlusOneAddO = $('#tbGuestAllowPlusOneAdd');
    var GuestAllowPlusOneAdd = GuestAllowPlusOneAddO.is(':checked');
    var GuestGroupCoupleAddO = $('#cmbGuestGroupCoupleAdd');
    var GuestGroupCoupleAdd = GuestGroupCoupleAddO.val();
    
    ///Validation
    if (!tcg.valid8r.req(GuestFirstNameAdd, GuestFirstNameAddO)) {
        isValid = false;
        errorText += "Please fill in Name<br />";
    }
    if (!tcg.valid8r.req(GuestEmailAdd, GuestEmailAddO)) {
        isValid = false;
        errorText += "Please fill in Email<br />";
    }
    if (!tcg.valid8r.req(GuestCellAdd, GuestCellAddO)) {
        isValid = false;
        errorText += "Please fill in Cell<br />";
    }

    if (isValid) {
        JsonO = {
            FirstName: GuestFirstNameAdd,
            LastName: GuestLastNameAdd,
            Email: GuestEmailAdd,
            Cell: GuestCellAdd,
            PasswordHash: GuestCellAdd,
            allowPlusOne: GuestAllowPlusOneAdd,
            groupCoupleID: GuestGroupCoupleAdd,
            isPlusOne: false,
            isGuest: true,
            isAdmin: false,
            hasRSVPd: false,
            isAttending: false
        };

        url = base_url + "/api/admin/guest/add";
        
        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtGuestList.fnDraw();
                cancelEdit();
                $("#modalAddGuest").modal("hide");
                tcg.widgets.Growl("Successfully added a Guest", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to add the Guest", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorAdd').html(errorText);
    }
}
function cancelEdit() {
    $('#tbGuestNameAdd').val('');
    $('#tbGuestSurnameAdd').val('');
    $('#tbGuestEmailAdd').val('');
    $('#tbGuestCellAdd').val('');
    $('#tbGuestAllowPlusOneAdd').prop('checked', false);
    $('#cmbGuestGroupCoupleAdd').html('');
    Select2CoupleList()

    $("#tbGuestNameEdit").val('');
    $("#tbGuestSurnameEdit").val('');
    $("#tbGuestEmailEdit").val('');
    $("#tbGuestCellEdit").val('');
    $("#tbGuestAllowPlusOneEdit").prop('checked', false);
    $("#cmbGuestGroupCoupleEdit").html("")
}

function OpenEditModal(row) {
    
    cancelEdit();
    var Guest = dtGuestList.fnGetData(row);
    
    var UserID = Guest["userID"];
    var FirstName = Guest["FirstName"];
    var LastName = Guest["LastName"];
    var Email = Guest["Email"];
    var Cell = Guest["Cell"];
    var allowPlusOne = Guest["allowPlusOne"];    
    var groupCoupleID = Guest["groupCoupleID"];

    var mainValue = null;
    var mainName = "";
    
    var GuestCoupleSelectEdit = "<option></option>"
    if (groupCoupleID == null) {
        $.each(mainJson.CouplesList, function (index, item) {
            GuestCoupleSelectEdit += '<option value="' + item.groupCoupleID + '">' + item.CoupleName + '</option>';            
        });
    } else {
        $.each(mainJson.CouplesList, function (index, item) {
            if (groupCoupleID == item.groupCoupleID) {
                GuestCoupleSelectEdit += '<option value="' + item.groupCoupleID + '" selected>' + item.CoupleName + '</option>';
                mainValue = item.groupCoupleID;
                mainName = item.CoupleName;
            } else {
                GuestCoupleSelectEdit += '<option value="' + item.groupCoupleID + '">' + item.CoupleName + '</option>';
            }

        });
    }
    
    $("#tbGuestNameEdit").val(FirstName);
    $("#tbGuestSurnameEdit").val(LastName);
    $("#tbGuestEmailEdit").val(Email);
    $("#tbGuestCellEdit").val(Cell);
    $("#tbGuestAllowPlusOneEdit").prop('checked', allowPlusOne);
    $("#cmbGuestGroupCoupleEdit").append(GuestCoupleSelectEdit);
    $('#cmbGuestGroupCoupleEdit').select2({
        placeholder: "Please select",
        width: "100%",
        allowClear: true
    });
    //$('#').select2().select2('val', mainValue)
    //$('#cmbGuestGroupCoupleEdit').select2('data', { id: mainValue, a_key: mainName });
    //$('#cmbGuestGroupCoupleEdit').select2('val', mainValue + "");
    $("cmbGuestGroupCoupleEdit").val(mainValue).trigger('change');
    //$("#").val(mainValue);
   
    
    

    userID = UserID;

    $("#modalEditGuest").modal("show");
}

function SaveGuestEdit(){
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var GuestFirstNameAddO = $('#tbGuestNameEdit');
    var GuestFirstNameAdd = GuestFirstNameAddO.val();
    var GuestLastNameAddO = $('#tbGuestSurnameEdit');
    var GuestLastNameAdd = GuestLastNameAddO.val();
    var GuestEmailAddO = $('#tbGuestEmailEdit');
    var GuestEmailAdd = GuestEmailAddO.val();
    var GuestCellAddO = $('#tbGuestCellEdit');
    var GuestCellAdd = GuestCellAddO.val();
    
    var GuestAllowPlusOneAddO = $('#tbGuestAllowPlusOneEdit');
    var GuestAllowPlusOneAdd = GuestAllowPlusOneAddO.is(':checked');
    var GuestGroupCoupleAddO = $('#cmbGuestGroupCoupleEdit');
    var GuestGroupCoupleAdd = GuestGroupCoupleAddO.val();
   
    ///Validation
    if (!tcg.valid8r.req(GuestFirstNameAdd, GuestFirstNameAddO)) {
        isValid = false;
        errorText += "Please fill in Name<br />";
    }
    if (!tcg.valid8r.req(GuestEmailAdd, GuestEmailAddO)) {
        isValid = false;
        errorText += "Please fill in Email<br />";
    }
    if (!tcg.valid8r.req(GuestCellAdd, GuestCellAddO)) {
        isValid = false;
        errorText += "Please fill in Cell<br />";
    }

    if (isValid) {
        JsonO = {
            userID: userID,
            FirstName: GuestFirstNameAdd,
            LastName: GuestLastNameAdd,
            Email: GuestEmailAdd,
            Cell: GuestCellAdd,            
            allowPlusOne: GuestAllowPlusOneAdd,
            groupCoupleID: GuestGroupCoupleAdd,            
        };

        url = base_url + "/api/admin/guest/edit";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtGuestList.fnDraw();
                cancelEdit();
                $("#modalEditGuest").modal("hide");
                tcg.widgets.Growl("Successfully edited a Guest", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to add the Guest", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorEdit').html(errorText);
    }
}

function DeleteGuest(userID) {

    tcg.widgets.confirm('Are you sure you want to delete this Guest?', 'Delete Guest', function () {

        var url = base_url + "/api/admin/guest/delete?userID=" + userID;

        var successF = function (data) {
            if (data.isSuccess) {

                dtGuestList.fnDraw();
                tcg.widgets.Growl("Guest has been successfully removed", "Success", "success", 3500);
            }
            else {
                tcg.widgets.Growl(data.errorText, "Error", "danger", 3500);
            }
        };

        var errorF = function (data) {
            tcg.widgets.Growl("An error has occurred. Please try again or contact support if problem persists.", "Error", "danger", 3500);
        }

        tcg.ajax.post(url, {}, successF, errorF);
    });
}

function Select2CoupleList() {
    
    var GuestCoupleSelectAdd = "<option></option>";
    $.each(mainJson.CouplesList, function (index, item) {
        GuestCoupleSelectAdd += '<option value="' + item.groupCoupleID + '">' + item.CoupleName + '</option>';
    });
    $('#cmbGuestGroupCoupleAdd').append(GuestCoupleSelectAdd);

    $('#cmbGuestGroupCoupleAdd').select2({
        placeholder: "Please select",
        width: "100%",
        allowClear: true
    });
}