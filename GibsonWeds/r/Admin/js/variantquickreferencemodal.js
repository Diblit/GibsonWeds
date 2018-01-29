//Developer: Clement
//Summary: This is the javascript file that handles the Variant Quick reference popup
//Note please make sure all variables in this file are unique in all pages because the script is referenced globally
$(document).ready(function () {
    InitVQRSelect()
});
function SearchQuickReference() {
    var tbVariantQuickReferenceO = $('#tbVariantQuickReference');
    var tbVariantQuickReference = tbVariantQuickReferenceO.val();
    window.location = base_url + "/variant-quick-reference/" + tbVariantQuickReference;
}

function InitVQRSelect() {
    
    $("#selVariantQuickReference").select2({
        placeholder: "Search By Typing",
        minimumInputLength: 1,
        width: "100%",
        allowClear: true,
        ajax: {
            url: base_url + "/api/variantquickreference/getvariantsforsearch",
            dataType: "json",
            quietMillis: 250,
            data: function (params) {

                return {
                    q: params.term
                };
            },
            processResults: function (data, page) {
                var result = [];
                $.each(data.List, function (index, item) {
                    result.push({
                        id: item.value,
                        text: item.text,
                        extra: item.ExtraID
                    });
                });
                return {
                    results: result
                };
            }
        }
    }).on("select2:selecting", function (e) {
        var selected = e.params.args.data;
        var variantID = selected.id;
        var modelID = selected.extra;
        window.location = base_url + "/variant-quick-reference/model/" + modelID + "/variant/" + variantID;
    });
}