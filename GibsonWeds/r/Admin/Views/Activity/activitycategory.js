var mainJson = null;
var dtActivityCategory = null;
var activityCategoryID;
$(document).ready(function () {
debugger;
    mainJson = jQuery.parseJSON($('#jsonData').html());
    debugger;
    InitActivityCategoryTable();

    //$('#cmbGuestCategoryAdd').select2({
    //    placeholder: "Please select",
    //    width: "100%",
    //    allowClear: true
    //});
});
function InitActivityCategoryTable() {

    dtActivityCategory = $('#tblActivityCategory').dataTable({

        "sSearch": true,
        "dom": '<"top"fl>rt<"bottom"p><"clear">', // "dom": '<"top"fl>rt<"bottom"ip><"clear">',
        "sPaginationType": "full_numbers",//IMPORTANT
        "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [2] }],
        "oLanguage": { "sEmptyTable": "No data to display" },
        "sSearch": true,
        "bJQueryUI": false,
        "bAutoWidth": false,
        "sAjaxSource": base_url + '/api/admin/activitycategory/list',
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
        "aoColumns": [{ "sTitle": "Activity Name", "mDataProp": "Name", "sWidth": "50%" },
            { "sTitle": "Activity Price", "mDataProp": "Price", "sWidth": "25%" },
            { "sTitle": "Actions", "mDataProp": "activityCategoryID", "sClass": "text-center w100 minw100" }


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
            actionHtml += ' <a href="javascript:void(0);" onclick="DeleteGuest(' + aData["activityCategoryID"] + ')" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times"></a>';
            // }
            //else {
            //    actionHtml += ' <a href="javascript:void(0);" data-toggle="tooltip" title="Delete" data-placement="left" data-original-title="Delete" class="btn btn-default btn-sm btn-danger fa fa-times" disabled></a>'
            // }



            $('td:eq(2)', nRow).html(actionHtml);

        },
        "fnDrawCallback": function (settings, json) {
            //Tooltips
            $('[data-toggle="tooltip"]').tooltip()
        }
    })

}

function cancelEdit() {
    $(".haserror").removeClass("haserror");
    $('#tbActCatNameAdd').val('');
    $("#tbActCatPriceAdd").val('');

    $('#tbActCatNameEdit').val('');
    $("#tbActCatPriceEdit").val('');
}

function SaveActivityCategoryAdd() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var ActivityCategoryNameAddO = $('#tbActCatNameAdd');
    var ActivityCategoryNameAdd = ActivityCategoryNameAddO.val();
    var ActivityCategoryPriceAddO = $('#tbActCatPriceAdd');
    var ActivityCategoryPriceAdd = ActivityCategoryPriceAddO.val();


    if (!tcg.valid8r.req(ActivityCategoryNameAdd, ActivityCategoryNameAddO)) {
        isValid = false;
        errorText += "Please fill in Activity Name<br />";
    }

    if (isValid) {
        JsonO = {
            Name: ActivityCategoryNameAdd,
            Price: ActivityCategoryPriceAdd,
        };

        url = base_url + "/api/admin/activitycategory/add";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtActivityCategory.fnDraw();
                cancelEdit();
                $("#modalAddActCat").modal("hide");
                tcg.widgets.Growl("Successfully added a Activity Category", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to add the Activity Category", "Error", "danger", 4000);
            });
    }
}

function OpenEditModal(row) {

    cancelEdit();
    var ActivityCategory = dtActivityCategory.fnGetData(row);

    var ActivityCategoryID = ActivityCategory["activityCategoryID"];
    var ActivityCategoryName = ActivityCategory["Name"];
    var ActivityCategoryPrice = ActivityCategory["Price"];

    $('#tbActCatNameEdit').val(ActivityCategoryName);
    $("#tbActCatPriceEdit").val(ActivityCategoryPrice);

    activityCategoryID = ActivityCategoryID;

    $("#modalEditActCat").modal("show");
}

function SaveActivityCategoryEdit() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var ActivityCategoryNameEditO = $('#tbActCatNameEdit');
    var ActivityCategoryNameEdit = ActivityCategoryNameEditO.val();
    var ActivityCategoryPriceEditO = $('#tbActCatPriceEdit');
    var ActivityCategoryPriceEdit = ActivityCategoryPriceEditO.val();


    ///Validation
    if (!tcg.valid8r.req(ActivityCategoryNameEdit, ActivityCategoryNameEditO)) {
        isValid = false;
        errorText += "Please fill in Activity Category Name<br />";
    }


    if (isValid) {
        JsonO = {
            activityCategoryID: activityCategoryID,
            Name: ActivityCategoryNameEdit,
            Price: ActivityCategoryPriceEdit,
        };

        url = base_url + "/api/admin/activitycategory/edit";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                dtActivityCategory.fnDraw();
                cancelEdit();
                $("#modalEditActCat").modal("hide");
                tcg.widgets.Growl("Successfully edited a Activity Category", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                tcg.widgets.Growl("An error has occured while trying to edit the Activity Category", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorEdit').html(errorText);
    }
}

function DeleteGuest(activityCategoryID) {

    tcg.widgets.confirm('Are you sure you want to delete this Activity Category?', 'Activity Category', function () {

        var url = base_url + "/api/admin/activitycategory/delete?activityCategoryID=" + activityCategoryID;

        var successF = function (data) {
            if (data.isSuccess) {

                dtActivityCategory.fnDraw();
                tcg.widgets.Growl("Activity Category has been successfully removed", "Success", "success", 3500);
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