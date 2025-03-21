$(document).ready(function () {
    $("#btnComparar").click(function () {
        var archivo = $("#archivo").val();
        var discriminaciones = $("#discriminaciones").val();

        if (archivo && discriminaciones) {
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
            $("#FormComparativa").submit();
        } else {
            if (!archivo) 
                mostrarAlerta('Por favor cargue el archivo excel');
            else
                mostrarAlerta('Por favor seleccione un periodo');
        }
    });

    function mostrarAlerta(mensaje) {
        Swal.fire({
            icon: 'warning',
            html: '<label class="fuenteSweetAlert2">' + mensaje + '</label>',
        });
    }
});
