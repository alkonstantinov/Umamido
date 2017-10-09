declare var jQuery: any;
declare var addresses: Addresses;

class Addresses {
    public static SaveAddress(addrnum) {
        var data = {
            AddressNum: addrnum,
            Name: jQuery("#tbName" + addrnum).val(),
            Family: jQuery("#tbFamily" + addrnum).val(),
            Address: jQuery("#tbAddress" + addrnum).val(),
            Phone: jQuery("#tbPhone" + addrnum).val()

        }

        var result;
        jQuery.ajax({
            cache: false,
            url: "/home/SetProfileAddress",
            method: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify(data),
            async: false,
            success: function (res) {
                result = res;
            },
            error: function (a, b, c) {

            }
        });

        if (result.ErrorMessage != null) {
            jQuery("#dError_" + addrnum).show();
            jQuery("#dError_" + addrnum).empty();
            jQuery("#dError_" + addrnum).append("<p>" + result.ErrorMessage + "</p>");
        }
        else
            jQuery("#dError_" + addrnum).hide();

        if (result.AddressVariants != null && result.AddressVariants.length > 1) {
            jQuery("#pSelectAddress_" + addrnum).show();
            jQuery("#dAddresses_" + addrnum).empty();
            for (var element of result.AddressVariants) {
                var s = encodeURI(element);
                jQuery("#dAddresses_" + addrnum).append('<p><input type="radio" name="ac" style="width:auto !important;" onclick="$(\'#tbAddress' + addrnum + '\').val(decodeURI(\'' + s + '\'))">' + element + '</p>');
            }
        }
        else
            jQuery("#pSelectAddress_" + addrnum).hide();

    }
}