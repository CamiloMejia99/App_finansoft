$(document).ready(function () {
    $("#example").on('click', 'button.btnEliminar', function () {

        var id = $(this).val();

        Swal.fire({
            title: 'Eliminar Producto?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'SI'
        }).then((result) => {
            if (result.value) {

                $.ajax({
                    type: "POST",
                    url: "/facturacion/Producto/Delete",
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
                                'No se puede eliminar este producto!',
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