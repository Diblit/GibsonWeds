var mainJson = null;

$(document).ready(function () {

    mainJson = jQuery.parseJSON($('#jsonData').html());
    generateRegistry()
});
function generateRegistry() {
    var registryHtml = ""

    $.each(mainJson.List, function (index, item) {
        if (item.isSelected) {
            registryHtml += '<div class="row rsvpContact" style="display:flex; align-items:center">' +
                '<div class="col-lg-8" style="text-align:right; font-size:13pt">' + item.GiftName + '</div>' +
                ' <div class="col-lg-4">' +
                '<a class="btn btn-default btn-sm disabled" style="background-color:#9a0101 !important; color:#fff !important; font-weight:bold"><i class="fa fa-gift" aria-hidden="true"></i> | I am buying this</a>' +
                '</div></div>';
        }
        else {
            registryHtml += '<div class="row rsvpContact" style="display:flex; align-items:center">' +
                '<div class="col-lg-8" style="text-align:right; font-size:13pt">' + item.GiftName + '</div>' +
                ' <div class="col-lg-4">' +
                '<a onclick="gonnaBuyGift(' + item.registryID + ')" class="btn btn-default btn-sm" style="background-color:#279a01 !important; color:#fff !important; font-weight:bold"><i class="fa fa-gift" aria-hidden="true"></i> &nbsp I want to buy this</a>' +
                '</div></div>';
        }        
    });
    if (registryHtml == "") {
        registryHtml += '<div class="row"><p>No data available yet</p></div>';
    }
    $('#dvLoadRegistry').html(registryHtml);
}
function gonnaBuyGift(registryID) {

    JsonO = {
        registryID: registryID,
        isSelected: true,
    };

    url = base_url + "/api/guest/registryselection";

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
