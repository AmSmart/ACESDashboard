$('.up-delete').on('click', function (e) {
    $('#deleteUpModal').modal('show');

    let updateText = $(this).data('itemText');
    let itemId = $(this).data('itemId');

    $('#upDeleteQuote').text(updateText);
    $('#confirmUpDelete').data('itemId', itemId);
});

$('#confirmUpDelete').on('click', function (e) {
    $.ajax({
        url: '/home/deleteupdate',
        type: 'POST',
        data: {
            id: $(this).data('itemId')
        },
        success: function () {
            const urlParams = new URLSearchParams(window.location.search);
            urlParams.set('returnMessage', 'S Delete Successful');
            window.location.search = urlParams;
        },
        error: function (response) {
            let errorMessage = "Delete Failed";
            if (response.responseText !== null && response.responseText !== "") {
                errorMessage = response.responseText;
            }
            Toastify({
                text: errorMessage,
                style: { background: "linear-gradient(to right, #e74168, #bd1010)" },
                position: 'center',
                duration: 3000
            }).showToast();
        }
    });
});

$('.doc-delete').on('click', function (e) {
    $('#deleteDocModal').modal('show');

    let documentName = $(this).data('itemName');
    let itemId = $(this).data('itemId');

    $('#docDeleteQuote').text(documentName);
    $('#confirmDocDelete').data('itemId', itemId);
});

$('#confirmDocDelete').on('click', function (e) {
    $.ajax({
        url: '/home/deletedocument',
        type: 'POST',
        data: {
            id: $(this).data('itemId')
        },
        success: function () {
            const urlParams = new URLSearchParams(window.location.search);
            urlParams.set('returnMessage', 'S Delete Successful');
            window.location.search = urlParams;
        },
        error: function (response) {
            let errorMessage = "Delete Failed";
            if (response.responseText !== null && response.responseText !== "") {
                errorMessage = response.responseText;
            }
            Toastify({
                text: errorMessage,
                style: { background: "linear-gradient(to right, #e74168, #bd1010)" },
                position: 'center',
                duration: 3000
            }).showToast();
        }
    });
});

$('.sec-delete').on('click', function (e) {
    $('#deleteSecModal').modal('show');

    let sectionName = $(this).data('itemName');
    let itemId = $(this).data('itemId');

    $('#secDeleteQuote').text(sectionName);
    $('#confirmSecDelete').data('itemId', itemId);
});

$('#confirmSecDelete').on('click', function (e) {
    $.ajax({
        url: '/home/deletesection',
        type: 'POST',
        data: {
            id: $(this).data('itemId')
        },
        success: function () {
            const urlParams = new URLSearchParams(window.location.search);
            urlParams.set('returnMessage', 'S Delete Successful');
            window.location.search = urlParams;
        },
        error: function (response) {
            let errorMessage = "Delete Failed";
            if (response.responseText !== null && response.responseText !== "") {
                errorMessage = response.responseText;
            }
            Toastify({
                text: errorMessage,
                style: { background: "linear-gradient(to right, #e74168, #bd1010)" },
                position: 'center',
                duration: 3000
            }).showToast();
        }
    });
});

$('.up-edit').on('click', function (e) {
    $('#editUpModal').modal('show');

    let updateText = $(this).data('itemText');
    let itemId = $(this).data('itemId');
    let itemExpiryTime = $(this).data('itemExpiryTime');

    itemExpiryTime = isoToJS(itemExpiryTime);

    $('#updateText').val(updateText);
    $('#confirmUpEdit').data('itemId', itemId);
    $('#expiryTime').val(itemExpiryTime);
});

$('#confirmUpEdit').on('click', function (e) {
    $.ajax({
        url: '/home/editupdate',
        type: 'POST',
        data: {
            id: $(this).data('itemId'),
            newUpdateText: $('#updateText').val(),
            expiryTime: $('#expiryTime').val()
        },
        success: function () {            
            const urlParams = new URLSearchParams(window.location.search);
            urlParams.set('returnMessage', 'S Edit Successful');
            window.location.search = urlParams;
        },
        error: function (response) {
            let errorMessage = "Edit Failed";
            console.log(response);
            if (response.responseText !== null && response.responseText !== "") {
                errorMessage = response.responseText;
            }
            Toastify({
                text: errorMessage,
                style: { background: "linear-gradient(to right, #e74168, #bd1010)" },
                position: 'center',
                duration: 3000
            }).showToast();
        }
    });
});

$('.sec-edit').on('click', function (e) {
    $('#editSecModal').modal('show');

    let sectionName = $(this).data('itemName');
    let itemId = $(this).data('itemId');
    
    $('#sectionName').val(sectionName);
    $('#confirmSecEdit').data('itemId', itemId);
});

$('#confirmSecEdit').on('click', function (e) {
    $.ajax({
        url: '/home/editsection',
        type: 'POST',
        data: {
            id: $(this).data('itemId'),
            name: $('#sectionName').val()
        },
        success: function () {            
            const urlParams = new URLSearchParams(window.location.search);
            urlParams.set('returnMessage', 'S Edit Successful');
            window.location.search = urlParams;
        },
        error: function (response) {
            let errorMessage = "Edit Failed";
            if (response.responseText !== null && response.responseText !== "") {
                errorMessage = response.responseText;
            }
            Toastify({
                text: errorMessage,
                style: { background: "linear-gradient(to right, #e74168, #bd1010)" },
                position: 'center',
                duration: 3000
            }).showToast();
        }
    });
});

$('#editMetadata').on('click', function (e) {
    $('#editMetadataModal').modal('show');

    let workspaceName = $(this).data('itemName');
    let workspaceTag = $(this).data('itemTag');
    let itemId = $(this).data('itemId');

    $('#workspaceName').val(workspaceName);
    $('#workspaceTag').val(workspaceTag);
    $('#confirmMetadataEdit').data('itemId', itemId);
});

$('#confirmMetadataEdit').on('click', function (e) {
    $.ajax({
        url: '/home/editworkspacenameandtag',
        type: 'POST',
        data: {
            id: $(this).data('itemId'),
            name: $('#workspaceName').val(),
            tag: $('#workspaceTag').val()
        },
        success: function () {
            const urlParams = new URLSearchParams(window.location.search);
            urlParams.set('returnMessage', 'S Edit Successful');
            window.location.search = urlParams;
        },
        error: function (response) {
            let errorMessage = "Edit Failed";
            if (response.responseText !== null && response.responseText !== "") {
                errorMessage = response.responseText;
            }
            Toastify({
                text: errorMessage,
                style: { background: "linear-gradient(to right, #e74168, #bd1010)" },
                position: 'center',
                duration: 3000
            }).showToast();
        }
    });
});

function isoToJS(datetime) {
    var datetimeObj = new Date(datetime);
    let jsTime = datetimeObj.toISOString();
    jsTime = jsTime.substring(0, jsTime.length - 1);
    return jsTime;
}