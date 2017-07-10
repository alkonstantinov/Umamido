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
var ForDeliver = (function (_super) {
    __extends(ForDeliver, _super);
    function ForDeliver() {
        var _this = _super.call(this) || this;
        var self = _this;
        setInterval(function () { self.LoadReqs(); }, 30000);
        return _this;
    }
    ForDeliver.prototype.LoadReqs = function () {
        var result = Comm.Get("/dispatch/AllForDeliver");
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
                "<td><span class=\"glyphicon glyphicon-gift\" aria-hidden=\"true\" onclick=\"forDeliver.ShowDetailsDialog(this)\"></span></td></tr>";
            tbl.append(row);
        });
    };
    ForDeliver.prototype.Delivered = function (deliv) {
        BasePage.HideErrors();
        var rslt = Comm.Post("/dispatch/SetDelivered", {
            ReqId: this.currentId,
            UserId: null,
            Note: $("#tbNote").val(),
            NewStatus: (deliv ? 5 : 6)
        });
        $("#dDeliver").modal('hide');
        this.LoadReqs();
    };
    ForDeliver.prototype.CancelDeliver = function () {
        BasePage.HideErrors();
        $("#dDeliver").modal('hide');
    };
    ForDeliver.prototype.ShowDetailsDialog = function (element) {
        var id = $(element).parent().parent().attr("recordid");
        this.currentId = id;
        var result = Comm.Post("/dispatch/CollectDetails", { reqId: id });
        $("#lReqId").text(id);
        $("#tbNote").val("");
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
        $("#dDeliver").modal('show');
    };
    return ForDeliver;
}(BasePage));
//# sourceMappingURL=ForDeliver.js.map