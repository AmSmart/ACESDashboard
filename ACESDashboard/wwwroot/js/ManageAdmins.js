$('#btnCreateAdmin').on('click', function(e) {
    $('#createAdminModal').modal('show');
});

$('#confirmCreateAdmin').on('click', function (e) {
    let password = $('#password').val();
    let confirmPassword = $('#confirmPassword').val();

    if (password === confirmPassword) {
        $.ajax({
            url: '/home/createadmin',
            type: 'POST',
            data: {
                email: $('#email').val(),
                password: password,
                confirmPassword: confirmPassword
            },
            success: function () {
                const urlParams = new URLSearchParams(window.location.search);
                urlParams.set('returnMessage', 'S Creation Successful');
                window.location.search = urlParams;
            },
            error: function (response) {
                let errorMessage = "Creation Failed";
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
    }
    else {
        Toastify({
            text: "The passwords do not match",
            style: { background: "linear-gradient(to right, #e74168, #bd1010)" },
            position: 'center',
            duration: 3000
        }).showToast();
    }    
});

$('.admin-delete').on('click', function (e) {
    $('#deleteAdminModal').modal('show');

    let adminEmail = $(this).data('itemEmail');
    let itemId = $(this).data('itemId');

    $('#adminDeleteQuote').text(adminEmail);
    $('#confirmAdminDelete').data('itemId', itemId);
});

$('#confirmAdminDelete').on('click', function (e) {
    $.ajax({
        url: '/home/deleteadmin',
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

$('.admin-add').on('click', function(e) {
    $('#addAdminWkspcModal').modal('show');

    let adminEmail = $(this).data('itemEmail');
    let nonBelongedWorkspacesData = $(this).data('itemNonBelonged');
    nonBelongedWorkspacesData = nonBelongedWorkspacesData.split(',');
    let $dropdown = $("#nonBelongedWorkspaces");
    let itemId = $(this).data('itemId');

    $('#addAdminWkspcQuote').text(adminEmail);
    $('#confirmAddAdminWkspc').data('itemId', itemId);

    $.each(nonBelongedWorkspacesData, function (e) {
        $dropdown.append($("<option />").val(this).text(this));
    });

});

$('#confirmAddAdminWkspc').on('click', function (e) {
    $.ajax({
        url: '/home/addadmintoworkspace',
        type: 'POST',
        data: {
            userId: $(this).data('itemId'),
            workspaceName: $('#nonBelongedWorkspaces').val()
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

$('.admin-remove').on('click', function (e) {
    $('#remAdminWkspcModal').modal('show');

    let adminEmail = $(this).data('itemEmail');
    let belongedWorkspacesData = $(this).data('itemBelonged');
    belongedWorkspacesData = belongedWorkspacesData.split(',');
    let $dropdown = $("#belongedWorkspaces");
    let itemId = $(this).data('itemId');

    $('#remAdminWkspcQuote').text(adminEmail);
    $('#confirmRemAdminWkspc').data('itemId', itemId);

    $.each(belongedWorkspacesData, function (e) {
        $dropdown.append($("<option />").val(this).text(this));
    });

});

$('#confirmRemAdminWkspc').on('click', function (e) {
    $.ajax({
        url: '/home/removeadminfromworkspace',
        type: 'POST',
        data: {
            userId: $(this).data('itemId'),
            workspaceName: $('#belongedWorkspaces').val()
        },
        success: function () {
            const urlParams = new URLSearchParams(window.location.search);
            urlParams.set('returnMessage', 'S Removal Successful');
            window.location.search = urlParams;
        },
        error: function (response) {
            let errorMessage = "Removal Failed";
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