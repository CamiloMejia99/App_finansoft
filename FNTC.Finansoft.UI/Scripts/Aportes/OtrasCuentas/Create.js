$(document).ready(function () {


    $("#AuxPorcentaje").change(function () {
        var porcentaje = $(this).val();
        var expreg = /^[+]?((\d+(\.\d{0,2})?)|(\.\d+))$/;
        var ok = expreg.exec(porcentaje);
        if (!ok) {
            Swal.fire(
                'Valor ingresado no válido',
                'Campo numérico positivo con dos dígitos decimales máximos permitidos',
                'info'
            );
            $(this).val("");
        } else {
            VerificarRango(porcentaje);
        }
    });

    function VerificarRango(porcentaje) {
        var valor = parseFloat(porcentaje);
        if (valor > 100) {
            $("#AuxPorcentaje").val("100");
        }
    }

    $("#Cuenta").change(function () {
        var cuenta = $(this).val();
        if (cuenta != "") {
            $.ajax({
                url: '/Aportes/Aportes/VerificaExisteOtrasCuentas',
                datatype: "Json",
                data: { cuenta: cuenta },//solo para enviar datos
                type: 'post',
            }).done(function (data) {
                if (data.respuesta) {
                    $('#Cuenta> option[value=""]').attr('selected', 'selected');
                    Swal.fire(
                        'Cuenta resitrada',
                        'Esta cuenta ya se ha configurado.',
                        'info'
                    );
                }
            });//fin ajax
        }

    });

    $("#btnRegistrar").click(function () {
        var porcentaje = $("#AuxPorcentaje").val();
        CalcularPorcentaje(porcentaje);
        document.getElementById("btnRegistrar").disabled = true;
    });


    function CalcularPorcentaje(porcentaje) {
        if (porcentaje != "") {
            $.ajax({
                url: '/Aportes/Aportes/CalcularPorcentaje',
                datatype: "Json",
                data: { porcentaje: porcentaje },//solo para enviar datos
                type: 'post',
            }).done(function (data) {
                if (data.respuesta) {
                    $("#theForm").submit();
                } else {
                    document.getElementById("btnRegistrar").disabled = false;
                    Swal.fire({
                        icon: 'warning',
                        title: 'No se puede establecer este porcentaje',
                        text: ''+data.mensaje
                    });
                }
            });

        }
    };
})