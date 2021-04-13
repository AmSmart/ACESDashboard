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
            const urlParams = new URLSearchParams(window.location.search);
            urlParams.set('returnMessage', 'S Add Successful');
            window.location.search = urlParams;
        },
        error: function (response) {
            let errorMessage = "Add Failed";
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