$(document).ready(function () {

    $("#btnGuardar").click(function () {
        Swal.fire({
            title: 'Realizar registro?',
            text: "Podrá editar una vez registrado!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Hecho!'
        }).then((result) => {
            if (result.isConfirmed) {
                $("#theForm").submit();
            }
        });
    });


    $("#btnCerrar").click(function () {
        window.location.href = "/SIAR/CargoResponsable/Index";
    });

    $("#btnEditar").click(function () {
        Swal.fire({
            title: 'Realizar cambios?',
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Hecho!'
        }).then((result) => {
            if (result.isConfirmed) {
                $("#theForm2").submit();
            }
        });
    });

})