$(document).ready(function () {

    $('#Crear').click(function () {
        $("#Funcion").val($("#func").val());
        $("#TipoComprobante").val($("#tipComp").val());

        var cuenta = $("#Cuenta_Cod").val();
        var funcion = $("#func").val();
        var tipC = $("#tipComp").val();
        var linea = $("#lineaId").val();
        var destino = $("#destinoId").val();
        var desc = $("#Cuenta_Descripcion").val();

        if (cuenta != "" && funcion != "" && tipC != "" && linea != "" && destino != "" && desc !="") {

            $('#theForm').submit();
        }
        else {
            Swal.fire({
                icon: 'info',
                title: 'Información!',
                text: 'Por favor, llene todos los campos'
            });
        }

    });

    $('#editar').click(function () {
        $("#Funcion").val($("#func").val());
        $("#TipoComprobante").val($("#tipComp").val());

        var cuenta = $("#Cuenta_Cod").val();
        var descr = $("#Cuenta_Descripcion").val();
        var funcion = $("#func").val();
        var tipC = $("#tipComp").val();
        var linea = $("#lineaId").val();
        var destino = $("#destinoId").val();

        if (cuenta != "" && funcion != "" && tipC != "" && linea != "" && destino != "" & descr !="") {

            $('#theForm').submit();
        }
        else {
            Swal.fire({
                icon: 'info',
                title: 'Información!',
                text: 'Por favor, llene todos los campos'
            });
        }

    });


    //autocomplete cuenta Contable #CAMBIOS JUN/2017
    $("#Cuenta_Cod").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/accounting/plandecuentas/GetCuentasAA",
                //url: "/accounting/plandecuentas/GetCuentas4Selects",
                type: "POST",
                dataType: "json",
                data: {
                    term: request.term
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.NOMBRE, value: item.CODIGO };
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            $('#Cuenta_Descripcion').val(ui.item.label);
            return false;
        }, change: function (event, ui) {
            if (!ui.item) {
                $(this).val("");
                $('#Cuenta_Descripcion').val("");
            }
        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        },
        focus: function (event, ui) {
            $('#Cuenta_Cod').val(ui.item.value);
            return false;
        }
    });//autocomplete cuenta Contable

})