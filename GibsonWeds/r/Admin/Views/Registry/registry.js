var mainJson = null;
var dtRegistry = null;
var registryID;
$(document).ready(function () {

    mainJson = jQuery.parseJSON($('#jsonData').html());
    debugger;
    InitRegistryTable();

    //$('#cmbGuestCategoryAdd').select2({
    //    placeholder: "Please select",
    //    width: "100%",
    //    allowClear: true
    //});
});

function InitRegistryTable() {

    dtRegistry = $('#tblRegistry').dataTable({

        "sSearch": true,
        "dom": '<"top"fl>rt<"bottom"p><"clear">', // "dom": '<"top"fl>rt<"bottom"ip><"clear">',
        "sPaginationType": "full_numbers",//IMPORTANT
        "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [3] }],
        "oLanguage": { "sEmptyTable": "No data to display" },
        "sSearch": true,
        "bJQueryUI": false,
        "bAutoWidth": false,
        "sAjaxSource": base_url + '/api/admin/registry/list',
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
        "aoColumns": [{ "sTitle": "Gift Url/Name", "mDataProp": "GiftName", "sWidth": "30%" },
            { "sTitle": "Is Selected", "mDataProp": "isSelected", "sWidth": "15%" },
            { "sTitle": "Selected Guest", "mDataProp": "selectedUserID", "sWidth": "30%" },
            { "sTitle": "Actions", "mDataProp": "registryID", "sClass": "text-center w100 minw100" },


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
            actionHtml += ' <a href="javascript:void(0);" onclick="DeleteGift(' + aData["registryID"] + ')" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times"></a>';
            // }
            //else {
            //    actionHtml += ' <a href="javascript:void(0);" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times" disabled></a>'
            // }



            $('td:eq(3)', nRow).html(actionHtml);

        },
        "fnDrawCallback": function (settings, json) {
            //Tooltips
            $('[data-toggle="tooltip"]').tooltip()
        }
    })

}

function SaveRegistryAdd() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var GuestRegistryGiftAddO = $('#tbRegistryGiftNameAdd');
    var GuestRegistryGiftAdd = GuestRegistryGiftAddO.val();


    if (!tcg.valid8r.req(GuestRegistryGiftAdd, GuestRegistryGiftAddO)) {
        isValid = false;
        errorText += "Please fill in Gift Url/Name<br />";
    }

    if (isValid) {
        JsonO = {
            GiftName: GuestRegistryGiftAdd,
            isSelected: false,
            selectedUserID: null
        };

        url = base_url + "/api/admin/registry/add";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtRegistry.fnDraw();
                cancelEdit();
                $("#modalAddRegistry").modal("hide");
                tcg.widgets.Growl("Successfully added a Registry Item", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to add the Registry Item", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorRegistryAdd').html(errorText);
    }
}

function cancelEdit() {
    $(".haserror").removeClass("haserror");
    $('#tbRegistryGiftNameAdd').val('');
    $("#tbRegistryGiftNameEdit").val('');
}

function OpenEditModal(row) {

    cancelEdit();
    var RegistryItem = dtRegistry.fnGetData(row);

    var RegistryID = RegistryItem["registryID"];
    var GiftName = RegistryItem["GiftName"];

    $("#tbRegistryGiftNameEdit").val(GiftName);

    registryID = RegistryID;

    $("#modalEditRegistry").modal("show");
}

function SaveRegistryEdit() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var GuestRegistryGiftEditO = $('#tbRegistryGiftNameEdit');
    var GuestRegistryGiftEdit = GuestRegistryGiftEditO.val();


    ///Validation
    if (!tcg.valid8r.req(GuestRegistryGiftEdit, GuestRegistryGiftEditO)) {
        isValid = false;
        errorText += "Please fill in Gift Url/Name<br />";
    }


    if (isValid) {
        JsonO = {
            registryID: registryID,
            GiftName: GuestRegistryGiftEdit,
        };

        url = base_url + "/api/admin/registry/edit";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtRegistry.fnDraw();
                cancelEdit();
                $("#modalEditRegistry").modal("hide");
                tcg.widgets.Growl("Successfully edited a Registry Item", "Success", "success", "3500");
            }
            else {
                tcg.widgets.Growl("Cannot edit Registry Item when it is already assigned to Guest", "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("Cannot edit Registry Item when it is already assigned to Guest", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorRegistryEdit').html(errorText);
    }
}

function DeleteGift(registryItemID) {

    tcg.widgets.confirm('Are you sure you want to delete this Registry Item?', 'Registry Item', function () {

        var url = base_url + "/api/admin/registry/delete?registryID=" + registryItemID;

        var successF = function (data) {
            if (data.isSuccess) {

                dtRegistry.fnDraw();
                tcg.widgets.Growl("Registry Item has been successfully removed", "Success", "success", 3500);
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