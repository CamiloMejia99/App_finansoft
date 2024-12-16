$(document).ready(function () {

    getValoresAuxiliares();


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


    function getValoresAuxiliares() {

        var id = $("#id").val();

        $.ajax({
            url: '/facturacion/Producto/getValoresAuxiliares',
            datatype: "Json",
            data: { id: id },//solo para enviar datos
            type: 'post',
        }).done(function (data) {
            if (data.status == true) {

                $("#auxPrecioSalida").val(data.precioSalida);
                $("#auxPrecioEntrada").val(data.precioEntrada);
                $("#iva").val(data.iva);
            }
            else if (data.status == false) {
                alert("Error");
            }

        });
    }

});