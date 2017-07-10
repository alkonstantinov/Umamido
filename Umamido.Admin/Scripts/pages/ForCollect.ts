declare var $: any;
declare var forCollect: ForCollect;

class ForCollect extends BasePage {

    currentId: number;

    constructor() {
        super();
        let self = this;
        setInterval(function () { self.LoadReqs(); }, 30000);
    }




    public LoadReqs() {
        
        let result = Comm.Get("/dispatch/AllForCollect");
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
                "<td><span class=\"glyphicon glyphicon-gift\" aria-hidden=\"true\" onclick=\"forCollect.ShowDetailsDialog(this)\"></span></td></tr>";

            tbl.append(row);
        });

    }

    public Collected() {
        BasePage.HideErrors();

        let rslt = Comm.Post("/dispatch/SetCollected", {
            ReqId: this.currentId,
            UserId: null
        });

        $("#dCollect").modal('hide');
        this.LoadReqs();
    }

    public CancelCollect() {
        BasePage.HideErrors();
        $("#dCollect").modal('hide');
    }

    
    public ShowDetailsDialog(element) {
        let id = $(element).parent().parent().attr("recordid");

        this.currentId = id;
        let result = Comm.Post("/dispatch/CollectDetails", { reqId: id});
        $("#lReqId").text(id);
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

        $("#dCollect").modal('show');
    }
    
}


