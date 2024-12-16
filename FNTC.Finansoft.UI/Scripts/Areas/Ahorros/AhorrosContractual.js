$(document).ready(function () {

    inicializarValores();

    function Table() {

        var botones = [
            //{
            //    extend: 'collection',
            //    text: 'Exportar A',
            //    autoClose: true,
            //    buttons: [
            //        {
            //            extend: 'excel',
            //            text: "Excel",
            //            exportOptions: {
            //                columns: [1, 2]
            //            }

            //        }
            //    ]

            //}
            //'excel', 
            {
                text: "Agregar configuración",
                action: function () {
                    $("#AddConfiguracion").modal("show");
                },
                className: 'btn btn-success btn-sm fa fa-plus',
            },
            {

                extend: 'pdf',
                title: "LÍNEAS DE CRÉDITO",
                className: 'btn btn-default btn-sm fa'

            }

        ]; //fin botones

        agregarDataTable("#tablaConfigAhorroContractual", '/Ahorros/Ahorros/GetConfigAhorroContractual', botones, false, true, false);
        function agregarDataTable(tablaLineas, urlDatos, botones, scroll, buscador, seleccion) {
            var TraduccionDatatable = {
                "sProcessing": "Procesando...", "sLengthMenu": "Mostrar _MENU_ registros", "sZeroRecords": "No se encontraron resultados", "sEmptyTable": "No hay registros", "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros", "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros", "sInfoFiltered": "(filtrado de un total de _MAX_ registros)", "sInfoPostFix": "", "sSearch": "Buscar:", "sUrl": "", "sInfoThousands": ",", "sLoadingRecords": "Cargando...", "select": { "rows": { _: "Has seleccionado %d filas", 0: "", 1: "1 fila seleccionada" } }, "oPaginate": { "sFirst": "<<", "sLast": ">>", "sNext": ">", "sPrevious": "<" }, "oAria": { "sSortAscending": ": Activar para ordenar la columna de manera ascendente", "sSortDescending": ": Activar para ordenar la columna de manera descendente" }
            };
            // iris2 = iris[c(1;10, 51:60, 101:110), ]
            table = $(tablaLineas).DataTable({
                destroy: true,
                dom: 'Bfrtip',
                "ajax": {
                    "method": "POST",
                    "url": urlDatos,
                    "data": function (data) { return data = JSON.stringify(data); }
                },
                searching: buscador,
                lengthChange: false,
                autoWidth: false,
                scrollX: scroll,
                buttons: botones,
                deferRender: true,
                select: seleccion,
                language: TraduccionDatatable,
                paging: true,
                lengthMenu: [10, 25, 50, 100],
                columnDefs: [{"sClass": "hide_me", "aTargets":[0]}]
            });

        } // fin funcion agregarDataTable

    }

    Table();

    $("#guardarCHA").click(function () {
        $("#formConfAhoCont").submit();
    });
    $("#EditguardarCHA").click(function () {
        $("#formEditConfAhoCont").submit();
    });


    $(".rango").change(function () {
        try {
            let valorMinimo = parseInt($("#AuxValorMinimo").val().split('.').join(""));
            let valorMaximo = parseInt($("#AuxValorMaximo").val().split('.').join(""));
            if (isNaN(valorMinimo) || isNaN(valorMaximo))
                return false;

            if (valorMinimo > valorMaximo) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Información.',
                    text: 'El valor máximo debe ser mayor o igual que el valor mínimo.'
                });
                $(this).val("");
            }

        } catch (e) {

        }
    });

    $(".tasa").change(function () {
        try {
            let valor = parseFloat($(this).val());
            if (!(valor >= 0 && valor <= 100)) {
                $(this).val("");
            } else {
                var id = $(this).attr('id');
                verificarTasa(id);
            }
        } catch (e) {
            $(this).val("");
        }
    });

    function verificarTasa(id) {
        try {
            let valorMinimo = parseFloat($("#AuxTasaEfectivaMinima").val().split(',').join("."));
            let valorMaximo = parseFloat($("#AuxTasaEfectivaMaxima").val().split(',').join("."));
            if (isNaN(valorMinimo) || isNaN(valorMaximo))
                return false;

            if (valorMinimo > valorMaximo) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Información.',
                    text: 'El valor de tasa máxima debe ser mayor o igual que el valor de tasa mínima.'
                });
                $("#"+id).val("");
            }

        } catch (e) {

        }
    }

    function inicializarValores() {
        let plazoMinimo = $("#PlazoMinimo").val();//
        if(plazoMinimo==null || plazoMinimo=="")
            $("#PlazoMinimo").val("0");

        let cuotaGraciaMora = $("#CuotasGraciaMora").val();//
        if (cuotaGraciaMora == null || cuotaGraciaMora == "")
            $("#CuotasGraciaMora").val("0");

        if ($("#SeCausaEnMora").prop('checked'))//
            quitarReadOnly("CuotasGraciaMora");
        else
            ponerReadOnly("CuotasGraciaMora");


    }

    $("#btnCAC").click(function () {
        window.location.href = "/Ahorros/Ahorros/ConfiguracionFAC";
    });
    $("#btnEditCAC").click(function () {
        window.location.href = "/Ahorros/Ahorros/ConfiguracionAhorroContractual";
    });

    $("#PlazoMinimo").change(function () {
        let valor = $(this).val();
        if (valor == null || valor == "")
            $(this).val("0");
    });
    $("#CuotasGraciaMora").change(function () {
        let valor = $(this).val();
        if (valor == null || valor == "")
            $(this).val("0");
    });

    $("#SeCausaEnMora").change(function () {
        if ($(this).prop('checked')) {
            quitarReadOnly("CuotasGraciaMora");
        } else {
            ponerReadOnly("CuotasGraciaMora");
            $("#CuotasGraciaMora").val("0");
        }
    });

    function ponerReadOnly(id) {
        // Ponemos el atributo de solo lectura
        $("#" + id).attr("readonly", "readonly");
        // Ponemos una clase para cambiar el color del texto y mostrar que
        // esta deshabilitado
        $("#" + id).addClass("readOnly");
    }

    function quitarReadOnly(id) {
        // Eliminamos el atributo de solo lectura
        $("#" + id).removeAttr("readonly");
        // Eliminamos la clase que hace que cambie el color
        $("#" + id).removeClass("readOnly");
    }

    //$("#tablaConfigAhorroContractual").on('click','.editAC',function(e) {
    //    e.preventDefault();
    //    var filaactual = $(this).closest("tr");
    //    var Id = filaactual.find("td:eq(0)").text();
    //    llenarCamposEdit(Id);

    //    //var url = $(this).attr("href");
    //    //$('#detailsAssetModal').modal('toggle');
    //});

    //function llenarCamposEdit(id) {
    //    limpiarInputEdit();
    //    $.ajax({
    //        url: '/Ahorros/Ahorros/GetConfigAhoCont',
    //        datatype: "Json",
    //        data: { id: id },//solo para enviar datos
    //        type: 'post',
    //    }).done(function (data) {
    //        if (data.status) {
    //            $("#Id").val(data.model.Id);
    //            $("#EditNombreConfiguracion").val(data.model.NombreConfiguracion);
    //            $("#EditPrefijo").val(data.model.Prefijo);
    //            $("#EditAuxValorMinimo").val(data.model.AuxValorMinimo);
    //            $("#EditAuxValorMaximo").val(data.model.AuxValorMaximo);
    //            $("#EditIdComprobante").val(data.model.IdComprobante);
    //            $("#EditIdCuenta").val(data.model.IdCuenta);
    //            if (data.model.SeCausa)
    //                document.getElementById("EditSeCausa").checked = true;
    //            else
    //                document.getElementById("EditSeCausa").checked = false;
    //            $("#EditTasaEfectiva").val(data.model.TasaEfectiva);
    //            if (data.model.Morosidad)
    //                document.getElementById("EditMorosidad").checked = true;
    //            else
    //                document.getElementById("EditMorosidad").checked = false;
    //            if (data.model.Estado)
    //                document.getElementById("EditEstado").checked = true;
    //            else
    //                document.getElementById("EditEstado").checked = false;
    //            $("#FechaRegistro").val(data.model.FechaRegistro);
    //            $("#UserId").val(data.model.UserId);
                
    //            //$('#EditIdCuenta> option[value="' + data.model.IdCuenta+'"]').attr('selected', 'selected');
                
    //            $("#EditConfiguracion").modal("show");
                
    //        }
    //    });
    //}

    //function limpiarInputEdit() {
    //    $(".edit").each(function () {
    //        $(this).val("");
    //    });
    //}

    //$("#EditNombreConfiguracion").val("sfsfs");
    //$("#EditConfiguracion").modal("show");

    
})