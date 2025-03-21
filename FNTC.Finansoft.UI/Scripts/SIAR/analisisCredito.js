$(document).ready(function () {
    document.getElementById("clasificacion").disabled = true;
    function Table() {

        var columnas = [
            { data: "id" },
            { data: "rango" },
            { data: "calificacion" },
            { data: "PI" },
            { data: "EA" },
            { data: "PDI" },
            { data: "PE" },
            { data: "PEA" }
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
                title: "Pérdida Esperada Acumulada"

            }

        ]; //fin botones

        agregarDataTable("#tablaPA", columnas, '/SIAR/Siar/perdidaAcumulada', botones, false, true, true);
        table.columns(0).visible(false); //por si quiero ocultar alguna columna en este caso la columna ID
        //function agregarDataTable(tabla, columnas, urlDatos, botones, scroll, buscador, seleccion) {
        function agregarDataTable(tablaPA, columnas, urlDatos, botones, scroll, buscador, seleccion) {
            var TraduccionDatatable = {
                "sProcessing": "Procesando...", "sLengthMenu": "Mostrar _MENU_ registros", "sZeroRecords": "No se encontraron resultados", "sEmptyTable": "No hay registros", "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros", "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros", "sInfoFiltered": "(filtrado de un total de _MAX_ registros)", "sInfoPostFix": "", "sSearch": "Buscar:", "sUrl": "", "sInfoThousands": ",", "sLoadingRecords": "Cargando...", "select": { "rows": { _: "Has seleccionado %d filas", 0: "", 1: "1 fila seleccionada" } }, "oPaginate": { "sFirst": "<<", "sLast": ">>", "sNext": ">", "sPrevious": "<" }, "oAria": { "sSortAscending": ": Activar para ordenar la columna de manera ascendente", "sSortDescending": ": Activar para ordenar la columna de manera descendente" }
            };
            // iris2 = iris[c(1;10, 51:60, 101:110), ]
            table = $(tablaPA).DataTable({
                destroy: true,
                dom: 'Bfrtip',
                ajax: {
                    type: "POST",
                    url: urlDatos,
                    contentType: 'application/json; charset=utf-8',
                    data: function (data) {return data = JSON.stringify(data);} 
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

    $("#btnAnalizar").click(function () {

        var clasificacion = $("#clasificacion").val();
        var linea = $("#linea").val();
        var subclasificacion = $("#subclasificacion").val();
        var fechaCorte = $("#fechaCorte").val();

        if (fechaCorte != "") {
            if (clasificacion == linea) {
                $.ajax({
                    url: '/SIAR/Siar/guardarParametros',
                    datatype: "Json",
                    data: {
                        clasificacion: clasificacion,
                        linea: linea,
                        subclasificacion: subclasificacion,
                        fechaCorte: fechaCorte
                    },//solo para enviar datos
                    type: 'post',
                }).done(function (data) {
                    if (data.status) {
                        $('#tablaPA').dataTable().fnDestroy();
                        Table();
                    } else {
                        alert(data.error);
                    }

                });//fin ajax

            } else {
                Swal.fire({
                    icon: 'warning',
                    title: 'Advertencia!',
                    html: ` <label class="fuenteSweetAlert">La clasificación debe ser igual a la línea</label>`,
                })
            }
        } else {
            Swal.fire({
                icon: 'warning',
                title: 'Advertencia!',
                html: ` <label class="fuenteSweetAlert">Por favor seleccione fecha de corte</label>`,
            })
        }

        

        




    });//fin btnAnalizar

    $("#linea").change(function () {
        var linea = $(this).val();
        if (linea != "0") {
            document.getElementById("clasificacion").disabled = false;
        } else {
            document.getElementById("clasificacion").value = '0';
            document.getElementById("clasificacion").disabled = true;
        }
    });//linea
});

