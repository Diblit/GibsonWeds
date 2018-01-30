var mainJson = null;

$(document).ready(function () {

    mainJson = jQuery.parseJSON($('#jsonData').html());    
    generateRSVP();
    CheckRsvp();
});

function generateRSVP() {
    var rsvpHtml = ""

    $.each(mainJson.List, function (index, item) {
        if (item.hasRSVPd == true) {
            if (item.isAttending == true) {
                rsvpHtml += '<div class="row rsvpContact">' +
                    '<div class="col-lg-6" style="text-align:right; font-size:18pt">' + item.FirstName + ' ' + item.LastName + '</div>' +
                    '<div class="col-lg-6">' +
                    '<a class="btn btn-default btn-sm disabled" style="background-color:#01699a !important; color:#fff !important; font-weight:bold">Is Attending</a></div></div>';
            }
            else {
                rsvpHtml += '<div class="row rsvpContact">' +
                    '<div class="col-lg-6" style="text-align:right; font-size:18pt">' + item.FirstName + ' ' + item.LastName + '</div>' +
                    '<div class="col-lg-6">' +
                    '<a class="btn btn-default btn-sm disabled" style="background-color:#01699a !important; color:#fff !important; font-weight:bold">Is Not Attending</a></div></div>';
            }
        } else {
            rsvpHtml += '<div class="row rsvpContact">' +
                '<div class="col-lg-6" style="text-align:right; font-size:18pt">' + item.FirstName + ' ' + item.LastName + '</div>' +
                '<div class="col-lg-6">' +
                '<a class="btn btn-default btn-sm" onclick="GuestAtt(' + item.userID + ')" style="background-color:#279a01 !important; color:#fff !important; font-weight:bold">Attending</a>' +
                '<a class="btn btn-default btn-sm" onclick="GuestNotAtt(' + item.userID + ')" style="background-color:#9a0101 !important; color:#fff !important; font-weight:bold">Not Attending</a></div></div>';
        }
        
    });


    $('#dvLoadRsvp').html(rsvpHtml);
}

function GuestAtt(userID) {
    JsonO = {
        userID: userID,
        hasRSVPd: true,
        isAttending: true,
    };

    url = base_url + "/api/guest/update";

    tcg.ajax.post(url, JsonO, function (data) {
        if (data.isSuccess == true) {
            location.reload();
        }
        else {
            tcg.widgets.Growl("Cannot edit Registry Item when it is already assigned to Guest", "Error", "danger", 4000);
        }
    },
        function () {
            tcg.widgets.Growl("Cannot edit Registry Item when it is already assigned to Guest", "Error", "danger", 4000);
        });
}

function GuestNotAtt(userID) {
    JsonO = {
        userID: userID,
        hasRSVPd: true,
        isAttending: false,
    };

    url = base_url + "/api/guest/update";

    tcg.ajax.post(url, JsonO, function (data) {
        if (data.isSuccess == true) {
            location.reload();
        }
        else {
            tcg.widgets.Growl("Cannot edit Registry Item when it is already assigned to Guest", "Error", "danger", 4000);
        }
    },
        function () {
            tcg.widgets.Growl("Cannot edit Registry Item when it is already assigned to Guest", "Error", "danger", 4000);
        });
}
function CheckRsvp() {
    //var isAtt = $('#isAttendingGuest').val();
    var isAtt = isAttendingWed;

    //isAttendingWed
    if (isAtt == "False") {
        $('.hide-no-rsvp').hide();
    }
    else {
        $('.hide-no-rsvp').show();
    }
}