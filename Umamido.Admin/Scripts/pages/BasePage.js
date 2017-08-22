var BasePage = (function () {
    function BasePage() {
    }
    BasePage.TinyMCE = function () {
        tinymce.EditorManager.editors = [];
        tinymce.init({ selector: 'textarea', plugins: "code" });
    };
    BasePage.GUID = function () {
        var d = new Date().getTime();
        if (window.performance && typeof window.performance.now === "function") {
            d += performance.now(); //use high-precision timer if available
        }
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
    };
    BasePage.ApplyNumbersOnly = function () {
        $(".numbersonly").keyup(function (e) {
            this.value = this.value.replace(/[^0-9]/gi, '');
        });
        $(".numbersdotonly").keyup(function (e) {
            this.value = this.value.replace(/[^0-9\.,]/gi, '');
            this.value = this.value.replace(/,/gi, '.');
        });
    };
    BasePage.prototype.parseJsonDate = function (jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    };
    BasePage.LoadPage = function (fnm) {
        var d = new Date();
        $("#dContent").load("app/pages/" + fnm + "?" + d.getUTCMilliseconds());
    };
    BasePage.ShowMenuActive = function (liMiName) {
        $("li[id^='liMi']").removeAttr("class");
        $("#liMi" + liMiName).prop("class", "active");
    };
    BasePage.ShowMenu = function () {
        $("#nMenu").show();
    };
    BasePage.HideMenu = function () {
        $("#nMenu").hide();
    };
    BasePage.LoadError = function (a, b, c) {
        var myRegexp = /<h2>([\w\W]+)<\/h2>/g;
        var match = myRegexp.exec(a.responseText);
        var form = document.createElement("form");
        form.setAttribute("method", "post");
        form.setAttribute("action", "/security/errorjs");
        form.setAttribute("target", "_top");
        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "hidden");
        hiddenField.setAttribute("name", "a");
        hiddenField.setAttribute("value", match[1]);
        form.appendChild(hiddenField);
        hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "hidden");
        hiddenField.setAttribute("name", "b");
        hiddenField.setAttribute("value", b);
        form.appendChild(hiddenField);
        hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "hidden");
        hiddenField.setAttribute("name", "c");
        hiddenField.setAttribute("value", c);
        form.appendChild(hiddenField);
        document.body.appendChild(form);
        form.submit();
    };
    BasePage.HideErrors = function () {
        $("div[id^='lErr']").hide();
    };
    BasePage.LoadLogin = function () {
        window.location.href = "/security/logoff";
    };
    BasePage.ValidEGN = function (s) {
        var t = [2, 4, 8, 5, 10, 9, 7, 3, 6];
        //if (typeof s != 'string') return false;
        if (s.length != 10)
            return false;
        var rv;
        var rr = 0;
        for (var i = 0; i < 9; i++) {
            if (s[i] == '0')
                continue;
            rr = rr + (parseInt(s[i]) * t[i]);
        }
        var chs = 0;
        chs = (rr % 11);
        if (chs == 10)
            chs = 0;
        if (parseInt(s[9]) == chs)
            return true;
        else
            return false;
    };
    BasePage.ToJavaScriptDate = function (value) {
        var pattern = /Date\(([^)]+)\)/;
        var results = pattern.exec(value);
        var dt = new Date(parseFloat(results[1]));
        var day = "" + dt.getDate();
        if (day.length == 1)
            day = "0" + day;
        var month = "" + (dt.getMonth() + 1);
        if (month.length == 1)
            month = "0" + month;
        return day + "." + month + "." + dt.getFullYear();
    };
    // Validates that the input string is a valid date formatted as "mm/dd/yyyy"
    BasePage.IsValidDate = function (dateString) {
        // First check for the pattern
        if (!/\d{2}\.\d{2}\.\d{4}$/.test(dateString))
            return false;
        // Parse the date parts to integers
        var parts = dateString.split(".");
        var day = parseInt(parts[0], 10);
        var month = parseInt(parts[1], 10);
        var year = parseInt(parts[2], 10);
        alert(parts);
        // Check the ranges of month and year
        if (year < 1000 || year > 3000 || month == 0 || month > 12)
            return false;
        var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        // Adjust for leap years
        if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
            monthLength[1] = 29;
        // Check the range of the day
        return day > 0 && day <= monthLength[month - 1];
    };
    ;
    return BasePage;
}());
//# sourceMappingURL=BasePage.js.map