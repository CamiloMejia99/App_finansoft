$(document).ready(function () {

    $(".idEmpresa").hide();

    $("#registrarEmpresa").click(function () {
        $.ajax({
            url: '/Auditoria/Auditoria/VerificaTiposEmpresa',
            datatype: "Json",
            type: 'post',
        }).done(function (data) {
            if (data.status == true) {
                window.location.href = "/Auditoria/Auditoria/CreateEmpresa"
            }
            else if (data.status == false) {
                swal("Importante!",data.mensaje , "warning");
            }

        });//fin ajax
    });

    $("#myTable").on('click', '.EditarEmpresa', function () {
        var fila = $(this).parents('tr');
        var id = $('td:nth-child(1)', fila).text();


        Swal.fire(
            'Para eliminar este registro debe contactarse con el administrador.',
            '',
            'info'
        )

        //Swal.fire({
        //    title: 'Seguro desea eliminar este registro?',
        //    text: "",
        //    icon: 'question',
        //    showCancelButton: true,
        //    confirmButtonColor: '#3085d6',
        //    cancelButtonColor: '#d33',
        //    confirmButtonText: 'Borrar',
        //    cancelButtonText: 'Cancelar'
        //}).then((result) => {
        //    if (result.value) {
        //        $.ajax({
        //            url: '/Auditoria/Auditoria/DeleteEmpresa',
        //            datatype: "Json",
        //            data: { id: id },//solo para enviar datos
        //            type: 'post',
        //        }).done(function (data) {
        //            if (data.status == true) {
        //                //swal("Eliminado!", "Registro eliminado exitosamente.", "success");
        //                window.location.href = "/Auditoria/Auditoria/Index";
        //            }
        //            else if (data.status == false) {
                        
        //            }

        //        });//fin ajax
        //    }
        //})




    });
    
})