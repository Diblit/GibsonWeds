var mainJson = null;
var dtGuestCouples = null;
var groupCoupleID;
$(document).ready(function () {

    mainJson = jQuery.parseJSON($('#jsonData').html());
    InitGuestCouplesTable();

    //$('#cmbGuestCategoryAdd').select2({
    //    placeholder: "Please select",
    //    width: "100%",
    //    allowClear: true
    //});
});

function InitGuestCouplesTable() {

    dtGuestCouples = $('#tblGuestCouples').dataTable({

        "sSearch": true,
        "dom": '<"top"fl>rt<"bottom"p><"clear">', // "dom": '<"top"fl>rt<"bottom"ip><"clear">',
        "sPaginationType": "full_numbers",//IMPORTANT
        "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [1] }],
        "oLanguage": { "sEmptyTable": "No data to display" },
        "sSearch": true,
        "bJQueryUI": false,
        "bAutoWidth": false,
        "sAjaxSource": base_url + '/api/admin/guestcouples/list',
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
        "aoColumns": [{ "sTitle": "Couple Name", "mDataProp": "CoupleName", "sWidth": "75%" },        
            { "sTitle": "Actions", "mDataProp": "groupCoupleID", "sClass": "text-center w100 minw100" }


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
            actionHtml += ' <a href="javascript:void(0);" onclick="DeleteGuest(' + aData["groupCoupleID"] + ')" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times"></a>';
            // }
            //else {
            //    actionHtml += ' <a href="javascript:void(0);" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times" disabled></a>'
            // }



            $('td:eq(1)', nRow).html(actionHtml);

        },
        "fnDrawCallback": function (settings, json) {
            //Tooltips
            $('[data-toggle="tooltip"]').tooltip()
        }
    })

}

function SaveGuestCoupleAdd() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var GuestCoupleNameAddO = $('#tbGuestCoupleNameAdd');
    var GuestCoupleNameAdd = GuestCoupleNameAddO.val();


    if (!tcg.valid8r.req(GuestCoupleNameAdd, GuestCoupleNameAddO)) {
        isValid = false;
        errorText += "Please fill in Name<br />";
    }

    if (isValid) {
        JsonO = {
            CoupleName: GuestCoupleNameAdd,            
        };

        url = base_url + "/api/admin/guestcouples/add";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtGuestCouples.fnDraw();
                cancelEdit();
                $("#modalAddGuestCouple").modal("hide");
                tcg.widgets.Growl("Successfully added a Guest Couple", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to add the Guest Couple", "Error", "danger", 4000);
            });
    }
}

function cancelEdit() {
    $(".haserror").removeClass("haserror");
    $('#tbGuestCoupleNameAdd').val('');   
    $("#tbGuestCoupleNameEdit").val('');
}

function OpenEditModal(row) {

    cancelEdit();
    var GuestCouple = dtGuestCouples.fnGetData(row);

    var GroupCoupleID = GuestCouple["groupCoupleID"];
    var CoupleName = GuestCouple["CoupleName"];

    $("#tbGuestCoupleNameEdit").val(CoupleName);
    
    groupCoupleID = GroupCoupleID;

    $("#modalEditGuestCouple").modal("show");
}

function SaveGuestCoupleEdit() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var GuestCoupleNameEditO = $('#tbGuestCoupleNameEdit');
    var GuestCoupleNameEdit = GuestCoupleNameEditO.val();


    ///Validation
    if (!tcg.valid8r.req(GuestCoupleNameEdit, GuestCoupleNameEditO)) {
        isValid = false;
        errorText += "Please fill in Name<br />";
    }
    

    if (isValid) {
        JsonO = {
            groupCoupleID: groupCoupleID,
            CoupleName: GuestCoupleNameEdit,
        };

        url = base_url + "/api/admin/guestcouples/edit";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtGuestCouples.fnDraw();
                cancelEdit();
                $("#modalEditGuestCouple").modal("hide");
                tcg.widgets.Growl("Successfully edited a Guest Couple", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to edit the Guest Couple", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorGuestCoupleEdit').html(errorText);
    }
}

function DeleteGuest(guestCoupleID) {

    tcg.widgets.confirm('Are you sure you want to delete this Couple?', 'Delete Couple', function () {

        var url = base_url + "/api/admin/guestcouples/delete?guestCoupleID=" + guestCoupleID;

        var successF = function (data) {
            if (data.isSuccess) {

                dtGuestCouples.fnDraw();
                tcg.widgets.Growl("Couple has been successfully removed", "Success", "success", 3500);
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