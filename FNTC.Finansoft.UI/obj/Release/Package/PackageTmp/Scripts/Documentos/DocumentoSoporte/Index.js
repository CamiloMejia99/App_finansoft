$(document).ready(function () {
    $(".btnDetails").click(function () {

        var numero = $(this).data("id");
        var tipo = $(this).data("value");

        $.ajax({
            url: '/Documentos/DocumentoSoporte/VerificaTipoComprobante',
            datatype: "Json",
            type: 'post',
            data: {
                tipo: tipo
            }
        }).done(function (data) {
            if (data.status) {
                var win = window.open("/DocumentoSoporte/GetDocumentoSoporte?tipo=" + tipo + "&numero=" + numero + "", "Comprobante", "directories=0,titlebar=0,toolbar=0,location=0,status=0,menubar=0,scrollbars=no,resizable=no,height=600,width=800");
                win.focus();
            }
            else {
                Swal.fire({
                    icon: 'warning',
                    title: 'Aviso',
                    text: 'No existe una configuración para este tipo de documento soporte.'
                })
            }
        });

    });
});