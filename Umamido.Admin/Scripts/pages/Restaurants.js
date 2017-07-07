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
var Restaurants = (function (_super) {
    __extends(Restaurants, _super);
    function Restaurants() {
        return _super.call(this) || this;
    }
    Restaurants.prototype.LoadLogos = function () {
        var result = Comm.Get("/nomen/AllImages");
        for (var _i = 0, result_1 = result; _i < result_1.length; _i++) {
            var e = result_1[_i];
            $("#ddlLogo").append("<option value='" + e.ImageId + "'>" + e.ImageName + "</option>");
        }
    };
    Restaurants.prototype.ShowGoods = function (restaurantId) {
        window.location.href = "/nomen/goods?restaurantId=" + restaurantId;
    };
    Restaurants.prototype.LoadRestaurants = function () {
        var result = Comm.Get("/nomen/AllRestaurants");
        if (result == -1) {
            //BasePage.LoadLogin();
        }
        if (result.length == 0) {
            $("#tItems").hide();
            $("#lNoItems").show();
        }
        else {
            $("#tItems").show();
            $("#lNoItems").hide();
        }
        var tbl = $("#tItems tbody");
        tbl.empty();
        for (var _i = 0, result_2 = result; _i < result_2.length; _i++) {
            var e = result_2[_i];
            var row = "<tr data='" + JSON.stringify(e) + "'>" +
                "<td>" + e.FirstTitle + "</td>" +
                "<td><img src='/nomen/GetImageContentFromDB?imageId=" + e.ImageId + "&GUID=" + BasePage.GUID() + "' alt='' width='100' height='100'/></td>" +
                "<td>" + e.IsActive + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"restaurants.EditRestaurant(this)\"></span>" +
                "<span class=\"glyphicon glyphicon-gift\" aria-hidden=\"true\" onclick=\"restaurants.ShowGoods(" + e.RestaurantId + ")\"></span>" +
                "<span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" onclick=\"restaurants.RestaurantChangeActive(this)\"></span></td></tr>";
            tbl.append(row);
        }
    };
    Restaurants.prototype.RedisplayLogo = function () {
        $("#iPreview").attr("src", "/nomen/GetImageContentFromDB?imageId=" + $("#ddlLogo").val() + "&GUID=" + BasePage.GUID());
    };
    Restaurants.prototype.SetRestaurant = function () {
        BasePage.HideErrors();
        var err = false;
        if ($("#ddlLogo").val() == null) {
            $("#lErrddlLogo").show();
            err = true;
        }
        if (err)
            return;
        var data = {
            RestaurantId: this.currentId,
            ImageId: $("#ddlLogo").val(),
            IsActive: $("#cbIsActive").prop("checked"),
            Titles: [],
            Descriptions: []
        };
        for (var _i = 0, _a = $("#dTitles").find("div"); _i < _a.length; _i++) {
            var d = _a[_i];
            if ($(d).attr('languageid') != null) {
                data.Titles.push({
                    ID: $(d).attr('rowid'),
                    LangId: $(d).attr('languageid'),
                    Text: tinymce.get('tmceTitle_' + $(d).attr('languageid')).getContent()
                });
            }
        }
        for (var _b = 0, _c = $("#dDescriptions").find("div"); _b < _c.length; _b++) {
            var d = _c[_b];
            if ($(d).attr('languageid') != null) {
                data.Descriptions.push({
                    ID: $(d).attr('rowid'),
                    LangId: $(d).attr('languageid'),
                    Text: tinymce.get('tmceDescription_' + $(d).attr('languageid')).getContent()
                });
            }
        }
        Comm.Post("/nomen/SetRestaurant", data);
        $("#dRestaurant").modal('hide');
        //tinymce.EditorManager.editors = [];
        this.LoadRestaurants();
    };
    Restaurants.prototype.CancelRestaurant = function () {
        BasePage.HideErrors();
        //tinymce.EditorManager.editors = [];
        $("#dRestaurant").modal('hide');
    };
    Restaurants.prototype.RestaurantChangeActive = function (element) {
        var restaurantId = JSON.parse($(element).parent().parent().attr('data')).RestaurantId;
        var result = Comm.Post("/nomen/RestaurantChangeActive", { restaurantId: restaurantId });
        this.LoadRestaurants();
    };
    Restaurants.prototype.EditRestaurant = function (element) {
        $("#cbIsActive").prop("checked", true);
        $("#dTitles").empty();
        $("#dDescriptions").empty();
        if (element == null) {
            this.currentId = -1;
            for (var _i = 0, _a = Comm.Get("/nomen/AllLangs"); _i < _a.length; _i++) {
                var lang = _a[_i];
                $("#dTitles").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='-1'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceTitle_" + lang.LangId + "'/></div>" +
                    "</div>");
                $("#dDescriptions").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='-1'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceDescription_" + lang.LangId + "'/></div>" +
                    "</div>");
            }
        }
        else {
            var obj = JSON.parse($(element).parent().parent().attr("data"));
            $("#cbIsActive").prop("checked", obj.IsActive);
            $("#ddlLogo").val(obj.ImageId);
            this.currentId = obj.RestaurantId;
            for (var _b = 0, _c = obj.Titles; _b < _c.length; _b++) {
                var lang = _c[_b];
                $("#dTitles").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='" + lang.ID + "'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceTitle_" + lang.LangId + "'>" + lang.Text + "</textarea></div>" +
                    "</div>");
            }
            for (var _d = 0, _e = obj.Descriptions; _d < _e.length; _d++) {
                var lang = _e[_d];
                $("#dDescriptions").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='" + lang.ID + "'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceDescription_" + lang.LangId + "'>" + lang.Text + "</textarea></div>" +
                    "</div>");
            }
        }
        $("#dRestaurant").modal('show');
        //tinymce.init({ selector: 'textarea' });
        BasePage.TinyMCE();
        this.RedisplayLogo();
    };
    return Restaurants;
}(BasePage));
//# sourceMappingURL=Restaurants.js.map