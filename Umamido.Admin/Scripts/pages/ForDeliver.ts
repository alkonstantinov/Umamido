declare var $: any;
declare var forDeliver: ForDeliver;

class ForDeliver extends BasePage {

    currentId: number;

    constructor() {
        super();
        let self = this;
        setInterval(function () { self.LoadReqs(); }, 30000);
    }




    public LoadReqs() {

        let result = Comm.Get("/dispatch/AllForDeliver");
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
                "<td><span class=\"glyphicon glyphicon-gift\" aria-hidden=\"true\" onclick=\"forDeliver.ShowDetailsDialog(this)\"></span>" +
                "<a href='geo:" + e.LatLong + "'><span class=\"glyphicon glyphicon-map-marker\" aria-hidden=\"true\"></span></a></td></tr>";

            tbl.append(row);
        });

    }

    public Delivered(deliv: boolean) {
        BasePage.HideErrors();

        let rslt = Comm.Post("/dispatch/SetDelivered", {
            ReqId: this.currentId,
            UserId: null,
            Note: $("#tbNote").val(),
            NewStatus: (deliv ? 5 : 6)
        });

        $("#dDeliver").modal('hide');
        this.LoadReqs();
    }

    public CancelDeliver() {
        BasePage.HideErrors();
        $("#dDeliver").modal('hide');
    }


    public ShowDetailsDialog(element) {
        let id = $(element).parent().parent().attr("recordid");

        this.currentId = id;
        let result = Comm.Post("/dispatch/CollectDetails", { reqId: id });
        $("#lReqId").text(id);
        $("#tbNote").val("");
        let tbl = $("#tDetails tbody");
        tbl.empty();
        $(result).each(function (i, e) {
            let row: String = "<tr>" +
                "<td>" + e.Restaurant + "</td>" +
                "<td>" + e.Good + "</td>" +
                "<td>" + e.CookMinutes + "</td>" +
                "<td>" + e.Quantity + "</td>" +
                "</tr>"

            tbl.append(row);
        });

        $("#dDeliver").modal('show');
    }

}


