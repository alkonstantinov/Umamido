declare var $: any;
declare var tinymce: any;
declare var texts: Texts;

class Texts extends BasePage {

    currentId: number;

    constructor() {
        super();
    }


    
    
    public LoadTexts() {
        let result = Comm.Get("/nomen/AllTexts");
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
        for (let e of result) {
            let row: String = "<tr data='" + JSON.stringify(e) + "'>" +
                "<td>" + e.TextName + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"texts.EditText(this)\"></span></td></tr>";

            tbl.append(row);
        }

    }
    
    public SetText() {
        BasePage.HideErrors();
        let err: boolean = false;
        if ($("#tbTextName").val() == "") {
            $("#lErrtbTextName").show();
            err = true;
        }


        if (err)
            return;
        let data = {
            TextId: this.currentId,
            TextName: $("#tbTextName").val(),
            Descriptions: []
        };

        

        for (let d of $("#dDescriptions").find("div")) {


            if ($(d).attr('languageid') != null) {
                data.Descriptions.push({
                    ID: $(d).attr('rowid'),
                    LangId: $(d).attr('languageid'),
                    Text: tinymce.get('tmceDescription_' + $(d).attr('languageid')).getContent()

                });
            }

        }
        Comm.Post("/nomen/SetText", data);
        $("#dText").modal('hide');
        //tinymce.EditorManager.editors = [];
        this.LoadTexts();
    }

    public CancelText() {
        BasePage.HideErrors();
        //tinymce.EditorManager.editors = [];
        $("#dText").modal('hide');
    }

    

    public EditText(element) {
        $("#dDescriptions").empty();
        if (element == null) {
            $("#tbTextName").val("");
            this.currentId = -1;
            for (let lang of Comm.Get("/nomen/AllLangs")) {
                $("#dDescriptions").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='-1'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceDescription_" + lang.LangId + "'/></div>" +
                    "</div>");

            }


        }
        else {
            let obj = JSON.parse($(element).parent().parent().attr("data"));

            this.currentId = obj.TextId;
            $("#tbTextName").val(obj.TextName);

            
            for (let lang of obj.Descriptions) {
                $("#dDescriptions").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='" + lang.ID + "'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceDescription_" + lang.LangId + "'>" + lang.Text + "</textarea></div>" +
                    "</div>");

            }

        }
        $("#dText").modal('show');
        //tinymce.init({ selector: 'textarea' });
        BasePage.TinyMCE();
    }

}


