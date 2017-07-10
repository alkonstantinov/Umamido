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
var ForCollect = (function (_super) {
    __extends(ForCollect, _super);
    function ForCollect() {
        var _this = _super.call(this) || this;
        var self = _this;
        setInterval(function () { self.LoadReqs(); }, 30000);
        return _this;
    }
    ForCollect.prototype.LoadReqs = function () {
        var result = Comm.Get("/dispatch/AllForCollect");
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
                "<td><span class=\"glyphicon glyphicon-gift\" aria-hidden=\"true\" onclick=\"forCollect.ShowDetailsDialog(this)\"></span></td></tr>";
            tbl.append(row);
        });
    };
    ForCollect.prototype.Collected = function () {
        BasePage.HideErrors();
        var rslt = Comm.Post("/dispatch/SetCollected", {
            ReqId: this.currentId,
            UserId: null
        });
        $("#dCollect").modal('hide');
        this.LoadReqs();
    };
    ForCollect.prototype.CancelCollect = function () {
        BasePage.HideErrors();
        $("#dCollect").modal('hide');
    };
    ForCollect.prototype.ShowDetailsDialog = function (element) {
        var id = $(element).parent().parent().attr("recordid");
        this.currentId = id;
        var result = Comm.Post("/dispatch/CollectDetails", { reqId: id });
        $("#lReqId").text(id);
        var tbl = $("#tDetails tbody");
        tbl.empty();
        $(result).each(function (i, e) {
            var row = "<tr>" +
                "<td>" + e.Restaurant + "</td>" +
                "<td>" + e.Good + "</td>" +
                "<td>" + e.CookMinutes + "</td>" +
                "<td>" + e.Quantity + "</td>" +
                "</tr>";
            tbl.append(row);
        });
        $("#dCollect").modal('show');
    };
    return ForCollect;
}(BasePage));
//# sourceMappingURL=ForCollect.js.map