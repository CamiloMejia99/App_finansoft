$(document).ready(function () {

    $("#btnConfigurar").click(function () {
        $.ajax({
            url: '/facturacion/FacConfiguracion/esCreateOEdit',
            datatype: "Json",
            type: 'post',
        }).done(function (data) {
            if (data.status == true) {

                window.location.href = '/facturacion/FacConfiguracion/Edit';
            }
            else if (data.status == false) {
                window.location.href = '/facturacion/FacConfiguracion/Create';
            }

        });

    });


});