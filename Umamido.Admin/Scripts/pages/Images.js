var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var Images = (function (_super) {
    __extends(Images, _super);
    function Images() {
        return _super.call(this) || this;
    }
    Images.prototype.LoadImages = function () {
        var result = Comm.Get("/nomen/AllImages");
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
        var tbl = $("#tItems tbody");
        tbl.empty();
        $(result).each(function (i, e) {
            var row = "<tr recordId='" + e.ImageId + "'>" +
                "<td>" + e.ImageName + "</td>" +
                "<td><img src='/nomen/GetImageContentFromDB?imageId=" + e.ImageId + "&GUID=" + BasePage.GUID() + "' alt='' width='100' height='100'/></td>" +
                "<td>" + e.IsActive + "</td>" +
                "<td><span class=\"glyphicon glyphicon-edit\" aria-hidden=\"true\" onclick=\"imgs.EditImage(this)\"></span><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\" onclick=\"imgs.ImageChangeActive(this)\"></span></td></tr>";
            tbl.append(row);
        });
    };
    Images.prototype.ImageChangeActive = function (element) {
        var imageId = $(element).parent().parent().attr('recordId');
        var result = Comm.Post("/nomen/ImageChangeActive", { imageId: imageId });
        this.LoadImages();
    };
    Images.prototype.EditImage = function (element) {
        if (element == null) {
            this.currentId = -1;
            $("#tbImageName").val("");
            $("#hFileGuid").val("");
            $("#cbIsActive").prop("checked", true);
            $("#iPreview").prop("src", "");
        }
        else {
            var id = $(element).parent().parent().attr("recordid");
            var obj = Comm.Post("/nomen/GetImage", { imageId: id });
            this.currentId = obj.ImageId;
            $("#tbImageName").val(obj.ImageName);
            $("#cbIsActive").prop("checked", obj.IsActive);
            $("#hFileGuid").val("");
            $("#iPreview").prop("src", "/nomen/GetImageContentFromDB?imageId=" + this.currentId);
        }
        $("#dImage").modal('show');
    };
    Images.prototype.AddPicture = function () {
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
    };
    Images.prototype.SetImage = function () {
        BasePage.HideErrors();
        var err = false;
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
        var rslt = Comm.Post("/nomen/SetImage", {
            ImageId: this.currentId,
            ImageName: $("#tbImageName").val(),
            IsActive: $("#cbIsActive").prop("checked"),
            Guid: $("#hFileGuid").val()
        });
        $("#dImage").modal('hide');
        this.LoadImages();
    };
    Images.prototype.CancelImage = function () {
        BasePage.HideErrors();
        $("#dImage").modal('hide');
    };
    return Images;
}(BasePage));
//# sourceMappingURL=Images.js.map