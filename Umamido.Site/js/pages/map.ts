declare var jQuery: any;
declare var map: Map;

class Map {
    private result = null;

    public PerformCheck() {
        var address = jQuery("#tbAddress").val();
        jQuery("#dOK").hide();
        jQuery("#dNotOK").hide();
        jQuery("#dNotFound").hide();
        jQuery("#tVariants").hide();
        
        var data = { address: address };
        var rslt;
        jQuery.ajax({
            cache: false,
            url: "map/CheckAddress",
            method: "post",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(data),
            async: false,
            success: function (res) {
                rslt = res;

            },
            error: function (a, b, c) {

            }

        });
        this.result = rslt;
        alert(JSON.stringify(this.result));

        if (this.result.length == 0) {
            jQuery("#dNotFound").show();
            return;
        }

        if (this.result.length == 1 && this.result.IsIn) {
            jQuery("#dOK").show();
            return;
        }

        if (this.result.length == 1 && !this.result.IsIn) {
            jQuery("#dNotOK").show();
            return;
        }

        jQuery("#tVariants").show();
        jQuery("#tVariants").empty();

        for (var i = 0; i < this.result.length; i++) {
            jQuery("#tVariants").append("<tr><td><a href='#' onclick='map.SelectAddress(" + i + ")'>" + this.result[i].Address + "</a></td></tr>");
        }
    }

    public SelectAddress(i)
    {
        jQuery("#dOK").hide();
        jQuery("#dNotOK").hide();
        jQuery("#dNotFound").hide();
        jQuery("#tVariants").hide();

        var r = this.result[i];
        jQuery("#tbAddress").val(r.Address);
        if (r.IsIn) {
            jQuery("#dOK").show();
            return;
        }

        if (!r.IsIn) {
            jQuery("#dNotOK").show();
            return;
        }


    }
}