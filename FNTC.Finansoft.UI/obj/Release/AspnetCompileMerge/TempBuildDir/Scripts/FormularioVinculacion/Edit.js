$(document).ready(function () {

    $("#btnGuardar").click(function () {
        $("#theForm").submit();       
    });   

    GetDataTercero();

    function GetDataTercero() {
        var terceroId = $("#Nit_Tercero").val();
        $.ajax({
            url: '/formularioVinculacion/formatoVinculacions/GetTerceroInfo',
            datatype: "Json",
            data: { terceroId: terceroId },
            type: 'post',
        }).done(function (data) {
            if (data.status) {

                $("#nombre_tercero").val(data.nombre);

                $("#apellido1").val(data.apellido1);
                $("#apellido2").val(data.apellido2);
                $("#lugarfechaexp").val(data.fechalugarexp);
                $("#fechanac").val(data.fechanac);
                $("#lugarnac").val(data.muniNac);
                $("#direccion").val(data.direccion);
                $("#barrio").val(data.barrio);
                $("#telefono").val(data.telefono);
                $("#celular").val(data.celular);
                $("#correo").val(data.correo);
                $("#municipio").val(data.municipioresidencia);
                $("#sexo").val(data.genero);
                $("#civil").val(data.estadocivil);
                $("#vivienda").val(data.vivienda);
                $("#departamentoresidencia").val(data.departamento);
                $("#sueldobasico").val(data.sueldobasico);

            }
            

        });//fin ajax
    }


})