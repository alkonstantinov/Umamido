var Restaurant = (function () {
    function Restaurant() {
    }
    Restaurant.CallRegister = function (goodId) {
        jQuery("#hGoodId").val(goodId);
        jQuery("#dlgOrder").modal("show");
    };
    Restaurant.ShowProperButton = function (canAdd) {
        if (!canAdd) {
            jQuery(".AddToCartLog").show();
            jQuery(".AddToCartNoLog").hide();
        }
        else {
            jQuery(".AddToCartLog").hide();
            jQuery(".AddToCartNoLog").show();
        }
    };
    Restaurant.AddToCart = function (goodId, quantity) {
        var data = {
            goodId: goodId,
            quantity: quantity
        };
        var result;
        jQuery.ajax({
            cache: false,
            url: "/home/AddToCart",
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
            jQuery("#dChooseAddress").hide();
            return;
        }
        if (result.length == 1 && result[0].IsOk) {
            jQuery("#dFarAddress").hide();
            jQuery("#dAddressOK").show();
            jQuery("#dChooseAddress").hide();
            jQuery("#lCorrectAddress").text(result[0].FormatedAddress);
            return;
        }
        if (result.length > 1) {
            jQuery("#dFarAddress").hide();
            jQuery("#dAddressOK").hide();
            jQuery("#dChooseAddress").show();
            jQuery("#dMultipleAddresses").empty();
            for (var _i = 0, result_1 = result; _i < result_1.length; _i++) {
                var a = result_1[_i];
                jQuery("#dMultipleAddresses").append('<p><input type="radio" name="ac" style="width:auto !important;" onclick="Restaurant.TryAddress(\'' + encodeURI(a.FormatedAddress) + '\')">' + a.FormatedAddress + '</p>');
            }
        }
    };
    Restaurant.TryAddress = function (address) {
        jQuery("#address").val(decodeURI(address));
        Restaurant.CheckAddress();
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
                Restaurant.AddToCart(jQuery("#hGoodId").val(), jQuery("#tbQuantity").val());
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
                    Restaurant.AddToCart(jQuery("#hGoodId").val(), jQuery("#tbQuantity").val());
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
        for (var _i = 0, result_2 = result; _i < result_2.length; _i++) {
            var g = result_2[_i];
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
    Restaurant.RedisplayCart = function () {
        var total = 0.00;
        jQuery("#tGoods tbody tr").each(function (i, e) {
            if (jQuery(e).find(".__Price").length == 0)
                return;
            var subtotal = parseFloat(jQuery(e).find(".__Price").text()) * parseFloat(jQuery(e).find(".__Count").val());
            jQuery(e).find(".__Total").text(subtotal.toFixed(2));
            total += subtotal;
        });
        jQuery("#sTotalGoods").text(total.toFixed(2));
        total += parseFloat(jQuery("#lDeliveryPrice").text());
        jQuery("#lFinalTotal").text(total.toFixed(2));
    };
    Restaurant.ChangeCartQuantity = function (el) {
        var data = {
            goodId: jQuery(el).parent().find(".__Id").val(),
            quantity: jQuery(el).val()
        };
        alert(JSON.stringify(data));
        var result;
        jQuery.ajax({
            cache: false,
            url: "/home/ChangeCartQuantity",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(data),
            async: false,
            success: function (res) {
                Restaurant.RedisplayCart();
            },
            error: function (a, b, c) {
            }
        });
    };
    Restaurant.DeleteFromCart = function (el) {
        var data = {
            goodId: jQuery(el).parent().find(".__Id").val()
        };
        var result;
        jQuery.ajax({
            cache: false,
            url: "/home/DeleteFromCart",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(data),
            async: false,
            success: function (res) {
                jQuery(el).parent().parent().remove();
                Restaurant.RedisplayCart();
            },
            error: function (a, b, c) {
            }
        });
    };
    Restaurant.OrderGood = function () {
        if (jQuery("#hCanOrder").val() == "False") {
            jQuery("#dlgOrder").modal("show");
        }
        else {
            Restaurant.AddToCart(jQuery("#hGoodId").val(), jQuery("#tbQuantity").val());
        }
    };
    return Restaurant;
}());
//# sourceMappingURL=restaurant.js.map