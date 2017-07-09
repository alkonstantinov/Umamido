

declare var $: any;

class Comm {

    static Get(url: String) {
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
        if (result == -1)
            BasePage.LoadLogin();
        return result;
    }


    static Post(url: string, data: any): any {
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
        if (result == -1)
            BasePage.LoadLogin();
        return result;
    }

}


