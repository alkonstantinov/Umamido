declare var jQuery: any;
declare var template: string;
declare var restaurant: Restaurant;

class Restaurant {
    public static ShowGoods(restaurantId) {

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
        for (var g of result) {
            var item = template;
            item = item.replace(/\{left\}/gi, left.toString());
            item = item.replace(/\{top\}/gi, top.toString());
            item = item.replace(/\{url\}/gi, "/home/good?goodId=" + g.GoodId);
            item = item.replace(/\{name\}/gi, g.FirstTitle);
            item = item.replace(/\{price\}/gi, parseFloat(g.Price).toFixed(2));
            item = item.replace(/\{img\}/gi, "home/getimage?imageId=" + g.ImageId);

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

    }
}