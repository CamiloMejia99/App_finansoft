$(document).ready(function () {

    $("#btnNuevaFicha").click(function () {
        $("#AddFichaContractual").modal("show");
    });

    function Table() {

        var botones = [
            //{
            //    extend: 'collection',
            //    text: 'Exportar A',
            //    autoClose: true,
            //    buttons: [
            //        {
            //            extend: 'excel',
            //            text: "Excel",
            //            exportOptions: {
            //                columns: [1, 2]
            //            }

            //        }
            //    ]

            //}
            //'excel', 
            //{
            //    text: "Agregar configuración",
            //    action: function () {
            //        $("#AddConfiguracion").modal("show");
            //    },
            //    className: 'btn btn-success btn-sm fa fa-plus',
            //},
            //{

            //    extend: 'pdf',
            //    title: "LÍNEAS DE CRÉDITO",
            //    className: 'btn btn-default btn-sm fa'

            //}

        ]; //fin botones

        agregarDataTable("#tablaFichasAhorroContractual", '/Ahorros/Ahorros/GetFichasAhorroContractual', botones, false, true, false);
        function agregarDataTable(tablaLineas, urlDatos, botones, scroll, buscador, seleccion) {
            var TraduccionDatatable = {
                "sProcessing": "Procesando...", "sLengthMenu": "Mostrar _MENU_ registros", "sZeroRecords": "No se encontraron resultados", "sEmptyTable": "No hay registros", "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros", "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros", "sInfoFiltered": "(filtrado de un total de _MAX_ registros)", "sInfoPostFix": "", "sSearch": "Buscar:", "sUrl": "", "sInfoThousands": ",", "sLoadingRecords": "Cargando...", "select": { "rows": { _: "Has seleccionado %d filas", 0: "", 1: "1 fila seleccionada" } }, "oPaginate": { "sFirst": "<<", "sLast": ">>", "sNext": ">", "sPrevious": "<" }, "oAria": { "sSortAscending": ": Activar para ordenar la columna de manera ascendente", "sSortDescending": ": Activar para ordenar la columna de manera descendente" }
            };
            // iris2 = iris[c(1;10, 51:60, 101:110), ]
            table = $(tablaLineas).DataTable({
                destroy: true,
                dom: 'Bfrtip',
                "ajax": {
                    "method": "POST",
                    "url": urlDatos,
                    "data": function (data) { return data = JSON.stringify(data); }
                },
                searching: buscador,
                lengthChange: false,
                autoWidth: false,
                scrollX: scroll,
                buttons: botones,
                deferRender: true,
                select: seleccion,
                language: TraduccionDatatable,
                paging: true,
                lengthMenu: [10, 25, 50, 100],
                columnDefs: [{ "sClass": "hide_me", "aTargets": [0] }]
            });

        } // fin funcion agregarDataTable

    }

    //seccion de funciones que se ejecutan al inicar la página
    Table();
    VerificaCamposReadOnly();
    //.....
    
    //SECCIÓN DE EVENTOS

    $('#IdAsociado').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Terceros/Terceros/GetTercerosAutocopletar2",
                data: { cadena: request.term },
                dataType: 'json',
                type: 'POST',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Nombre,
                            value: item.Id,
                        }
                    }));
                }
            })
        },
        select: function (event, ui) {
            $('#IdAsociado').val(ui.item.value);
            $('#NombreAsociado').val(ui.item.label);
            return false;
        },
        minLength: 1
    });

    $("#IdAsociado").change(function () {
        verificaTercero($(this).val());
    });

    $("#IdConfiguracion").change(function () {

        var Id = $(this).val()
        GetPlazoAndTasa(Id);

        if (Id == "" || Id == null)
            $("#NumeroCuenta").val("");
        else
            VerificaTerceroByFichaAC();
    });

    $("#AuxTasaEfectiva").change(function () {//Función que verifica si el valor de tasa ingresado cumple el rango registrado en la configuración escogida
        try {
            let valor = parseFloat($(this).val());
            if (!(valor >= 0 && valor <= 100)) {
                $(this).val("");
            } else {
                verificarTasaAC();
            }
        } catch (e) {
            $(this).val("");
        }
    });

    $("#AuxValorCuota").change(function () {
        var idConfig = $("#IdConfiguracion").val();
        var valor = $(this).val();
        if (idConfig == "" || valor== "")
            $(this).val("");
        else
            VerificaRangoCuotaAC(idConfig,valor);
    });

    $("#Plazo").change(function () {
        var idConfig = $("#IdConfiguracion").val();
        var valor = $(this).val();
        var esNumero = isNaN(valor);//verifica que el valor ingresado sea número o cadena de texto
        if (idConfig == "" || valor == "" || esNumero)
            $(this).val("");
        else
            VerificaRangoPlazoAC(idConfig, valor);
    });

    $(".dueDate").change(function () {
        CalcularFechaVencimiento();
    });

    $("#guardarFichaAC").click(function () {
        Swal.fire({
            title: '¿Agregar registro?',
            text: "",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $("#formFichaAhoCont").submit();
            }
        })
    });

    $("#editarFichaAC").click(function () {
        Swal.fire({
            title: '¿Guardar cambios?',
            text: "",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                $("#formEditFichaAC").submit();
            }
        })
    });

    $("#cancelarFAC").click(function () {
        window.location.href = "/Ahorros/Ahorros/ConfiguracionFAC";
    });
    //........

    //SECCIÓN DE FUNCIONES

    function verificaTercero(nit) {
        $.ajax({
            url: '/Terceros/Terceros/verificaExisteTercero',
            datatype: "Json",
            data: { nit: nit },//solo para enviar datos
            type: 'post',
        }).done(function (data) {
            if (!data.status) {
                ResetInfoAsociado();
            } else {
                VerificaTerceroByFichaAC();
            }
        });
    }
    function VerificaTerceroByFichaAC() { //función que verifica que el usuario sólo puede tener una ficha de ahorro contractual por configuración
        var tipoFicha = $("#IdConfiguracion").val();
        var nit = $("#IdAsociado").val();
        if (tipoFicha == "" || nit == "")
            return false;

        $.ajax({
            url: '/Ahorros/Ahorros/VerificaTerceroByFichaAC',
            datatype: "Json",
            data: {
                nit: nit,
                idConfig: tipoFicha
            },
            type: 'post',
        }).done(function (data) {
            if (data.status) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Registro ya existente.',
                    text: 'El asociado ya cuenta con una ficha de ahorro contractual con esta configuración.'
                });
                ResetInfoAsociado();
            } else {
                $("#NumeroCuenta").val(data.numeroCuenta);
            }
        });


    }

    function ResetInfoAsociado() {
        $(".reset").each(function () {
            $(this).val("");
        });
    }

    function ResetValores() {
        $(".resetValue").each(function () {
            var id = $(this).attr('id');
            ponerReadOnly(id);
            $(this).val("");
        });
    }

    function SetReadonly() {
        $(".resetValue").each(function () {
            var id = $(this).attr('id');
            quitarReadOnly(id);
        });
    }

    function VerificaCamposReadOnly() {
        var IdConfig = $("#IdConfiguracion").val();
        if (IdConfig == "")
            ResetValores();
    }

    function GetPlazoAndTasa(IdConfig) { //Retorna los valores de plazo y tasa efectiva registradas en la configuración

        if (IdConfig == "" || IdConfig == null) {
            ResetValores();
        } else {
            $.ajax({
                url: '/Ahorros/Ahorros/GetPlazoAndTasa',
                datatype: "Json",
                data: { id: IdConfig },
                type: 'post',
            }).done(function (data) {
                if (data.status) {
                    $("#Plazo").val(data.plazo);
                    $("#AuxTasaEfectiva").val(data.tasa);
                    $("#AuxValorCuota").val(data.cuotaMinima);
                }
                SetReadonly();
            });
        }


    }
    
    function verificarTasaAC() {
        try {
            let valor = parseFloat($("#AuxTasaEfectiva").val().split(',').join("."));
            var idConfig = $("#IdConfiguracion").val();
            if (isNaN(valor) || idConfig == "")
                return false;
            else {
                $.ajax({
                    url: '/Ahorros/Ahorros/VerificaRangoTasaAC',
                    datatype: "Json",
                    data: {
                        idConfig: idConfig,
                        valor: valor
                    },
                    type: 'post',
                }).done(function (data) {
                    if (!data.status) {
                        Swal.fire({
                            icon: 'warning',
                            title: '' + data.encabezado,
                            text: '' + data.mensaje
                        });
                        $("#AuxTasaEfectiva").val("");
                    }
                });
            }



        } catch (e) {

        }
    }
    function VerificaRangoCuotaAC(idConfig,valor) { //Función que verifica si el valor de cuota ingresado cumple el rango registrado en la configuración escogida
        $.ajax({
            url: '/Ahorros/Ahorros/VerificaRangoCuotaAC', 
            datatype: "Json",
            data: {
                idConfig: idConfig,
                valor: valor
            },
            type: 'post',
        }).done(function (data) {
            if (!data.status) {
                Swal.fire({
                    icon: 'warning',
                    title: ''+data.encabezado,
                    text: ''+data.mensaje
                });
                $("#AuxValorCuota").val("");
            }
        });
    }

    function VerificaRangoPlazoAC(idConfig, valor) { //Función que verifica si el valor de plazo ingresado cumple el rango registrado en la configuración escogida
        $.ajax({
            url: '/Ahorros/Ahorros/VerificaRangoPlazoAC',
            datatype: "Json",
            data: {
                idConfig: idConfig,
                valor: valor
            },
            type: 'post',
        }).done(function (data) {
            if (!data.status) {
                Swal.fire({
                    icon: 'warning',
                    title: '' + data.encabezado,
                    text: '' + data.mensaje
                });
                $("#Plazo").val("");
            }
        });
    }


    function ponerReadOnly(id) {
        // Ponemos el atributo de solo lectura
        $("#" + id).attr("readonly", "readonly");
        // Ponemos una clase para cambiar el color del texto y mostrar que
        // esta deshabilitado
        $("#" + id).addClass("readOnly");
    }

    function quitarReadOnly(id) {
        // Eliminamos el atributo de solo lectura
        $("#" + id).removeAttr("readonly");
        // Eliminamos la clase que hace que cambie el color
        $("#" + id).removeClass("readOnly");
    }

    function CalcularFechaVencimiento() {
        let plazo = parseInt($("#Plazo").val());
        let fechaApertura = $("#FechaApertura").val()

        if (isNaN(plazo) || fechaApertura=="")
        {
            $("#AuxFechaVencimiento").val("");
            return false;
        }
        
        try {
            $.ajax({
                url: '/Ahorros/Ahorros/GetFechaVencimiento',
                datatype: "Json",
                data: {
                    plazo: plazo,
                    fecha: fechaApertura
                },
                type: 'post',
            }).done(function (data) {
                $("#AuxFechaVencimiento").val(data.fechaVencimiento);
            });
        } catch (e) {
            return false;
        }
        

    }
    
})