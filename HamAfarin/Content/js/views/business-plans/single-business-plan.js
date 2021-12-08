$(document).ready(function () {
    $(window).bind('scroll', function () {
        var vPos = $(window).scrollTop();
        var totalH = $('.stickyNav_container').offset().top;
        var finalSize = totalH - vPos;
        var btnSarmayeGozari = $("#btnSarmayeGozari").html();

        if (finalSize <= 0) {
            $('.stickyNav').css({
                'position': 'fixed',
                'top': 0
            });
            $('.stickyNav').addClass('w-100');
            $("#btnSarmayeGozariInMenu").html(btnSarmayeGozari);
        } else {
            $('.stickyNav').css({
                'position': 'static'
            })
            $('.stickyNav').removeClass('w-100');
            $("#btnSarmayeGozariInMenu").html("");
        }
    });
});

function CopyToClipboard() {
    var textToCopy = window.location.href;
    // navigator clipboard api needs a secure context (https)
    if (navigator.clipboard && window.isSecureContext) {
        // navigator clipboard api method'
        alert("آدرس صفحه در کلیپ بورد شما کپی شد: " + textToCopy);
        return navigator.clipboard.writeText(textToCopy);
    } else {
        // text area method
        let textArea = document.createElement("textarea");
        textArea.value = textToCopy;
        // make the textarea out of viewport
        textArea.style.position = "fixed";
        textArea.style.left = "-999999px";
        textArea.style.top = "-999999px";
        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();
        alert("آدرس صفحه در کلیپ بورد شما کپی شد: " + textToCopy);
        return new Promise((res, rej) => {
            // here the magic happens
            document.execCommand('copy') ? res() : rej();
            textArea.remove();
        });
    }
}

function AlertInvestment(message, redirect) {
    Swal.fire({
        icon: 'warning',
        title: 'توجه',
        text: message,
        confirmButtonText: `قبول`,
        confirmButtonColor: '#d0aa45',
    }).then((result) => {
        if (result.isConfirmed && redirect) {
            window.location.href = "/BusinessPlans";
        }
    });
    //if (confirm(message))
    //        window.location.href = "/BusinessPlans";
}

function ShowWarranty() {
    $("#WarranyModal").modal();
}


function Success() {
    $("#addCommentModal").modal('hide');
    $("#addQuestionModal").modal('hide');
}

function showAlert(message) {
    alert(message);
}

function GoToElement(elementId) {
    $("#" + elementId).addClass("show mt-5");
    document.getElementById("header" + elementId).scrollIntoView();
    //location.href("#"+elementId);
}