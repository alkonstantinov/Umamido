declare var $: any;
declare var tinymce: any;
declare var restaurants: Restaurants;

class Restaurants extends BasePage {

    currentId: number;

    constructor() {
        super();
    }


    public LoadLogos() {
        let result = Comm.Get("/nomen/AllImages");
        for (let e of result) {
            $("#ddlImage").append("<option value='" + e.ImageId + "'>" + e.ImageName + "</option>");
            $("#ddlLogo").append("<option value='" + e.ImageId + "'>" + e.ImageName + "</option>");
            $("#ddlBigImage").append("<option value='" + e.ImageId + "'>" + e.ImageName + "</option>");
        }

    }

    public ShowGoods(restaurantId) {
        window.location.href = "/nomen/goods?restaurantId=" + restaurantId;
    }

    public LoadRestaurants() {
        let result = Comm.Get("/nomen/AllRestaurants");
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
                "<td><img src='/nomen/GetImageContentFromDB?imageId=" + e.ImageId + "&GUID=" + BasePage.GUID() + "' alt='' width='100' height='100'/></td>" +
                "<td>" + e.IsActive + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"restaurants.EditRestaurant(this)\"></span>" +
                "<span class=\"glyphicon glyphicon-gift\" aria-hidden=\"true\" onclick=\"restaurants.ShowGoods(" + e.RestaurantId + ")\"></span>" +
                "<span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" onclick=\"restaurants.RestaurantChangeActive(this)\"></span></td></tr>";

            tbl.append(row);
        }

    }

    public RedisplayBigImage() {
        $("#iBigImagePreview").attr("src", "/nomen/GetImageContentFromDB?imageId=" + $("#ddlBigImage").val() + "&GUID=" + BasePage.GUID());
    }

    public RedisplayImage() {
        $("#iImagePreview").attr("src", "/nomen/GetImageContentFromDB?imageId=" + $("#ddlImage").val() + "&GUID=" + BasePage.GUID());
    }

    public RedisplayLogo() {
        $("#iLogoPreview").attr("src", "/nomen/GetImageContentFromDB?imageId=" + $("#ddlLogo").val() + "&GUID=" + BasePage.GUID());
    }

    public SetRestaurant() {
        BasePage.HideErrors();
        let err: boolean = false;
        if ($("#ddlLogo").val() == null) {
            $("#lErrddlLogo").show();
            err = true;
        }
        if ($("#ddlImage").val() == null) {
            $("#lErrddlImage").show();
            err = true;
        }
        if ($("#ddlBigImage").val() == null) {
            $("#lErrddlBigImage").show();
            err = true;
        }


        if (err)
            return;
        let data = {
            RestaurantId: this.currentId,
            ImageId: $("#ddlImage").val(),
            BigImageId: $("#ddlBigImage").val(),
            LogoImageId: $("#ddlLogo").val(),
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
        Comm.Post("/nomen/SetRestaurant", data);
        $("#dRestaurant").modal('hide');
        //tinymce.EditorManager.editors = [];
        this.LoadRestaurants();
    }

    public CancelRestaurant() {
        BasePage.HideErrors();
        //tinymce.EditorManager.editors = [];
        $("#dRestaurant").modal('hide');
    }

    public RestaurantChangeActive(element: any) {
        let restaurantId = JSON.parse($(element).parent().parent().attr('data')).RestaurantId;
        var result = Comm.Post("/nomen/RestaurantChangeActive", { restaurantId: restaurantId });

        this.LoadRestaurants();
    }

    public EditRestaurant(element) {
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
            $("#ddlImage").val(obj.ImageId);
            $("#ddlLogo").val(obj.LogoImageId);
            $("#ddlBigImage").val(obj.BigImageId);
            this.currentId = obj.RestaurantId;


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
        $("#dRestaurant").modal('show');
        //tinymce.init({ selector: 'textarea' });
        BasePage.TinyMCE();
        this.RedisplayLogo();
        this.RedisplayBigImage();
        this.RedisplayImage();
    }

}


