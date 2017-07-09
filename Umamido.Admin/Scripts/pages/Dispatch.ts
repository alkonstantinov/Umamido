declare var $: any;
declare var dispatch: Dispatch;

class Dispatch extends BasePage {

    currentId: number;

    constructor() {
        super();
        let self = this;
        setInterval(function () { self.LoadReqs(); }, 30000);
    }




    public LoadReqs() {
        
        let result = Comm.Get("/dispatch/AllForDispatch");
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
            let row: String = "<tr recordId='" + e.ReqId + "'>" +
                "<td>" + e.Receiver + "</td>" +
                "<td>" + e.Address + "</td>" +
                "<td><span class=\"glyphicon glyphicon-gift\" aria-hidden=\"true\" onclick=\"dispatch.ShowDispatchDialog(this)\"></span></td></tr>";

            tbl.append(row);
        });

    }

    public Dispatch() {
        BasePage.HideErrors();
        let err: boolean = false;
        if ($("#ddlUser").val() == null) {
            $("#lErrddlUser").show();
            err = true;
        }


        if (err)
            return;
        let rslt = Comm.Post("/dispatch/setdispatch", {
            ReqId: this.currentId,
            UserId: $("#ddlUser").val()
        });

        $("#dDispatch").modal('hide');
        this.LoadReqs();
    }

    public CancelDispatch() {
        BasePage.HideErrors();
        $("#dDispatch").modal('hide');
    }

    
    public ShowDispatchDialog(element) {
        let id = $(element).parent().parent().attr("recordid");

        this.currentId = id;


        $("#dDispatch").modal('show');
    }


    public LoadUsers() {
        let result = Comm.Post("/security/AllUsers", { userLevelId: 2 });

        let ddl = $("#ddlUser");

        $(result).each(function (i, e) {

            ddl.append("<option value='" + e.UserId + "'>" + e.UserName + "</option>");
        });

    }

}


