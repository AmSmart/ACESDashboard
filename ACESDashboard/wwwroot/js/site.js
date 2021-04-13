// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function (e) {
    const urlParams = new URLSearchParams(window.location.search);
    const returnMessage = urlParams.get('returnMessage');
    if (returnMessage !== null) {
        if (returnMessage[0] === 'S') {
            Toastify({
                text: returnMessage.substring(2),
                style: { background: "linear-gradient(to right, #9ee741, #10bd41)" },
                position: 'center',
                duration: 3000
            }).showToast();
        }
        else if (returnMessage[0] === 'F') {
            Toastify({
                text: returnMessage.substring(2),
                style: { background: "linear-gradient(to right, #e74168, #bd1010)" },
                position: 'center',
                duration: 3000
            }).showToast();
        }
    }
});