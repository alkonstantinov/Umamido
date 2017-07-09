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
var Dispatch = (function (_super) {
    __extends(Dispatch, _super);
    function Dispatch() {
        var _this = _super.call(this) || this;
        var self = _this;
        setInterval(function () { self.LoadReqs(); }, 30000);
        return _this;
    }
    Dispatch.prototype.LoadReqs = function () {
        var result = Comm.Get("/dispatch/AllForDispatch");
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
        $(result).each(function (i, e) {
            var row = "<tr recordId='" + e.ReqId + "'>" +
                "<td>" + e.Receiver + "</td>" +
                "<td>" + e.Address + "</td>" +
                "<td><span class=\"glyphicon glyphicon-gift\" aria-hidden=\"true\" onclick=\"dispatch.ShowDispatchDialog(this)\"></span></td></tr>";
            tbl.append(row);
        });
    };
    Dispatch.prototype.Dispatch = function () {
        BasePage.HideErrors();
        var err = false;
        if ($("#ddlUser").val() == null) {
            $("#lErrddlUser").show();
            err = true;
        }
        if (err)
            return;
        var rslt = Comm.Post("/dispatch/setdispatch", {
            ReqId: this.currentId,
            UserId: $("#ddlUser").val()
        });
        $("#dDispatch").modal('hide');
        this.LoadReqs();
    };
    Dispatch.prototype.CancelDispatch = function () {
        BasePage.HideErrors();
        $("#dDispatch").modal('hide');
    };
    Dispatch.prototype.ShowDispatchDialog = function (element) {
        var id = $(element).parent().parent().attr("recordid");
        this.currentId = id;
        $("#dDispatch").modal('show');
    };
    Dispatch.prototype.LoadUsers = function () {
        var result = Comm.Post("/security/AllUsers", { userLevelId: 2 });
        var ddl = $("#ddlUser");
        $(result).each(function (i, e) {
            ddl.append("<option value='" + e.UserId + "'>" + e.UserName + "</option>");
        });
    };
    return Dispatch;
}(BasePage));
//# sourceMappingURL=Dispatch.js.map