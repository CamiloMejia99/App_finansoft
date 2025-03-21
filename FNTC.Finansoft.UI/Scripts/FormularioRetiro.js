$(document).ready(function () {
    var aceptar = ` <label class="fuenteSweetAlert">Aceptar</label>`
    var cancelar = ` <label class="fuenteSweetAlert">Cancelar</label>`
    $(".DeleteForAsoRe").click(function () {
        Swal.fire({
            title: '¿Esta Seguro?',
            html: ` <label class="fuenteSweetAlert">No podrá revertir este cambio!</label>`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#8ED813',
            cancelButtonColor: '#FF2929',
            confirmButtonText: aceptar,
            cancelButtonText: cancelar
        }).then((result) => {
            if (result.value) {
                var id = $(this).val();

                $.ajax({
                    url: '/FormularioRetiro/FormularioRetiro/Delete',
                    datatype: "Json",
                    data: { id: id },//solo para enviar datos
                    type: 'post',
                }).done(function (data) {
                    if (data.status == true) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Eliminado con éxito!',
                            showConfirmButton: false,
                            timer: 1600
                        });
                        setTimeout(espera, 1000);
                    }
                    else if (data.status == false) {
                        if (data.formulario == 'liquidacion') {
                            Swal.fire({
                                title: "NO SE PUEDE ELIMINAR: En proceso de liquidación!",
                                icon: 'error'
                            });
                        } else {
                            Swal.fire({
                                title: "NO SE PUEDE ELIMINAR",
                                html:'<label class="fuenteSweetAlert">Los formularios únicamente se podrán eliminar el mismo día de su creación, si desea eliminarlo comuníquese con Administración </label>',
                                icon: 'error'
                            });
                        }
                       
                    }

                });
            }
        })  
    });

    function espera() {
        location.reload();
    }
   
});