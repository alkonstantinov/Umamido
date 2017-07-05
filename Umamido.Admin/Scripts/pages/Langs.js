var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Langs = (function (_super) {
    __extends(Langs, _super);
    function Langs() {
        return _super.call(this) || this;
    }
    Langs.prototype.LoadLangs = function () {
        var result = Comm.Get("/nomen/AllLangs");
        if (result == -1) {
            //BasePage.LoadLogin();
        }
        var tbl = $("#tItems tbody");
        tbl.empty();
        $(result).each(function (i, e) {
            var row = "<tr recordId='" + e.LangId + "'>" +
                "<td>" + e.LangName + "</td>" +
                "<td>" + e.IsActive + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"langs.EditLang(this)\"></span><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" onclick=\"langs.LangChangeActive(this)\"></span></td></tr>";
            tbl.append(row);
        });
    };
    Langs.prototype.SetLang = function () {
        BasePage.HideErrors();
        var err = false;
        if ($("#tbLangName").val() == "") {
            $("#lErrtbLangName").show();
            err = true;
        }
        if (err)
            return;
        var rslt = Comm.Post("/nomen/setlang", {
            LangId: this.currentId,
            LangName: $("#tbLangName").val(),
            IsActive: $("#cbIsActive").prop("checked")
        });
        $("#dLang").modal('hide');
        this.LoadLangs();
    };
    Langs.prototype.CancelLang = function () {
        BasePage.HideErrors();
        $("#dLang").modal('hide');
    };
    Langs.prototype.LangChangeActive = function (element) {
        var langId = $(element).parent().parent().attr('recordId');
        var result = Comm.Post("/nomen/LangChangeActive", { langId: langId });
        this.LoadLangs();
    };
    Langs.prototype.EditLang = function (element) {
        if (element == null) {
            this.currentId = -1;
            $("#tbLangName").val("");
            $("#cbIsActive").prop("checked", true);
        }
        else {
            var id = $(element).parent().parent().attr("recordid");
            var obj = Comm.Post("/nomen/GetLang", { langId: id });
            this.currentId = obj.LangId;
            $("#tbLangName").val(obj.LangName);
            $("#cbIsActive").prop("checked", obj.IsActive);
        }
        $("#dLang").modal('show');
    };
    return Langs;
}(BasePage));
//# sourceMappingURL=Langs.js.map