$(document).ready(function () {

});
function onLoginClick() {
    loading(true);
    // clear old error
    $('#lblError').html('');

    //objects
    var emailO = $('#tbEmail');
    var passwordO = $('#tbPassword');

    var email = emailO.val();
    var password = passwordO.val();

    var isValid = true;

    //do validation
    var validations = [tcg.valid8r.req(email, emailO), tcg.valid8r.req(password, passwordO)];

    if (!tcg.valid8r.range(validations)) {
        isValid = false;
        $('#lblError').text('Please enter your Username and Password');
        loading(false);
        return false;
    }

    var JsonO = {
        returnUrl: returnUrl,
        Email: email,
        Password: password
    };

    // sends json to webservice
    var url = base_url + '/api/login';

    tcg.ajax.post(url, JsonO, function (data) {
        if (data.isSuccess == true) {
            if (data.userType == "User") {
                location.href = "../Guest/RSVP";
            } else {
                location.href = "../Admin/AdminDash";
            }
            
            
        }
        else {
            $('#lblError').html(data.errorText);
            loading(false);
        }
        //loading(false);
    }, function () {
        $('#lblError').html("An authentication error has occured, please try again or contact support");
        loading(false);
    });
}

function loading(load) {
    if (load) {
        $('#dvProgress').show();
        document.getElementById("btnLogin").disabled = true;
        document.getElementById("tbEmail").disabled = true;
        document.getElementById("tbPassword").disabled = true;
    }

    else {
        $('#dvProgress').hide();
        document.getElementById("btnLogin").disabled = false;
        document.getElementById("tbEmail").disabled = false;
        document.getElementById("tbPassword").disabled = false;
    }
}