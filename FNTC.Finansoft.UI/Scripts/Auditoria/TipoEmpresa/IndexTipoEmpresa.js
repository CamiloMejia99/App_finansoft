$(document).ready(function () {
   
    $("#myTable").on('click', '.EditarTipoEmpresa', function () {
        var fila = $(this).parents('tr');
        var id = $('td:nth-child(1)', fila).text();

        var aceptar = ` <label class="fuenteSweetAlert">Aceptar</label>`
        var cancelar = ` <label class="fuenteSweetAlert">Cancelar</label>`


        Swal.fire({
            title: '¿Seguro desea eliminar este registro?',
            text: "",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#8ED813',
            cancelButtonColor: '#FF2929',
            confirmButtonText: aceptar,
            cancelButtonText: cancelar
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: '/Auditoria/Auditoria/DeleteTipoEmpresa',
                    datatype: "Json",
                    data: { id: id },//solo para enviar datos
                    type: 'post',
                }).done(function (data) {
                    if (data.status == true) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Registro eliminado exitosamente',
                            showConfirmButton: false,
                            timer: 1000
                        })
                        setTimeout(espera, 1000);
                        ////swal("Eliminado!", "Registro eliminado exitosamente.", "success");
                        //window.location.href = "/Auditoria/Auditoria/IndexTipoEmpresa";
                    }
                    else if (data.status == false) {
                        Swal.fire(data.mensaje,"", "error");
                    }

                });//fin ajax
            }
        })

        


    });

    function espera() {
        location.reload();
    }

    
})