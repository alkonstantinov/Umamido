declare var $: any;
declare var imgs: Images;

class Images extends BasePage {

    currentId: number;

    constructor() {
        super();
    }



    public LoadImages() {
        let result = Comm.Get("/nomen/AllImages");
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
            let row: String = "<tr recordId='" + e.ImageId + "'>" +
                "<td>" + e.ImageName + "</td>" +
                "<td><img src='/nomen/GetImageContentFromDB?imageId=" + e.ImageId + "&GUID=" + BasePage.GUID() + "' alt='' width='100' height='100'/></td>" +
                "<td>" + e.IsActive + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"imgs.EditImage(this)\"></span><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" onclick=\"imgs.ImageChangeActive(this)\"></span></td></tr>";

            tbl.append(row);
        });

    }


    public ImageChangeActive(element: any) {
        let imageId: number = $(element).parent().parent().attr('recordId');
        var result = Comm.Post("/nomen/ImageChangeActive", { imageId: imageId });

        this.LoadImages();
    }


    public EditImage(element) {
        if (element == null) {
            this.currentId = -1;
            $("#tbImageName").val("");
            $("#hFileGuid").val("");
            $("#cbIsActive").prop("checked", true);
            $("#iPreview").prop("src", "");

        }
        else {
            let id = $(element).parent().parent().attr("recordid");

            let obj = Comm.Post("/nomen/GetImage", { imageId: id });
            this.currentId = obj.ImageId;
            $("#tbImageName").val(obj.ImageName);
            $("#cbIsActive").prop("checked", obj.IsActive);
            $("#hFileGuid").val("");
            $("#iPreview").prop("src", "/nomen/GetImageContentFromDB?imageId=" + this.currentId);
        }
        $("#dImage").modal('show');
    }

    public AddPicture() {
        $("#hFileGuid").val(BasePage.GUID());
        var file = $("#fuFile")[0].files[0]; //Files[0] = 1st file
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function (event) {
            Comm.Post("/nomen/StoreImageInCache", {
                guid: $("#hFileGuid").val(),
                content: reader.result,
                filename: $("#fuFile").val()

            });
            $("#iPreview").prop("src", "/nomen/GetImageContentFromCache?guid=" + $("#hFileGuid").val());    
        };


        
    }

    public SetImage() {
        BasePage.HideErrors();
        let err: boolean = false;
        if ($("#tbImageName").val() == "") {
            $("#lErrtbImageName").show();
            err = true;
        }
        if ($("#hFileGuid").val() == "" && this.currentId == -1) {
            $("#lErrfuFile").show();
            err = true;
        }


        if (err)
            return;
        let rslt = Comm.Post("/nomen/SetImage", {
            ImageId: this.currentId,
            ImageName: $("#tbImageName").val(),
            IsActive: $("#cbIsActive").prop("checked"),
            Guid: $("#hFileGuid").val()
        });

        $("#dImage").modal('hide');
        
        this.LoadImages();
    }

    public CancelImage() {
        BasePage.HideErrors();
        $("#dImage").modal('hide');
    }





}


