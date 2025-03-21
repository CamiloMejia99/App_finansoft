$(document).ready(function () {
    //VALIDACION POR MONTO

    var selectedIds = [];
    var table;


    $("#btnGuardar").click(function (event) {
        event.preventDefault(); // Prevenir el comportamiento por defecto del botón

        var valorSolicitudes = $("#Valor_solicitudes").val().trim();
        var valorfactura = $("#Valor_factura").val().trim();
        var factura = $("#Factura_proveedor").val().trim();
        var fechafactura = $("#Fecha_factura").val().trim();

        // Verificar si todos los campos están llenos
        if (valorSolicitudes == "" || valorfactura == "" || factura == "" || fechafactura == "") {
            mostrarAlerta('Por favor llene todos los campos');
        } else {
            var valorS = $("#Valor_solicitudes").val().trim();
            valorS = valorS.replace(/\./g, "");
            $("#Valor_solicitudes").val(valorS);

            var valorF = $("#Valor_factura").val().trim();
            valorF = valorF.replace(/\./g, "");
            $("#Valor_factura").val(valorF);

            var Desc = $("#Valor_diferencial").val().trim();
            Desc = Desc.replace(/\./g, "");
            $("#Valor_diferencial").val(Desc);

            // Asegurarse de que se ejecute el submit
            setTimeout(function () {
                $("#FormAuxilio").off('submit'); // Eliminar cualquier manejador de submit previo
                $("#FormAuxilio").submit(); // Enviar el formulario
            }, 100); // Pequeño retraso para asegurar que todo se complete
        }
    });


    $("#Fk_codigo_auxilio").change(function () {

        var auxilio = $("#Fk_codigo_auxilio").val();
        var Proveedor = $("#FK_proveedor").val();

        if (Proveedor != "") {
            // DESTRUIR LA INSTANCIA DEL DATATABLE SI YA EXISTE
            if ($.fn.DataTable.isDataTable('#SolicitudesPorProveedor')) {
                table.clear().destroy(); // Limpiar y destruir la instancia del DataTable
                selectedIds = [];
                calcularTotalAprobado();
            }

            table = $('#SolicitudesPorProveedor').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.12/i18n/Spanish.json"
                },
                "ajax": {
                    "url": '../../../Auxilios/AuxiliosProveedorExterno/GetSolicitudes',
                    "type": "POST", // Puede ser GET o POST según tu configuración
                    "data": { auxilio: auxilio, Proveedor: Proveedor }, // Envía el parámetro cuenta en la solicitud
                    "dataType": "json"
                },
                "columns": [
                    { "data": "NumeroSolicitud" },
                    { "data": "Afiliado" },
                    { "data": "Concepto" },
                    { "data": "ValorAprobado" },
                    { "data": "Factura" },
                    { "data": "FechaFactura" },
                    { "data": "FechaSolicitud" },
                    { "data": "Ver" },
                    {
                        "data": "Check",
                        "render": function (data, type, row) {
                            return '<input type="checkbox" class="checkSolicitud" value="' + row.NumeroSolicitud + '">';
                        }
                    }
                ]
            });
        }
    });

    $("#FK_proveedor").change(function () {

        var auxilios = $("#Fk_codigo_auxilio").val();
        var Proveedor = $("#FK_proveedor").val();

        if (auxilios != "") {
            // DESTRUIR LA INSTANCIA DEL DATATABLE SI YA EXISTE
            if ($.fn.DataTable.isDataTable('#SolicitudesPorProveedor')) {
                table.clear().destroy(); // Limpiar y destruir la instancia del DataTable
                selectedIds = [];
                calcularTotalAprobado();
            }

            table = $('#SolicitudesPorProveedor').DataTable({
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.10.12/i18n/Spanish.json"
                },
                "ajax": {
                    "url": '../../../Auxilios/AuxiliosProveedorExterno/GetSolicitudes',
                    "type": "POST", // Puede ser GET o POST según tu configuración
                    "data": { auxilios: auxilios, Proveedor:Proveedor }, // Envía el parámetro cuenta en la solicitud
                    "dataType": "json"
                },
                "columns": [
                    { "data": "NumeroSolicitud" },
                    { "data": "Afiliado" },
                    { "data": "Concepto" },
                    { "data": "ValorAprobado" },
                    { "data": "Factura" },
                    { "data": "FechaFactura" },
                    { "data": "FechaSolicitud" },
                    { "data": "Ver" },
                    {
                        "data": "Check",
                        "render": function (data, type, row) {
                            return '<input type="checkbox" class="checkSolicitud" value="' + row.NumeroSolicitud + '">';
                        }
                    }
                ]
            });
        }


    });

    $('#marcarTodos').on('click', function () {

        // Desactivar la paginación temporalmente
        table.page.len(-1).draw();

        // Marcar todos los checkboxes en la tabla
        $('#SolicitudesPorProveedor').find('.checkSolicitud').prop('checked', true);

        // Actualizar el array de IDs seleccionados
        actualizarArrayIds();
        calcularTotalAprobado();
        habilitarDeshabilitarBoton();

        // Volver a activar la paginación
        table.page.len(10).draw();
    });

    $('#marcarninguno').on('click', function () {
        // Desactivar la paginación temporalmente
        table.page.len(-1).draw();
        // Desmarcar todos los checkboxes en la tabla
        $('#SolicitudesPorProveedor').find('.checkSolicitud').prop('checked', false);

        // Vaciar el array de IDs seleccionados
        selectedIds = [];

        // Recalcular el total
        calcularTotalAprobado();
        // Actualizar el array de IDs seleccionados
        actualizarArrayIds();

        habilitarDeshabilitarBoton();
        // Volver a activar la paginación
        table.page.len(10).draw();
    });

    $('#SolicitudesPorProveedor tbody').on('change', '.checkSolicitud', function () {
        // Actualizar el array de IDs seleccionados
        actualizarArrayIds();
        calcularTotalAprobado();
        habilitarDeshabilitarBoton();
    });


    // Función para actualizar el array de IDs seleccionados
    function actualizarArrayIds() {

        // Recorre todas las casillas de verificación
        $('.checkSolicitud').each(function () {
            var solicitudId = $(this).val();
            var isChecked = $(this).prop('checked');

            // Verificar si la solicitudId está presente en el array
            var index = $.inArray(solicitudId, selectedIds);

            if (isChecked && index === -1) {
                // Si está marcado y no está en el array, agrégalo
                selectedIds.push(solicitudId);
            } else if (!isChecked && index !== -1) {
                // Si está desmarcado y está en el array, quítalo
                selectedIds.splice(index, 1);
            }
        });

        $('#selectedIds').val(selectedIds.join(','));
        console.log(selectedIds);
    }


    function calcularTotalAprobado() {
        var totalAprobado = 0;
        table.rows().nodes().to$().find('input[type="checkbox"]:checked').each(function () {
            var rowData = table.row($(this).closest('tr')).data();
            // Eliminar puntos y comas de los separadores decimales
            var valorAprobadoSinSeparadores = rowData.ValorAprobado.replace(/[.]/g, '');
            totalAprobado += parseFloat(valorAprobadoSinSeparadores);
        });

        var totalFormateado = formatoMiles(totalAprobado); // Formatea el total

        $('#Valor_solicitudes').val(totalFormateado);
    }


    function habilitarDeshabilitarBoton() {
        var checkboxesMarcados = $('.checkSolicitud:checked').length > 0;
        $('#btnGuardar').prop('disabled', !checkboxesMarcados);
    }

    habilitarDeshabilitarBoton();

    function formatoMiles(numero) {
        return numero.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.');
    }

    $("#Valor_factura").on({
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

    $("#Valor_factura").change(function () {

        var valorFactura = $("#Valor_factura").val().trim();
        valorFactura = valorFactura.replace(/[.]/g, '');

        var valorSolicitudes = $("#Valor_solicitudes").val().trim();
        valorSolicitudes = valorSolicitudes.replace(/[.]/g, '');

        var descuento = parseFloat(valorFactura) - parseFloat(valorSolicitudes);

        var totalFormateado = formatoMiles(descuento); // Formatea el total

        $('#Valor_diferencial').val(totalFormateado);

    });

 
    // Manejar clic en el botón "Ver Detalle"
    $('#SolicitudesPorProveedor').on('click', '.ver-detalle', function (e) {
        e.preventDefault();
        var solicitudId = $(this).data('id');
        cargarDetallesSolicitud(solicitudId);
    });

    // Función para cargar los detalles de la solicitud en la vista modal
    function cargarDetallesSolicitud(idSolicitud) {
        $.ajax({
            url: '/Auxilios/AuxiliosProveedorExterno/DetallesSolicitud?id=' + idSolicitud,
            type: 'GET',
            success: function (response) {
                $('#centro .modal-body').html(response);
                $('#centro').modal('show');
            },
            error: function () {
                alert('Error al cargar los detalles de la solicitud');
            }
        });
    }




    function mostrarAlerta(mensaje) {
        Swal.fire({
            icon: 'warning',
            html: '<label class="fuenteSweetAlert2">' + mensaje + '</label>',
        });
    }

});