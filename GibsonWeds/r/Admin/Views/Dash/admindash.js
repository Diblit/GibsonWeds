var mainJson = null;

$(document).ready(function () {

    mainJson = jQuery.parseJSON($('#jsonData').html());
    debugger;

    var InvitedGuests = '';
    var RSVP = '';
    var ActivityInterest = '';

    $.each(mainJson.List, function (index, item) {
        if (index == "invitedGuests") {
            InvitedGuests = item;
        }
        if (index == "RSVPdYes") {
            RSVP = item;
        }
        if (index == "ActivityInterest") {
            ActivityInterest = item;    
        }
    });

    $('#lblDashInvitedGuests').html(InvitedGuests);
    $('#lblDashRSVPd').html(RSVP);
    $('#lblDashActivityInterest').html(ActivityInterest);

});