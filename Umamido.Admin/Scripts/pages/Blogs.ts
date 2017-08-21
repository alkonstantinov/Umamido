declare var $: any;
declare var tinymce: any;
declare var blogs: Blogs;

class Blogs extends BasePage {

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


    public LoadBlogs() {
        let result = Comm.Get("/nomen/AllBlogs?restaurantId=" + $("#RestaurantId").val());
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
                "<td>" + e.FirstTitle + "</td>" +
                "<td>" + BasePage.ToJavaScriptDate(e.OnDate)+ "</td>" +
                "<td><img src='/nomen/GetImageContentFromDB?imageId=" + e.ImageId + "&GUID=" + BasePage.GUID() + "' alt='' width='100' height='100'/></td>" +
                "<td>" + e.IsActive + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"blogs.EditBlog(this)\"></span>" +
                "<span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" onclick=\"blogs.BlogChangeActive(this)\"></span></td></tr>";

            tbl.append(row);
        }

    }

    public RedisplayLogo() {
        $("#iPreview").attr("src", "/nomen/GetImageContentFromDB?imageId=" + $("#ddlLogo").val() + "&GUID=" + BasePage.GUID());
    }

    public SetBlog() {
        BasePage.HideErrors();
        let err: boolean = false;
        if ($("#ddlLogo").val() == null) {
            $("#lErrddlLogo").show();
            err = true;
        }

        if (!BasePage.IsValidDate($("#tbOnDate").val())) {
            $("#lErrtbOnDate").show();
            err = true;
        }


        if (err)
            return;
        let data = {
            BlogId: this.currentId,
            OnDate: $("#tbOnDate").val(),
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
        Comm.Post("/nomen/SetBlog", data);
        $("#dBlog").modal('hide');
        //tinymce.EditorManager.editors = [];
        this.LoadBlogs();
    }

    public CancelBlog() {
        BasePage.HideErrors();
        //tinymce.EditorManager.editors = [];
        $("#dBlog").modal('hide');
    }

    public BlogChangeActive(element: any) {
        let blogId = JSON.parse($(element).parent().parent().attr('data')).BlogId;
        
        var result = Comm.Post("/nomen/BlogChangeActive", { blogId: blogId });

        this.LoadBlogs();
    }

    public EditBlog(element) {
        $("#cbIsActive").prop("checked", true);

        $("#dTitles").empty();
        $("#dDescriptions").empty();
        if (element == null) {
            this.currentId = -1;
            $("#tbOnDate").val("");
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

            $("#tbOnDate").val(BasePage.ToJavaScriptDate(obj.OnDate));
            $("#cbIsActive").prop("checked", obj.IsActive);
            $("#ddlLogo").val(obj.ImageId);

            this.currentId = obj.BlogId;


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
        $("#dBlog").modal('show');
        //tinymce.init({ selector: 'textarea' });
        BasePage.TinyMCE();
        this.RedisplayLogo();
    }

}


