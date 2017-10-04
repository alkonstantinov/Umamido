var Restaurant = (function () {
    function Restaurant() {
    }
    Restaurant.CallRegister = function (goodId) {
        jQuery("#hGoodId").val(goodId);
        jQuery("#dlgOrder").modal("show");
    };
    Restaurant.CheckAddress = function () {
        var data = {
            address: jQuery("#address").val()
        };
        var result;
        jQuery.ajax({
            cache: false,
            url: "/home/CheckAddress",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(data),
            async: false,
            success: function (res) {
                result = res;
            },
            error: function (a, b, c) {
            }
        });
        if (result.length == 0 || (result.length == 1 && !result[0].IsOk)) {
            jQuery("#dFarAddress").show();
            jQuery("#dAddressOK").hide();
            return;
        }
        if (result.length == 1 && result[0].IsOk) {
            jQuery("#dFarAddress").hide();
            jQuery("#dAddressOK").show();
            jQuery("#lCorrectAddress").text(result[0].FormatedAddress);
            return;
        }
    };
    Restaurant.SendDistantAddress = function () {
        var data = {
            Email: jQuery("#email").val(),
            Address: jQuery("#address").val()
        };
        var result;
        jQuery.ajax({
            cache: false,
            url: "/home/SaveDistantAddress",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(data),
            async: false,
            success: function (res) {
            },
            error: function (a, b, c) {
            }
        });
        jQuery("#dlgOrder").modal("hide");
    };
    Restaurant.ConfirmAddress = function () {
        var data = {
            address2: jQuery("#lCorrectAddress").text(),
            address1: jQuery("#address").val()
        };
        var result;
        jQuery.ajax({
            cache: false,
            url: "/home/GoodAddress",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(data),
            async: false,
            success: function (res) {
                location.reload(true);
            },
            error: function (a, b, c) {
            }
        });
    };
    Restaurant.LoginClient = function () {
        var data = {
            Username: jQuery("#username").val(),
            Password: jQuery("#pass").val()
        };
        var result;
        jQuery.ajax({
            cache: false,
            url: "/home/clientlogin",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(data),
            async: false,
            success: function (res) {
                if (res == null) {
                    jQuery("#dInvalidLogin").show();
                }
                else {
                    location.reload(true);
                }
            },
            error: function (a, b, c) {
            }
        });
    };
    Restaurant.ShowGoods = function (restaurantId) {
        jQuery("#categories-menu li").each(function (i, e) {
            if (jQuery(e).attr('restaurantid') == restaurantId)
                jQuery(e).attr('class', "menu-item current-menu-item rest-menu-item current-rest");
            else
                jQuery(e).attr('class', "menu-item rest-menu-item");
        });
        var result = null;
        var url = "home/getgoods?restaurantId=" + restaurantId;
        jQuery.ajax({
            cache: false,
            url: url,
            method: "GET",
            async: false,
            success: function (res) {
                result = res;
            },
            error: function (a, b, c) {
            }
        });
        jQuery("#ulProducts").empty();
        var left = 0;
        var top = 0;
        var dLeft = 413;
        var dTop = 407;
        var currentItem = 0;
        for (var _i = 0, result_1 = result; _i < result_1.length; _i++) {
            var g = result_1[_i];
            var item = template;
            item = item.replace(/\{left\}/gi, left.toString());
            item = item.replace(/\{top\}/gi, top.toString());
            item = item.replace(/\{url\}/gi, "/home/good?goodId=" + g.GoodId);
            item = item.replace(/\{name\}/gi, g.FirstTitle);
            item = item.replace(/\{price\}/gi, parseFloat(g.Price).toFixed(2));
            item = item.replace(/\{img\}/gi, "home/getimage?imageId=" + g.ImageId);
            item = item.replace(/\{ID\}/gi, g.GoodId);
            jQuery("#ulProducts").append(item);
            currentItem++;
            if (currentItem % 3 == 0) {
                left = 0;
                top += dTop;
            }
            else
                left += dLeft;
            jQuery("#ulProducts").append(item);
        }
    };
    return Restaurant;
}());
//# sourceMappingURL=restaurant.js.map