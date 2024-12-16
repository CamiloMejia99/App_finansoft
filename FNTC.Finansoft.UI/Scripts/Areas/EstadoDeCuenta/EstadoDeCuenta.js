$(document).ready(function () {

    OcultarTablas();
    

    $("#Tercero").chosen().change(function () {
        if (!($("#Tercero").val() == "")) {
            $('#mitabla tr').empty();
            $('#tablaCreditos tr').empty();
            OcultarTablas();

            var cuenta = "";
            var fechaApertura = "";
            var cuota = "";
            var saldoActual = "";
            var saldoEnMora = "";
            var intereses = "";
            var saldoEnCanje = "";
            var EstadoFicha = "";


            $.ajax({
                url: '/Terceros/TercerosFallecidos/VerificaFallecido',
                datatype: "Json",
                data: { nit: $('#Tercero').val() },//solo para enviar datos
                type: 'post',
            }).done(function (data) {
                if (data.status == true) {

                    document.getElementById('alertFallecido').style.display = 'block';
                }
                else if (data.status == false) {
                    document.getElementById('alertFallecido').style.display = 'none';
                }

            });


            $.ajax({
                type: "POST",
                url: "/EstadoCuenta/EstadoDeCuenta/GetDatosEstadoCuenta",
                datatype: "Json",
                data: { NIT: $('#Tercero').val() },
                success: function (data) {
                    $("#nombreTercero").text(data.modelTercero.Nombre);
                    $("#nomCerApo").text(data.modelTercero.Nombre); // imprime variable para modal certificado de aporte
                    $("#cedCerApo").text($('#Tercero').val()); // imprime variable para modal certificado de aporte
                    $("#salario").text(data.modelTercero.Salario);
                    $("#antiguedad").text(data.modelTercero.Antiguedad + " " + "MESES");
                    $("#agencia").text(data.modelTercero.Agencia);
                    //var fff = data[5];
                    //$("#fecCerApo").text(fff.substring(0, 10)); // imprime variable para modal certificado de aporte
                    //$("#salCerApo").text(formatNumberMiles.new(data[7])); // imprime variable para modal certificado de aporte
                    // cuenta = data[4];
                    //fechaApertura = data[5];
                    //EstadoFicha = data[10];
                    //cuota = formatNumberMiles.new(data[6]);
                    //saldoEnMora = parseInt(data[8]);
                    //if (saldoEnMora < 0) {
                    //    saldoEnMora = 0;
                    //} else {
                    //    saldoEnMora = formatNumberMiles.new(data[8]);
                    //}
                    //saldoActual = formatNumberMiles.new(data[7]);
                    //saldoEnMora = saldoEnMora;
                    $.each(data.modelFichasAportes, function (key, val) {
                        var cuenta = val.Cuenta;
                        var FechaApertura = val.FechaApertura;
                        var Cuota = val.Cuota;
                        var SaldoActual = val.SaldoActual;
                        var SaldoMora = val.SaldoMora;
                        var Intereses = val.Intereses;
                        var SaldoCanje = val.SaldoCanje;
                        var Estado = val.Estado;
                        var fila = "<tr><td>" + cuenta + "</td><td>" + FechaApertura + "</td><td>" + Cuota + "</td><td>" + SaldoActual + "</td><td>" + SaldoMora + "</td><td>" + Intereses + "</td><td>" + SaldoCanje + "</td><td><b>" + Estado + "</b></td><td><button type='button' class='btnVerAportes btn btn - info btn - lg' data-toggle='modal' data-target='#modalVerAportes' data-producto='1'>Ver</button></td></tr>";
                        var btn = document.createElement("TR");
                        btn.innerHTML = fila;
                        document.getElementById("mitabla").appendChild(btn);
                    });

                }
            });

            var fechaDesembolso = "";
            var cuota = "";
            var pagare = "";
            var totalPagado = "";
            var saldoCapital = "";
            var plazo = "";


            $.ajax({ //OBTIENE LOS CRÉDITOS
                type: "POST",
                url: "/EstadoCuenta/EstadoDeCuenta/GetCreditos",
                data: { NIT: $('#Tercero').val() },
                datatype: "Json",
                success: function (data) {
                    if (data != "NO") {
                        $.each(data.prestamoss, function (key, val) {

                            var tr = '<tr>'; //nueva fila
                            tr += '<td>' + val[0] + '</td>';
                            tr += '<td>' + val[1] + '</td>';
                            tr += '<td>' + val[2] + '</td>';
                            tr += '<td>' + val[3] + '</td>';
                            tr += '<td>' + val[4] + '</td>';
                            tr += '<td>' + val[5] + '</td>';
                            tr += '<td>' + val[6] + '</td>';
                            tr += '<td><button type="button" class="btnVerCuotasPagadas btn btn - info btn - lg" data-toggle="modal" data-target="#modalVerCuotasCreditos">Ver</button></td>'
                            tr += '</tr>';
                            $('#tablaCreditos').append(tr);//añadiendo toda la fila a la tabla
                        });//fin foreach
                        //..............

                        var tr = '<tr>';
                        tr += '<td></td>';
                        tr += '<td>' + '<b>Total Saldo Capital</b>' + '</td>';
                        tr += '<td></td>';
                        tr += '<td>' + '<b>' + data.totalCreditos + '</b>' + '</td>';
                        tr += '<td></td>';
                        tr += '<td>' + '<b>' + data.totalSaldoMora + '</b>' + '</td>';
                        tr += '<td>' + '<b>' + data.totalGeneral + '</b>' + '</td>';
                        tr += '<td></td>';
                        tr += '</tr>';

                        $('#tablaCreditos').append(tr);
                    }//fin condicion IF

                }//fin success
            });   //fin ajax

            //se lista ahorros permanentes
            GetDatosAhorroPermanente($("#Tercero").val());
            GetDatosAhorroContractual($("#Tercero").val());
            GetDatosAporteExtra($("#Tercero").val());

        }
    });





    //OTRAS FUNCIONES
    function OcultarTablas() {
        $('#divAhorroPermanente').hide();
        $('#divAhorroContractual').hide();
        $('#divAporteExtra').hide();
    }

    function LimpiarTablas() {
        $('#tablaAhorroPermanente tr').empty();
        $('#tablaAhorroContractual tr').empty();
        $('#tablaAporteExtra tr').empty();
    }
    //............

    //FUNCIONES AHORRO PERMANENTE
    function GetDatosAhorroPermanente(nit) {
        $('#tablaAhorroPermanente tr').empty();
        $.ajax({
            type: "POST",
            url: "/EstadoCuenta/EstadoDeCuenta/GetDatosAhorroPermanente",
            datatype: "Json",
            data: { NIT: $('#Tercero').val() },
            success: function (data) {
                if (data.status) {
                    $.each(data.list, function (key, val) {
                        var cuenta = val.Cuenta;
                        var FechaApertura = val.FechaApertura;
                        var SaldoActual = val.SaldoActual;
                        var Rendimiento = val.Rendimiento;
                        var Estado = val.Estado;
                        var fila = "<tr><td>" + cuenta + "</td><td>" + FechaApertura + "</td><td>" + SaldoActual + "</td><td>" + Rendimiento + "</td><td><b>" + Estado + "</b></td><td><button type='button' class='btnVerAportes btn btn - info btn - lg' data-toggle='modal' data-target='#modalVerAportes' data-producto='3' >Ver</button></td></tr>";
                        var btn = document.createElement("TR");
                        btn.innerHTML = fila;
                        document.getElementById("tablaAhorroPermanente").appendChild(btn);
                    });

                    $("#divAhorroPermanente").show();
                } else {
                    $("#divAhorroPermanente").hide();
                }
            }
        });
    }

    //..................

    //FUNCIONES AHORRO CONTRACTUAL
    function GetDatosAhorroContractual(nit) {
        $('#tablaAhorroContractual tr').empty();
        $.ajax({
            type: "POST",
            url: "/EstadoCuenta/EstadoDeCuenta/GetDatosAhorroContractual",
            datatype: "Json",
            data: { NIT: $('#Tercero').val() },
            success: function (data) {
                if (data.status) {
                    $.each(data.list, function (key, val) {
                        var fila = "<tr><td>" + val.Cuenta + "</td>" +
                            "<td>" + val.TipoAhorro + "</td>" +
                            "<td>" + val.Plazo + "</td>" +
                            "<td>" + val.FechaApertura + "</td>" +
                            "<td>" + val.FechaVencimiento + "</td>" +
                            "<td>" + val.TEM + "</td>" +
                            "<td>" + val.ValorCuota + "</td>" +
                            "<td>" + val.TotalAhorros + "</td>" +
                            "<td>" + val.Rendimientos + "</td>" +
                            "<td>" + val.SaldoTotal + "</td>" +
                            "<td><b>" + val.Estado + "</b></td>" +
                            "<td><button type='button' class='btnVerAportes btn btn - info btn - lg' data-toggle='modal' data-target='#modalVerAportes' data-producto='4' >Ver</button></td></tr>";
                        var btn = document.createElement("TR");
                        btn.innerHTML = fila;
                        document.getElementById("tablaAhorroContractual").appendChild(btn);
                    });

                    $("#divAhorroContractual").show();
                } else {
                    $("#divAhorroContractual").hide();
                }
            }
        });
    }
    //...............

    //FUNCIONES APORTE EXTRAORDINARIO
    function GetDatosAporteExtra(nit) {
        $('#tablaAporteExtra tr').empty();
        $.ajax({
            type: "POST",
            url: "/EstadoCuenta/EstadoDeCuenta/GetDatosAporteExtra",
            datatype: "Json",
            data: { NIT: $('#Tercero').val() },
            success: function (data) {
                if (data.status) {
                    $.each(data.list, function (key, val) {
                        var fila = "<tr><td>" + val.Cuenta + "</td>" +
                            "<td>" + val.FechaApertura + "</td>" +
                            "<td>" + val.SaldoActual + "</td>" +
                            "<td><b>" + val.Estado + "</b></td>" +
                            "<td><button type='button' class='btnVerAportes btn btn - info btn - lg' data-toggle='modal' data-target='#modalVerAportes' data-producto='2' >Ver</button></td></tr>";
                        var btn = document.createElement("TR");
                        btn.innerHTML = fila;
                        document.getElementById("tablaAporteExtra").appendChild(btn);
                    });

                    $("#divAporteExtra").show();
                } else {
                    $("#divAporteExtra").hide();
                }
            }
        });
    }
    //...............
});