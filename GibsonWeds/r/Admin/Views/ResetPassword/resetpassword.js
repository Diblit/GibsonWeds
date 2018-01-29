$(document).ready(function () {

    
});

function SaveNewPassword() {
    var isValid = true;
    var errorText = "";
    $(".haserror").removeClass("haserror");

    var OldPasswordO = $('#txtOldPassword');
    var OldPassword = OldPasswordO.val();
    var NewPasswordO = $('#txtNewPassword');
    var NewPassword = NewPasswordO.val();
    var ConfirmNewPasswordO = $('#txtConfirmNewPassword');
    var ConfirmNewPassword = ConfirmNewPasswordO.val();

    //do validation
    var validations = [tcg.valid8r.req(OldPassword, OldPasswordO), tcg.valid8r.req(NewPassword, NewPasswordO), tcg.valid8r.req(ConfirmNewPassword, ConfirmNewPasswordO)];
    if (!tcg.valid8r.range(validations)) {
        isValid = false;
        $('#lblError').text('Please enter all Password fields');        
        return false;
    }

    if(!tcg.valid8r.passwordvalid(NewPassword, NewPasswordO)){
        isValid = false;
        $('#lblError').text('Password must be greater than 6 characters and contain at least 1 number, letter and capital letter.');
        return false;
    };

    if(!tcg.valid8r.passwordconfirmed(NewPassword, ConfirmNewPassword, ConfirmNewPasswordO)){
        isValid = false;
        $('#lblError').text('Confirm Password does not match New Password');
        return false;
    };

    if (isValid) {
        JsonO = {
            OldPassword: OldPassword,
            NewPassword: NewPassword,
        };

        url = base_url + "/api/guest/resetpassword";

        tcg.ajax.post(url, JsonO, function (data) {
            if (data.isSuccess == true) {
                $('#lblError').text('');
                OldPasswordO.val('');
                NewPasswordO.val('');
                ConfirmNewPasswordO.val('');
                $(".haserror").removeClass("haserror");
                $('#lblSuccess').text('Successfully reset you Password.')
                tcg.widgets.Growl("Successfully reset your Password", "Success", "success", 4000);
            }
            else {
                $('#lblError').text('Invalid Password');
            }
        },
            function () {
                
                //$('#lblError').text('Invalid Old Password');
            });
    }
    else {
        //$('#lblErrorGuestCoupleEdit').html(errorText);
    }
}