$(document).ready(function () {


    $("#btnVacaciones").click(function () {

        var periodo = $("#Fk_discriminacion").val();
        var afiliado = $("#archivo_afiliados").val();

        if (periodo && afiliado) {
            $("#FormVacacion").submit();
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
