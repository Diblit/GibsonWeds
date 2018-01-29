var tbl_csrfToken = null;

/* Filtering Delay */
jQuery.fn.dataTableExt.oApi.fnSetFilteringDelay = function (oSettings, iDelay) { var _that = this, iDelay = (typeof iDelay == 'undefined') ? 250 : iDelay; this.each(function (i) { $.fn.dataTableExt.iApiIndex = i; var $this = this, oTimerId = null, sPreviousSearch = null, anControl = $('input', _that.fnSettings().aanFeatures.f); anControl.unbind('keyup').bind('keyup', function () { var $$this = $this; if (sPreviousSearch === null || sPreviousSearch != anControl.val()) { window.clearTimeout(oTimerId); sPreviousSearch = anControl.val(); oTimerId = window.setTimeout(function () { $.fn.dataTableExt.iApiIndex = i; _that.fnFilter(anControl.val()); }, iDelay); } }); return this; }); return this; };

/* Ajax post with retrieve */
$.fn.dataTableExt.oApi.fnPostAndReload = function (oSettings, sPostUrl, sPostData, keepPage) {
    if (typeof sNewSource != 'undefined' && sNewSource != null) {
        oSettings.sAjaxSource = sNewSource;
    }

    this.oApi._fnProcessingDisplay(oSettings, true);
    this._myPostUrl = sPostUrl;
    this._myPostData = sPostData;

    if (keepPage === true) {
        this._myPostDisplayStart = oSettings._iDisplayStart;
    }
    else {
        this._myPostDisplayStart = 0;
    }

    this.fnDraw();
};

/* Refresh data table with new ajax source */
$.fn.dataTableExt.oApi.fnReloadAjax = function (oSettings, sNewSource, fnCallback, bStandingRedraw) {
    if (typeof sNewSource != 'undefined' && sNewSource != null) {
        oSettings.sAjaxSource = sNewSource;
    }
    this.oApi._fnProcessingDisplay(oSettings, true);
    var that = this;
    var iStart = oSettings._iDisplayStart;

    this.fnDraw();

}

function doDataTablePostAjaxCalling(thisO, sSource, aoData, fnCallback, oSettings) {
    
    //Getting auth token
    if (tbl_csrfToken == null) {
        tbl_csrfToken = $("input[name='__RequestVerificationToken']").val();
    }
    var h_opt = tbl_csrfToken ? { "X-XSRF-Token": tbl_csrfToken } : {};

    // check if there is any json info to post
    if (thisO._myPostData != null && thisO._myPostUrl != null) {
        // fetch tracking post vars
        var myUrl = thisO._myPostUrl;
        var myData = thisO._myPostData;
        var myDataDisplayStart = thisO._myPostDisplayStart;

        // resert vars in widget
        thisO._myPostUrl = null;
        thisO._myPostData = null;
        thisO._myPostDisplayStart = 0;

        // find Display Start and set from tracking vars
        for (var i = 0; i < aoData.length; i++) {
            var item = aoData[i];
            if (item.name == 'iDisplayStart') {
                item.value = myDataDisplayStart;
                aoData[i] = item;
                oSettings._iDisplayStart = myDataDisplayStart;
                break;
            }
        }

        // start creating url to post to
        if (myUrl.indexOf('?') == -1) {
            myUrl += '?';
        }
        else {
            if (aoData.length > 0) {
                myUrl += '&';
            }
        }
        myUrl += $.param(aoData);

        // do ajax post
        oSettings.jqXHR = $.ajax({
            "url": myUrl,
            "data": $.toJSON(myData),
            "success": function (json) {
                $(oSettings.oInstance).trigger('xhr', oSettings);
                fnCallback(json);
            },
            "dataType": "json",
            "contentType": "application/json",
            "cache": false,
            "type": "POST",
            "headers": h_opt,
            "error": function (xhr, error, thrown) {
                if (error == "parsererror") {
                    oSettings.oApi._fnLog(oSettings, 0, "DataTables warning: JSON data from " +
                            "server could not be parsed. This is caused by a JSON formatting error.");
                }
            }
        });
    }
    else {
        // normal ajax GET
        oSettings.jqXHR = $.ajax({
            "url": sSource,
            "data": aoData,
            "success": function (json) {
                $(oSettings.oInstance).trigger('xhr', oSettings);
                fnCallback(json);
            },
            "dataType": "json",
            "contentType": "application/json",
            "cache": false,
            "type": oSettings.sServerMethod,
            "headers": h_opt,
            "error": function (xhr, error, thrown) {
                if (error == "parsererror") {
                    oSettings.oApi._fnLog(oSettings, 0, "DataTables warning: JSON data from " +
                            "server could not be parsed. This is caused by a JSON formatting error.");
                }
            }
        });
    }
}

/* Row Editing */
function DtCustomRowEdit(event) {
    // Get Options from event
    var o = event.data;

    // create default options
    var _opt = {
        myData: null,
        iDisplayIndex: null,
        EditCallback: null,
        ValidateCallback: null,
        SaveCallback: null,
        CancelCallback: null,
        Classes: {
            btnSave: '',
            btnCancel: ''
        }
    };
    // merge options
    $.extend(_opt, o);

    // get ref to Edit Button
    var that = $(this);

    // Create Save and cancel Buttons
    var saveBtn = $('<input type="button" class="' + _opt.Classes.btnSave + '" value="save"/>"');
    var cancelBtn = $('<input type="button" class="' + _opt.Classes.btnCancel + '" value="cancel"/>"');

    // Cancel Button Click Event
    cancelBtn.click({
        editBtn: that,
        saveBtn: saveBtn,
        _opt: _opt
    }, function (event) {
        // get ref to Cancel Button
        var that = $(this);
        var _opt = event.data._opt;

        // get tr container
        var cont = that.closest('tr');

        // Callback user function - for event handlers etc
        // for cancel action
        var callbk = event.data._opt.CancelCallback;
        if (typeof callbk == 'function' && callbk != null) {
            callbk(_opt.myData, _opt.iDisplayIndex, cont);
        }

        // cleanup
        event.data.saveBtn.remove();
        that.remove();
        event.data.editBtn.css('display', 'inline');
    });

    // Save Button Click Event
    saveBtn.click({
        editBtn: that,
        cancelBtn: cancelBtn,
        _opt: _opt
    }, function (event) {
        // get ref to Save Button
        var that = $(this);
        var _opt = event.data._opt;

        // get tr container
        var cont = that.closest('tr');

        // Callback user function - for event handlers etc
        // This is for Validation checks
        var isValid = true;
        var callbk = event.data._opt.ValidateCallback;
        if (typeof callbk == 'function' && callbk != null) {
            isValid = callbk(_opt.myData, _opt.iDisplayIndex, cont);
        }

        // Validation failed - Stop
        if (!isValid) {
            return;
        }

        // Callback user function - for event handlers etc
        // for save action
        var callbk = event.data._opt.SaveCallback;
        if (typeof callbk == 'function' && callbk != null) {
            callbk(_opt.myData, _opt.iDisplayIndex, cont);
        }

        // Cleanup
        event.data.cancelBtn.remove();
        that.remove();
        event.data.editBtn.css('display', 'inline');
    });

    // get and insert controls for edit fields
    var cont = that.closest('tr');

    var callbk = _opt.EditCallback;
    if (typeof callbk == 'function' && callbk != null) {
        callbk(_opt.myData, _opt.iDisplayIndex, cont);
    }

    // set button display
    that.css('display', 'none');
    that.after(cancelBtn);
    that.after(saveBtn);
}