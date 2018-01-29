var tcg = tcg || {};
tcg.widgets = tcg.widgets || {};
tcg.widgets.date = {};
tcg.widgets.spin = {};
tcg.widgets.spin.perc = {};
tcg.widgets.qty = {};
tcg.widgets.perc = {};
tcg.widgets.money = {};
tcg.widgets.selectbox = {};
tcg.widgets.selectbox.ajax = {};
tcg.widgets.loading = {};
tcg.ajax = {};
tcg.event = {};
tcg.button = {};
tcg.valid8r = {};
tcg.utils = {};
tcg.utils.numericals = {};

function urlNameSafe(text) {
    // a-z
    // A-Z
    // 0-9
    // _-.,
    // /\[]{}
    // |#@
    var allowedChars = "abcdefghijklmnopqrstuvwxyz";
    allowedChars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    allowedChars += "0123456789";
    allowedChars += "-_,/\()[]{}|#@";

    for (var i = 0; i < text.length; i++) {
        if (allowedChars.indexOf(text.charAt(i)) == -1) {
            return false;
        }
    }
    return true;
}

// the same as urlNameSafe just passing in allowedChars String
function FriendlyUrlSafe(text, allowedChars) {

    if (allowedChars == "") {
        return urlNameSafe(text)
    }
    else {

        for (var i = 0; i < text.length; i++) {
            if (allowedChars.indexOf(text.charAt(i)) == -1) {
                return false;
            }
        }
        return true;
    }

}

//Boostrap growl notifactions
tcg.widgets.Growl = function (body, header, growlType, Timedelay) {
    // growlType = success, info, warning, danger
    if (typeof growlType === "undefined") {
        growlType = "info"
    }
    if (typeof Timedelay === "undefined") {
        Timedelay = "5000";
    }
    if (typeof header === "undefined") {
        header = "popup Message"
    }
    if (typeof body === "undefined") {
        body = "";
    }
    $.bootstrapGrowl('<h4>' + header + '</h4> <p>' + body + '</p>', {
        type: growlType,
        delay: Timedelay,
        allow_dismiss: true
    });
};

//validate date ranges
tcg.valid8r.daterange = function (startval, endval, startcont, endcont) {

    var startDt = new Date(startval);
    var endDt = new Date(endval);

    //if there is a start date
    if (startval != null && startval != "") {
        ///if start date is greater than end date
        if (startDt > endDt && endval != null) {
            tcg.valid8r.seterror(endcont);
            return false;
        }
            //else remove error
        else {
            tcg.valid8r.clearerror(endcont);
            return true;
        }
    }
    //if there is no start date and there is an end date
    if (endval != null && endval != "") {
        if (endDt <= startDt && startval != null) {
            tcg.valid8r.seterror(endcont);
            return false;
        }
        //if selected end date is greater than start date - remove error
        if (endDt > startDt && startval != null) {
            tcg.valid8r.clearerror(endcont);
            return true;
        }
    }
    //if no date selected
    return true;
}



/* DATAPICKER */
tcg.widgets.date.create = function (selector, within, options) {
    var defaults = { changeMonth: true, changeYear: true, dateFormat: 'dd M yy', onSelect: function (dateText, inst) { }, onClose: function (dateText, inst) { $(this).trigger("change"); } };
    var sel = (within == null) ? $(selector) : $(selector, within);

    sel.datepicker($.extend(defaults, options));

    // bind blur event
    sel.change(function () {
        var thisO = $(this);
        var dtVal = Date.parse(thisO.val());
        if (isNaN(dtVal)) {
            thisO.val('');
        }
        else {
            thisO.datepicker("setDate", new Date(dtVal));
        }
    });

    return sel;
};
tcg.widgets.date.getdate = function (SelOrObj) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (isNaN(Date.parse(SelOrObj.val()))) {
        return null
    }
    return SelOrObj.datepicker('getDate');
}
tcg.widgets.date.localdatestring = function (d) {
    function pad(n) { return n < 10 ? '0' + n : n }
    return d.getFullYear() + '-'
      + pad(d.getMonth() + 1) + '-'
      + pad(d.getDate()) + 'T'
      + pad(d.getHours()) + ':'
      + pad(d.getMinutes()) + ':'
      + pad(d.getSeconds()); //  + 'Z';
}

/* SPINNER */
tcg.widgets.spin.create = function (selector, within, options) {
    var sel = (within == null) ? $(selector) : $(selector, within);
    sel.each(function () {
        var meta = eval("(" + $(this).attr('meta') + ')');
        $(this).spinner($.extend(meta, options));
    });
    return sel;
}
tcg.widgets.spin.perc.create = function (selector, within, options) {
    var defaults = { suffix: '%', min: 0, max: 100, places: 2 };
    var sel = (within == null) ? $(selector) : $(selector, within);
    return sel.spinner($.extend(defaults, options));
}
tcg.widgets.spin.val = function (SelOrObj, val) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (typeof val === "undefined") {
        return SelOrObj.spinner("value");
    }
    else {
        SelOrObj.spinner("value", val);
    }
}

/* QTY */
tcg.widgets.qty.create = function (selector) {
    var sel = $(selector);
    sel.each(function () {
        //check to see if its a  number
        var val = parseInt($(this).val());
        if (isNaN(val)) {
            //clear the val
            $(this).val('');
        }
        else {
            $(this).val(val);
        }
    });
    //on chnage, check if its a number
    sel.change(function () {
        //check to see if its a  number
        var val = parseInt($(this).val());
        if (isNaN(val)) {
            //clear the val
            $(this).val('');
        }
        else {
            $(this).val(val);
        }
    });
    sel.blur(function () {
        //check to see if its a  number
        var val = parseInt($(this).val());
        if (isNaN(val)) {
            //clear the val
            $(this).val('');
        }
        else {
            $(this).val(val);
        }
    });
    return sel;
}
tcg.widgets.qty.val = function (SelOrObj, val) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (typeof val === "undefined") {
        //get the value
        var value = parseInt(SelOrObj.val());
        if (isNaN(value))
            return null;
        return value;
    }
    else {
        SelOrObj.val(val);
    }
}

/* PERC */
tcg.widgets.perc.create = function (selector) {
    var sel = $(selector);
    sel.each(function () {
        //check to see if its a  number
        var val = parseInt($(this).val());
        if (isNaN(val)) {
            //clear the val
            $(this).val('');
        }
        else if (val > 100) {
            $(this).val(100); //set max
        }
        else if (val < 0) {
            $(this).val(0); //set min
        }
        else {
            $(this).val(val);
        }
    });
    //on chnage, check if its a number
    sel.change(function () {
        //check to see if its a  number
        var val = parseInt($(this).val());
        if (isNaN(val)) {
            //clear the val
            $(this).val('');
        }
        else if (val > 100) {
            $(this).val(100); //set max
        }
        else if (val < 0) {
            $(this).val(0); //set min
        }
        else {
            $(this).val(val);
        }
    });
    sel.blur(function () {
        //check to see if its a  number
        var val = parseInt($(this).val());
        if (isNaN(val)) {
            //clear the val
            $(this).val('');
        }
        else if (val > 100) {
            $(this).val(100); //set max
        }
        else if (val < 0) {
            $(this).val(0); //set min
        }
        else {
            $(this).val(val);
        }
    });
    return sel;
}
tcg.widgets.perc.val = function (SelOrObj, val) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (typeof val === "undefined") {
        //get the value
        var value = parseInt(SelOrObj.val());
        if (isNaN(value))
            return null;
        return value;
    }
    else {
        SelOrObj.val(val);
    }
}

tcg.widgets.perc.isvalid = function (d, min, max) {
    var rgx = /^[0-9]*\.?[0-9]*$/;
    var isValid = d.match(rgx);

    var dFloat = parseFloat(d);

    if (isNaN(dFloat)) {
        isValid = false;
    }

    if (isValid) {
        if (min != null && min != undefined && !isNaN(min)) {
            if (dFloat < min) isValid = false;
        }

        if (max != null && max != undefined && !isNaN(max)) {
            if (dFloat > max) isValid = false;
        }
    }

    return isValid;
}

/* MONEY - NEW - Created by Bilal, Modified by MI Laher to inc cur symbol */
/*Reece - Added decimal variable*/
tcg.widgets.money.create = function (selector, symbol, decimal) {
    var sel = $(selector);
    var symbolExists = symbol != undefined && symbol != null && symbol != "";

    if (decimal == '' || decimal == null || decimal < 0) {
        decimal = 2;
    }

    sel.each(function () {

        //strip cur symbol 
        var selVal = $(this).val();
        if (symbolExists) {
            selVal = selVal.replace(symbol, "");
            selVal = selVal.trim();
        }

        //check if its  a number
        var value = parseFloat(selVal);
        if (isNaN(value)) {
            //clear the val
            $(this).val('');
        }
        else {

            if (symbolExists) {
                $(this).val(symbol + " " + value.toFixed(decimal));
            }
            else {
                $(this).val(value.toFixed(decimal));
            }
        }
    });
    //on chnage, check if its a number
    sel.change(function () {
        //strip cur symbol 
        var selVal = $(this).val();
        if (symbolExists) {
            selVal = selVal.replace(symbol, "");
            selVal = selVal.trim();
        }

        //check to see if its a  number
        var value = parseFloat(selVal);
        if (isNaN(value)) {
            //clear the val
            $(this).val('');
        }
        else {
            if (symbolExists) {
                $(this).val(symbol + " " + value.toFixed(decimal));
            }
            else {
                $(this).val(value.toFixed(decimal));
            }
        }
    });
    sel.blur(function () {
        //strip cur symbol 
        var selVal = $(this).val();
        if (symbolExists) {
            selVal = selVal.replace(symbol, "");
            selVal = selVal.trim();
        }

        //check to see if its a  number
        var value = parseFloat(selVal);
        if (isNaN(value)) {
            //clear the val
            $(this).val('');
        }
        else {
            if (symbol != undefined && symbol != null && symbol != "") {
                $(this).val(symbol + " " + value.toFixed(decimal));
            }
            else {
                $(this).val(value.toFixed(decimal));
            }
        }
    });

    return sel;
}
tcg.widgets.money.val = function (SelOrObj, symbol, val) {

    var symbolExists = symbol != undefined && symbol != null && symbol != "";

    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (typeof val === "undefined") {
        var selVal = SelOrObj.val();
        if (symbolExists) {
            selVal = selVal.replace(symbol, "");
            selVal = selVal.trim();
        }
        //get the value
        var value = parseFloat(selVal);
        if (isNaN(value))
            return null;
        return value;
    }
    else {
        val = val.toString();

        if (symbolExists) {
            val = val.replace(symbol, "");
            val = val.trim();
        }
        var value = parseFloat(val);
        if (isNaN(value)) {
            //clear the val
            value = 0;
            SelOrObj.val(symbolExists ? symbol + " " + value.toFixed(2) : value.toFixed(2));
        }
        else {
            SelOrObj.val(symbolExists ? symbol + " " + value.toFixed(2) : value.toFixed(2));
        }
    }
}

tcg.widgets.money.format = function (amount, places, symbol, thousand, decimal) {
    /*
        To reverse use: priceVal = parseFloat(price.replace(/[^0-9-.]/g, '')); // 12345.99
    */

    places = !isNaN(places = Math.abs(places)) ? places : 2;
    symbol = symbol !== undefined ? symbol : "$";
    thousand = thousand || ",";
    decimal = decimal || ".";
    var number = amount,
	    negative = number < 0 ? "-" : "",
	    i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10) + "",
	    j = (j = i.length) > 3 ? j % 3 : 0;
    return symbol + negative + (j ? i.substr(0, j) + thousand : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousand) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : "");
};

/* MONEY */
//tcg.widgets.money.create = function (selector, within, options, useMetaOptions) {
//    var defaults = { prefix: 'R', group: ',', step: 0.01, largeStep: 10.00, min: 0, max: null };
//    var sel = (within == null) ? $(selector) : $(selector, within);

//    if (useMetaOptions != 'undefined' && useMetaOptions != null && useMetaOptions != true) {
//        sel.each(function () {
//            var meta = eval("(" + $(this).attr('meta') + ')');
//            $(this).spinmoney($.extend($.extend(defaults, meta), options));
//        });
//    }
//    else {
//        sel.spinmoney($.extend(defaults, options));
//    }
//    return sel;
//}
//tcg.widgets.money.val = function (SelOrObj, val) {
//    if (typeof SelOrObj === "string") {
//        SelOrObj = $(SelOrObj);
//    }

//    if (typeof val === "undefined") {
//        return SelOrObj.spinmoney("value");
//    }
//    else {
//        SelOrObj.spinmoney("value", val);
//    }
//}

/* SELECTBOX */
tcg.widgets.selectbox.supported = function () {
    if ($.browser.msie && ($.browser.version === "6.0" || $.browser.version === "7.0")) {
        return false;
    }
    return true;
}
tcg.widgets.selectbox.create = function (selector, within, options) {
    var sel = (within == null) ? $(selector) : $(selector, within);
    if (!tcg.widgets.selectbox.supported()) {
        return sel;
    }
    return sel.select2(options);
}
tcg.widgets.selectbox.enable = function (SelOrObj, isenabled) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (!tcg.widgets.selectbox.supported()) {
        isenabled == true ? SelOrObj.removeAttr('disabled') : SelOrObj.attr('disabled', 'disabled');
    }
    else {
        isenabled == true ? SelOrObj.select2("enable") : SelOrObj.select2("disable");
    }
}
tcg.widgets.selectbox.val = function (SelOrObj, val) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (typeof val === "undefined") {
        if (!tcg.widgets.selectbox.supported()) {
            return SelOrObj.val();
        }
        else {
            return SelOrObj.select2("val");
        }
    }
    else {
        if (!tcg.widgets.selectbox.supported()) {
            SelOrObj.val(val);
        }
        else {
            SelOrObj.select2("val", val);
        }
    }
}

tcg.widgets.selectbox.validatecont = function (SelOrObj) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (!tcg.widgets.selectbox.supported()) {
        return SelOrObj;
    }
    else {
        return SelOrObj.select2("container");
    }
}

tcg.widgets.selectbox.ajax.create = function (selector, within, options) {
    // for on select - pass in option onselect as function

    var defaults = {
        minimumInputLength: 1,
        initSelection: function (element, callback) {
            var v = element.val();
            if (v != '') {
                var a = v.split(",");
                if (a.length == 2) {
                    var result = { id: a[0], value: a[1] };
                    element.val(a[0]);
                    callback(result);
                }
            }
        },
        ajax: {
            dataType: 'json',
            quietMillis: 300,
            data: function (term, page) { // page is the one-based page number tracked by Select2
                return {
                    term: term, //search term
                    page_limit: 10, // page size
                    page: page // page number
                };
            },
            results: function (data, page) {
                var more = (page * 10) < data.total; // whether or not there are more results available

                // notice we return the value of more so Select2 knows if more results can be loaded
                return { results: data.items, more: more };
            }
        }
    };
    var ajax_opt = $.extend(defaults.ajax, options.ajax);
    options = $.extend(defaults, options);
    options.ajax = ajax_opt;

    var sel = (within == null) ? $(selector) : $(selector, within);
    if (!tcg.widgets.selectbox.supported()) {
        // not supported - render as input autocomplete

        sel.css('display', 'none');         // make orig hidden field - display none
        sel.after('<input type="text" />'); // create automcomplete input
        var auto = sel.next();      // add to dom
        auto.css('width', sel.css('width'));

        // sel initial value
        options.initSelection(sel, function (result) {
            auto.val(result.value);
            sel.attr('meta', result.value);
        });

        // create auto complete
        auto = auto.autocomplete({
            minLength: options.minimumInputLength,
            select: options.onselect != 'undefined' ? function (event, ui) {
                // we have our own onselect
                sel.val(ui.item.id);
                sel.attr('meta', ui.item.value);
                options.onselect(ui.item.id, ui.item);
            } : function (event, ui) {
                // we don't have our own onselect
                sel.val(ui.item.id);
                sel.attr('meta', ui.item.value);
            },
            change: function () {
                if (sel.attr('meta') != auto.val()) {
                    auto.val(sel.attr('meta'));
                }
            },
            delay: options.ajax.quietMillis,
            source: function (request, response) {
                var term = request.term;
                $.getJSON(options.ajax.url, request, function (data, status, xhr) {
                    response(data.items);
                });
            }
        });

        // if we format results - do format
        if (options.formatResult != 'undefined') {
            auto.data("autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                .data("item.autocomplete", item)
                .append("<a>" + options.formatResult(item) + "</a>")
                .appendTo(ul);
            };
        }
        return auto;
    }

    var cb = sel.select2(options);
    if (options.onselect != 'undefined') {
        cb.on("change", function (e) {
            options.onselect(e.val, $(this).select2("data"));
        });
    }

    return cb;
}
tcg.widgets.selectbox.ajax.enable = function (SelOrObj, isenabled) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (!tcg.widgets.selectbox.supported()) {
        isenabled == true ? SelOrObj.next().autocomplete("enable").removeAttr('disabled') : SelOrObj.next().autocomplete("disable").attr('disabled', 'disabled');
    }
    else {
        isenabled == true ? SelOrObj.select2("enable") : SelOrObj.select2("disable");
    }
}
tcg.widgets.selectbox.ajax.val = function (SelOrObj, val, text) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (typeof val === "undefined") {
        if (!tcg.widgets.selectbox.supported()) {
            return SelOrObj.val();
        }
        else {
            return SelOrObj.select2("val");
        }
    }
    else {
        if (!tcg.widgets.selectbox.supported()) {
            SelOrObj.val(val);
            SelOrObj.attr('meta', text);
            SelOrObj.next().val(text);
        }
        else {
            SelOrObj.select2("val", val + "," + text);
        }
    }
}
tcg.widgets.selectbox.ajax.text = function (SelOrObj) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (!tcg.widgets.selectbox.supported()) {
        return SelOrObj.attr('meta');
    }
    else {
        return SelOrObj.select2("data").value;
    }
}
tcg.widgets.selectbox.ajax.validatecont = function (SelOrObj) {
    if (typeof SelOrObj === "string") {
        SelOrObj = $(SelOrObj);
    }

    if (!tcg.widgets.selectbox.supported()) {
        return SelOrObj.next();
    }
    else {
        return SelOrObj.select2("container");
    }
}

/* LOADING */
//tcg.widgets.loading.show = function (functionAfter, closeAfter) {
//    var dbb = $("#loadingScreen");
//    if (dbb.length == 0) {

//        $('body').append('<div class="modal fade" id="loadingScreen" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
//         '<div class="modal-dialog modal-sm"><div class="modal-content">' +
//         '<div class="modal-body center-block"><div class="bar text-center"><i class="fa fa-spinner fa-pulse" style="font-size:40px;"></i></div></div></div></div></div></div>');
//        $('body').append(loadingScreen);
//        $('#loadingScreen').modal();
//    }
//    else {
//        dbb.modal();
//    }
//    if (functionAfter != null) {
//        if (closeAfter == true) {
//            setTimeout(function () { functionAfter(); closeLoadingScreen(); }, 0);
//        }
//        else {
//            setTimeout(functionAfter, 0);
//        }
//    }
//}
/* LOADING */
tcg.widgets.loading.show = function (title, functionAfter, closeAfter) {
    var dbb = $("#loadingScreen");
    if (dbb.length == 0) {

        $('body').append('<div class="modal fade" id="loadingScreen" tabindex="-1" data-backdrop="static" data-keyboard="false" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
         '<div class="modal-dialog modal-sm"><div class="modal-content"><div class="modal-header"><h4 class="modal-title" id="myModalLabel">' + title + '</h4></div>' +
         '<div class="modal-body center-block"><div class="bar text-center"><span class="loading-spinner"><i class="fa fa-spinner fa-spin fa-3x fa-fw text-info"></i><span></div></div></div></div></div></div>');
        $('body').append(loadingScreen);
        $('#loadingScreen').modal();
    }
    else {
        dbb.find('modal-header').find('h4').html(title);
        dbb.modal();
    }
    if (functionAfter != null) {
        if (closeAfter == true) {
            setTimeout(function () { functionAfter(); closeLoadingScreen(); }, 0);
        }
        else {
            setTimeout(functionAfter, 0);
        }
    }
}

tcg.widgets.loading.close = function () {
    //$("#loadingScreen").dialog('close');
    $("#loadingScreen").modal('hide');
}
tcg.widgets.loading.close = function () {
    //$("#loadingScreen").dialog('close');
    $("#loadingScreen").modal('hide');
}


tcg.widgets.loading.wrap = function (thisO) {
    thisO.wrapInner('<div class="loadwrapper" style="display:none;"></div>');
    thisO.append('<div class="loadwrapper_loader"><center><img src="/r/img/chaseB_64.gif"/></center></div>');
}
tcg.widgets.loading.unwrap = function (thisO) {
    thisO.find(".loadwrapper").contents().unwrap();
    thisO.find(".loadwrapper_loader").remove();
}

/* AJAX */
tcg.ajax.csrfToken = null;

tcg.ajax.getcustomheaders = function () {
    if (tcg.ajax.csrfToken == null) {
        tcg.ajax.csrfToken = $("input[name='__RequestVerificationToken']").val();
    }

    var h_opt = tcg.ajax.csrfToken ? { "X-XSRF-Token": tcg.ajax.csrfToken } : {};

    return h_opt
}

tcg.ajax.get = function (url, successF, errorF) {
    if (tcg.ajax.csrfToken == null) {
        tcg.ajax.csrfToken = $("input[name='__RequestVerificationToken']").val();
    }
    var h_opt = tcg.ajax.csrfToken ? { "X-XSRF-Token": tcg.ajax.csrfToken } : {};

    $.ajax({
        type: 'GET',
        url: url,
        dataType: 'json',
        contentType: "application/json",
        headers: h_opt,
        success: function (result) {
            if (typeof successF !== "undefined") {
                successF(result);
            }
        },
        error: function (result) {
            if (errorF == null || typeof errorF === "undefined") {
                tcg.widgets.Growl(result.responseText, result.statusText, 'danger', 3000);
            }
            else {
                errorF(result);
            }
        }
    });
}
tcg.ajax.post = function (url, jsonO, successF, errorF) {
    if (tcg.ajax.csrfToken == null) {
        tcg.ajax.csrfToken = $("input[name='__RequestVerificationToken']").val();
    }
    var h_opt = tcg.ajax.csrfToken ? { "X-XSRF-Token": tcg.ajax.csrfToken } : {};

    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(jsonO),
        processData: false,
        dataType: "json",
        contentType: "application/json",
        headers: h_opt,
        success: function (result) {
            if (typeof successF !== "undefined") {
                successF(result);
            }
        },
        error: function (result) {
            if (errorF == null || typeof errorF === "undefined") {
                tcg.widgets.Growl(result.responseText, result.statusText, 'danger', 3000);
            }
            else {
                errorF(result);
            }
        }
    });
}

/* EVENTS */
tcg.event.stopPropagate = function (e) {
    if (!e) { e = window.event };
    try {
        e.stopPropagation();
    }
    catch (err) {
        try {
            event.cancelBubble = true;
        }
        catch (err2) {
            e.preventDefault();
        }
    }
}

/* BUTTONS */
tcg.button.backcapture = function () {
    window.onbeforeunload = function () {
        return "Are you sure you wish to leave this page?";
    }
}
tcg.button.backclear = function () {
    window.onbeforeunload = null;
}

/* VALID8R */
tcg.valid8r.seterror = function (cont, msg) {
    cont.addClass("haserror");
    //var n = cont.next();
    //if (n.hasClass('errorIcon')) {
    //    return;
    //}
    //var html = '<img class="errorIcon" title="' + msg + '" onmouseout="tcg.valid8r.ttout(this)" onmouseover="tcg.valid8r.ttIn(this)" alt="?" src="/r/img/icons/error.png"/>';
    //cont.after(html);
}
tcg.valid8r.clearerror = function (cont) {
    cont.removeClass("haserror");
    //var n = cont.next();
    //if (n.hasClass('errorIcon')) {
    //    n.remove();
    //}
}


tcg.valid8r.IDNumber = function (val, cont) {
    //length must be 13
    if (val == null || val.length < 13 || val.length > 13) {
        tcg.valid8r.seterror(cont);
        return false;
    } else {
        //numbers only
        var reg = new RegExp(/^\d+$/);
        if (!reg.test(val)) {
            tcg.valid8r.seterror(cont);
            return false;
        } else {
            tcg.valid8r.clearerror(cont);
            return true;
        }
    }
}

tcg.valid8r.range = function (validationsArray) {
    var isValid = true;
    for (var i = 0; i < validationsArray.length; i++) {
        var item = validationsArray[i];
        if (item != true) {
            isValid = false;
        }
    }
    return isValid;
}

tcg.valid8r.req = function (val, cont, error) {
    //if it's NOT valid  
    if (val == null || val.length < 1) {
        tcg.valid8r.seterror(cont, error || 'Required!');
        return false;
    }
        //if it's valid  
    else {
        tcg.valid8r.clearerror(cont);
        return true;
    }
}
tcg.valid8r.reqamt = function (val, cont, error) {
    /* function to check if amount is not <= R0.00, just included [val <= 0] - Anele */
    //if it's NOT valid  
    if (val == null || val <= 0 || val.length < 1) {
        tcg.valid8r.seterror(cont, error || 'Required!');
        return false;
    }
        //if it's valid  
    else {
        tcg.valid8r.clearerror(cont);
        return true;
    }
}
tcg.valid8r.email = function (val, cont, error) {
    //Running the following code before any other code will create String.trim if it's not natively available.
    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }

    val = val.trim();

    //if it's Empty
    if (val == null || val.length < 1) {
        tcg.valid8r.seterror(cont, error || 'Invalid Email!');
        return false;
    }
    else {
        //if it's NOT valid 
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        if (re.test(val) == false) {
            tcg.valid8r.seterror(cont, 'Invalid Email!');
            return false;
        }
            //if it's valid  
        else {
            tcg.valid8r.clearerror(cont);
            return true;
        }
    }
}

tcg.valid8r.tel = function (val, cont, error) {
    //if it's NOT valid 
    var re = /^[0-9\-()+.\s]{0,30}$/;
    if (re.test(val) == false) {
        tcg.valid8r.seterror(cont, 'Invalid Number!');
        return false;
    }
        //if it's valid  
    else {
        tcg.valid8r.clearerror(cont);
        return true;
    }
}

/* Test - validate alpha characters – not allow - Anele */
tcg.valid8r.numbersonly = function (val, cont, error) {
    var re = /^[0-9]{0,30}$/;
    if (val != "" || val.length > 1) {
        if (re.test(val) == false) {
            tcg.valid8r.seterror(cont, error || 'Invalid Number!');
            return false;
        }
            //if it's valid  
        else {
            tcg.valid8r.clearerror(cont);
            return true;
        }
    }
    else {
        tcg.valid8r.seterror(cont, error || 'Required!');
        return false;
    }
}

tcg.valid8r.naturalnumbersonly = function (val, cont, error) {
    var re = /^[1-9][0-9]{0,30}$/;
    if (val != "" || val.length > 1) {
        if (re.test(val) == false) {
            tcg.valid8r.seterror(cont, error || 'Invalid Number!');
            return false;
        }
            //if it's valid  
        else {
            tcg.valid8r.clearerror(cont);
            return true;
        }
    }
    else {
        tcg.valid8r.seterror(cont, error || 'Required!');
        return false;
    }
}
tcg.valid8r.integer = function (val, cont, min, max) {
    //Running the following code before any other code will create String.trim if it's not natively available.
    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }

    val = val.trim();

    //if it's Empty
    if (val == null || val.length < 1) {
        tcg.valid8r.seterror(cont);
        return false;
    }
    else {
        var intVal = parseInt(val, 10);
        if (val == intVal) {

            if (min != null && intVal < min) {
                tcg.valid8r.seterror(cont);
                return false;
            }


            if (max != null && intVal > max) {
                tcg.valid8r.seterror(cont);
                return false;
            }

            tcg.valid8r.clearerror(cont);
            return true;
        }
        else {
            tcg.valid8r.seterror(cont);
            return false;
        }
    }
}

/* Freemium - Disallowed strings when sending a message to the guest - Anele */
tcg.valid8r.restrictedwords = function (val, cont, error) {
    var isValid = true;
    var disallowedwords = ['Email', 'Cell', 'Phone', 'AfricanBank', 'African', 'Bidvest', 'Capitec', 'FirstRand', 'FNB', 'FirstNational',
                           'RandMerchant', 'RMB', 'Grindrod', 'ImperialBank', 'Investec', 'Nedbank', 'Sasfin', 'TebaBank', 'Ubank', 'StandardBank',
                           'Std', 'Absa', 'baraka', 'Habib', 'Mercantile', 'SouthAfricanBankofAthens', 'SABankAthens', 'GBS', 'VBS', 'DevelopmentBank',
                           'DBSA', 'LandandAgriculturalDevelopment', 'Landbank', 'Postbank', 'E-pos', 'Epos', 'Sel', 'Selfoon', 'Selnommer', 'Telefoon',
                           'Tel', 'Mobile', 'Zelle', 'Zellenzahl', 'E-Mail Adresse', 'Telefon', 'Téléphone', 'adresse électronique', 'cellule', 'nombre de cellule',
                           'nombre mobile', 'F N B', 'First National Bank', 'Rand Merchant Bank', 'Imperial Bank', 'Imperial', 'U-bank', 'Standard Bank', 'SA Bank Athens',
                           'Development Bank', 'D B S A', 'Land and Agricultural Development', 'Land & Agricultural Development'];

    if (val != "" || val.length > 1) {
        for (var i = 0; i < disallowedwords.length; i++) {
            var word = disallowedwords[i].toLowerCase();
            if (val.toLowerCase().match('\\b' + word + '\\b') != null) {
                tcg.valid8r.seterror(cont, error || 'Sending bank details or contact information via the Booksure system is prohibited for your subscription!');
                return false;
            }
            else {
                tcg.valid8r.clearerror(cont);
                isValid = true;
            }
        }
    }
    //else {
    //    tcg.valid8r.seterror(cont, error || 'Required!');
    //    isValid = false;
    //}

    return isValid;
}

tcg.valid8r.ttIn = function (obj) {
    var that = $(obj);
    var pos = that.position();
    obj.t = obj.title;
    obj.title = "";
    that.after("<p id='valTTip' style='width: auto; height: auto;'>" + obj.t + "</p>");
    $("#valTTip").css("top", (pos.top - 10) + "px").css("left", (pos.left + 20) + "px").fadeIn("fast");
}
tcg.valid8r.ttout = function (obj) {
    var that = $(obj);
    obj.title = obj.t;
    that.next('#valTTip').remove();
}


// confirm popup widget
tcg.widgets.confirm = function (body, title, confirmYes, confirmNo) {
    var dbb = $("#PopConfirmation");
    if (dbb.length == 0) {
        //Confirmation Popup
        $('body').append('<div class="modal fade" id="PopConfirmation" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
                '<div class="modal-dialog"><div class="modal-content"><div class="modal-header">' +
                '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
                '<h4 id="PopConfirmHeader" class="modal-title">' + title + '</h4></div><div class="modal-body">' +
                '<div class="form-group"><label id="PopConfirmBody" class="control-label">' + body + '</label></div></div>' +
                '<div class="modal-footer"><input type="button" class="btn btn-success" value="Yes" id="btnConfirmYes" />' +
                '<input type="button" class="btn btn-danger" data-dismiss="modal" value="No" id="btnConfirmNo" /></div></div></div></div>');
        $('#PopConfirmation').modal();
    }
    else {
        $('#PopConfirmHeader').html(title); // add Header 
        $('#PopConfirmBody').html(body); // add text 
        dbb.modal();
    }
    $('#btnConfirmYes').unbind("click"); // unbind click event
    $('#btnConfirmYes').click(function () { // add click event
        $('#PopConfirmation').modal('hide');
        $('#btnConfirmYes').unbind("click"); // unbind click event
        confirmYes();
    });

    if (confirmNo !== undefined) {
        $('#btnConfirmNo').unbind("click"); // unbind click event
        $('#btnConfirmNo').click(function () { // add click event
            $('#PopConfirmation').modal('hide');
            $('#btnConfirmNo').unbind("click"); // unbind click event
            confirmNo();
        });
    }
    else {
        $('#btnConfirmNo').unbind("click");
    }
}

tcg.valid8r.passwordconfirmed = function (password, confirmation, confirmationCont) {
    var isValid = password === confirmation;

    if (!isValid) {
        tcg.valid8r.seterror(confirmationCont);
        return false;
    }
        //if it's valid  
    else {
        tcg.valid8r.clearerror(confirmationCont);
        return true;
    }
}

tcg.valid8r.passwordvalid = function (password, passwordO) {
    //password length > 6
    if (password.length < 6) {
        tcg.valid8r.seterror(passwordO);
        return false;
    }
    //password contains at least 1 number
    reN = /[0-9]/;
    if (!reN.test(password)) {
        tcg.valid8r.seterror(passwordO);
        return false;
    }
    //password contains at least one letter
    reL = /[a-z]/;
    if (!reL.test(password)) {
        tcg.valid8r.seterror(passwordO);
        return false;
    }
    //password contains at least one capital letter
    reC = /[A-Z]/;
    if (!reC.test(password)) {
        tcg.valid8r.seterror(passwordO);
        return false;
    }

    tcg.valid8r.clearerror(passwordO);
    return true;
}

tcg.valid8r.decimal = function (val, cont, min, max) {

    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }

    val = val.trim();

    //if it's Empty
    if (val == null || val.length < 1) {
        tcg.valid8r.seterror(cont);
        return false;
    }
    else {
        var rgx = /^[0-9]*\.?[0-9]*$/;
        var isValid = val.match(rgx);

        var valFloat = parseFloat(val);

        if (isNaN(valFloat)) {
            isValid = false;
        }

        if (!isValid) {
            tcg.valid8r.seterror(cont);
            return false;
        }

        if (val == valFloat) {

            if (min != null && valFloat < min) {
                tcg.valid8r.seterror(cont);
                return false;
            }


            if (max != null && valFloat > max) {
                tcg.valid8r.seterror(cont);
                return false;
            }

            tcg.valid8r.clearerror(cont);
            return true;
        }
        else {
            tcg.valid8r.seterror(cont);
            return false;
        }
    }

}


tcg.utils.numericals.init = function (container) {

    var decimalInputs = $('.input-decimal', container);
    var integerInputs = $('.input-integer', container);
    var moneyInputs = $('.input-money', container);
    var unfixedDecimalInputs = $('.input-udecimal', container);

    decimalInputs.on('click', function () { inputSelect(this); });
    integerInputs.on('click', function () { inputSelect(this); });
    moneyInputs.on('click', function () { inputSelect(this); });
    unfixedDecimalInputs.on('click', function () { inputSelect(this); });

    decimalInputs.on('keypress', function (e) { allowDecimalOnly(e); });
    integerInputs.on('keypress', function (e) { allowIntegerOnly(e); });
    moneyInputs.on('keypress', function (e) { allowDecimalOnly(e); });
    unfixedDecimalInputs.on('keypress', function (e) { allowDecimalOnly(e); });

    decimalInputs.on('change', function () {
        var thatO = $(this);

        if (thatO.val() == ".") {
            thatO.val(0)
        }
        if (thatO.val() != "") {
            thatO.val(parseFloat(thatO.val()).toFixed(2));
        }
        else {
            thatO.val("0.00")
        }
    });

    moneyInputs.on('change', function () {

        var thatO = $(this);
        var val = thatO.val().replace(/\s/g, '');
        if (val == ".") {
            thatO.val(0)
        }
        if (val != "") {
            thatO.val(parseFloat(val).toFixed(2));
        }
        else {
            thatO.val("0.00")
        }
    });

    unfixedDecimalInputs.on('change', function () {
        var thatO = $(this);
        var val = thatO.val().replace(/\s/g, '');
        if (val == ".") {
            thatO.val(0)
        }
        if (val != "") {
            thatO.val(parseFloat(val));
        }
        else {
            thatO.val("0.00")
        }
    });

    function allowDecimalOnly(e) {

        var keycode = e.charCode || e.keyCode;
        var key = String.fromCharCode(keycode);
        var regex = /^[0-9.\b\t]+$/;
        if (!regex.test(key)) {
            e.returnValue = false;
            e.preventDefault();
        }
    }

    function allowIntegerOnly(e) {
        var keycode = e.charCode || e.keyCode;
        var key = String.fromCharCode(keycode);
        var regex = /^[0-9\b\t]+$/;
        if (!regex.test(key)) {
            e.returnValue = false;
            e.preventDefault();
        }
    }

    function inputSelect(that) {
        that.select();
    }
}


/* BACKWARD SUPPORT */
// Date Picker
function createDatePicker(selector, options) { return tcg.widgets.date.create(selector, null, options); }
function createDatePickerWithin(selector, within, options) { return tcg.widgets.date.create(selector, within, options); }
function getDatePickerDate(selector) { return tcg.widgets.date.getdate(selector); }
function getDatePickerDateO(object) { return tcg.widgets.date.getdate(object); }
function localDateString(d) { return tcg.widgets.date.localdatestring(d); }

// Spin
function createSpinQty(selector, options) { return tcg.widgets.spin.create(selector, null, options); }
function createSpinQtyWithin(selector, within, options) { return tcg.widgets.spin.create(selector, within, options); }
function createSpinPerc(selector, options) { return tcg.widgets.spin.perc.create(selector, null, options); }
function createSpinPercWithin(selector, within, options) { return tcg.widgets.spin.perc.create(selector, within, options); }

// Spin Money
function createSpinMoney(selector, options) { return tcg.widgets.money.create(selector, null, options, false); }
function createSpinMoneyMeta(selector, options) { return tcg.widgets.money.create(selector, null, options, true); }
function createSpinMoneyWithin(selector, within, options) { return tcg.widgets.money.create(selector, within, options, false); }
function createSpinMoneyWithinMeta(selector, within, options) { return tcg.widgets.money.create(selector, within, options, true); }

// Ajax
function ajaxGet(url, successF) { tcg.ajax.get(url, successF); }
function ajaxPost(url, jsonO, successF, errorF) { tcg.ajax.post(url, jsonO, false, successF, errorF); }
function ajaxPostWithLoading(url, jsonO, successF, errorF) { tcg.ajax.post(url, jsonO, true, successF, errorF); }
function ajaxPostWithLoadingAndUrl(url, jsonO, successF) { tcg.ajax.post(url, jsonO, true, successF); }
function ajaxPostwithLoadingAndData(url, jsonO, successF) { tcg.ajax.post(url, jsonO, true, successF); }

// Loading
function showLoadingScreen(functionAfter, closeAfter) { tcg.widgets.loading.show('Processing...', functionAfter, closeAfter); }
function closeLoadingScreen() { tcg.widgets.loading.close(); }

// Events
function stopEventPropagate(e) { tcg.event.stopPropagate(e); }

// Buttons
function backButtonCapture() { tcg.button.backcapture(); }
function backButtonClear() { tcg.button.backclear(); }

// Valid8r
function valid8r_SetError(cont, msg) { tcg.valid8r.seterror(cont, msg); }
function valid8r_ClearError(cont) { tcg.valid8r.clearerror(cont); }
function valid8r_Range(validationsArray) { return tcg.valid8r.range(validationsArray); }
function valid8r_Req(val, cont) { return tcg.valid8r.req(val, cont); }
function valid8r_ReqAmount(val, cont) { return tcg.valid8r.reqamt(val, cont); }
function valid8r_Email(val, cont) { return tcg.valid8r.email(val, cont); }