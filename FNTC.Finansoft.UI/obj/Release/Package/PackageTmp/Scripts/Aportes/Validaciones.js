$(document).ready(function () {

    $("#VistaCuentasDistribucion").click(function () {
        $.ajax({
            url: '/Aportes/Aportes/VerificarConfiguracion',
            method: 'GET'
        }).done(function (data) {
            if (data) {
                window.location.href = "/Aportes/Aportes/OtrasCuentas";
            }
            else{
                swal({
                    icon: 'warning',
                    title: 'Accion no permitida',
                    text: 'No existe una configuracion en aportes o ahorros'
                });
            }

        });
    });

});