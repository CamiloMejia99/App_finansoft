$(document).ready(function () {

    $(".genero").each(function () {
        var gen = $(this).val();
        if (gen == "M") { $(this).val("Masculino"); } else if (gen == "F") { $(this).val("Femenino"); } else { $(this).val("");}
    });

    GetEstadoCivil();
    GetTipoVivienda();

    function GetEstadoCivil() {
        var data = $("#estadoCivil").val();
        $.ajax({
            url: '/formularioVinculacion/formatoVinculacions/GetEstadoCivil',
            datatype: "Json",
            data: { data: data },//solo para enviar datos
            type: 'post',
        }).done(function (data) {
            if (data.status == true) {

                $("#estadoCivil").val(data.estadoCivil);
                
            }
            
        });
    }//fin funcion GetEstadoCivil

    function GetTipoVivienda() {
        var data = $("#tipoVivienda").val();
        $.ajax({
            url: '/formularioVinculacion/formatoVinculacions/GetTipoVivienda',
            datatype: "Json",
            data: { data: data },//solo para enviar datos
            type: 'post',
        }).done(function (data) {
            if (data.status == true) {

                $("#tipoVivienda").val(data.tipoVivienda);

            }

        });
    }//fin funcion GetEstadoCivil

});