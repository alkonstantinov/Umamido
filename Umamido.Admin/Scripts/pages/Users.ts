declare var $: any;
declare var users: Users;

class Users extends BasePage {

    currentId: number;

    constructor() {
        super();
    }




    public LoadUsers() {
        let result = Comm.Get("/security/AllUsers");
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
        let tbl = $("#tItems tbody");
        tbl.empty();
        $(result).each(function (i, e) {
            let row: String = "<tr recordId='" + e.UserId + "'>" +
                "<td>" + e.UserName + "</td>" +
                "<td>" + e.IsActive + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"users.EditUser(this)\"></span><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" onclick=\"users.UserChangeActive(this)\"></span></td></tr>";

            tbl.append(row);
        });

    }

    public SetUser() {
        BasePage.HideErrors();
        let err: boolean = false;
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
        let rslt = Comm.Post("/security/setuser", {
            UserId: this.currentId,
            UserName: $("#tbUserName").val(),
            Password: $("#tbPassword").val(),
            IsActive: $("#cbIsActive").prop("checked")
        });

        $("#dUser").modal('hide');
        this.LoadUsers();
    }

    public CancelUser() {
        BasePage.HideErrors();
        $("#dUser").modal('hide');
    }

    public UserChangeActive(element: any) {
        let userId: number = $(element).parent().parent().attr('recordId');
        var result = Comm.Post("/security/UserChangeActive", { userId: userId });

        users.LoadUsers();
    }

    public EditUser(element) {
        if (element == null) {
            this.currentId = -1;
            $("#tbUserName").val("");
            $("#tbUserName").prop("disabled", false);
            $("#tbPassword").val("");
            $("#tbRePassword").val("");
            $("#cbIsActive").prop("checked", true);

        }
        else {
            let id = $(element).parent().parent().attr("recordid");

            let obj = Comm.Post("/security/GetUser", { userId: id });
            this.currentId = obj.UserId;
            $("#tbUserName").val(obj.UserName);
            $("#tbUserName").prop("disabled", true);
            $("#tbPassword").val("");
            $("#tbRePassword").val("");
            $("#cbIsActive").prop("checked", obj.IsActive);

        }
        $("#dUser").modal('show');
    }

}


