$(document).ready(function () {

    $("#btnAgregar").click(function () {

        var tercero = $("#NitTercero").val();
        var estrato = $("#IdEstrato").val();
        var contrato = $("#IdContrato").val();
        var estudio = $("#IdNivelEstudio").val();
        var personas = $("#PersonasCargo").val();
        var ocupacion = $("#Ocupacion").val();
        var fecha = $("#Fechalaboral").val();
        var salud = $("#IdSalud").val();
        var pension = $("#IdPension").val();
        var arl = $("#IdArl").val();
        var empresa = $("#EmpresaLaboraNit").val();

        if (tercero != "" && estrato != "" && contrato != "" && estudio != "" && personas != "" && ocupacion != "" && fecha != "" && salud != "" && pension!="" && arl!="" && empresa!="") {

            $("#theForm").submit();
          
        } else {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor llene todos los campos!</label>`,
            });
          
        }


    });//fin btnAgregar


    $("#btnEditar").click(function () {

        var tercero = $("#NitTercero").val();
        var estrato = $("#IdEstrato").val();
        var contrato = $("#IdContrato").val();
        var estudio = $("#IdNivelEstudio").val();
        var personas = $("#PersonasCargo").val();
        var ocupacion = $("#Ocupacion").val();
        var fecha = $("#Fechalaboral").val();
        var salud = $("#IdSalud").val();
        var pension = $("#IdPension").val();
        var arl = $("#IdArl").val();
        var empresa = $("#EmpresaLaboraNit").val();

        if (tercero != "" && estrato != "" && contrato != "" && estudio != "" && personas != "" && ocupacion != "" && fecha != "" && salud != "" && pension != "" && arl != "" && empresa!="") {

            $("#theForm").submit();

        } else {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor llene todos los campos!</label>`,
            });
        }


    });//fin btnEditar


    $("#btnAgregarFinanciera").click(function () {

        var tercero = $("#Vinculacion").val();

        if (tercero == "") {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor seleccione un tercero!</label>`,
            });
        }
        else
            if ($("#id_periodo").val() == "") {
                Swal.fire({
                    icon: 'warning',
                    html: `<label class="fuenteSweetAlert2">Por favor seleccione Periodicidad de Pago!</label>`,
                });
            }
        else
            if ($("#codigo_banco").val() == "") {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor seleccione un banco!</label>`,
            });
        }else
        if ($("#tipo_cuenta").val() == "") {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor seleccione un tipo de cuenta!</label>`,
            });
        }
        else {
            var ingresos = $("#IngresosMensuales").val();
            var gastos = $("#GastosMensuales").val();
            var pasivos = $("#PasivosTotales").val();
            var activos = $("#ActivosTotales").val();
            if (ingresos != null) {
                $("#IngresosMensuales").val(quitarPuntosDeMiles(ingresos));
            }
            if (gastos != null) {
                $("#GastosMensuales").val(quitarPuntosDeMiles(gastos));
            }
            if (pasivos != null) {
                $("#PasivosTotales").val(quitarPuntosDeMiles(pasivos));
            }
            if (activos != null) {
                $("#ActivosTotales").val(quitarPuntosDeMiles(activos));
            }

            $("#theForm").submit();

        }
    });//fin btnAgregarFinanciera

    function quitarPuntosDeMiles(numero) {
        // Convierte el número a una cadena y usa replace para quitar los puntos
        return numero.toString().replace(/\./g, '');
    }

   
    $("#btnEditarFinanciera").click(function () {

        var ingresos = $("#IngresosMensuales").val();
        var gastos = $("#GastosMensuales").val();
        var pasivos = $("#PasivosTotales").val();
        var activos = $("#ActivosTotales").val();
        if (ingresos != null) {
            $("#IngresosMensuales").val(quitarPuntosDeMiles(ingresos));
        }
        if (gastos != null) {
            $("#GastosMensuales").val(quitarPuntosDeMiles(gastos));
        }
        if (pasivos != null) {
            $("#PasivosTotales").val(quitarPuntosDeMiles(pasivos));
        }
        if (activos != null) {
            $("#ActivosTotales").val(quitarPuntosDeMiles(activos));
        }

        $("#theForm").submit();


    });//fin btnAgregar


    

   

});