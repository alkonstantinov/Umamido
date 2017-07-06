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
    return BasePage;
}());
//# sourceMappingURL=BasePage.js.map