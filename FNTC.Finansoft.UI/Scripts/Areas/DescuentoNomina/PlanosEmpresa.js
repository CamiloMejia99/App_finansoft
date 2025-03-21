$(document).ready(function () {

    $("#Tercero").chosen().change(function () {
        var Tercero = $("#Tercero").val();
        if (Tercero != "") {
            $.ajax({
                type: "POST",
                url: "/Nomina/PlanoEmpresas/GetDatosTerceros",
                datatype: "Json",
                data: { NIT: $('#Tercero').val() },
                success: function (data) {
                    $("#Codigo").val(data[0]);
                    $("#Nombre").val(data[1]);
                }
            });
        }
    });

    $("#btnGuardar").click(function () {

        var tercero = $("#Tercero").val();
        var plano = $("#NOMPLANO").val();

        if (tercero != "" && plano != "") {
            var Tercero = $("#Tercero").val();
            $.ajax({
                type: "POST",
                url: "/Nomina/PlanoEmpresas/ValidarEmpresa",
                datatype: "Json",
                data: { Nit_empresa: Tercero, id_plano: plano },
                success: function (data) {
                    if (data.status) {
                        Swal.fire({
                            icon: 'warning',
                            title: 'El plano ya esta registrado con la empresa seleccionada'
                        })
                    } else {
                        $("#theForm").submit();
                    }
                }
            });
        } else {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor llene todos los campos!</label>`,
            });
        }
    });//fin btnGuardar



    $("#btnEditarPE").click(function () {
        var Tercero = $("#Tercero").val();
        var plano = $("#Clase_Plano").val();
        var id = $("#id_plano_empresa").val();
        $.ajax({
            type: "POST",
            url: "/Nomina/PlanoEmpresas/ValidarEmpresaEditar",
            datatype: "Json",
            data: { Nit_empresa: Tercero, id_plano: plano, id_plano_empresa: id },
            success: function (data) {
                if (data.status) {
                    Swal.fire({
                        icon: 'warning',
                        title: 'El plano ya esta registrado con la empresa seleccionada'
                    })
                } else {
                    $("#theForm").submit();
                }
            }
        });
    });//fin btnGuardar



    
});