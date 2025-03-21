$(document).ready(function () {
    $("#btnGuardar").click(function () {

        var id = $("#nit").val();
        
        if (id == "") {
            Swal.fire({
                icon: 'warning',
                title: 'Seleccione un asociado',
                text: ''
            })
        }
        else {
            $.ajax({
                url: '/Terceros/TercerosFallecidos/VerificaAsociadoFallecido',
                datatype: "Json",
                data: { id: id },//solo para enviar datos
                type: 'post',
            }).done(function (data) {
                if (data.status == true) {

                    Swal.fire({
                        icon: 'warning',
                        title: 'El asociado ya se encuentra registrado como fallecido',
                        text: ''
                    })
                }
                else if (data.status == false) {
                    $("#theForm").submit();
                }

            });

        }

        

        
    });

    $("#btnCerrar").click(function () {
        window.location.href = '/Terceros/TercerosFallecidos/Index';
    });



});

