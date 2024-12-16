$(document).ready(function () {

    function Table() {

        var columnas = [
            { data: "id" },
            { data: "descripcion" },
            { data: "codigo" },
            { data: "estado" },
        ]; // fin columnas

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

        agregarDataTable("#tablaLineas", columnas, '/SIAR/Siar/getLineas', botones, false, true, true);
        table.columns(0).visible(false); //por si quiero ocultar alguna columna en este caso la columna ID
        //function agregarDataTable(tabla, columnas, urlDatos, botones, scroll, buscador, seleccion) {
        function agregarDataTable(tablaLineas, columnas, urlDatos, botones, scroll, buscador, seleccion) {
            var TraduccionDatatable = {
                "sProcessing": "Procesando...", "sLengthMenu": "Mostrar _MENU_ registros", "sZeroRecords": "No se encontraron resultados", "sEmptyTable": "No hay registros", "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros", "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros", "sInfoFiltered": "(filtrado de un total de _MAX_ registros)", "sInfoPostFix": "", "sSearch": "Buscar:", "sUrl": "", "sInfoThousands": ",", "sLoadingRecords": "Cargando...", "select": { "rows": { _: "Has seleccionado %d filas", 0: "", 1: "1 fila seleccionada" } }, "oPaginate": { "sFirst": "<<", "sLast": ">>", "sNext": ">", "sPrevious": "<" }, "oAria": { "sSortAscending": ": Activar para ordenar la columna de manera ascendente", "sSortDescending": ": Activar para ordenar la columna de manera descendente" }
            };
            // iris2 = iris[c(1;10, 51:60, 101:110), ]
            table = $(tablaLineas).DataTable({
                destroy: true,
                dom: 'Bfrtip',
                ajax: {
                    type: "POST",
                    url: urlDatos,
                    contentType: 'application/json; charset=utf-8',
                    data: function (data) { return data = JSON.stringify(data); }
                },
                searching: buscador,
                lengthChange: false,
                autoWidth: false,
                scrollX: scroll,
                columns: columnas,
                buttons: botones,
                deferRender: true,
                select: seleccion,
                language: TraduccionDatatable
            });
            table.buttons().container().appendTo('.col-sm-6:eq(0)');
        } // fin funcion agregarDataTable

    }
    Table();


});