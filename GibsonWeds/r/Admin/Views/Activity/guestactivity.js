var mainJson = null;

$(document).ready(function () {

    mainJson = jQuery.parseJSON($('#jsonData').html());
    generateActivity()
});
function generateActivity() {
    var activityHtml = ""

    $.each(mainJson.List, function (index, item) {
        activityHtml += '<div class="row rsvpContact">' +
            '<div class="col-lg-6" style="text-align:right; font-size:18pt">' + item.Name + '</div>' +
            '<div class="col-lg-6"><a onclick="addToActivity(' + item.activityCategoryID + ')" class="btn btn-default btn-sm" style="background-color:#279a01 !important; color:#fff !important; font-weight:bold">R ' + item.Price + ' - Interested</a>' +
            '</div ></div>';
    });
    if (activityHtml == "") {
        activityHtml += '<div class="row"><p>No data available yet</p></div>';
    }
    $('#dvLoadActivity').html(activityHtml);
}

function addToActivity(actCatID) {
    JsonO = {
        activityCategoryID: actCatID,
    };

    url = base_url + "/api/guest/activitysategoryselect";

    tcg.ajax.post(url, JsonO, function (data) {
        if (data.isSuccess == true) {
            location.reload();
        }
        else {
            //tcg.widgets.Growl("Cannot edit Registry Item when it is already assigned to Guest", "Error", "danger", 4000);
        }
    },
        function () {
            //tcg.widgets.Growl("Cannot edit Registry Item when it is already assigned to Guest", "Error", "danger", 4000);
        });
}