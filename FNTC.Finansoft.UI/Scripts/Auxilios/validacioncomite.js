
$(document).ready(function () {
    $("#btnAgregar").click(function () {
        var nombre = $("#nom_comite").val();
        var nomres = $("#nomres_comit").val();

        if (nombre != "" && nomres != "") {
            $("#theForm").submit();

        } else {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor, Ingrese todos los campos</label>`,
            });
        }


    });//fin btnAgregar


    $("#btnEditar").click(function () {
        var nombre = $("#nom_comite").val();
        var nomres = $("#nomres_comit").val();

        if (nombre != "" && nomres != "") {
            $("#theForm").submit();

        } else {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor, Ingrese todos los campos</label>`,
            });
        }


    });

});