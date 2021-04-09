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
            location.reload();
            alert('Delete Successful!');
        },
        error: function () {
            alert('Delete failed');
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
            location.reload();
            alert('Delete Successful!');
        },
        error: function () {
            alert('Delete failed');
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
            location.reload();
            alert('Delete Successful!');
        },
        error: function () {
            alert('Delete failed');
        }
    });
});

$('.up-edit').on('click', function (e) {
    $('#editUpModal').modal('show');

    let updateText = $(this).data('itemText');
    let itemId = $(this).data('itemId');
    let itemExpiryTime = $(this).data('itemExpiryTime');
    let itemPostedTime = $(this).data('itemPostedTime');

    itemExpiryTime = isoToJS(itemExpiryTime);
    itemPostedTime = isoToJS(itemPostedTime);

    $('#updateText').val(updateText);
    $('#confirmUpEdit').data('itemId', itemId);
    $('#expiryTime').val(itemExpiryTime);
    $('#postedTime').val(itemPostedTime);
});

$('#confirmUpEdit').on('click', function (e) {
    $.ajax({
        url: '/home/editupdate',
        type: 'POST',
        data: {
            id: $(this).data('itemId'),
            newUpdateText: $('#updateText').val(),
            expiryTime: $('#expiryTime').val(),
            postedAt: $('#postedTime').val()
        },
        success: function () {
            location.reload();
            alert('Edit Successful!');
        },
        error: function () {
            alert('Edit failed');
        }
    });
});

function isoToJS(datetime) {
    var datetimeObj = new Date(datetime);
    let jsTime = datetimeObj.toISOString();
    jsTime = jsTime.substring(0, jsTime.length - 1);
    return jsTime;
}