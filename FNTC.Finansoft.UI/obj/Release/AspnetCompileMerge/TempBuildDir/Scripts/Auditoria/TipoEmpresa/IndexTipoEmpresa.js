$(document).ready(function () {
   
    $("#myTable").on('click', '.EditarTipoEmpresa', function () {
        var fila = $(this).parents('tr');
        var id = $('td:nth-child(1)', fila).text();


        Swal.fire({
            title: 'Seguro desea eliminar este registro?',
            text: "",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Borrar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: '/Auditoria/Auditoria/DeleteTipoEmpresa',
                    datatype: "Json",
                    data: { id: id },//solo para enviar datos
                    type: 'post',
                }).done(function (data) {
                    if (data.status == true) {
                        //swal("Eliminado!", "Registro eliminado exitosamente.", "success");
                        window.location.href = "/Auditoria/Auditoria/IndexTipoEmpresa";
                    }
                    else if (data.status == false) {
                        swal("Error!", data.mensaje, "error");
                    }

                });//fin ajax
            }
        })

        


    });

    
})