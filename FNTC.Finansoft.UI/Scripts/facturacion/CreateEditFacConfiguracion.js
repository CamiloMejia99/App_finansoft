$(document).ready(function () {

    $("#id").val("1");
    

    $("#btnGuardar").click(function () {
        $("#theForm").submit();
    });
    $("#btnCerrar").click(function () {
        window.location.href = '/facturacion/FacConfiguracion/Index';
    });

    

});