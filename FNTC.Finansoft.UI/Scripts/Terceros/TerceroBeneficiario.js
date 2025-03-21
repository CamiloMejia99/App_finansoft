$(document).ready(function () {
    $("#idAfiliado").change(function () {
       
        var selectedIdAfiliado = $(this).val();

        $.ajax({
            url: "/Terceros/TerceroBeneficiario/GetNombreAfiliado",
            type: "GET",
            data: { idAfiliado: selectedIdAfiliado },
            success: function (result) {
                $("#NombreAfiliado").text(result);
                $("#alertContainer").show();
            },
            error: function () {
                console.log("Error al obtener el nombre del afiliado.");
            }
        });
    });
});