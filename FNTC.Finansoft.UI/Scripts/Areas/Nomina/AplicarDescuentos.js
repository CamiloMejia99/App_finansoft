$(document).ready(function () {
    $("#btnDescuentos").click(function () {
        var archivo = $("#archivo").val();
        var cuenta = $("#cuentas").val();
        var discrmininacion = $("#discriminaciones").val();
        var comprobante = $("#comprobantes");

        if (archivo && cuenta && discrmininacion && comprobante) {
            // Mostrar modal de carga
            Swal.fire({
                icon: 'warning',
                title: 'Procesando',
                html: 'Por favor, espere...',
                allowOutsideClick: false,
                showConfirmButton: false,
                onBeforeOpen: () => {
                    Swal.showLoading();
                }
            });
            $("#FormDescuentos").submit();
        } else {
            mostrarAlerta('Por favor seleccione todos los campos!');
        }
    });

    function mostrarAlerta(mensaje) {
        Swal.fire({
            icon: 'warning',
            html: '<label class="fuenteSweetAlert2">' + mensaje + '</label>',
        });
    }
});
