// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showNotification(title, message, iconUrl) {
    $.notify({
        icon: iconUrl,
        title: title,
        message: message
    }, {
            allow_dismiss: true,
            type: 'minimalist',
            delay: 4000,
            mouse_over: "pause",
            icon_type: 'image',
            newest_on_top: true,
            animate: {
                enter: 'animated bounceInDown',
                exit: 'animated fadeOutRight'
            },
            template: '<div data-notify="container" class="col-6 col-md-5 col-lg-4 col-xl-3 alert alert-{0}" role="alert">' +
                '<button type="button" aria-hidden="true" class="close" data-notify="dismiss">×</button>' +
                '<img data-notify="icon" class="img-circle pull-left">' +
                '<span data-notify="title">{1}</span>' +
                '<span data-notify="message">{2}</span>' +
                '</div>'
        });
}

function saveFormData(isNew, controllerName, iconUrl) {
    if (!validateForm("form-edit")) {
        return;
    }

    $("#loading").show();

    var url = isNew ? "/" + controllerName + "/SaveNew/" : "/" + controllerName + "/SaveEdited/";
    var successMessage = isNew ? "Se agrego el registro satisfactoriamente." : "Se actualizo el registro satisfactoriamente.";

    $.ajax({
        url: url,
        type: "POST",
        data: $('#form-edit').serialize(),
        dataType: 'html',
        headers: { RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val() },
        success: function (response) {
            showNotification("Exito.", successMessage, iconUrl);
            $("#tableData").html(response);
            hideModificationModal();
        },
        error: function (response) {
            showNotification("Error.", response.responseText, iconUrl);
        },
        complete: function () {
            $('#loading').hide();
        }
    });
}

function uploadFile(controllerName, profileImageUrl, outputId) {
    $("#loading").show();

    var formData = new FormData();
    formData.append('file', $('#File')[0].files[0]);

    $.ajax({
        url: "/" + controllerName + "/UploadFile/",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            $("#" + outputId).val(response);
            $("#player").attr("src", response);
            showNotification("¡Listo!", "Se ha subido el archivo al servidor.", profileImageUrl);
        },
        error: function (response) {
            showNotification("Error.", response.responseText);
        },
        complete: function () {
            $('#loading').hide();
        }
    });
}

function validateForm(formId) {
    var form = document.getElementById(formId);
    $(form).validate("submit");
    if (!$(form).valid()) {
        return false;
    }
    else {
        return true;
    }
}

function showConfirmationDialog(title, content, confirmedAction, firstParameter = null, secondParameter = null) {
    $.confirm({
        title: title,
        content: content,
        theme: 'modern',
        icon: 'fa fa-warning',
        draggable: false,
        autoClose: 'cancel|10000',
        backgroundDismiss: true,
        buttons: {
            ok: {
                text: "ok!",
                btnClass: 'btn-primary',
                keys: ['enter'],
                action: function () {
                    if (firstParameter === null && secondParameter === null) {
                        confirmedAction();
                    }
                    else if (firstParameter !== null && secondParameter === null) {
                        confirmedAction(firstParameter);
                    }
                    else if (firstParameter === null && secondParameter !== null) {
                        confirmedAction(null, secondParameter);
                    }
                    else if (firstParameter !== null && secondParameter !== null) {
                        confirmedAction(firstParameter, secondParameter);
                    }
                }
            },
            cancel: {
                text: "cancelar",
                keys: ['esc'],
                action: function () {

                }
            }
        }
    });
}

function deleteRecordConfirmed(controllerName, id) {
    $("#loading").show();
    $.ajax({
        url: "/" + controllerName + "/Delete?id=" + id,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            showNotification("Exito.", "Se elimino el registro satisfactoriamente.", '');
            $("#tableData").html(response);
        },
        error: function (response) {
            showNotification("Error.", response.responseText, '');
        },
        complete: function () {
            $('#loading').hide();
        }
    });
}

function postFormData(url, formId, successMessage, successCallback) {
    if (!validateForm(formId)) {
        return;
    }

    var form = document.getElementById(formId);

    $("#loading").show();

    $.ajax({
        url: url,
        type: "POST",
        data: $(form).serialize(),
        dataType: 'html',
        headers: { RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val() },
        success: function (response) {
            if (successMessage !== null) {
                showNotification("Exito.", successMessage, '');
            }
            successCallback(response);
        },
        error: function (response) {
            showNotification("Error.", response.responseText, '');
        },
        complete: function () {
            $('#loading').hide();
        }
    });
}

function getData(url, successMessage, successCallback) {
    $("#loading").show();
    $.ajax({
        url: url,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (successMessage !== null) {
                showNotification("Exito.", successMessage, '');
            }
            successCallback(data);
        },
        error: function (response) {
            showNotification("Error.", response.responseText, '');
        },
        complete: function () {
            $('#loading').hide();
        }
    });
}

function postData(url, successMessage, successCallback, firstParameter = null, secondParameter = null) {
    $("#loading").show();

    $.ajax({
        url: url,
        type: "POST",
        dataType: 'html',
        headers: { RequestVerificationToken: $('input:hidden[name="__RequestVerificationToken"]').val() },
        success: function () {
            if (successMessage !== null) {
                showNotification("Exito.", successMessage, '');
            }

            if (firstParameter === null && secondParameter === null) {
                successCallback();
            }
            else if (firstParameter !== null && secondParameter === null) {
                successCallback(firstParameter);
            }
            else if (firstParameter === null && secondParameter !== null) {
                successCallback(null, secondParameter);
            }
            else if (firstParameter !== null && secondParameter !== null) {
                successCallback(firstParameter, secondParameter);
            }
        },
        error: function (response) {
            showNotification("Error.", response.responseText, '');
        },
        complete: function () {
            $('#loading').hide();
        }
    });
}

function toggleModificacionModal() {
    $("#modificationModal").toggle();
}

function onLoadModificationModalSuccess(response) {
    $("#modificationModal").html(response);
    modificationModal.style.display = "block";
}

function onLoadPageSuccess(response) {
    $("#tableData").html(response);
}