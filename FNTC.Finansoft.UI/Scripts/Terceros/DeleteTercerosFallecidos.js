$(document).ready(function () {
    $("#example").on('click', 'button.btnEliminar', function () {

        var id = $(this).val();

        Swal.fire({
            title: '¿Seguro desea eliminar este registro?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#8ED813',
            cancelButtonColor: '#FF2929',
            cancelButtonText: 'CANCELAR',
            confirmButtonText: 'ACEPTAR'
        }).then((result) => {
            if (result.value) {

                $.ajax({
                    type: "POST",
                    url: "/Terceros/TercerosFallecidos/Delete",
                    datatype: "Json",
                    data: { id: id },//solo para enviar datos
                    success: function (data) {
                        if (data.status) {
                            Swal.fire(
                                'Eliminado!',
                                '',
                                'success'
                            )
                            var table = $('#example').DataTable();
                            table.ajax.reload();
                        } else {
                            Swal.fire(
                                'No se puede eliminar este Registro!',
                                '',
                                'error'
                            )
                        }

                    }
                });
            };
        });

    });//fin botón eliminar
});