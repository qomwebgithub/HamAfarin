AOS.init();

window.onload = function () {
    var id = localStorage.getItem("menu");
    var classes = ["search-menu", "guidcreatecartable-menu", "guidsetrequest-menu", "news-menu", "home-menu"];
    classes.forEach(function (item) {
        var Element = document.getElementById(item);
    });
    var currntelement = document.getElementById(id);
};

function setcolor(id) {
    localStorage.setItem("menu", id);
    document.getElementById("menu-" + id).classList.add("text-dark");
};

$(function () {
    $("select,input[type=text],textarea,input[type=number]").addClass("form-control");
});

new WOW().init();

$(document).ready(function () {
    $("#owl-demo").owlCarousel({
        navigation: true
    });
});

(function ($) {
    $(document).ready(function () {
        $(".main-slider").owlCarousel({
            rtl: true,
            items: 1,
            dots: true,
            loop: true,
            autoplay: true,
            autoplayTimeout: 3000,
            autoplayHoverPause: true,
            smartSpeed: 800,
            autoHeight: true,
            nav: true,
        });

        $(".main-topside > .owl-carousel").owlCarousel({
            rtl: true,
            items: 1,
            dots: false,
            loop: true,
            autoplay: true,
            mouseDrag: false,
            autoplayTimeout: 4000,
            smartSpeed: 1000,
        });

        $(".header__bars").on("click", function (e) {
            e.preventDefault();
            $(".sidemenu").addClass("is-active");
            $(".overlay").addClass("is-active");
            $("body").addClass("overflow-hidden");
            $(".header__logo").addClass("is-hidden");
        });

        $(".overlay").on("click", function () {
            if ($(this).hasClass("is-active")) {
                $(".sidemenu").removeClass("is-active");
                $(this).removeClass("is-active");
                $("body").removeClass("overflow-hidden");
                $(".header__logo").removeClass("is-hidden");
            }
        });

        $(".sidemenu__menu > li > a").on("click", function (e) {
            if ($(this).parent("li").hasClass("has-sub")) {
                e.preventDefault();
            }
            if ($(this).next("ul").css("display") === "block") {
                $(".sidemenu__menu > li > ul").slideUp(300);
                $(this).next("ul").slideDown(300);
            } else {
                $(this).next("ul").slideUp(300);
            }
        });

        $('.owl-3-item').owlCarousel({
            loop: true,
            margin: 10,
            nav: true,
            rtl: true,
            items: 3,
            dots: false,
            navText: ["<i class='glyphicon glyphicon-chevron-left'></i>", "<i class='glyphicon glyphicon-chevron-right'></i>"],
            responsive: {
                0: { items: 1, },
                600: { items: 2, },
                1000: { items: 3, }
            }
        });

        $('.owl-carousel').owlCarousel({
            loop: true,
            margin: 10,
            nav: true,
            rtl: true,
            items: 5,
            responsive: {
                100: { items: 1, },
                600: { items: 2, },
                1000: { items: 4, }
            }
        });
    });
}(jQuery));