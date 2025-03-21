$(document).ready(function () {
    $("#guardar").click(function () {
        var nombre = $("#tipo").val();
        var id = $("#id").val();
        if (nombre != "") {

            $.ajax({
                url: '/Auditoria/Auditoria/EditVerificaNombreTipoEmpresa',
                datatype: "Json",
                data: {
                    nombre: nombre,
                    id: id
                },
                type: 'post',
            }).done(function (data) {
                if (data.status == true) {
                    Swal.fire({
                        icon: 'error',
                        title: 'El tipo de empresa ya se encuentra registrado!',
                    })
                }
                else if (data.status == false) {
                    $("#theForm").submit();
                }

            });//fin ajax




        } else {

            Swal.fire("Debe digitar un tipo de empresa!", "", "error");
        }
    });


})