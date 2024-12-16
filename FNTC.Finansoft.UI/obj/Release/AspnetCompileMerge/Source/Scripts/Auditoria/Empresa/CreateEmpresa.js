$(document).ready(function () {

    $("#nit").change(function () {

        var nit = $("#nit").val();
        if (nit != "") {
            $.ajax({
                url: '/Auditoria/Auditoria/GetTercero',
                datatype: "Json",
                data: { nit: nit },//solo para enviar datos
                type: 'post',
            }).done(function (data) {
                if (data.status == true) {
                    $("#nombre").val(data.nombre);

                }
                else if (data.status == false) {
                    $("#nit").val("");
                    $("#nombre").val("");
                    Swal.fire({
                        icon: 'error',
                        title: 'Oops...',
                        text: data.mensaje,
                    })
                }

            });//fin ajax
        }

    });

    $("#guardar").click(function () {
        $("#theForm").submit();
    });
})