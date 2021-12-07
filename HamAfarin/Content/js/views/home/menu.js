(function () {
    //خط کشیدن زیر منوی صفحه ی جاری
    var pathname = decodeURI(window.location.pathname);
    let x = document.getElementsByName('menuBarItems');
    x.forEach((child) => {
        var menuItemUrl = child.id.toString().replace("menuItemId", "");
        if (menuItemUrl == pathname) {
            var ItemId = child.id.toString();
            var elementQuery = document.getElementById(ItemId);
            elementQuery.classList.add("active");
            //$(elementQuery).addClass("active");
            //$("#"+ItemId).addClass("active");
        }
    });
})();

(function () {
    //خط کشیدن زیر منوی صفحه ی جاری در حالت موبایل
    var pathname = decodeURI(window.location.pathname);
    let x = document.getElementsByName('menuBarItemsInMobile');
    x.forEach((child) => {
        var menuItemUrl = child.id.toString().replace("menuItemIdInMobile", "");
        if (menuItemUrl == pathname) {
            var ItemId = child.id.toString();
            var elementQuery = document.getElementById(ItemId);
            elementQuery.firstElementChild.classList.replace("text-dark", "text-danger");
            elementQuery.firstElementChild.classList.add("text-underline");
        }
    });
})();