$(document).ready(function () {   
    var correo = "";
    $("#BtnEnviarEmail").click(function () {

        var nit = $("#Tercero").val();

        if (nit != "") {
            $.ajax({
                url: '/Email/Email/getCorreo',
                datatype: "Json",
                data: { nit: nit },//solo para enviar datos
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

    $("#btnEnviaCorreo").click(function () {
        $('#modalEmail').modal('hide');
        var asunto = $("#txtAsunto").val();
        var mensaje = $("#txtMensaje").val();
        var nit = $("#Tercero").val();
        $.ajax({
            beforeSend: function () {
                $('#modalcargar').modal({ backdrop: 'static', keyboard: false })

            },
            url: '/Email/Email/EnviarCorreo',
            datatype: "Json",
            data: {
                asunto: asunto,
                mensaje: mensaje,
                para: correo,
                nit: nit
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


    });//fin btnEnviaCorreo

    
    
    $("#BtnEnviarEmailTodos").click(function () {
       
        var bool = confirm("Una vez ejecutado este proceso no se puede interrumpir el envió de la información, desea continuar?")
        if (bool) {
           $.ajax({
            beforeSend: function () {
                 $('#modalcargar').modal({ backdrop: 'static', keyboard: false })

            },
            url: '/Email/Email/sendAllEmailAsync',
            datatype: "Json",
            type: 'post',
        }).done(function (data) {
            if (data.status == true) {
                $('#modalcargar').modal('hide');
                Swal.fire(
                    'Finalizado!',
                    'Proceso realizado exitosamente.' + data.n + " asociados",
                    'Proceso realizado exitosamente',
                    'success'
                )
            }
            else if (data.status == false) {
                $('#modalcargar').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Hubo un problema, verifique la configuracion del correo en parametros!'
                })
            }

        });//fin ajax enviarCorreo

        } else {
            alert("El envio de datos fue cancelado por el usuario");
        }  
    });//fin BtnEnviarEmailTodos


})

