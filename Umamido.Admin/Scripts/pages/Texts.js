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
var Texts = (function (_super) {
    __extends(Texts, _super);
    function Texts() {
        return _super.call(this) || this;
    }
    Texts.prototype.LoadTexts = function () {
        var result = Comm.Get("/nomen/AllTexts");
        if (result == -1) {
            //BasePage.LoadLogin();
        }
        var tbl = $("#tItems tbody");
        tbl.empty();
        for (var _i = 0, result_1 = result; _i < result_1.length; _i++) {
            var e = result_1[_i];
            var row = "<tr data='" + JSON.stringify(e) + "'>" +
                "<td>" + e.TextName + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"texts.EditText(this)\"></span></td></tr>";
            tbl.append(row);
        }
    };
    Texts.prototype.SetText = function () {
        BasePage.HideErrors();
        var err = false;
        if ($("#tbTextName").val() == "") {
            $("#lErrtbTextName").show();
            err = true;
        }
        if (err)
            return;
        var data = {
            TextId: this.currentId,
            TextName: $("#tbTextName").val(),
            Descriptions: []
        };
        for (var _i = 0, _a = $("#dDescriptions").find("div"); _i < _a.length; _i++) {
            var d = _a[_i];
            if ($(d).attr('languageid') != null) {
                data.Descriptions.push({
                    ID: $(d).attr('rowid'),
                    LangId: $(d).attr('languageid'),
                    Text: tinymce.get('tmceDescription_' + $(d).attr('languageid')).getContent()
                });
            }
        }
        Comm.Post("/nomen/SetText", data);
        $("#dText").modal('hide');
        //tinymce.EditorManager.editors = [];
        this.LoadTexts();
    };
    Texts.prototype.CancelText = function () {
        BasePage.HideErrors();
        //tinymce.EditorManager.editors = [];
        $("#dText").modal('hide');
    };
    Texts.prototype.EditText = function (element) {
        $("#dDescriptions").empty();
        if (element == null) {
            $("#tbTextName").val("");
            this.currentId = -1;
            for (var _i = 0, _a = Comm.Get("/nomen/AllLangs"); _i < _a.length; _i++) {
                var lang = _a[_i];
                $("#dDescriptions").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='-1'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceDescription_" + lang.LangId + "'/></div>" +
                    "</div>");
            }
        }
        else {
            var obj = JSON.parse($(element).parent().parent().attr("data"));
            this.currentId = obj.TextId;
            $("#tbTextName").val(obj.TextName);
            for (var _b = 0, _c = obj.Descriptions; _b < _c.length; _b++) {
                var lang = _c[_b];
                $("#dDescriptions").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='" + lang.ID + "'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceDescription_" + lang.LangId + "'>" + lang.Text + "</textarea></div>" +
                    "</div>");
            }
        }
        $("#dText").modal('show');
        //tinymce.init({ selector: 'textarea' });
        BasePage.TinyMCE();
    };
    return Texts;
}(BasePage));
//# sourceMappingURL=Texts.js.map