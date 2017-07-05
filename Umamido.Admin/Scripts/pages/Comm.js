var Comm = (function () {
    function Comm() {
    }
    Comm.Get = function (url) {
        var result;
        $.ajax({
            cache: false,
            url: url,
            method: "GET",
            async: false,
            success: function (res) {
                result = res;
            },
            error: function (a, b, c) {
                BasePage.LoadError(a, b, c);
                return;
            }
        });
        return result;
    };
    Comm.Post = function (url, data) {
        var result = -100;
        $.ajax({
            cache: false,
            url: url,
            method: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(data),
            async: false,
            success: function (res) {
                result = res;
            },
            error: function (a, b, c) {
                BasePage.LoadError(a, b, c);
            }
        });
        return result;
    };
    return Comm;
}());
//# sourceMappingURL=Comm.js.map