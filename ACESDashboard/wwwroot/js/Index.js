$('#btnAddWkspc').on('click', e => {
    $('#addWkspcModal').modal('show');
});

$('#confirmWkspcAdd').on('click', function (e) {
    $.ajax({
        url: '/home/createworkspace',
        type: 'POST',
        data: {
            name: $('#newWorkspaceName').val(),
            tag: $('#newWorkspaceTag').val(),
        },
        success: function () {
            location.reload();
            alert('Add Successful!');
        },
        error: function () {
            alert('Add failed');
        }
    });
});