$(document).ready(function () {

    $("#Fk_discriminacion").change(function () {
        var selectedId = $(this).val();
        $.ajax({
            url: "/Nomina/CorreccionNominas/GetAfiliadosDiscriminacion",
            type: "GET",
            data: { discriminacion: selectedId },
            success: function (result) {
                // Limpiar el select antes de agregar nuevas opciones
                $('#Fk_afiliado').empty();

                // Agregar el primer elemento "Seleccione un afiliado" deshabilitado
                $('#Fk_afiliado').append($('<option>', {
                    value: '',
                    text: 'Seleccione un afiliado',
                    disabled: true,
                    selected: true
                }));

                // Verificar si la factura existe
                if (result && result.length > 0) {
                    // Recorrer el resultado y agregar opciones al select
                    $.each(result, function (index, afiliado) {
                        $('#Fk_afiliado').append($('<option>', {
                            value: afiliado.Value,
                            text: afiliado.Text
                        }));
                    });
                    // Después de agregar las opciones, inicializar Select2
                    $('#Fk_afiliado').select2(); // Inicialización de Select2
                } else {
                    // Si no hay afiliados disponibles, agregar una opción de mensaje
                    $('#Fk_afiliado').append($('<option>', {
                        value: '',
                        text: 'No hay afiliados disponibles'
                    }));
                    // Después de agregar el mensaje, inicializar Select2
                    $('#Fk_afiliado').select2(); // Inicialización de Select2
                }
            },
            error: function () {
                console.log("Error al obtener el nombre del afiliado.");
            }
        });
    });



    $("#Fk_afiliado").change(function () {
        var selectedId = $(this).val();
        var periodo = $("#Fk_discriminacion").val();
        var concepto = $("#Concepto").val();

        $.ajax({
            url: "/Nomina/CorreccionNominas/GetValorDiscriminacion",
            type: "GET",
            data: { afiliado: selectedId, discriminacion: periodo, concepto: concepto },
            success: function (result) {
                if (result.valor !== 0) {
                    $("#valor_novedad").val(agregarSeparadorMiles(result.valor))
                }
                else {
                    var respuesta = "No hay valor por el concepto seleccionado"
                    $("#valor_novedad").val(respuesta)
                }   
            },
            error: function () {
                console.log("Error al obtener los datos");
            }
        });
    });

    $("#Concepto").change(function () {

        var selectedId = $(this).val();
        var periodo = $("#Fk_discriminacion").val();
        var afiliado = $("#Fk_afiliado").val();
        if (afiliado) {
            $.ajax({
                url: "/Nomina/CorreccionNominas/GetValorDiscriminacion",
                type: "GET",
                data: { afiliado: afiliado, discriminacion: periodo, concepto: selectedId },
                success: function (result) {
                    if (result.valor !== 0) {
                        $("#valor_novedad").val(agregarSeparadorMiles(result.valor))
                    }
                    else {
                        var respuesta = "No hay valor por el concepto seleccionado"
                        $("#valor_novedad").val(respuesta)
                    }                
                },
                error: function () {
                    console.log("Error al obtener los datos");
                }
            });
        }    
    });


    //agregar separador de miles al campo nuevo valor
    $("#Valor_corregido").on({
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


    $("#btnCorreccion").click(function () {

        var valor_antiguo = $("#valor_novedad").val();
        var periodo = $("#Fk_discriminacion").val();
        var afiliado = $("#Fk_afiliado").val();
        var Valor_corregido = $("#Valor_corregido").val().trim();

        if (valor_antiguo == "No hay valor por el concepto seleccionado") {
            mostrarAlerta('El afiliado debe contar con un valor por el concepto seleccionado');
        }
        else if (periodo && afiliado && Valor_corregido) {
                Valor_corregido = Valor_corregido.replace(/\./g, "");
                $("#Valor_corregido").val(Valor_corregido);
                $("#FormCorreccion").submit();
             } else {
                 mostrarAlerta('Por favor seleccione todos los campos!');
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



   

    function mostrarAlerta(mensaje) {
        Swal.fire({
            icon: 'warning',
            html: '<label class="fuenteSweetAlert2">' + mensaje + '</label>',
        });
    }
});
