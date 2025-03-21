$(document).ready(function () {
    //VALIDACION POR MONTO 
    $("#valorSolicitado").change(function () {

        //var selectedId = $(this).val();
        var idAuxilio = $("#idAuxilio").val().trim();
        var valorAprobado = $("#valorAprobado").val().trim();

        if (idAuxilio != "") {
            $.ajax({
                url: "/Auxilios/Solicitudes/GetMonto",
                type: "GET",
                data: { auxilio: idAuxilio, valorIngresado: valorAprobado },
                success: function (result) {
                    // Verificar si el resultado es un número válido
                    if (!isNaN(result) && result !== null) {
                        // Verificar si el monto es diferente de cero
                        if (result !== 0) {
                            // Mostrar el div y establecer el contenido de la etiqueta label
                            $("#mostrar_monto").show();
                            $("#monto").text("El monto excede el tope. Monto Máximo: " + formatNumber(result));
                            $("#itemEstado").val("R");
                        } else {
                            // Ocultar el div si el monto es cero
                            $("#mostrar_monto").hide();
                            $("#itemEstado").val("A");
                        }
                    } else {
                        // En caso de que el resultado no sea un número válido, ocultar el div
                        $("#mostrar_monto").hide();
                        $("#itemEstado").val("B");
                    }
                },
                error: function () {
                    console.log("Error al obtener el nombre del afiliado.");
                }
            });
        }

    });

    $("#valorAprobado").change(function () {

        //var selectedId = $(this).val();
        var idAuxilio = $("#idAuxilio").val().trim();
        var valorAprobado = $("#valorAprobado").val().trim();
        if (idAuxilio != "") {
            $.ajax({
                url: "/Auxilios/Solicitudes/GetMonto",
                type: "GET",
                data: { auxilio: idAuxilio, valorIngresado: valorAprobado },
                success: function (result) {
                    // Verificar si el resultado es un número válido
                    if (!isNaN(result) && result !== null) {
                        // Verificar si el monto es diferente de cero
                        if (result !== 0) {
                            // Mostrar el div y establecer el contenido de la etiqueta label
                            $("#mostrar_monto2").show();
                            $("#monto2").text("El monto excede el tope. Monto Máximo: " + formatNumber(result));
                            $("#itemEstado").val("R");
                        } else {
                            // Ocultar el div si el monto es cero
                            $("#mostrar_monto").hide();
                            $("#mostrar_monto2").hide();
                            $("#itemEstado").val("A");
                        }
                    } else {
                        // En caso de que el resultado no sea un número válido, ocultar el div
                        $("#mostrar_monto").hide();
                        $("#mostrar_monto2").hide();
                        $("#itemEstado").val("B");
                    }
                },
                error: function () {
                    console.log("Error al obtener el nombre del afiliado.");
                }
            });
        }

    });

    //VALIDACION POR MONTO 
    $("#idAuxilio").change(function () {

        var selectedId = $(this).val();
        if (selectedId != "") {
            $.ajax({
                url: "/Auxilios/Solicitudes/GetClaseValor",
                type: "GET",
                data: { auxilio: selectedId },
                success: function (result) {
                    if (result == 0) {
                        $("#valorAprobado").prop("readonly", false);
                    } else {
                        $("#valorAprobado").prop("readonly", true);
                    }
                },
                error: function () {
                    console.log("Error al obtener el nombre del afiliado.");
                }
            });

        }
        //    $("#valorAprobado").val("");
    });
    //VALIDACION POR FACTURA
    $("#fechaFactura").change(function () {

        var selectedId = $(this).val();
        var idAfiliado = $("#idAfiliado").val().trim();
        var auxilio = $("#idAuxilio").val().trim();
        var valorSolicitado = $("#valorSolicitado").val().trim();
        var valorAprobado = $("#valorAprobado").val().trim();
        var observaciones = $('#observaciones').val();

        if (auxilio != "" && idAfiliado != "") {
            $.ajax({
                url: "/Auxilios/Solicitudes/GetControlFecha",
                type: "GET",
                data: {
                    fechaFactura: selectedId, idAfiliado: idAfiliado, auxilio: auxilio,
                    valorSolicitado: valorSolicitado, valorAprobado: valorAprobado
                },
                success: function (result) {
                    if (result == 1) {
                        $("#mostrar_validacion").show();
                        $("#validacion").text("El valor excede el Monto del auxilio");
                        $("#itemEstado").val("R");
                    }
                    else if (result == 2) {
                        $("#mostrar_validacion").show();
                        $("#validacion").text("Excede el número de solicitudes por tope ");
                        $("#itemEstado").val("R");
                    }
                    else if (result == 0) {

                        $("#mostrar_validacion").hide();

                        if (observaciones != "") {
                            $("#itemEstado").val("R");
                        } else {
                            $("#itemEstado").val("A");

                        }
                    }
                },
                error: function () {
                    console.log("Error al obtener el nombre del afiliado.");
                }
            });
        }


    });

    function formatNumber(number) {
        // Formatear el número con separadores de miles
        return number.toLocaleString('es-CO', { style: 'currency', currency: 'COP' });
    }


});