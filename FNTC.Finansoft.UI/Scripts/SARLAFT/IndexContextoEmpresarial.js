$(document).ready(function () {

    $("#btnAgregar").click(function () {


        $.ajax({
            url: '/SIAR/Sarlaft/VerificaContextoEmpresa',
            datatype: "Json",
            type: 'post',
        }).done(function (data) {
            if (data.status) {

                Swal.fire({
                    icon: 'warning',
                    title: 'Aviso',
                    html: ` <label class="fuenteSweetAlert">El contexto de la empresa ya ha sido registrado.</label>`,
                })
                
            }
            else {
                window.location.href = "/SIAR/Sarlaft/CreateContextoEmpresa";
            }
        });
    });

    $(".btnDetails").click(function (eve) {
        //$("#modal-content").load("/SIAR/Sarlaft/DetailsContexto/" + $(this).data("id"));
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "/SIAR/Sarlaft/DetailsContexto",
            data: { id: id },
            datatype: "Json",
            success: function (data) {
                if (data.status) {
                    $("#descripcion").text(data.datos.descripcion);
                    $("#mision").text(data.datos.mision);
                    $("#vision").text(data.datos.vision);
                    $("#objetivo").text(data.datos.objetivo);
                    $("#contextoExterno").text(data.datos.contextoExterno);
                    $("#contextoInterno").text(data.datos.contextoInterno);
                }
            }
        });
        $('#myModal').modal('toggle');
    });


});