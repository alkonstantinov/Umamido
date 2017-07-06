declare var $: any;
declare var tinymce: any;
declare var sliders: Sliders;

class Sliders extends BasePage {

    currentId: number;

    constructor() {
        super();
    }


    public LoadLogos() {
        let result = Comm.Get("/nomen/AllImages");
        for (let e of result) {
            $("#ddlLogo").append("<option value='" + e.ImageId + "'>" + e.ImageName + "</option>");
        }

    }

    public ShowGoods(sliderId) {
        window.location.href = "/nomen/goods?sliderId=" + sliderId;
    }

    public LoadSliders() {
        let result = Comm.Get("/nomen/AllSliders");
        if (result == -1) {
            //BasePage.LoadLogin();
        }
        let tbl = $("#tItems tbody");
        tbl.empty();
        for (let e of result) {
            let row: String = "<tr data='" + JSON.stringify(e) + "'>" +
                "<td>" + e.FirstTitle + "</td>" +
                "<td><img src='/nomen/GetImageContentFromDB?imageId=" + e.ImageId + "&GUID=" + BasePage.GUID() + "' alt='' width='100' height='100'/></td>" +
                "<td>" + e.IsActive + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"sliders.EditSlider(this)\"></span>" +
                "<span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" onclick=\"sliders.SliderChangeActive(this)\"></span></td></tr>";

            tbl.append(row);
        }

    }

    public RedisplayLogo() {
        $("#iPreview").attr("src", "/nomen/GetImageContentFromDB?imageId=" + $("#ddlLogo").val() + "&GUID=" + BasePage.GUID());
    }

    public SetSlider() {
        BasePage.HideErrors();
        let err: boolean = false;
        if ($("#ddlLogo").val() == null) {
            $("#lErrddlLogo").show();
            err = true;
        }


        if (err)
            return;
        let data = {
            SliderId: this.currentId,
            ImageId: $("#ddlLogo").val(),
            IsActive: $("#cbIsActive").prop("checked"),
            Titles: [],
            Descriptions: []
        };

        for (let d of $("#dTitles").find("div")) {
            if ($(d).attr('languageid') != null) {
                data.Titles.push({
                    ID: $(d).attr('rowid'),
                    LangId: $(d).attr('languageid'),
                    Text: tinymce.get('tmceTitle_' + $(d).attr('languageid')).getContent()

                });
            }

        }

        for (let d of $("#dDescriptions").find("div")) {


            if ($(d).attr('languageid') != null) {
                data.Descriptions.push({
                    ID: $(d).attr('rowid'),
                    LangId: $(d).attr('languageid'),
                    Text: tinymce.get('tmceDescription_' + $(d).attr('languageid')).getContent()

                });
            }

        }
        Comm.Post("/nomen/SetSlider", data);
        $("#dSlider").modal('hide');
        //tinymce.EditorManager.editors = [];
        this.LoadSliders();
    }

    public CancelSlider() {
        BasePage.HideErrors();
        //tinymce.EditorManager.editors = [];
        $("#dSlider").modal('hide');
    }

    public SliderChangeActive(element: any) {
        let sliderId = JSON.parse($(element).parent().parent().attr('data')).SliderId;
        var result = Comm.Post("/nomen/SliderChangeActive", { sliderId: sliderId });

        this.LoadSliders();
    }

    public EditSlider(element) {
        $("#cbIsActive").prop("checked", true);

        $("#dTitles").empty();
        $("#dDescriptions").empty();
        if (element == null) {
            this.currentId = -1;
            for (let lang of Comm.Get("/nomen/AllLangs")) {
                $("#dTitles").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='-1'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceTitle_" + lang.LangId + "'/></div>" +
                    "</div>");
                $("#dDescriptions").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='-1'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceDescription_" + lang.LangId + "'/></div>" +
                    "</div>");

            }


        }
        else {
            let obj = JSON.parse($(element).parent().parent().attr("data"));

            $("#cbIsActive").prop("checked", obj.IsActive);
            $("#ddlLogo").val(obj.ImageId);
            this.currentId = obj.SliderId;


            for (let lang of obj.Titles) {
                $("#dTitles").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='" + lang.ID + "'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceTitle_" + lang.LangId + "'>" + lang.Text + "</textarea></div>" +
                    "</div>");
            }

            for (let lang of obj.Descriptions) {
                $("#dDescriptions").append("<div class='row form-group' languageid='" + lang.LangId + "' rowid='" + lang.ID + "'>" +
                    "<div class='col-lg-1'><label class='control-label'>" + lang.LangName + "</label></div>" +
                    "<div class='col-lg-11'><textarea id='tmceDescription_" + lang.LangId + "'>" + lang.Text + "</textarea></div>" +
                    "</div>");

            }

        }
        $("#dSlider").modal('show');
        //tinymce.init({ selector: 'textarea' });
        BasePage.TinyMCE();
        this.RedisplayLogo();
    }

}


