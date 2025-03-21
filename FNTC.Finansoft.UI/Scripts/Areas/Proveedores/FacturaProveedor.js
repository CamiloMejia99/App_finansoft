$(document).ready(function () {

    var sumatoria_final = 0.0;
    var descuentoReteFuente = 0.0;
    var descuentoReteIca = 0.0;
    var descuentocruce = 0.0;


    var facturasSeleccionadas = [];

    $("#btnFactura").click(function () {

        var cuentaIca = $("#cuenta_retefuente").val().trim();
        var cuentaFuente = $("#cuenta_reteica").val().trim();
        var porcentje1 = $("#Porcentaje_rete_fuente").val().trim();
        var porcentje2 = $("#Porcentaje_rete_ica").val().trim();
        var checkbox1 = $("#chkReteFuente");
        var checkbox2 = $("#chkReteica");
        var validar1 = false;
        var validar2 = false;

        if (checkbox1.is(":checked") && (cuentaIca == "" || porcentje1 == "" || isNaN(porcentje1))) {
            mostrarAlerta('Por favor llene los campos de RETEFUENTE!');
            validar1 = true
        }
        if (checkbox2.is(":checked") && (cuentaFuente == "" || porcentje2 == "" || isNaN(porcentje2))) {
            mostrarAlerta('Por favor llene los campos de RETEICA!');
            validar2 = true
        }
        if (!validar1 && !validar2) {
            var numerosFactura = facturasSeleccionadas.map(item => item.numFactura);
            $("#facturasSeleccionadas").val(numerosFactura.join(','));


            var valor_aprobado = $("#Valor_aprobado").val().trim();
            valor_aprobado = valor_aprobado.replace(/\./g, "");
            $("#Valor_aprobado").val(valor_aprobado);

            var valor_solicitado = $("#Valor_solicitado").val().trim();
            valor_solicitado = valor_solicitado.replace(/\./g, "");
            $("#Valor_solicitado").val(valor_solicitado);

            var valor_retefuente = $("#Base_retefuente").val().trim();
            valor_retefuente = valor_retefuente.replace(/\./g, "");
            $("#Base_retefuente").val(valor_retefuente);

            var valor_reteica = $("#Base_reteica").val().trim();
            valor_reteica = valor_reteica.replace(/\./g, "");
            $("#Base_reteica").val(valor_reteica);

            $("#FormFactura").submit();
        }
    });

    function mostrarAlerta(mensaje) {
        Swal.fire({
            icon: 'warning',
            html: '<label class="fuenteSweetAlert2">' + mensaje + '</label>',
        });
    }

    $("#Num_factura").change(function () {

        var selectedId = $(this).val();
        $.ajax({
            url: "/Proveedor/FacturaProveedor/ValidarFactura",
            type: "GET",
            data: { factura: selectedId, nitProveedor: $('#Fk_nit_proveedor').val() },
            success: function (result) {
                // Verificar si la factura existe
                if (result) {
                    mostrarAlerta('El número de factura ya está registrado');
                }
            },
            error: function () {
                console.log("Error al obtener el nombre del afiliado.");
            }
        });


    });

    $("#Valor_aprobado").change(function () {

        var selectedId = $(this).val();

        selectedId = selectedId.replace(/\./g, "");

        sumatoria_final = parseFloat(selectedId) - (descuentoReteFuente + descuentoReteIca + descuentocruce);


        $("#valor_final").val(agregarSeparadorMiles(sumatoria_final));

    });

    $("#Base_retefuente").change(function () {

        var selectedId = $(this).val();

        selectedId = selectedId.replace(/\./g, "");
        var valor_aprobado = $("#Valor_aprobado").val().trim();
        valor_aprobado = valor_aprobado.replace(/\./g, "");
        var porcentaje = $("#Porcentaje_rete_fuente").val();
        if (porcentaje != "") {
            descuentoReteFuente = parseFloat((porcentaje / 100) * selectedId);
            var sumaValores = 0;
            $("#valorRetefuente").val(agregarSeparadorMiles(descuentoReteFuente));
            // Recorrer el array y sumar los valores
            for (var i = 0; i < facturasSeleccionadas.length; i++) {
                sumaValores += facturasSeleccionadas[i].valor;
            }
            sumatoria_final = parseFloat(valor_aprobado) - (descuentoReteFuente + descuentoReteIca + sumaValores);
            $("#valor_final").val(agregarSeparadorMiles(sumatoria_final));
        }
        
    });

    $("#Base_reteica").change(function () {

        var selectedId = $(this).val();

        selectedId = selectedId.replace(/\./g, "");
        var valor_aprobado = $("#Valor_aprobado").val().trim();
        valor_aprobado = valor_aprobado.replace(/\./g, "");
        var porcentaje = $("#Porcentaje_rete_ica").val();
        if (porcentaje != "") {
            descuentoReteIca = parseFloat((porcentaje / 1000) * selectedId);
            var sumaValores = 0;
            $("#valorReteica").val(agregarSeparadorMiles(descuentoReteIca));
            // Recorrer el array y sumar los valores
            for (var i = 0; i < facturasSeleccionadas.length; i++) {
                sumaValores += facturasSeleccionadas[i].valor;
            }
            sumatoria_final = parseFloat(valor_aprobado) - (descuentoReteFuente + descuentoReteIca + sumaValores);
            $("#valor_final").val(agregarSeparadorMiles(sumatoria_final));
        }

    });

    //OBTENER EL NOMBRE DEL PROVEEDOR
    $("#Fk_nit_proveedor").change(function () {

        var selectedId = $(this).val();


        $.ajax({
            url: "/Proveedor/ClaseProveedor/GetNombreProveedor",
            type: "GET",
            data: { idProveedor: selectedId },
            success: function (result) {
                $("#NombreProveedor").text(result);
                $("#alertContainer").show();
            },
            error: function () {
                console.log("Error al obtener el nombre del afiliado.");
            }
        });
    });

    $("#cuenta_retefuente").change(function () {

       var selectedId = $(this).val();
        var valor_base = $("#Base_retefuente").val().trim();
        valor_base = valor_base.replace(/\./g, "");
        var valor_aprobado = $("#Valor_aprobado").val().trim();
        valor_aprobado = valor_aprobado.replace(/\./g, "");

        $.ajax({
            url: "/Proveedor/FacturaProveedor/GetPorcentajeCuenta",
            type: "GET",
            data: { cuenta: selectedId },
            success: function (result) {
                $("#Porcentaje_rete_fuente").val(result);
                descuentoReteFuente = parseFloat((result / 100) * valor_base);
           
                var sumaValores = 0;
                descuentoReteFuente = parseFloat(descuentoReteFuente.toFixed(2));

                $("#valorRetefuente").val(agregarSeparadorMiles(descuentoReteFuente));
                // Recorrer el array y sumar los valores
                for (var i = 0; i < facturasSeleccionadas.length; i++) {
                    sumaValores += facturasSeleccionadas[i].valor;
                }
                sumatoria_final = parseFloat(valor_aprobado) - (descuentoReteFuente + descuentoReteIca + sumaValores);
                $("#valor_final").val(agregarSeparadorMiles(sumatoria_final));            
            },
            error: function () {
                console.log("Error al obtener el nombre del afiliado.");
            }
        });
    });

    $("#cuenta_reteica").change(function () {

        var selectedId = $(this).val();
        var valor_base = $("#Base_reteica").val().trim();
        valor_base = valor_base.replace(/\./g, "");

        var valor_aprobado = $("#Valor_aprobado").val().trim();
        valor_aprobado = valor_aprobado.replace(/\./g, "");

        $.ajax({
            url: "/Proveedor/FacturaProveedor/GetPorcentajeReteica",
            type: "GET",
            data: { cuenta: selectedId },
            success: function (result) {
                $("#Porcentaje_rete_ica").val(result);

                descuentoReteIca = parseFloat((result / 1000) * valor_base);
                var sumaValores = 0;

                descuentoReteIca = parseFloat(descuentoReteIca.toFixed(2));
                $("#valorReteica").val(agregarSeparadorMiles(descuentoReteIca));
                // Recorrer el array y sumar los valores
                for (var i = 0; i < facturasSeleccionadas.length; i++) {
                    sumaValores += facturasSeleccionadas[i].valor;
                }
                sumatoria_final = parseFloat(valor_aprobado) - (descuentoReteFuente + descuentoReteIca + sumaValores);
                $("#valor_final").val(agregarSeparadorMiles(sumatoria_final));  
            },
            error: function () {
                console.log("Error al obtener el nombre del afiliado.");
            }
        });
    });

    $("#Fk_Clase_Proveedor").change(function () {

        var selectedId = $(this).val();
        $.ajax({
            url: "/Proveedor/FacturaProveedor/GetSaldoConcepto",
            type: "GET",
            data: { concepto: selectedId },
            success: function (result) {
                $("#Valor_solicitado").val(agregarSeparadorMiles(result));
            },
            error: function () {
                console.log("Error al obtener el saldo del concepto.");
            }
        });
    });

    $('#chkReteFuente').change(gestionarReteFuente);
    $('#chkReteica').change(gestionarReteIca);
    $('#chkCruce').change(gestionarCruces);

    function gestionarReteFuente() {
        var chkReteFuente = $('#chkReteFuente');
        var porcentaje = $('#mostrar_porcentaje_retefuente');
        var divMostrarReteFuente = $('#mostrar_retefuente');
        var dropdownCuentaReteFuente = document.getElementById('cuenta_retefuente');
        var porcentajeValor = document.getElementById('Porcentaje_rete_fuente')
        var valor_base = document.getElementById('Base_retefuente');
        var valorRetefuente = document.getElementById('valorRetefuente');
        var valor_aprobado = $("#Valor_aprobado").val().trim();
        valor_aprobado = valor_aprobado.replace(/\./g, "");

        if (chkReteFuente.prop('checked')) {
            // Si el checkbox está marcado, muestra el div
            divMostrarReteFuente.show();
            porcentaje.show()
        }
        if (!chkReteFuente.prop('checked')) {
            divMostrarReteFuente.hide();
            porcentaje.hide();
            dropdownCuentaReteFuente.value = "";
            porcentajeValor.value = 0;
            valor_base.value = 0
            valorRetefuente.value = 0;
            descuentoReteFuente = 0;
            var sumaValores = 0;
           
            // Recorrer el array y sumar los valores
            for (var i = 0; i < facturasSeleccionadas.length; i++) {
                sumaValores += facturasSeleccionadas[i].valor;
            }
            sumatoria_final = valor_aprobado - (descuentoReteFuente + descuentoReteIca + sumaValores)
            $("#valor_final").val(agregarSeparadorMiles(sumatoria_final));
        }
    }

    function gestionarReteIca() {
        var chkReteIca = $('#chkReteica');
        var divMostrarReteIca = $('#mostrar_reteica');
        var dropdownCuentaReteIca = document.getElementById('cuenta_reteica');
        var porcentajeValor = document.getElementById('Porcentaje_rete_ica')
        var porcentaje = $('#mostrar_porcentaje_reteica');
        var valor_aprobado = $("#Valor_aprobado").val().trim();
        var valor_base = document.getElementById('Base_reteica');
        var valorReteica = document.getElementById('valorReteica');
        valor_aprobado = valor_aprobado.replace(/\./g, "");

        if (chkReteIca.prop('checked')) {
            // Si el checkbox está marcado, muestra el div
            divMostrarReteIca.show();
            porcentaje.show()
        }
        if (!chkReteIca.prop('checked')) {
            divMostrarReteIca.hide();
            porcentaje.hide();
            dropdownCuentaReteIca.value = "";
            valor_base.value = 0;
            porcentajeValor.value = 0;
            descuentoReteIca = 0;
            var sumaValores = 0;
            valorReteica.value = 0;

            $("#valorReteica").val(0);
            // Recorrer el array y sumar los valores
            for (var i = 0; i < facturasSeleccionadas.length; i++) {
                sumaValores += facturasSeleccionadas[i].valor;
            }
            sumatoria_final = valor_aprobado - (descuentoReteFuente + descuentoReteIca + sumaValores)
            $("#valor_final").val(agregarSeparadorMiles(sumatoria_final));
        }
    }

    function gestionarCruces() {
        var chkCruce = $('#chkCruce');
        var divCruces = $('#mostrar_cuentas_cruce');
        var dropdownCuentaCruce = document.getElementById('cuenta_cruce');
        var valor_aprobado = $("#Valor_aprobado").val().trim();
        valor_aprobado = valor_aprobado.replace(/\./g, "");
        if (chkCruce.prop('checked')) {
            divCruces.show();
        }
        if (!chkCruce.prop('checked')) {
            divCruces.hide();
            dropdownCuentaCruce.value = "";
            var sumaValores = 0;
            facturasSeleccionadas = [];
            // Recorrer el array y sumar los valores
            for (var i = 0; i < facturasSeleccionadas.length; i++) {
                sumaValores += facturasSeleccionadas[i].valor;
            }
            sumatoria_final = valor_aprobado - (descuentoReteFuente + descuentoReteIca + sumaValores)
            
            $("#valor_final").val(agregarSeparadorMiles(sumatoria_final));
        }
    }

    $("#agregar_cuenta").click(function () {

        var uniqueId = generateUniqueId();
        // Obtener el HTML del dropdown desde el contenedor
        var dropdownHtml = $("#dropdownHtmlContainer").find(".chosen-select-single").html();

        // Agregar los inputs dinámicamente
        var nuevoInput = '<div class="row margensuperior " data-id="' + uniqueId + '">'  +
            '<div class="col-md-3 col-sm-6">' +
            '<label>CUENTA CRUCE: </label> <br />' +
            '<select class="form-control chosen-select-single">' + dropdownHtml + '</select>' +
            '</div>' +
            '<div class="col-md-2 col-sm-6">' +
            '<label>FACTURAS: </label>' +
            '<select class="form-control facturas"></select>' +
            '</div>' +
            '<div class="col-md-2 col-sm-6">' +
            '<label>VALOR FACTURA: </label>' +
            '<input class="form-control valor_factura" readonly />' +
            '</div>' +
            '<div class="col-md-2 col-sm-6">' +
            '<label>RETEFUENTE: </label>' +
            '<input class="form-control aplico_retefuente" readonly />' +
            '</div>' +
            '<div class="col-md-2 col-sm-6">' +
            '<label>RETEICA: </label>' +
            '<input class="form-control aplico_reteica" readonly />' +
            '</div>' +
            '<div class="col-md-1 col-sm-6">' +
            '<button quitar_cuenta type="button" class="btn btn-danger" style="margin-top:20px"><i class="fa fa-minus-circle"></i></button>' +
            '</div>'
            '</div> <br/>';

        // Agregar el nuevo input al contenedor
        $("#contenedorCuentas").append(nuevoInput);

        // Inicializar Chosen en el nuevo dropdown
        $(".chosen-select-single").last().chosen();
        // Inicializar el cambio de evento para el nuevo select de facturas
        
    });
    $("#contenedorCuentas").on("click", "[quitar_cuenta]", function () {
        // Obtén la fila a la que pertenece el botón
        var valor_aprobado = $("#Valor_aprobado").val().trim();
        valor_aprobado = valor_aprobado.replace(/\./g, "");
        valor_aprobado = parseFloat(valor_aprobado) || 0;
       
        var row = $(this).closest('.row');

        // Obtén el número de factura de la fila
        var numFacturaEliminar = row.find('.facturas').val();

        // Encuentra y elimina la fila correspondiente del array facturasSeleccionadas
        facturasSeleccionadas = facturasSeleccionadas.filter(function (factura) {
            return factura.numFactura !== numFacturaEliminar;
        });

        // Recalcular la suma de los valores restantes en el array
        var sumaValores = facturasSeleccionadas.reduce(function (total, factura) {
            return total + factura.valor;
        }, 0);

        // Recalcula el valor final
        sumatoria_final = valor_aprobado - (descuentoReteFuente + descuentoReteIca + sumaValores);
        $("#valor_final").val(agregarSeparadorMiles(sumatoria_final));

        console.log(facturasSeleccionadas)

        // Elimina la fila
        row.remove();
    });


    $("#contenedorCuentas").on("change", ".chosen-select-single, .facturas", function () {

        var valor_aprobado = $("#Valor_aprobado").val().trim();
        valor_aprobado = valor_aprobado.replace(/\./g, "");
        var row = $(this).closest('.row');
        console.log(row)
        if ($(this).hasClass("chosen-select-single")) {
            // Lógica para cargar las facturas relacionadas con la cuenta seleccionada
            var selectedCuenta = $(this).val();


            $.ajax({
                url: '/Proveedor/FacturaProveedor/GetFacturasCruce',
                type: 'POST',
                data: { nitProveedor: $('#Fk_nit_proveedor').val(), cuentaCruce: selectedCuenta },
                success: function (facturas) {
                    // Actualiza el dropdown de facturas en la misma fila
                    var facturasDropdown = row.find('.facturas');
                    facturasDropdown.empty(); // Limpia las opciones actuales

                    facturasDropdown.append($('<option>', {
                        value: '',
                        text: 'Seleccionar',
                        disabled: true,
                        selected: true
                    }));

                    $.each(facturas, function (index, item) {
                        facturasDropdown.append($('<option>', {
                            value: item.Value,
                            text: item.Text
                        }));
                    });
                },
                error: function () {
                    console.error('Error al obtener las facturas.');
                }
            });
        } else if ($(this).hasClass("facturas")) {
            var selectedFactura = $(this).val();
            

            $.ajax({
                url: '/Proveedor/FacturaProveedor/GetValorFactura',
                type: 'POST',
                data: { numFactura: selectedFactura, nit_proveedor: $('#Fk_nit_proveedor').val() },
                success: function (data) {
                    if (data) {
                        // Actualiza el valor de la factura en la misma fila
                        var valorFacturaInput = row.find('.valor_factura');
                        valorFacturaInput.val(data.ValorAprobado);
                        // Verifica y establece el valor para retefuente
                        row.find('.aplico_retefuente').val(data.retefuente);

                        // Verifica y establece el valor para reteica
                        row.find('.aplico_reteica').val(data.reteica);

                        var nuevoValorFactura = parseFloat(row.find('.valor_factura').val().replace(/\./g, '')) || 0;

                        

                        // Buscar si la fila ya está en facturasSeleccionadas
                        var existingIndex = -1;
                        var currentRowId = row.data('id');
                        for (var i = 0; i < facturasSeleccionadas.length; i++) {
                            if (areRowsEqual(facturasSeleccionadas[i], currentRowId)) {
                                existingIndex = i;
                                break;
                            }
                        }

                        if (existingIndex !== -1) {
                            // La fila ya está en facturasSeleccionadas, actualizar la factura y su valor
                            facturasSeleccionadas[existingIndex].numFactura = selectedFactura;
                            facturasSeleccionadas[existingIndex].valor = parseFloat(row.find('.valor_factura').val().replace(/\./g, '')) || 0;
                        } else {
                            // La fila no está en facturasSeleccionadas, agregarla
                            var nuevoValorFactura = parseFloat(row.find('.valor_factura').val().replace(/\./g, '')) || 0;
                            facturasSeleccionadas.push({
                                control: currentRowId,
                                numFactura: selectedFactura,
                                valor: nuevoValorFactura
                            });
                        }

                        var sumaValores = 0;

                        // Recorrer el array y sumar los valores
                        for (var i = 0; i < facturasSeleccionadas.length; i++) {
                            sumaValores += facturasSeleccionadas[i].valor;
                        }
                        sumatoria_final = parseFloat((valor_aprobado) - (descuentoReteFuente + descuentoReteIca + sumaValores));
                        $("#valor_final").val(agregarSeparadorMiles(sumatoria_final));                  
                        //console.log(facturasSeleccionadas)


                    } else {
                        console.error('No se encontró la factura.');
                    }
                },
                error: function () {
                    console.error('Error al obtener el valor de la factura.');
                }
            });
        }
    });

    function generateUniqueId() {
        return new Date().getTime(); // Utilizando el tiempo actual como un valor único simple
    }
    function areRowsEqual(row, currentRowId) {
        return row.control === currentRowId;
    }

    $("#Valor_aprobado").on({
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

    $("#Base_retefuente").on({
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

    $("#Base_reteica").on({
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




});