$(document).ready(function () {

    var tipo = "";
    var fechaDesde = "";
    var fechaHasta = "";
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

        agregarDataTable("#example", '../../../accounting/comprobantes/getcomprobantes', botones, false, true, true);
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
                            "tipo": tipo,
                            "fechaDesde": fechaDesde,
                            "fechaHasta": fechaHasta
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
                lengthMenu:[10,25,50,100]
            });
            
        } // fin funcion agregarDataTable

    }
    Table();

    $("#buscarComprobante").click(function () {
        tipo = $("#tiposComprobantes").val();
        fechaDesde = $("#idFechaDesde").val();
        fechaHasta = $("#idFechaHasta").val();

        if ((fechaDesde != "" && fechaHasta == "") || (fechaDesde == "" && fechaHasta != "")) {
            Swal.fire(
                'Por favor complete el filtro de fecha',
                '',
                'info'
            );
        } else { Table();}

        

    });

    $("#limpiar").click(function () {
        $('#tiposComprobantes').val("");
        $('#idFechaDesde').val("");
        $('#idFechaHasta').val("");
    });
    
})