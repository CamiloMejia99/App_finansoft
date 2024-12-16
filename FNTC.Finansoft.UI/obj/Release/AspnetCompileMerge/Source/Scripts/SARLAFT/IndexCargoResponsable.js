$(document).ready(function () {
    $(".btnDetails").click(function (eve) {
        //$("#modal-content").load("/SIAR/Sarlaft/DetailsContexto/" + $(this).data("id"));
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "/SIAR/CargoResponsable/Details",
            data: { id: id },
            datatype: "Json",
            success: function (data) {
                if (data.status) {
                    $("#cargo").text(data.datos.cargo);
                    if (data.datos.estado) { $("#estado").text("Habilitado"); } else { $("#estado").text("Deshabilitado"); }
                }
            }
        });
        $('#myModal').modal('toggle');
    });


    $(".btnDisable").click(function (eve) {

        var info = "";
        var id = $(this).data("id");
        var value = $(this).data("value");
        if (value == 1) { info = "Deshabilitar"; } else { info = "Habilitar"; }

        Swal.fire({
            title: '' + info + ' Macroproceso?',
            text: "",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si,' + info + '!',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {

                $.ajax({
                    type: "POST",
                    url: "/SIAR/CargoResponsable/EstablecerEstado",
                    data: {
                        id: id,
                        value: value
                    },
                    datatype: "Json",
                    success: function (data) {
                        if (data.status) {

                            Swal.fire({
                                title: 'Hecho!',
                                text: "Se ha cambiado el estado",
                                icon: 'success',
                                showCancelButton: false,
                                confirmButtonColor: '#3085d6',
                                cancelButtonColor: '#d33',
                                confirmButtonText: 'Continuar'
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    window.location.reload();
                                }
                            })
                        }
                    }
                });//fin ajax


            }//fin result
        })//fin swal1





    });//fin evento


});