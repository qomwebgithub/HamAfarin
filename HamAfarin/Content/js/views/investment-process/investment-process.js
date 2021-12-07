function GoToElement(elementId) {

    //document.getElementById("header" + elementId).scrollIntoView();

    $(document).ready(function () {

        $('html, body').animate({
            scrollTop: $("#header" + elementId).offset().top
        }, 2000);
    });
    //location.href("#"+elementId);
}