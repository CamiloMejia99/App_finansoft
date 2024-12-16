//import { parse } from "mustache";

$(document).ready(function () {

    document.getElementById("ValorPagar").disabled = true;

    $("#SelectPago").change(function () {
        document.getElementById("ValorPagar").disabled = false;
        var opcion = $(this).val();
        switch (opcion) {
            case 'PA':
                GetCuotaActual();
                break;
            case 'AB':
                document.getElementById("ValorPagar").disabled = false;
                $("#ValorPagar").val("0");
                $("#ValorRecibido").val("0");
                $("#Cambio").val("0");
                break;
            case 'PC':
                GetCreditoAlDia();
                break;
            case 'LC':
                GetCreditoLiquidar();
                break;
            default:
                $("#ValorPagar").val("0");
                break;
        }
    });

    $("#ValorPagar").change(function () { 
        var vr = $("#ValorRecibido").val();
        var seleccion = $("#SelectPago").val();
        if (seleccion != "AB") {
            if (vr != "") { ValidarValores(); }
        }
        
          
    });

    $("#ValorRecibido").change(function () {
        ValidarValores();
    });

    $("#RealizarPago").click(function () {

        Swal.fire({
            title: 'Continuar?',
            text: "No podrás revertir esta operación!",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar!',
            cancelButtonText: 'Cancelar!'
        }).then((result) => {
            if (result.isConfirmed) {
                var vr = $("#ValorRecibido").val();
                if (vr != "" && vr != "0") {
                    var opcion = $("#SelectPago").val();
                    if (opcion != "AB") {
                        Pagar(opcion, vr);
                    } else {
                        VerificaValorAbono();
                    }
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: '',
                        text: 'Valor Insuficiente!'
                    });
                }
            }
        });

        
        
    });

    $("#Cancelar").click(function () {
        window.location.href = "/OperativaDeCaja/FactOpcajas/CuentaOperacion";
    });

    function ValidarValores() {

        var ValorPagar = $("#ValorPagar").val();
        var ValorRecibido = $("#ValorRecibido").val();

        $.ajax({
            url: '/Creditos/ProcesosCrediticios/ValidarValores',
            datatype: "Json",
            data: {
                ValorPagar: ValorPagar,
                ValorRecibido: ValorRecibido
            },
            type: 'post',
        }).done(function (data) {
            if (data.status) {
                $("#Cambio").val(data.Cambio);
            }else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Valor Insuficiente!'
                })
                $("#Cambio").val("0");
                $("#ValorRecibido").val("0");
            }
            
        });
    };

    function GetCuotaActual() {

        var Pagare = $("#Pagare").val();
        $("#ValorRecibido").val("0");
        $("#Cambio").val("0");

        $.ajax({
            url: '/Creditos/ProcesosCrediticios/GetCuotaActual',
            datatype: "Json",
            data: { Pagare: Pagare },
            type: 'post',
        }).done(function (data) {
            document.getElementById("ValorPagar").disabled = true;
            $("#ValorPagar").val(data.CuotaActual);
        });
    }

    function GetCreditoAlDia() {
        $("#ValorRecibido").val("0");
        $("#Cambio").val("0");
        document.getElementById("ValorPagar").disabled = true;
        var valor = $("#TotalCreditoPendiente").val();
        $("#ValorPagar").val(valor);
    };

    function GetCreditoLiquidar() {
        $("#ValorRecibido").val("0");
        $("#Cambio").val("0");
        document.getElementById("ValorPagar").disabled = true;
        var valor = $("#TotalCreditoLiquidar").val();
        $("#ValorPagar").val(valor);
    };

    function Pagar(Opcion, ValorRecibido) {
        var Pagare = $("#Pagare").val();
        var FormaPago = $("#SelectFormaPago").val();
        var FechaPago = $("#FechaPago").val();
        var NumFactura = $("#NumFactura").val();

        $.ajax({
            url: '/Creditos/ProcesosCrediticios/Pago',
            datatype: "Json",
            data: {
                Pagare: Pagare,
                Opcion: Opcion,
                ValorRecibido: ValorRecibido,
                FormaPago: FormaPago,
                FechaPago: FechaPago,
                NumFactura: NumFactura
            },
            type: 'post',
        }).done(function (data) {
            if (data.status) {
                Swal.fire({
                    title: 'Hecho!',
                    text: "Proceso realizado con éxito!",
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Continuar!'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/OperativaDeCaja/FactOpcajas/DetailsConsCuotaCredito?id=" + data.Id;
                    }
                })
            } else {
                alert("Ha ocurrido un error");
            }
        });
    };

    function Abonar(ValorConsignado, ValorRecibido) {
        var Pagare = $("#Pagare").val();
        var FormaPago = $("#SelectFormaPago").val();
        var FechaPago = $("#FechaPago").val();
        var NumFactura = $("#NumFactura").val();

        $.ajax({
            url: '/Creditos/ProcesosCrediticios/Abono',
            datatype: "Json",
            data: {
                Pagare: Pagare,
                ValorConsignado: ValorConsignado,
                ValorRecibido: ValorRecibido,
                FormaPago: FormaPago,
                FechaPago: FechaPago,
                NumFactura: NumFactura
            },
            type: 'post',
        }).done(function (data) {
            if (data.status) {
                Swal.fire({
                    title: 'Hecho!',
                    text: "Proceso realizado con éxito!",
                    icon: 'success',
                    showCancelButton: false,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Continuar!'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/OperativaDeCaja/FactOpcajas/DetailsConsCuotaCredito?id=" + data.Id;
                    }
                })
            } else {
                alert("Ha ocurrido un error");
            }
        });//fin ajax
    };

    function VerificaValorAbono() {

        var Pagare = $("#Pagare").val();
        var vc = $("#ValorPagar").val();
        var vr = $("#ValorRecibido").val();

        if (vc != "" && vc !="0") {
            $.ajax({
                url: '/Creditos/ProcesosCrediticios/VerificaValorAbono',
                datatype: "Json",
                data: {
                    Pagare: Pagare,
                    ValorConsignado: vc
                },
                type: 'post',
            }).done(function (data) {
                if (data.status) {
                    Abonar(vc, vr);
                } else {
                    Swal.fire({
                        icon: 'info',
                        title: 'Información',
                        text: 'El valor a pagar no puede ser mayor al valor total del crédito!'
                    })
                }
            });//fin ajax
        } else {
            Swal.fire({
                icon: 'warning',
                title: 'Advertencia',
                text: 'Debe consignar un valor válido!'
            })
        }

        


        
    }

})