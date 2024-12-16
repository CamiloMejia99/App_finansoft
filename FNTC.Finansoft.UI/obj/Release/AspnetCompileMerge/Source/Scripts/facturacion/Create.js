$(document).ready(function () {

    $("#precioEntrada").val("0");
    $("#precioSalida").val("0");


    $("#btnGuardar").click(function () {
        $("#theForm").submit();
    });



    $("#auxPrecioSalida").on({
        "focus": function (event) {
            $(event.target).select();
        },
        "keyup": function (event) {
            $(event.target).val(function (index, value) {
                return value.replace(/\D/g, "")
                    .replace(/([0-9])([0-9]{3})$/, '$1.$2')
                    .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ".");
            });
        }
    });
    $("#auxPrecioEntrada").on({
        "focus": function (event) {
            $(event.target).select();
        },
        "keyup": function (event) {
            $(event.target).val(function (index, value) {
                return value.replace(/\D/g, "")
                    .replace(/([0-9])([0-9]{3})$/, '$1.$2')
                    .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ".");
            });
        }
    });




    $("#btnCerrar").click(function () {
        window.location.href = '/facturacion/Producto/Index';
    });

});