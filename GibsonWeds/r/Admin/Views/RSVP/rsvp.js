var mainJson = null;
var mainGroupCoupleID = null;
$(document).ready(function () {

    mainJson = jQuery.parseJSON($('#jsonData').html());    
    generateRSVP();
    CheckRsvp();
    CheckAllowPlusOne();
});

function generateRSVP() {
    var rsvpHtml = ""

    $.each(mainJson.List, function (index, item) {
        mainGroupCoupleID = item.groupCoupleID;
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
function CheckAllowPlusOne() {
    //var isAtt = $('#isAttendingGuest').val();

    var allowPlus1 = allowPlusOne;
    var isAtt = isAttendingWed;

    if (isAtt == "True") {
        if (allowPlus1 == "True") {
            $('.guest-plus-one').show();
        } else {
            $('.guest-plus-one').hide();
        }        
    } else {
        $('.guest-plus-one').hide();
    }
   
}
function SavePlusOneAdd() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var PlusOneFirstNameAddO = $('#tbPlusOneNameAdd');
    var PlusOneFirstNameAdd = PlusOneFirstNameAddO.val();
    var PlusOneLastNameAddO = $('#tbPlusOneSurnameAdd');
    var PlusOneLastNameAdd = PlusOneLastNameAddO.val();
    var PlusOneEmailAddO = $('#tbPlusOneEmailAdd');
    var PlusOneEmailAdd = PlusOneEmailAddO.val();
    var PlusOneCellAddO = $('#tbPlusOneCellAdd');
    var PlusOneCellAdd = PlusOneCellAddO.val();


    ///Validation
    if (!tcg.valid8r.req(PlusOneFirstNameAdd, PlusOneFirstNameAddO)) {
        isValid = false;
        errorText += "Please fill in Name<br />";
    }
    if (!tcg.valid8r.req(PlusOneEmailAdd, PlusOneEmailAddO)) {
        isValid = false;
        errorText += "Please fill in Email<br />";
    }
    if (!tcg.valid8r.req(PlusOneCellAdd, PlusOneCellAddO)) {
        isValid = false;
        errorText += "Please fill in Cell<br />";
    }


    if (isValid) {
        JsonO = {
            FirstName: PlusOneFirstNameAdd,
            LastName: PlusOneLastNameAdd,
            Email: PlusOneEmailAdd,
            Cell: PlusOneCellAdd,
            PasswordHash: PlusOneCellAdd,
            allowPlusOne: false,
            groupCoupleID: mainGroupCoupleID,
            isPlusOne: true,
            isGuest: true,
            isAdmin: false,
            hasRSVPd: true,
            isAttending: true
        };

        url = base_url + "/api/admin/plusone/add";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {

                $('.guest-plus-one').hide();
                location.reload();
                //tcg.widgets.Growl("Successfully added a Guest", "Success", "success", "3500");
            }
            else {
                //tcg.widgets.Growl(data.errorText, "Error", "danger", 4000);
            }
        },
            function () {
                //tcg.widgets.Growl("An error has occured while trying to add the Guest", "Error", "danger", 4000);
            });
    }
    else {
        $('#lblErrorP1Add').html(errorText);
    }
}