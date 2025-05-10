$(document).ready(function () {
    var correo = "";

    // ---------------------------------- Enviar correo Ahorro contractual --------------------------------------------

    $("#BtnEnviarEmailAhorroContractual").click(function () {

        var nit = $("#Tercero").val();
        //var nit = $("#nitAsociado").val();

        if (nit != "") {
            $.ajax({
                url: '/Email/Email/getCorreo',
                datatype: "Json",
                data: { nit: nit },//solo para enviar datoss
                type: 'post',
            }).done(function (data) {
                if (data.status == true) {

                    correo = data.correo;
                    $('#modalEmail').modal('toggle');

                }
                else if (data.status == false) {
                    Swal.fire({
                        icon: 'warning',
                        title: " " + data.error,
                        text: ''
                    })
                }

            });//fin ajax getCorreo
        }//fin if

    });

    $("#btnEnviaCorreoAhorroContractual").click(function () {
        $('#modalEmail').modal('hide');
        var asunto = $("#txtAsunto").val();
        var mensaje = $("#txtMensaje").val();
        var nit = $("#Tercero").val();
        var id = $("#idRecibo").val();
        $.ajax({
            beforeSend: function () {
                $('#modalcargar').modal({ backdrop: 'static', keyboard: false })

            },
            url: '/Email/Email/EnviarCorreoAhorroContractual',
            datatype: "Json",
            data: {
                asunto: asunto,
                mensaje: mensaje,
                para: correo,
                nit: nit,
                id: id
            },//solo para enviar datos
            type: 'post',
        }).done(function (data) {
            if (data.status == true) {
                $('#modalcargar').modal('hide');
                Swal.fire(
                    'Enviado!',
                    'Su correo ha sido enviado exitosamente.',
                    'success'
                )

            }
            else if (data.status == false) {
                $('#modalcargar').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Hubo un problema al envíar el correo!'
                })
            }

        });//fin ajax enviarCorreo


    });//fin 
})

