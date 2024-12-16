$(document).ready(function () {

    

    $("#btnPagar").click(function () {
        Swal.fire({
            title: '¿Realizar transacción?',
            text: "",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                RealizarPagoAC();
            }
        })
    });

    $("#btnCancelar").click(function () {
        window.location.href = "/OperativaDeCaja/FactOpcajas/CuentaOperacion";
    });

    $(".calcular").change(function () {
        CalcularValores();
    });

    

    function RealizarPagoAC() {

        var NumeroCuenta = $("#NumeroCuenta").val();
        var valor = $("#inpValorConsignar").val();
        var observacion = $("#inpObservacion").val();

        if (valor == "" || valor == "0") {
            ErrorMensajeValorVacio();
        } else {
            $.ajax({
                url: '/OperativaDeCaja/FactOpcajas/ConsignacionAhorroContractual',
                datatype: "Json",
                data: {
                    NumeroCuenta: NumeroCuenta,
                    valor: valor,
                    observacion: observacion
                },
                type: 'post',
            }).done(function (data) {
                if (data.status) {
                    ExitoMensajePago(data.IdFactura);
                }
                else{
                    ErrorMensajePago(data.mensaje);
                }
            });
        }
    }

    function ExitoMensajePago(IdFactura) {
        Swal.fire({
            title: 'Transacción exitosa!',
            text: "",
            icon: 'success',
            showCancelButton: false,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Imprimir recibo'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = "/OperativaDeCaja/FactOpcajas/DetalleFacturaAhorroContractual?Id=" + IdFactura;
            }
        });
    }

    function ErrorMensajeValorVacio() {
        Swal.fire({
            icon: 'warning',
            title: 'Campo: Valor a consginar vacío',
            text: 'Por favor ingrese un valor válido y vuelva a registrar el pago'
        });
    }

    function ErrorMensajePago(mensaje) {
        Swal.fire({
            icon: 'error',
            title: 'Transacción fallida.',
            text: ''+mensaje
        });
    }

    function ErrorMensajeValorRecibido(){
        Swal.fire({
            icon: 'warning',
            title: 'Importante!',
            text: 'El valor recibido no puede ser menor al valor consignado.'
        });
    }

    function CalcularValores() {
        try {
            let valorRecibido = parseInt($("#inpValorRecibido").val().split('.').join(""));
            let valorConsignar = parseInt($("#inpValorConsignar").val().split('.').join(""));
            if (!(isNaN(valorRecibido) || isNaN(valorConsignar)))
            {
                let cambio = valorRecibido - valorConsignar;
                if (cambio < 0) {
                    $("#inpValorRecibido").val("0");
                    ErrorMensajeValorRecibido();
                } else {
                    $("#inpCambio").val(formatNumberMiles.new(cambio));
                    CalcularNuevoSaldo();
                }
            }

        } catch (e) {

        }
    }

    function CalcularNuevoSaldo() {
        try {
            let valorActual = parseInt($("#inpSaldoActual").val().split('.').join(""));
            let valorConsignar = parseInt($("#inpValorConsignar").val().split('.').join(""));
            if (!(isNaN(valorActual) || isNaN(valorConsignar))) {
                let saldoNuevo = valorActual + valorConsignar;
                $("#inpNuevoSaldo").val(formatNumberMiles.new(saldoNuevo));
            }
        } catch (e) {
        }
    }

    var formatNumberMiles = {
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

})