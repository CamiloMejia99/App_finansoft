$(document).ready(function () {
  
    $("#btnBuscar").click(function () {
        var documento = $("#documento").val();
        if (documento == "") {
            Swal.fire({
                icon: 'info',
                title: 'Información!',
                text: 'Por favor, ingrese un número de documento'
            });
        } else {
            $.ajax({
                url: '/Creditos/FormularioSolicitudMicrocredito/BuscarTercero',
                datatype: "Json",
                data: {
                    documento: documento
                },//solo para enviar datos
                type: 'post',
            }).done(function (data) {
                if (data.status) {
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Usuario Encontrado',
                        showConfirmButton: false,
                        timer: 1600
                    })
                    $('#datos').html(data.nombre);
   
                } else {
                    $('#datos').html("");
                    Swal.fire({
                        icon: 'info',
                        title: 'Información',
                        text: 'El número de documento ingresado NO se encuentra registrado',
                    })
                    
                }
            });//fin ajax
        }
    });//fin btnBuscar


    $("#limpiar").click(function () {
        $('#documento').val("");
        $('#datos').html("");
    });


    $("#btnGuardar").click(function () {
        var documento = $("#documento").val();
        var archivo = $("#archivo").val();
        if (documento == "" || archivo == "") {
            if (documento == "") {
                Swal.fire({
                    icon: 'info',
                    title: 'Información!',
                    text: 'Por favor, ingrese un número de documento'
                });
            }
            else {
                Swal.fire({
                    icon: 'info',
                    title: 'Información!',
                    text: 'Por favor, seleccione un archivo'
                });
            }

        }
        else {
            $.ajax({
                url: '/Creditos/FormularioSolicitudMicrocredito/BuscarTercero',
                datatype: "Json",
                data: {
                    documento: documento
                },//solo para enviar datos
                type: 'post',
            }).done(function (data) {
                if (data.status) {
                    $('#idForm').submit();
                } else {
                    Swal.fire({
                        icon: 'warning',
                        title: 'Advertencia!',
                        text: 'Verifique que el documento se encuentre registrado',
                    })
                }
            });//fin ajax  
        }   
    });//fin btnGuardar


    $("#btnGuardarCambios").click(function () {
        var documento = $("#documento").val();
        var fechaAf = $("#fechaAf").val();
        var archivo = $("#archivo").val();
        if ( archivo == "") {
            Swal.fire({
                icon: 'info',
                title: 'Información!',
                text: 'Por favor, seleccione un archivo'
            });
        }
        else {
            $('#idForm').submit();
        }
    });//fin btnGuardarCambios
});