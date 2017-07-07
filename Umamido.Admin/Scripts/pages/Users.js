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
var Users = (function (_super) {
    __extends(Users, _super);
    function Users() {
        return _super.call(this) || this;
    }
    Users.prototype.LoadUsers = function () {
        var result = Comm.Get("/security/AllUsers");
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
        $(result).each(function (i, e) {
            var row = "<tr recordId='" + e.UserId + "'>" +
                "<td>" + e.UserName + "</td>" +
                "<td>" + e.IsActive + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"users.EditUser(this)\"></span><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" onclick=\"users.UserChangeActive(this)\"></span></td></tr>";
            tbl.append(row);
        });
    };
    Users.prototype.SetUser = function () {
        BasePage.HideErrors();
        var err = false;
        if ($("#tbUserName").val() == "") {
            $("#lErrtbUserName").show();
            err = true;
        }
        if (this.currentId == -1 && Comm.Post("/security/UserExists", { userName: $("#tbUserName").val() })) {
            $("#lErrUserNameUsed").show();
            err = true;
        }
        if ($("#tbRePassword").val() != $("#tbPassword").val()) {
            $("#lErrtbRePassword").show();
            err = true;
        }
        if (this.currentId == -1 && $("#tbPassword").val() == "") {
            $("#lErrtbPassword").show();
            err = true;
        }
        if (err)
            return;
        var rslt = Comm.Post("/security/setuser", {
            UserId: this.currentId,
            UserName: $("#tbUserName").val(),
            Password: $("#tbPassword").val(),
            IsActive: $("#cbIsActive").prop("checked")
        });
        $("#dUser").modal('hide');
        this.LoadUsers();
    };
    Users.prototype.CancelUser = function () {
        BasePage.HideErrors();
        $("#dUser").modal('hide');
    };
    Users.prototype.UserChangeActive = function (element) {
        var userId = $(element).parent().parent().attr('recordId');
        var result = Comm.Post("/security/UserChangeActive", { userId: userId });
        users.LoadUsers();
    };
    Users.prototype.EditUser = function (element) {
        if (element == null) {
            this.currentId = -1;
            $("#tbUserName").val("");
            $("#tbUserName").prop("disabled", false);
            $("#tbPassword").val("");
            $("#tbRePassword").val("");
            $("#cbIsActive").prop("checked", true);
        }
        else {
            var id = $(element).parent().parent().attr("recordid");
            var obj = Comm.Post("/security/GetUser", { userId: id });
            this.currentId = obj.UserId;
            $("#tbUserName").val(obj.UserName);
            $("#tbUserName").prop("disabled", true);
            $("#tbPassword").val("");
            $("#tbRePassword").val("");
            $("#cbIsActive").prop("checked", obj.IsActive);
        }
        $("#dUser").modal('show');
    };
    return Users;
}(BasePage));
//# sourceMappingURL=Users.js.map