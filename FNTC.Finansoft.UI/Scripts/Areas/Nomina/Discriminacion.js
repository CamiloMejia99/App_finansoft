$(document).ready(function () {
    $("#btnDiscriminacion").click(function () {
        var plano = $("#FK_plano_empresa").val();
        var periodo = $("#FK_Periodo_deduccion").val();
        var mes = $("#FK_mes_deduccion").val();
        var checkbox1 = $("#opcion1");
        var checkbox2 = $("#opcion2");
        var opcionQuincena = 0;

        if (plano && periodo && mes) {
            if (periodo === "2") {
                if (!checkbox1.is(":checked") && !checkbox2.is(":checked")) {
                    mostrarAlerta('Por favor seleccione el tipo de quincena!');
                }
                else {
                    if (checkbox1.is(":checked"))
                        opcionQuincena = 1;
                    else
                        opcionQuincena = 2;
                    $.ajax({
                        url: "/Nomina/Discriminacions/ValidarDiscriminacion",
                        type: "GET",
                        data: { plano: plano, periodo: periodo, mes: mes, opcionQuincena: opcionQuincena },
                        success: function (result) {
                            // Verificar si la factura existe
                            if (result) {
                                mostrarAlerta('La discriminación ya fue Generada');
                            }
                            else {
                                // Mostrar modal de carga
                                Swal.fire({
                                    icon: 'warning',
                                    title: 'Procesando',
                                    html: 'Por favor, espere...',
                                    allowOutsideClick: false,
                                    showConfirmButton: false,
                                    onBeforeOpen: () => {
                                        Swal.showLoading();
                                    }
                                });
                                $("#FormDiscriminacion").submit();
                            }
                        },
                        error: function () {
                            console.log("Error al obtener el nombre del afiliado.");
                        }
                    });
                }
            } else {
                // Mostrar modal de carga
                Swal.fire({
                    icon: 'warning',
                    title: 'Procesando',
                    html: 'Por favor, espere...',
                    allowOutsideClick: false,
                    showConfirmButton: false,
                    onBeforeOpen: () => {
                        Swal.showLoading();
                    }
                });
                $("#FormDiscriminacion").submit();
            }
        } else {
            mostrarAlerta('Por favor seleccione todos los campos!');
        }
    });

    function mostrarAlerta(mensaje) {
        Swal.fire({
            icon: 'warning',
            html: '<label class="fuenteSweetAlert2">' + mensaje + '</label>',
        });
    }
});
