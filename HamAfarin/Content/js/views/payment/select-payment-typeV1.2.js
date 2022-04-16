$(document).ready(function () {
    var check = Getchecked_SiteRules_RiskStatement_InvestmentContract();
    handleInvestmentRules(check);

    $('#SiteRules').change(function () {

        check = Getchecked_SiteRules_RiskStatement_InvestmentContract();

        handleInvestmentRules(check);
    });
    $('#RiskStatement').change(function () {
        check = Getchecked_SiteRules_RiskStatement_InvestmentContract();

        handleInvestmentRules(check);
    });
    $('#InvestmentContract').change(function () {
        check = Getchecked_SiteRules_RiskStatement_InvestmentContract();

        handleInvestmentRules(check);
    });

    if ($("#IsOnline").val().toLowerCase() == "true") {
        document.getElementById("onlineRadio").checked = true;
        document.getElementById("offlineRadio").checked = false;
        handlePaymentOnline(true);
    }
    else {
        document.getElementById("offlineRadio").checked = true;
        document.getElementById("onlineRadio").checked = false;
        handlePaymentOffline(true);
    }
    $('#onlineRadio').change(function () {
        handlePaymentOnline($(this).is(":checked"));
    });
    $('#offlineRadio').change(function () {
        handlePaymentOffline($(this).is(":checked"));
    });
});

function Getchecked_SiteRules_RiskStatement_InvestmentContract() {
    var check = false;

    if ($(SiteRules).is(":checked") && $(RiskStatement).is(":checked") && $(InvestmentContract).is(":checked")) {
        check = true;
    }
    return check;
}

function handleInvestmentRules(checked) {
    if (checked) {
        $('#paymentType').show();
        $('#btnSubmit').show();
    }
    else {
        $('#paymentType').hide();
        $('#btnSubmit').hide();
    }
};

function handlePaymentOnline(isOnlineRadio) {
    $("#IsOnline").val(true);
    if (isOnlineRadio) {
        $('#onlineView').show();
        $('#offlineView').hide();
    }
    else {
        $('#onlineView').hide();
        $('#offlineView').show();
    }
};

function handlePaymentOffline(isOnlineRadio) {
    $("#IsOnline").val(false);
    if (isOnlineRadio) {
        $('#offlineView').show();
        $('#onlineView').hide();
    }
    else {
        $('#offlineView').hide();
        $('#onlineView').show();
    }
};

$('#OnlinePaymentPrice').keyup(function (event) {
    var PaymentPriceSeprate = document.getElementById('OnlinePaymentPriceSeprate');
    var Num = $("#OnlinePaymentPrice").val();
    var seprateValue = Num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    PaymentPriceSeprate.innerHTML = seprateValue + " تومان";
});

$('#OfflinePaymentPrice').keyup(function (event) {
    var PaymentPriceSeprate = document.getElementById('OfflinePaymentPriceSeprate');
    var Num = $("#OfflinePaymentPrice").val();
    var seprateValue = Num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    PaymentPriceSeprate.innerHTML = seprateValue + " تومان";

});

function inputPriceHandeler(id) {
    let inputVal = $(id).val();
    let inputLength = inputVal.length;
    if (inputLength == 0)
        return false;
    var x = inputVal.charAt(inputLength - 1);
    if (x != '۰' && x != '۱' && x != '۲' && x != '۳' && x != '۴' && x != '۵' && x != '۶' && x != '۷' && x != '۸' && x != '۹') {

        $(id).val(inputVal.replace(/[^0-9]/g, ''));
    }
}

// بستن مودال با دکمه بک در گوشی
if (window.history && window.history.pushState) {
    $('#privacyModal').on('show.bs.modal', function (e) {
        window.history.pushState('forward', null, '#modal');
    });

    $(window).on('popstate', function () {
        $('#privacyModal').modal('hide');
    });
}