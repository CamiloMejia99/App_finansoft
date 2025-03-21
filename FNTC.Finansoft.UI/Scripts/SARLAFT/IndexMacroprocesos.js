$(document).ready(function () {
    $(".btnDetails").click(function (eve) {
        //$("#modal-content").load("/SIAR/Sarlaft/DetailsContexto/" + $(this).data("id"));
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "/SIAR/Sarlaft/DetailsMacroproceso",
            data: { id: id },
            datatype: "Json",
            success: function (data) {
                if (data.status) {
                    $("#codigo").text(data.datos.codigo);
                    $("#nombre").text(data.datos.nombre);
                    $("#objetivo").text(data.datos.objetivo);
                    if (data.datos.estado) { $("#estado").text("Habilitado"); } else { $("#estado").text("Deshabilitado");}
                }
            }
        });
        $('#myModal').modal('toggle');
    });


    $(".btnDisable").click(function (eve) {

        var aceptar = ` <label class="fuenteSweetAlert">Sí, </label>`
        var cancelar = ` <label class="fuenteSweetAlert">Cancelar</label>`

        var info = "";
        var id = $(this).data("id");
        var value = $(this).data("value");
        if (value == 1) { info = "Deshabilitar"; } else { info = "Habilitar";}

        Swal.fire({
            title: ''+info+' Macroproceso?',
            text: "",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#8ED813',
            cancelButtonColor: '#FF2929',
            confirmButtonText: aceptar +" "+ info,
            cancelButtonText: cancelar
        }).then((result) => {
            if (result.isConfirmed) {

                $.ajax({
                    type: "POST",
                    url: "/SIAR/Sarlaft/EstablecerEstado",
                    data: {
                        id: id,
                        value: value
                    },
                    datatype: "Json",
                    success: function (data) {
                        if (data.status) {

                            Swal.fire({
                                title: 'Hecho!',
                                html: ` <label class="fuenteSweetAlert">Se ha cambiado el estado!</label>`,
                                icon: 'success',
                                showCancelButton: false,
                                confirmButtonColor: '##8ED813',
                                cancelButtonColor: '#d33',
                                confirmButtonText: 'OK!'
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