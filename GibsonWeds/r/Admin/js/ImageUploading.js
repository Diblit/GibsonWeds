function initFineUploader(control, btn, endpoint, customHeaders, image_size_bytes, crop_complete_callback) {
    var uploader = $(control).fineUploader({
        button: $(btn),
        uploaderType: 'basic',
        request: {
            endpoint: endpoint,
            forceMultipart: false,        // validation token
            customHeaders: customHeaders
        },
        multiple: false,
        validation: {
            allowedExtensions: ['jpg', 'jpeg', 'tiff', 'tif', 'png', 'pdf'],
            sizeLimit: image_size_bytes
        },
        callbacks: {
            onValidate: function (data, buttonContainer) {
            },
            onSubmit: function (id, name) {
            },
            onComplete: function (id, name, responseJSON, xhr) {

                // do cropping
                if (responseJSON.success) {
                    //construct json
                    var data = {
                        guid_key: responseJSON.guid_key,
                        FileName: responseJSON.fileName
                    };

                    crop_complete_callback(data);
                }
                else {
                    //tcg.widgets.loading.close(); // hide Popup loading
                }
            },
            onError: function (id, name, errorReason, xhr) {
                tcg.widgets.Growl(errorReason, "Error", "danger", 3500);
            }
        }
    });
}

function initFineUploader_WithCrop(control, btn, endpoint, customHeaders, image_size_bytes, minWidth, minHeight, crop_complete_callback) {

    var uploader = $(control).fineUploader({
        button: $(btn),
        uploaderType: 'basic',
        request: {
            endpoint: endpoint,
            forceMultipart: false,        // validation token
            customHeaders: customHeaders
        },
        multiple: false,
        validation: {
            allowedExtensions: ['jpeg', 'jpg', 'png', 'bmp', 'pdf'],
            sizeLimit: image_size_bytes,
            image: { minWidth: minWidth, minHeight: minHeight },
        },
        callbacks: {
            onValidate: function (data, buttonContainer) {

            },
            onSubmit: function (id, name) {
                tcg.widgets.loading.show("Processing...");
            },
            onComplete: function (id, name, responseJSON, xhr) {
                tcg.widgets.loading.close(); // hide Popup loading
                // do cropping
                if (responseJSON.success) {

                    ShowCropper(responseJSON, minWidth, minHeight, 1 / 1, crop_complete_callback);
                }
                else {
                    //tcg.widgets.loading.close(); // hide Popup loading
                }
            },
            onError: function (id, name, errorReason, xhr) {
                tcg.widgets.loading.close(); // hide Popup loading
                tcg.widgets.Growl(errorReason, "Error", "danger", 3500);
            }
        }
    });
}

//image cropper
function ShowCropper(image_data, minWidth, minHeight, aspectRatio, crop_complete_callback) {

    var img_url = '/api/uploadimage/imageview?guid_key=' + image_data.guid_key;

    // GlobaltempImageID = newTempImgRes.tempImageID;
    $('#dvCover').empty(); // empty div to clear jCrop

    $('#dvCover').html('  <img id="cropper_image"  style="width:100%;" />'); // add new Image
    $('#cropper_image').prop('src', img_url); // add image source
    $('#PopImageCropping').modal(); // open cropping image

    var actualWidth = image_data.dimensions.width; // true length width before cropping
    var actualHeight = image_data.dimensions.height; // true length height before cropping

    var padLeft = parseInt((actualWidth - 250) / 2);
    var padTop = parseInt((actualHeight - 250) / 2);

    $('#cropper_image').Jcrop({
        onChange: showCoords,
        onSelect: showCoords,
        boxWidth: 700,
        boxHeight: 'auto',
        minSize: [minWidth, minHeight],
        //maxSize: [250, 250],
        trueSize: [actualWidth, actualHeight],
        setSelect: [padLeft, padTop, 250 + padLeft, 250 + padTop],
        aspectRatio: 25 / 25
    });

    $('#btnCropImage').unbind("click"); // unbind click event
    $('#btnCropImage').click(function () { //add click event

        // dimensions from crop
        var x1Num = parseFloat(CovX1);
        var y1Num = parseFloat(CovY1);
        var x2Num = parseFloat(CovX2);
        var y2Num = parseFloat(CovY2);

        //construct json
        var data = {
            guid_key: image_data.guid_key,
            FileName: image_data.fileName,
            x1: x1Num.toFixed(0),
            y1: y1Num.toFixed(0),
            x2: x2Num.toFixed(0),
            y2: y2Num.toFixed(0),
        };

        $('#PopImageCropping').modal('hide'); // hide model

        crop_complete_callback(data);
    });
}

// on cropping change fires this method to get dimensions
function showCoords(c) {
    CovX1 = c.x;
    CovY1 = c.y;
    CovX2 = c.x2;
    CovY2 = c.y2;
};