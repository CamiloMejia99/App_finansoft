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
            case 'CEX':
                GetCuotaExtra();
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
        var vc = $("#ValorPagar").val();
        var vr = $("#ValorRecibido").val();

        vc = vc.replace(/\./g, "");
        vr = vr.replace(/\./g, "");

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
                if (vr != "" && vr != "0" && vc!="" && vc!="0") {
                    var opcion = $("#SelectPago").val();
                    if (opcion == "AB" || opcion =="PA") {                     
                        Abonar(vc, vr);
                    }
                    else if (opcion == "CEX") {
                        AbonarCuota(vc, vr);
                    }
                    else {
                        Pagar(opcion, vr);
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

        ValorPagar = ValorPagar.replace(/\./g, "");
        ValorRecibido = ValorRecibido.replace(/\./g, "");

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
            } else {
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

    function GetCuotaExtra() {
        var Pagare = $("#Pagare").val();
        $("#ValorRecibido").val("0");
        $("#Cambio").val("0");

        $.ajax({
            url: '/Creditos/ProcesosCrediticios/GetCuotaExtra',
            datatype: "json",
            data: { Pagare: Pagare },
            type: 'post',
        }).done(function (data) {
            console.log(data); // Para depuración: mostrar los datos recibidos
            if (data.status) {
                document.getElementById("ValorPagar").disabled = true;
                $("#ValorPagar").val(agregarSeparadorMiles(data.valorCuota));
                $("#NumCuota").val(data.NumCuota);
                $("#ValorCuotaExtra").val(agregarSeparadorMiles(data.valorCuota));
                $("#fechaPagoCuota").val(data.fechaPago);
                document.getElementById("CuotaExtraContainer").classList.remove("oculto");
                document.getElementById("SinCuotaExtraContainer").classList.add("oculto");
            } else {
                document.getElementById("CuotaExtraContainer").classList.add("oculto");
                document.getElementById("SinCuotaExtraContainer").classList.remove("oculto");
                document.getElementById("ValorPagar").disabled = true;
                var valorc = 0;
                $("#ValorPagar").val(valorc);
            }
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.error("Error en la solicitud AJAX:", textStatus, errorThrown);
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
            url: '/Creditos/ProcesosCrediticios/PagoCuotaAbono',
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
                        window.location.href = "/OperativaDeCaja/FactOpcajas/DetailsConsCuotaCreditoNuevo?tipo=" + data.Tipo + "&numero=" + data.Numero;
                    }

                })
            } else {
                alert("Ha ocurrido un error");
            }
        });//fin ajax
    };


    function AbonarCuota(ValorConsignado, ValorRecibido) {
        var Pagare = $("#Pagare").val();
        var FormaPago = $("#SelectFormaPago").val();
        var FechaPago = $("#FechaPago").val();
        var NumFactura = $("#NumFactura").val();

        $.ajax({
            url: '/Creditos/ProcesosCrediticios/PagoCuotaExtra',
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
                        window.location.href = "/OperativaDeCaja/FactOpcajas/DetailsConsCuotaCreditoNuevo?tipo=" + data.Tipo + "&numero=" + data.Numero;
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

        if (vc != "" && vc != "0") {
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


    $("#ValorPagar").on({
        "focus": function (event) {
            $(event.target).select();
        },
        "keyup": function (event) {
            $(event.target).val(function (index, value) {
                return value.replace(/\D/g, "")
                    .replace(/([0-9])([0-9]{3})$/, '$1.$2')
                    .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ".");
            });
        }
    });

    $("#ValorRecibido").on({
        "focus": function (event) {
            $(event.target).select();
        },
        "keyup": function (event) {
            $(event.target).val(function (index, value) {
                return value.replace(/\D/g, "")
                    .replace(/([0-9])([0-9]{3})$/, '$1.$2')
                    .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ".");
            });
        }
    });

    function agregarSeparadorMiles(numero) {
        // Convertir el número a cadena
        let numeroStr = numero.toString();

        // Dividir la cadena en dos partes: la parte entera y la parte decimal
        let partes = numeroStr.split(".");

        // Formatear la parte entera con separador de miles
        let parteEntera = partes[0].replace(/(\d)(?=(\d{3})+$)/g, "$1.");

        // Volver a unir el valor con la parte decimal (si existe)
        let valorFormateado = parteEntera;
        if (partes.length > 1) {
            valorFormateado += "," + partes[1]; // Cambiamos el punto por una coma
        }

        // Retornar el valor formateado
        return valorFormateado;
    }



})