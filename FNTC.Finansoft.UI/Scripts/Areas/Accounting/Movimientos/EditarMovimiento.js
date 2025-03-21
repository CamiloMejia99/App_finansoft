$(document).ready(function () {

    var datosCuentas = [];
    $("#Codigo_comprobante").change(function () {

        var selectedId = $(this).val();
        $.ajax({
            url: "/Accounting/NuevoMovimiento/GetDatosComprobante",
            type: "GET",
            data: { codigo: selectedId },
            success: function (result) {
                console.log(result)
                if (result) {
                    $("#consecutivo").val(result.consecutivo);
                    $("#forma_pago").val(result.forma_pago);
                }

            },
            error: function () {
                console.log("Error al obtener los datos del comprobante");
            }
        });
    });



    $("#agregar_cuenta").click(function () {
        var validacionExitosa = true;

        // Validar cada fila agregada
        $("#TB_nuevo_comprobante tbody tr").each(function () {
            var cuenta = $(this).find('select[name="cuenta"]').val();
            var tercero = $(this).find('select[name="tercero"]').val();
            var ccosto = $(this).find('select[name="ccosto"]').val();
            var debito = $(this).find('input[name="debito"]').val();
            var credito = $(this).find('input[name="credito"]').val();

            // Verificar si algún campo está vacío
            if (cuenta === '' || tercero === '' || ccosto === '' || debito === '' || credito === '') {
                validacionExitosa = false;
                return false; // Salir del bucle each si encontramos algún campo vacío
            }
        });

        // Si la validación no es exitosa, mostrar un mensaje de error
        if (!validacionExitosa) {
            mostrarAlerta('Por favor, complete todos los campos en la fila antes de agregar otra cuenta.');
            return; // Detener la ejecución de la función si la validación no es exitosa
        }

        // Obtener el HTML del dropdown desde el contenedor
        var dropdownHtml = $("#dropdownHtmlContainer").find(".chosen-select-single").html();
        var lista_terceros = $("#lista_terceros").find(".chosen-select-single").html();
        var centrocosto = $("#centro_costos").find(".chosen-select-single").html();

        // Agregar los inputs dinámicamente
        var nuevaFila = '<tr class="margensuperior">' +
            '<td>' +
            '<select class="form-control select2" name="cuenta">' + dropdownHtml + '</select>' +
            '</td>' +
            '<td>' +
            '<select class="form-control select2" name="tercero">' + lista_terceros + '</select>' +
            '</td>' +
            '<td>' +
            '<select class="form-control select2" name="ccosto">' + centrocosto + '</select>' +
            '</td>' +
            '<td>' +
            '<input class="form-control numerofactura"  />' +
            '</td>' +
            '<td>' +
            '<input class="form-control base"  />' +
            '</td>' +
            '<td>' +
            '<input class="form-control debito" name="debito" />' +
            '</td>' +
            '<td>' +
            '<input class="form-control credito" name="credito" />' +
            '</td>' +
            '<td>' +
            '<button quitar_cuenta type="button" class="btn btn-danger"><i class="fa fa-minus-circle"></i></button>' +
            '</td>' +
            '</tr>';

        // Agregar la nueva fila a la tabla
        $("#TB_nuevo_comprobante tbody").append(nuevaFila);

        // Inicializar Chosen en el nuevo dropdown
        $(".select2").select2();
        // Inicializar el cambio de evento para el nuevo select

        // Agregar separador de miles a los campos de débito y crédito
        $(".debito, .credito").on({
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
    });



    $("#TB_nuevo_comprobante").on("change", ".debito, .credito", function () {
        // Calcular la suma total de débitos y créditos
        var totalDebito = 0;
        var totalCredito = 0;

        // Recorrer cada fila de la tabla
        $("#TB_nuevo_comprobante tbody tr").each(function () {
            // Obtener los valores de débito y crédito de la fila actual
            var debitoInput = $(this).find(".debito");
            var creditoInput = $(this).find(".credito");


            // Quitar el separador de miles y convertir el valor a número
            var debito = parseFloat(debitoInput.val().replace(/\./g, '')) || 0;
            var credito = parseFloat(creditoInput.val().replace(/\./g, '')) || 0;

            // Actualizar los valores a las sumas totales
            totalDebito += debito;
            totalCredito += credito;
        });

        // Actualizar los valores en los campos de entrada de la segunda tabla
        $("#debito").val(agregarSeparadorMiles(totalDebito));
        $("#credito").val(agregarSeparadorMiles(totalCredito));
    });


    $("#btnNuevoMovimiento").click(function () {
        var todasFilasCompletas = true
        var validar = ValidarIngresos();


        // Resto del código para la validación de suma igual y envío del formulario
        var comprobante = $("#Codigo_comprobante").val();
        var detalle = $("#Detalle").val();
        var debito = $("#debito").val();
        var credito = $("#credito").val();

        if (comprobante === "" || detalle === "") {
            mostrarAlerta("Los campos comprobante y detalle son obligatorios.");
        } else if (debito !== credito) {
            mostrarAlerta("La sumatoria de los débitos y créditos deben coincidir.");
        }
        else if (validar == 0) {
            mostrarAlerta('Agregue mínimo  dos cuentas para realizar el movimiento, o diligencie los campos ');
        } else if (validar == 1) {
            mostrarAlerta("Se necesita mínimo un crédito y un débito para realizar el movimiento.");
        }
        else {
            // Recorrer cada fila de la tabla
            $("#TB_nuevo_comprobante tbody tr").each(function () {
                var fila = $(this);
                // Obtener los valores de los campos de la fila
                var cuenta = fila.find("select:eq(0)").val();
                var tercero = fila.find("select:eq(1)").val();
                var centrocosto = fila.find("select:eq(2)").val();
                var numerofactura = fila.find("input.numerofactura").val();
                var base = fila.find("input.base").val();
                var debito = fila.find("input.debito").val();
                var credito = fila.find("input.credito").val();

                if (cuenta === "" || tercero === "" || centrocosto === "" || debito === "" || credito === "") {
                    mostrarAlerta('Por favor, complete todos los campos en la fila.');
                    // Salir del bucle each si alguna fila no está completa
                    return false;
                }
                console.log(validar)
                if (validar > 1) {
                    // Crear un nuevo objeto con los datos de la fila
                    var datosFila = crearObjetoFila(cuenta, tercero, numerofactura, centrocosto, base, debito.replace(/\./g, ''), credito.replace(/\./g, ''));
                    // Agregar el nuevo objeto al array
                    datosCuentas.push(datosFila);
                }

            });

            // Convertir el array a JSON y asignarlo a un campo oculto en el formulario
            $("#facturasSeleccionadas").val(JSON.stringify(datosCuentas));

            // Si alguna fila no está completa, mostrar un mensaje de alerta
            if (!todasFilasCompletas) {
                mostrarAlerta("Por favor, complete todos los campos en todas las filas antes de continuar.");
                return; // Detener el envío del formulario
            }
            // Enviar el formulario
            $("#FormMovimiento").submit();
        }

    });





    //ELIMINAR UNA FILA 
    $("#contenedorCuentas").on("click", "[quitar_cuenta]", function () {
        // Eliminar la fila correspondiente al botón clickeado
        $(this).closest('tr').remove();

        // Recalcular los valores de débito y crédito finales
        var totalDebito = 0;
        var totalCredito = 0;

        // Iterar sobre todas las filas y sumar los valores de débito y crédito
        $("#TB_nuevo_comprobante tbody tr").each(function () {
            var debito = parseFloat($(this).find(".debito").val().replace(/\./g, "").replace(",", "."));
            var credito = parseFloat($(this).find(".credito").val().replace(/\./g, "").replace(",", "."));

            if (!isNaN(debito)) {
                totalDebito += debito;
            }

            if (!isNaN(credito)) {
                totalCredito += credito;
            }
        });

        // Actualizar los campos de débito y crédito finales en la interfaz de usuario
        $("#debito").val(agregarSeparadorMiles(totalDebito));
        $("#credito").val(agregarSeparadorMiles(totalCredito));
    });

    //eliminar todas las filas DESCARTAR EL MOVIMIENTO
    $("#descartar").click(function () {
        // Eliminar todas las filas del cuerpo de la tabla
        $("#TB_nuevo_comprobante tbody").empty();
        var debito = 0;
        var credito = 0;
        $("#debito").val(debito);
        $("#credito").val(credito);
    });

    function crearObjetoFila(cuenta, tercero, numerofactura, centrocosto, base, debito, credito) {
        return {
            cuenta,
            tercero,
            numerofactura,
            centrocosto,
            base,
            debito,
            credito,
        };
    }

    function mostrarAlerta(mensaje) {
        Swal.fire({
            icon: 'warning',
            html: '<label class="fuenteSweetAlert2">' + mensaje + '</label>',
        });
    }

    function agregarSeparadorMiles(numero) {
        // Convertir el número a cadena
        let numeroStr = numero.toString();

        // Dividir la cadena en dos partes: la parte entera y la parte decimal
        let partes = numeroStr.split(".");

        // Formatear la parte entera con separador de miles
        let parteEntera = partes[0].replace(/(\d)(?=(\d{3})+$)/g, "$1.");

        // Volver a unir el valor con la parte decimal (si existe)
        let valorFormateado = parteEntera;
        if (partes.length > 1) {
            valorFormateado += "," + partes[1]; // Cambiamos el punto por una coma
        }

        // Retornar el valor formateado
        return valorFormateado;
    }


    function ValidarIngresos() {
        var validarIngresos = 0;
        $("#TB_nuevo_comprobante tbody tr").each(function () {
            var fila = $(this);
            // Obtener los valores de los campos de la fila
            var cuenta = fila.find("select:eq(0)").val();
            var tercero = fila.find("select:eq(1)").val();
            var centrocosto = fila.find("select:eq(2)").val();
            var numerofactura = fila.find("input.numerofactura").val();
            var base = fila.find("input.base").val();
            var debito = fila.find("input.debito").val();
            var credito = fila.find("input.credito").val();
            // Incrementar el contador solo si la fila está completa
            validarIngresos++;

            if (cuenta === "" || tercero === "" || centrocosto === "" || debito === "" || credito === "") {
                validarIngresos = 0;

                return validarIngresos
            }
        });

        return validarIngresos
    }

});