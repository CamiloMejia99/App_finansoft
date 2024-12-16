$(document).ready(function () {
    var documento = "";
    var idFechaHasta = "";
    var idFechaDesde = "";
    function Table() {

        var botones = [
            {

                extend: 'pdf',
                title: "LÍNEAS DE CRÉDITO"

            }

        ]; //fin botones

        agregarDataTable("#formularios", '../../../formularioVinculacion/FormularioVinculacion/GetFormularios', botones, false, true, true);
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
                            "documento": documento,
                            "idFechaDesde": idFechaDesde,
                            "idFechaHasta": idFechaHasta
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
                lengthMenu: [10, 25, 50, 100]
            });

        } // fin funcion agregarDataTable

    }
    Table();



    $("#buscarDocumento").click(function () {
        documento = $("#Terceros").val();
        idFechaHasta = $("#idFechaHasta").val();
        idFechaDesde = $("#idFechaDesde").val();
        if ((idFechaDesde != "" && idFechaHasta == "") || (idFechaDesde == "" && idFechaHasta != "")) {
            Swal.fire({
                icon: 'warning',
                title: 'Advertencia!',
                text: 'Por favor, complete el filtro de fecha'
            });
        } else { Table(); }
    });//fin btnBuscar


    $("#limpiar").click(function () {
        $("#Terceros").val("");
        $('#idFechaHasta').val("");
        $('#idFechaDesde').val("");
    });

})