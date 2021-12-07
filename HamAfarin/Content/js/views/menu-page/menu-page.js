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

function ShowWarranty() {
    $("#WarranyModal").modal();
}

/* function CreateComment(id) {
    $.ajax({
        url: "/Comment/AddComment/" + id + "?BusinessPlanId=" +@Model.BussinessPlanID,
    type: "Get"
}).done(function (result) {
    $("#myModalLabel").html("ثبت نظر شما");
    $("#myModalBody").html(result);
    $("#addCommentModal").modal();

    $(".modal-backdrop").each(function () {
        $(this).removeClass("modal-backdrop");
    });
});
        }
function CreateQuestion(id) {
    $.ajax({
        url: "/Question/AddQuestion/" + id + "?BusinessPlanId=" +@Model.BussinessPlanID,
    type: "Get"
}).done(function (result) {
    $("#addQuestionLabel").html("ثبت سوال شما");
    $("#addQuestionBody").html(result);
    $("#addQuestionModal").modal();

    $(".modal-backdrop").each(function () {


        $(this).removeClass("modal-backdrop");
    });

});
        }*/

function Success() {
    $("#addCommentModal").modal('hide');
    $("#addQuestionModal").modal('hide');
}

function showAlert(message) {
    alert(message);
}