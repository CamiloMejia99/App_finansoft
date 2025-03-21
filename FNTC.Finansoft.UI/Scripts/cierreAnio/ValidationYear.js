$(document).ready(function () {
    
    $("#btnContabilizar").click(function () {
      
        var anios2 = $("#anios").val();
        var comprobanteId2 = $("#TipoC").val();
        var nit2 = $("#nit").val();
        var cuentaAux2 = $("#aux").val();
        var aceptar = ` <label class="fuenteSweetAlert">Aceptar</label>`
        var cancelar = ` <label class="fuenteSweetAlert">Cancelar</label>`

        if (cuentaAux2 != "" && nit2 != "" && comprobanteId2 != "" && anios2 != "") {
            Swal.fire({
                title: 'Va a ejecutar cierre de año',
                html: `<label class="fuenteSweetAlert2">¿Está seguro?</label>`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#8ED813',
                cancelButtonColor: '#FF2929',
                cancelButtonText: cancelar,
                confirmButtonText: aceptar
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: '/CierreAnio/CierreAnio/VerificaAnio',
                        datatype: "Json",
                        data: {
                            anios: anios2
                        },//solo para enviar datos
                        type: 'post',
                    }).done(function (data) {
                        if (data.status) {
                            Swal.fire({
                                icon: 'warning',
                                title: 'Cierre de año ya existente'
                            })
                        } else {
                            $("#idForm").submit();
                            $('#idForm').html('<div class="loader"> <h2>  <b> Por favor no cierre la pestaña mientras se ejecuta el proceso, Gracias </b> </h2></div>');
                        }

                    });//fin ajax
                }
            })

        } else {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor seleccione un elemento de la lista</label>`,
            });
        }


    });//fin btnAnalizar

});