$(document).ready(function () {
    var tercero = "";
    var fecha = "";
    Table();
    $("#asociado").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Terceros/Terceros/BuscarTerceros",
                data: { cadena: request.term },
                dataType: 'json',
                type: 'POST',
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Nombre,
                            value: item.Id
                        }
                    }));
                }
            });
        },
        select: function (event, ui) {

            $(this).val(ui.item.value);
            $("#lblAsociado").text(ui.item.label);
            return false;
        },
        minLength: 1
    });

    $("#btnBuscar").click(function () {
        tercero = $("#asociado").val();
        fecha = $("#fecha").val();
        Table();
    })

    $("#btnLimpiar").click(function () {
        $("#asociado").val("");
        $("#lblAsociado").text("");
        $("#fecha").val("");
    })

    $("#asociado").change(function () {
        if ($(this).val() == "")
            $("#lblAsociado").text("");

    })

    $("#example").on('click', '.anular', function (e) {
        e.preventDefault();
        var id = $(this).attr('id');

        Swal.fire({
            title: '¿Anular factura?',
            text: "No se podrá revertir este proceso!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar',
        }).then((result) => {
            if (result.isConfirmed) {
                AnularFactura(id);
            }
        })
        
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
            {

                extend: 'pdf',
                title: "LÍNEAS DE CRÉDITO"

            }

        ]; //fin botones

        agregarDataTable("#example", '../../../OperativaDeCaja/FactOpcajas/GetFacturasAsync', botones, false, true, true);
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
                    "data": function (d) {
                        return $.extend({}, d, {
                            "tercero": tercero,
                            "fecha": fecha
                        });
                    }
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
                columnDefs: [{ "sClass": "hide_me", "aTargets": [1] }],
                aaSorting: []
            });

        } // fin funcion agregarDataTable

    }

    function AnularFactura(id) {
        $.ajax({
            url: '/OperativaDeCaja/FactOpcajas/AnularFacturaAporteAsync',
            datatype: "Json",
            data: { id: id },//solo para enviar datos
            type: 'post',
        }).done(function (data) {
            if (data.status == true) {
                Swal.fire({
                    title: 'Listo!',
                    text: "Proceso realizado correctamente.",
                    icon: 'warning',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Aceptar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/OperativaDeCaja/FactOpcajas/AnularFacturaAporte";
                    }
                })
            }
            else if (data.status == false) {
                Swal.fire({
                    icon: 'error',
                    title: 'Ha ocurrido un error.',
                    text: 'Por favor, intente nuevamente. Si el problema persiste comuníquese con soporte técnico'
                })
            }

        });
    }
    
})