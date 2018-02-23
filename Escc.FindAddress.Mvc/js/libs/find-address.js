if (typeof (jQuery) !== 'undefined') {
    jQuery(function ($) {
        function SetManualAddressAreaVisible(visible, context) {
            if (visible == true) $('div.manual-address-area', context).show();
            else $('div.manual-address-area', context).hide();
        }

        function SetSelectAddressAreaVisible(visible, context) {
            if (visible == true) $('div.select-address-area', context).show();
            else $('div.select-address-area', context).hide();
        }

        function SetManualButtonAreaVisible(visible, context) {
            if (visible == true) $('span.manual-button-area', context).show();
            else $('span.manual-button-area', context).hide();
        }

        function SelectAddressErrorVisible(visible, context, message) {
            if (visible == true) $('span.select-address-error', context).show();
            else $('span.select-address-error', context).hide();

            $('span.select-address-error', context).text(message)
        }

        function HasManualAddressData(context) {
            var paon = $('input.paon', context).val();
            var saon = $('input.saon', context).val();
            var locality = $('input.locality', context).val();
            var streetName = $('input.street', context).val();
            var town = $('input.town', context).val();
            var adminArea = $('input.administrative-area', context).val();

            var emptyManualAddressData = paon == "" && saon == "" && locality == "" && streetName == "" && town == "" && adminArea == "";

            return !emptyManualAddressData;
        }

        $("div.find-address-container").each(function () {
            var findAddress = $(this);

            $('input.find-address', findAddress).attr("style", "display: inline");
            $('input.type-address', findAddress).attr("style", "display: inline");

            SetManualAddressAreaVisible(HasManualAddressData(findAddress), findAddress);
            SetSelectAddressAreaVisible(false, findAddress);
            SetManualButtonAreaVisible(true, findAddress);
            SelectAddressErrorVisible(false, findAddress, '');

            $('input.find-address', findAddress).on('click', function () {

                SetManualAddressAreaVisible(false, findAddress);
                SetSelectAddressAreaVisible(false, findAddress);
                SetManualButtonAreaVisible(true, findAddress);
                SelectAddressErrorVisible(false, findAddress, '');

                $.ajax({
                    url: $("input.find-address", findAddress).data("button-url"),
                    type: 'GET',
                    data: { postCode: $('input.postcode', findAddress).val() },
                    dataType: 'json',
                    cache: false,
                    success: function (response) {
                        if (response != null && response.success) {
                            SetSelectAddressAreaVisible(true, findAddress);

                            if (response.data != null) {
                                $('select.possible-addresses', findAddress)
                                    .find('option')
                                    .remove();

                                $.each(response.data, function (i, obj) {
                                    $('select.possible-addresses', findAddress).append(
                                        $('<option></option>')
                                            .val(obj['Key'])
                                            .html(obj['Value']));
                                });
                            }
                        } else {
                            SelectAddressErrorVisible(true, findAddress, response.responseText);
                        }
                    },
                    error: function (response) {
                        SelectAddressErrorVisible(true, findAddress, "Error communicating with server");
                    }
                });
            });

            $('input.type-address', findAddress).on('click', function () {
                SetManualAddressAreaVisible(true, findAddress);
                SetSelectAddressAreaVisible(false, findAddress);
                SetManualButtonAreaVisible(false, findAddress);
                SelectAddressErrorVisible(false, findAddress, '');
            });

            $('input.confirm-address', findAddress).on('click', function () {
                SelectAddressErrorVisible(false, findAddress, '');

                $.ajax({
                    url: $('input.confirm-address', findAddress).data("button-url"),
                    type: 'GET',
                    data: { postCode: $('input.postcode', findAddress).val(), uprn: $('select.possible-addresses option:selected', findAddress).val() },
                    dataType: 'json',
                    cache: false,
                    success: function (response) {
                        if (response != null && response.success) {
                            SetManualAddressAreaVisible(true, findAddress);
                            SetSelectAddressAreaVisible(false, findAddress);
                            SetManualButtonAreaVisible(false, findAddress);

                            if (response.data != null) {
                                var bs7666Address = response.data['BS7666Address']
                                $('input[name$="Id"]', findAddress).val(bs7666Address['Id']);
                                $('input[name$="Uprn"]', findAddress).val(bs7666Address['Uprn']);
                                $('input[name$="Usrn"]', findAddress).val(bs7666Address['Usrn']);
                                $('input[name$="GridEasting"]', findAddress).val(bs7666Address['GridEasting']);
                                $('input[name$="GridNorthing"]', findAddress).val(bs7666Address['GridNorthing']);
                                $('input.paon', findAddress).val(bs7666Address['Paon']);
                                $('input.saon', findAddress).val(bs7666Address['Saon']);
                                $('input.locality', findAddress).val(bs7666Address['Locality']);
                                $('input.street', findAddress).val(bs7666Address['StreetName']);
                                $('input.town', findAddress).val(bs7666Address['Town']);
                                $('input.administrative-area', findAddress).val(bs7666Address['AdministrativeArea']);
                                $('input.postcode', findAddress).val(bs7666Address['Postcode']);
                            }
                        } else {
                            SelectAddressErrorVisible(true, findAddress, response.responseText);
                        }
                    },
                    error: function (response) {
                        SelectAddressErrorVisible(true, findAddress, "Error communicating with server");
                    }
                });
            });
        });
    });
}