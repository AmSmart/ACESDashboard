$('#btnAddDoc').on('click', e => {
    $('#addDocModal').modal('show');
});

$('#btnAddUp').on('click', e => {
    $('#addUpModal').modal('show');
});

$('#confirmUpAdd').on('click', function (e) {
    $.ajax({
        url: '/home/addupdate',
        type: 'POST',
        data: {
            workspaceId: $('#workspaceId').val(),
            updateText: $('#updateText').val(),
            expiryTime: new Date($('#expiryTime').val()).toISOString()
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

$('#btnAddSec').on('click', e => {
    $('#addSecModal').modal('show');
});

$('#confirmSecAdd').on('click', function (e) {    
    $.ajax({
        url: '/home/addsection',
        type: 'POST',
        data: {
            workspaceId: $('#workspaceId').val(),
            sectionName: $('#newSectionName').val()        
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
                style: { background: "linear-gradient(to right, #e74168, #bd1010)"},
                position: 'center',
                duration: 3000
            }).showToast();
        }
    });
});