declare var $: any;
declare var langs: Langs;

class Langs extends BasePage {

    currentId: number;

    constructor() {
        super();
    }




    public LoadLangs() {
        let result = Comm.Get("/nomen/AllLangs");
        if (result == -1) {
            //BasePage.LoadLogin();
        }
        let tbl = $("#tItems tbody");
        tbl.empty();
        $(result).each(function (i, e) {
            let row: String = "<tr recordId='" + e.LangId + "'>" +
                "<td>" + e.LangName + "</td>" +
                "<td>" + e.IsActive + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"langs.EditLang(this)\"></span><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" onclick=\"langs.LangChangeActive(this)\"></span></td></tr>";

            tbl.append(row);
        });

    }

    public SetLang() {
        BasePage.HideErrors();
        let err: boolean = false;
        if ($("#tbLangName").val() == "") {
            $("#lErrtbLangName").show();
            err = true;
        }

        
        if (err)
            return;
        let rslt = Comm.Post("/nomen/setlang", {
            LangId: this.currentId,
            LangName: $("#tbLangName").val(),
            IsActive: $("#cbIsActive").prop("checked")
        });

        $("#dLang").modal('hide');
        this.LoadLangs();
    }

    public CancelLang() {
        BasePage.HideErrors();
        $("#dLang").modal('hide');
    }

    public LangChangeActive(element: any) {
        let langId: number = $(element).parent().parent().attr('recordId');
        var result = Comm.Post("/nomen/LangChangeActive", { langId: langId });

        this.LoadLangs();
    }

    public EditLang(element) {
        if (element == null) {
            this.currentId = -1;
            $("#tbLangName").val("");
            $("#cbIsActive").prop("checked", true);

        }
        else {
            let id = $(element).parent().parent().attr("recordid");

            let obj = Comm.Post("/nomen/GetLang", { langId: id });
            this.currentId = obj.LangId;
            $("#tbLangName").val(obj.LangName);
            $("#cbIsActive").prop("checked", obj.IsActive);

        }
        $("#dLang").modal('show');
    }

}


