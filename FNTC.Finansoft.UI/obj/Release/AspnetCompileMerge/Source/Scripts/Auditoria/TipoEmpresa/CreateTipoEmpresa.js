﻿$(document).ready(function () {
    $("#guardar").click(function () {
        var nombre = $("#tipo").val();
        if (nombre != "") {

            $.ajax({
                url: '/Auditoria/Auditoria/CreateVerificaNombreTipoEmpresa',
                datatype: "Json",
                data: { nombre: nombre },
                type: 'post',
            }).done(function (data) {
                if (data.status == true) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: 'El tipo de empresa ya se encuentra registrado!',
                    })
                }
                else if (data.status == false) {
                    $("#theForm").submit();
                }

            });//fin ajax



            
        } else {

            swal("Debe digitar un tipo de empresa!","", "error");
        }
    });
})