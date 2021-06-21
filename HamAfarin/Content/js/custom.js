$('#owl-future').owlCarousel({
    autoplay: true,
    margin: 10,
    rtl: true,
    loop: true,
    responsiveClass: true,
    responsive: {
        0: {
            items: 1,
            nav: true
        },
        600: {
            items: 1,
            nav: false
        },
        1000: {
            items: 3,
            nav: true,
            loop: false
        }
    }
})

$('#owl-news').owlCarousel({
    autoplay: true,
    margin: 10,
    rtl: true,
    responsiveClass: true,
    responsive: {
        0: {
            items: 1,
            nav: true
        },
        600: {
            items: 2,
            nav: false
        },
        1000: {
            items: 4,
            nav: true,
            loop: false
        }
    }
})

$('#owl-logoCompany').owlCarousel({
    loop: true,
    margin: 10,
    padding: 10,
    nav: true,
    rtl: true,
    autoplay: true,
    items: 4,
    responsive: {
        0: {
            items: 1,
            margin: 0,
            padding: 0,
        },
        600: {
            items: 2,
        },
        1000: {
            items: 4,
        }
    }
})

$('#chart-carousel').owlCarousel({
    autoplay: true,
    margin: 10,
    rtl: true,
    responsiveClass: true,
    responsive: {
        0: {
            items: 1,
            nav: true
        },
        600: {
            items: 1,
            nav: false
        },
        1000: {
            items: 1,
            nav: true,
            loop: false
        }
    }
})


//////$('.loginIcon').click(function () {
//////    //$(':not(.loginIcon)').css('background-color','red')
//////    $('.login').toggle("blind");
//////})
//////var ing=false;
//////$('.loginIcon').mouseenter(function () {
//////    ing = true;
//////})
//////$('.loginIcon').mouseleave(function () {
//////    ing = false;
//////})

//$('.loginIcon').mouseleave(function () {
//    if ($(window).outerWidth() >= 560) {
//        $('.login').hide("blind")
//    }
//})

//////$("*").click(function (e) {
//////    if (!ing) {
//////        $('.login').hide("blind")
//////    }
//////})

//alert("wth");
//var wth = $(window).width()*0.8 + "px";

//alert(wth);
//$('#owl-logoCompany img').css("width",wth);
//$('#owl-logoCompany img').css("height",wth);
//alert(wth);

$(".owl-next").html('<img src="/Content/img/Previous.svg" class="w-25">');

$(".owl-prev").switchClass("owl-prev", "carousel-control-prev");

$(".owl-prev").html('<img src="/Content/img/next.svg" class="w-25">');

$(".owl-next").switchClass("owl-next", "carousel-control-next");

$('#owl-logoCompany .owl-dots').addClass("d-none");
$("#owl-future button img , #owl-news button img").switchClass('w-25', 'w-c');
//$("img.w-25").parent().outerWidth('20px')
$('li:not(.not-li)').click(function () {
    var This = this;
    $(This).siblings().children().removeClass('active');
    $(This).parent().siblings().children('li').children().removeClass('active');
    $(This).parent().siblings('li').children().removeClass('active');
    $(This).siblings().children('li').children().removeClass('active');
})

//$('#filter-box').hide();

$('.close,#F-btn').click(function () {
    $('#filter-box').hide("fade");
})

$('#filter-btn').click(function () {
    $('#filter-box').show("fade");
})
$(".pagination li:not(.active)").mouseenter(function () {
    $(this).addClass("hover-page");
    $(this).children("a").addClass("hover-page");
})
$(".pagination li:not(.active)").mouseleave(function () {
    $(this).removeClass("hover-page");
    $(this).children("a").removeClass("hover-page");

})

$(".PagedList-skipToNext").children("a").text("بعدی");
$(".PagedList-skipToPrevious").children("a").text("قبلی");
$(".PagedList-skipToLast").children("a").text("آخرین برگه");
$(".PagedList-skipToFirst").children("a").text("اولین برگه");
var url = window.location.hash;
//alert(url);
if (url != null && url != "") {
    // alert(url);
    $('#faq').addClass('active');
    $('#faq').addClass('show');
    $('#faq').siblings().removeClass('active');
    $('#faq').siblings().removeClass('show');
    $('#financial-tab').removeClass('active');
}
