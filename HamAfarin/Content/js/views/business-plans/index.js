var pageIndex = 1;
var pageSize = 6;
var totalPageCount = 1;
var searchTextInput = "";

if ($(window).data('ajaxready') == true) {
    appendData();
}

//.data('ajaxready', true) این مقدار برای جلوگیری ارسال درخواست تکراری استفاده شده است
$(window).data('ajaxready', true).scroll(function () {

    if ($(window).data('ajaxready') == false) return;

    var footer = $('#risk');

    if (totalPageCount >= pageIndex &&
        $(window).scrollTop() + $(window).height() >= $(footer).offset().top) {
        appendData();
    }
});

// ارسال اولین درخواست هنگام لود صفحه بدون اسکرول
if ($(window).data('ajaxready') == true) {
    appendData();
}

// درخواست ایجکس
function appendData() {
    $(window).data('ajaxready', false);
    $.ajax({
        type: 'GET',
        url: '/BusinessPlans/ActivePlans',
        data: { "searchText": searchTextInput, "lowestPrice": minFilterPriceInput, "highestPrice": maxFilterPriceInput, "page": pageIndex },
        success: function (data) {
            if (data != null) {
                $("#list-plans").append(data);
                totalPageCount = Math.ceil(parseInt($(".hidden-input-paging").val()) / pageSize);
                pageIndex++;
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("appendData, " + xhr.status + ", " + thrownError);
        },
        beforeSend: function () {
            $("#progress").show();
        },
        complete: function () {
            $("#progress").hide();
            $(window).data('ajaxready', true);
        },
    });
}

function filterPriceInput() {
    var min = $('.min-value').val();
    var max = $('.max-value').val();

    if (min != "" && max != "") {
        maxFilterPriceInput = min;
        minFilterPriceInput = max;
    }
}

// جستجو ایجکس
function search() {
    pageIndex = 1;
    $("#list-plans").empty();
    filterPriceInput();
    searchTextInput = $('#search_box').val();
    appendData();
}

$("#search_box").on('keyup', function (e) {
    if (e.which == 13) {
        search();
    }
});

//range silder
; (function () {

    var doubleHandleSlider = document.querySelector('.double-handle-slider');
    var minValInput = document.querySelector('.min-value');
    var maxValInput = document.querySelector('.max-value');


    noUiSlider.create(doubleHandleSlider, {
        start: [lowestPrice, highestPrice],
        connect: true,
        tooltips: true,
        step: (highestPrice - lowestPrice) / 100,
        range: {
            'min': [lowestPrice],
            'max': [highestPrice]
        },
        format: {
            to: function (value) {
                return value;
            },
            from: function (value) {
                return value;
            }
        }
    });

    // can also be on 'update' for instant update
    doubleHandleSlider.noUiSlider.on('change', function (values, handle) {

        // This version updates both inputs.
        var rangeValues = values;
        minValInput.value = rangeValues[0];
        maxValInput.value = rangeValues[1];

        /*
                // This version updates a single input on change
                var val = values[handle]; // 0 or 1

                if(handle) {
                    maxValInput.value = Math.round(val);
                } else {
                    minValInput.value = Math.round(val);
                }*/
    });

    minValInput.addEventListener('change', function () {
        doubleHandleSlider.noUiSlider.set([this.value, null]);
    });

    maxValInput.addEventListener('change', function () {
        doubleHandleSlider.noUiSlider.set([null, this.value]);
    });
})();