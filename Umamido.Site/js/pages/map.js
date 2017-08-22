var Map = (function () {
    function Map() {
    }
    Map.prototype.PerformCheck = function (address) {
        var result = null;
        var data = { address: address };
        jQuery.ajax({
            cache: false,
            url: "/map/CheckAddress",
            method: "post",
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
    };
    return Map;
}());
//# sourceMappingURL=map.js.map