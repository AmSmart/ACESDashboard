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
            location.reload();
            alert('Add Successful!');
        },
        error: function () {
            alert('Add failed');
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
            location.reload();
            alert('Add Successful!');
        },
        error: function () {
            alert('Add failed');
        }
    });
});