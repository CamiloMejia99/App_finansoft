$(document).ready(function () {

    $(".DeleteForAsoRe").click(function () {

        Swal.fire({
            title: 'Estás seguro?',
            text: "No podrás revertir este cambio!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, borralo!'
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
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                        location.reload(true)
                    }
                    else if (data.status == false) {
                        swal("NO SE PUEDE ELIMINAR: En proceso de liquidación!");
                    }

                });
            }
        })
                
            
        
    });

    

});