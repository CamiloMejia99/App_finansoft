$(document).ready(function () {

    $("#BtnConsulta").click(function () {
        var Pagare = $("#Pagare").val();
        if (Pagare != "") {
            GetCuotasCredito(Pagare);
        } else {
            Swal.fire({
                icon: 'warning',
                title: 'Advertencia',
                text: 'Por favor seleccione un pagaré'
            })
        }
    });

    $("#btnSalir").click(function () {
        window.location.href = "/OperativaDeCaja/FactOpcajas/ProcesosAdministrador";
    })

    function GetCuotasCredito(Pagare) {
        $("#ListCuotas").empty();
        $.ajax({
            type: "POST",
            url: "/Creditos/ProcesosCrediticios/GetCuotasCredito",
            datatype: "Json",
            data: {
                Pagare: Pagare
            },
            success: function (data) {
                if (data.status) {
                    $.each(data.array, function (index, value) {
                        
                        var tr = '<tr id='+value[0]+'>';
                        tr += '<td>' + value[0] + '</td>';
                        tr += '<td>' + value[1] + '</td>';
                        tr += '<td>' + value[2] + '</td>';
                        tr += '<td>' + value[3] + '</td>';
                        tr += '<td>' + value[4] + '</td>';
                        tr += '<td>' + value[5] + '</td>';
                        tr += '<td>' + value[6] + '</td>';
                        tr += '<td>' + '<input class="form-control DM" type="text" name="DM'+value[0]+'" value="'+value[7]+'" id="DM'+value[0]+'" />'+'</td>';
                        tr += '<td>' + value[8] + '</td>';
                        tr += '<td style="color:#172aff">' + value[9] + '</td>';
                        tr += '<td>' + '<input class="form-control IC miles" type="text" name="IC'+value[0]+'" id="IC'+value[0]+'" value="'+value[3]+'" />'+'</td>';
                        tr += '<td>' + '<input class="form-control IM miles" type="text" name="IM'+value[0]+'" id="IM'+value[0]+'" value="'+value[4]+'" />'+'</td>';
                        tr += '<td>' + '<input class="form-control seguro miles" type="text" name="seguro'+value[0]+'" id="seguro'+value[0]+'" value="'+value[5]+'" />'+'</td>';
                        tr += '<td>' + '<input class="form-control admon miles" type="text" name="admon'+value[0]+'" id="admon'+value[0]+'" value="'+value[6]+'" />'+'</td>';
                        tr += '<td>' + '<button type="button" class="btn btn-warning btn-sm fa fa-calculator BtnCalcular" id="BC' + value[0] + '" title="Calcular interés mora"></button>&nbsp;' + '</td>';
                        tr += '<td>' + '<button class="btn btn-success btn-sm fa fa-search BtnPreview" title="Previsualizar"></button>' + '</td>';
                        tr += '<td>' + '<button class="btn btn-primary btn-sm fa fa-floppy-o BtnGuardar" title="Guardar"></button>' + '</td>';
                        tr += '</tr>';

                        $("#ListCuotas").append(tr);
                        
                    });
                    $("#lblAsociado").text(data.dataAsociado);
                } else {
                    alert("Ha ocurrido un error");
                    $("#lblAsociado").text("");
                }
                
            }
        });//fin ajax
    };



    $("#ListCuotas").on('click', '.BtnCalcular', function () {

        var Fila = $(this).parents('tr');
        var Id = $('td:nth-child(1)',Fila).text();
        
        var DiasMora = $("#DM"+Id).val();
        //var Capital = $('td:nth-child(3)',Fila).text();

        if (DiasMora != "" && DiasMora!="0") {
            CalcularIM(Id, DiasMora,Fila);
        } else {
            $("#DM" + Id).val("0");
            $("#IM" + Id).val("0");
            SumaParcial(Fila, Id);
        }

    });

    $("#ListCuotas").on('click', '.BtnPreview', function () {

        var Fila = $(this).parents('tr');
        var Id = $('td:nth-child(1)', Fila).text();
        SumaParcial(Fila, Id);
    });

    $("#ListCuotas").on('click', '.BtnGuardar', function () {

        var Fila = $(this).parents('tr');
        var Id = $('td:nth-child(1)', Fila).text();

        var IC = $("#IC" + Id).val();
        var IM = $("#IM" + Id).val();
        var seguro = $("#seguro" + Id).val();
        var admon = $("#admon" + Id).val();

        if (IC >= 0 && IC != "" && IM >= 0 && IM != "" && seguro >= 0 && seguro != "" && admon >= 0 && admon != "") {
            Swal.fire({
                title: 'Continuar con esta operación?',
                text: "Podrá volver a modificar los valores en cualquier momento",
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Aceptar',
                cancelButtonText: 'Cancelar',
            }).then((result) => {
                if (result.isConfirmed) {
                    GuardarValores(Fila, Id, IC, IM,seguro,admon);
                }
            })
        } else {
            Swal.fire({
                icon: 'warning',
                title: 'Advertencia',
                text: 'Los valores ingresados contienen errores, por favor verifique e intente nuevamente.'
            })
        }
        
    });
    
    function GuardarValores(Fila,Id,IC,IM,seguro,admon) {
        $.ajax({
            type: "POST",
            url: "/Creditos/ProcesosCrediticios/GuardarValores",
            datatype: "Json",
            data: {
                Id: Id,
                IC: IC,
                IM: IM,
                seguro: seguro,
                admon: admon
            },
            success: function (data) {
                if (data.status) {
                    $('td:nth-child(4)', Fila).text(data.NuevoIC);
                    $('td:nth-child(5)', Fila).text(data.NuevoIM);
                    $('td:nth-child(6)', Fila).text(data.NuevoSeguro);
                    $('td:nth-child(7)', Fila).text(data.NuevoAdmon);
                    $('td:nth-child(10)', Fila).text(data.NuevoValorCuota);
                    Swal.fire(
                        'Listo!',
                        'Valores guardados correctamente',
                        'success'
                    )
                } else {
                    alert("Ha ocurrido un error");
                }

            }
        });//fin ajax
    }

    function CalcularIM(Id,DiasMora,Fila)//calcular interés de mora
    {
        $.ajax({
            type: "POST",
            url: "/Creditos/ProcesosCrediticios/CalcularIM",
            datatype: "Json",
            data: {
                Id: Id,
                DiasMora: DiasMora             
            },
            success: function (data) {
                if (data.status) {
                    $("#IM" + Id).val(data.NuevoIM);
                    SumaParcial(Fila, Id);
                } else {
                    alert("Ha ocurrido un error");
                }

            }
        });//fin ajax
    };

    function SumaParcial(Fila,Id) {
        var Capital = parseInt(($('td:nth-child(3)', Fila).text()).split('.').join(""));
        var IC = parseInt(($("#IC"+Id).val()).split('.').join(""));
        var IM = parseInt(($("#IM" + Id).val()).split('.').join(""));
        var Seguro = parseInt(($("#seguro"+Id).val()).split('.').join(""));
        var Admon = parseInt(($("#admon"+Id).val()).split('.').join(""));

        var Total = Capital + IC + IM + Seguro+Admon;

        $('td:nth-child(10)', Fila).text(formatNumber.new(Total));
        
    };

    var formatNumber = {
        separador: ".", // separador para los miles
        sepDecimal: ',', // separador para los decimales
        formatear: function (num) {
            num += '';
            var splitStr = num.split('.');
            var splitLeft = splitStr[0];
            var splitRight = splitStr.length > 1 ? this.sepDecimal + splitStr[1] : '';
            var regx = /(\d+)(\d{3})/;
            while (regx.test(splitLeft)) {
                splitLeft = splitLeft.replace(regx, '$1' + this.separador + '$2');
            }
            return this.simbol + splitLeft + splitRight;
        },
        new: function (num, simbol) {
            this.simbol = simbol || '';
            return this.formatear(num);
        }
    }

    
});