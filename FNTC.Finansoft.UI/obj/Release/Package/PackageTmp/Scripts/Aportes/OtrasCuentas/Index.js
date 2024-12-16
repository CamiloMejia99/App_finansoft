$(document).ready(function () {


    function Table() {
        var columnas = [
            { data: "Id","width":"7%" },
            { data: "Cuenta" },
            { data: "NombreCuenta" },
            { data: "Porcentaje" },
            { data: "Estado", "width": "7%", render: function (data) { return data ? "Activa" : "Inactiva";} },
            { data: "Id", "width": "7%", render: function (data) { return '<button class="fa fa-times btn-danger btnEliminar" id="'+data+'" title="Quitar cuenta" ></button>'; } },
        ];

        var botones = [
            {
                extend: 'collection',
                text: 'Exportar A',
                autoClose: true,
                buttons: [
                    {
                        extend: 'excel',
                        text: "Excel",
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'pdf',
                        text: "PDF",
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'print',
                        text: "Imprimir",
                        exportOptions: {
                            columns: ':visible'
                        }
                    }
                ]
            }
        ];

        agregarDataTable("#tablaOtrasCuentasAportes", columnas, '/Aportes/Aportes/ObtenerOtrasCuentasAportes', botones, false, true, false);
        //table.columns(9).visible(false); table.columns(10).visible(false);

        function agregarDataTable(tabla, columnas, urlDatos, botones, scroll, buscador, seleccion) {
            var TraduccionDatatable = {
                "sProcessing": "Procesando...", "sLengthMenu": "Mostrar _MENU_ registros", "sZeroRecords": "No se encontraron resultados", "sEmptyTable": "No hay registros", "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros", "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros", "sInfoFiltered": "(filtrado de un total de _MAX_ registros)", "sInfoPostFix": "", "sSearch": "Buscar:", "sUrl": "", "sInfoThousands": ",", "sLoadingRecords": "Cargando...", "select": { "rows": { _: "Has seleccionado %d filas", 0: "", 1: "1 fila seleccionada" } }, "oPaginate": { "sFirst": "<<", "sLast": ">>", "sNext": ">", "sPrevious": "<" }, "oAria": { "sSortAscending": ": Activar para ordenar la columna de manera ascendente", "sSortDescending": ": Activar para ordenar la columna de manera descendente" }
            };
            table = $(tabla).DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.12/i18n/Spanish.json"
                },
                destroy:true,
                dom: 'frtip',
                select: true,
                ajax: {
                    type: "POST",
                    url: urlDatos,
                    contentType: 'application/json; charset=utf-8',
                    data: function (data) { return data = JSON.stringify(data); }
                },
                "bSort": true,
                searching: buscador,
                lengthChange: false,
                autoWidth: false,
                scrollX: scroll,
                columns: columnas,
                buttons: botones,
                deferRender: true,
                select: seleccion
                //language: TraduccionDatatable
            });
            table.buttons().container().appendTo('.col-sm-6:eq(0)');
        }

    };//fin función Table

    Table();

    $('#tablaOtrasCuentasAportes').on("click", ".btnEliminar", function (event) {
        var Id = $(this).attr("id");
        Swal.fire({
            title: 'Eliminar cuenta?',
            text: "",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                EliminarCuentaDistribucion(Id);
            }
        });
        
    });

    function EliminarCuentaDistribucion(Id) {
        $.ajax({
            url: '/Aportes/Aportes/EliminarCuentaDistribucion',
            datatype: "Json",
            data: { Id: Id },
            type: 'post',
        }).done(function (data) {
            if (data.status) {
                Swal.fire({
                    title: 'Proceso exitoso',
                    text: data.mensaje,
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Aceptar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        Table();
                    }
                });
                
            }
            else{
                Swal.fire({
                    icon: 'error',
                    title: 'No se puedo eliminar esta cuenta',
                    text: '' + data.mensaje
                });
            }

        });
    };

});