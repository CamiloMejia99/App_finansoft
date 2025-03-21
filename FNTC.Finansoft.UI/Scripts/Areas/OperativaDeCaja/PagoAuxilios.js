$(document).ready(function () {
    $('#marcarTodos').on('click', function () {
        // Marcar todos los checkboxes en la tabla
        $('.checkSolicitud').prop('checked', true);

        // Actualizar el array de IDs seleccionados
        actualizarArrayIds();
        calcularTotalAprobado();
    });

    $('#tbSolicitud tbody').on('change', '.checkSolicitud', function () {
        // Actualizar el array de IDs seleccionados
        actualizarArrayIds();
    });



    // Función para actualizar el array de IDs seleccionados
    function actualizarArrayIds() {
        var selectedIds = [];

        // Recorre todas las casillas de verificación
        $('.checkSolicitud:checked').each(function () {
            // Agrega el valor (ID de solicitud) al array
            selectedIds.push($(this).val());
        });

        $('#selectedIds').val(selectedIds.join(','));
        console.log(selectedIds);
    }

    function calcularTotalAprobado() {
        var totalAprobado = 0;
        table.rows().nodes().to$().find('input[type="checkbox"]:checked').each(function () {
            var rowData = table.row($(this).closest('tr')).data();
            totalAprobado += parseFloat(rowData.ValorAprobado);
        });
        var totalFormateado = formatoMiles(totalAprobado); // Formatea el total
        $('#ValorPagar').val(totalFormateado);
    }

    $('#ValorRecibido').on('input', function () {
        actualizarValorRecibido();
        calcularCambio();
    });

    function actualizarValorRecibido() {
        var valorRecibido = $('#ValorRecibido').val().replace(/\./g, '').replace(',', '.');
        $('#ValorRecibido').val(formatoMiles(valorRecibido));
    }

    $("#ValorRecibido").on({
        "focus": function (event) {
            $(event.target).select();
        },
        "input": function (event) {
            var valorConComas = $(event.target).val().replace(/\./g, '');
            $(event.target).val(formatoMiles(valorConComas));
        }
    });

    function calcularCambio() {
        var valorRecibido = formatoNumero($('#ValorRecibido').val()); // Convierte a número el valor recibido
        var valorPagar = formatoNumero($('#ValorPagar').val()); // Convierte a número el valor a pagar
        var cambio = valorRecibido - valorPagar;
        $('#Cambio').val(formatoMiles(cambio.toFixed(3))); // Muestra el cambio con formato de miles y dos decimales
    }


    function formatoMiles(numero) {
        return numero.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }

    function formatoNumero(numero) {
        return parseFloat(numero.toString().replace(/\./g, '').replace(',', '.')) || 0;
    }




});