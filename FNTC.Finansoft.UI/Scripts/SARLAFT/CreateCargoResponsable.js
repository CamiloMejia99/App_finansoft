$(document).ready(function () {
    var aceptar = ` <label class="fuenteSweetAlert">Aceptar</label>`
    var cancelar = ` <label class="fuenteSweetAlert">Cancelar</label>`

    $("#btnGuardar").click(function () {
        Swal.fire({
            title: '¿Realizar registro?',
            html: ` <label class="fuenteSweetAlert">Podrá editar una vez registrado!</label>`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#8ED813',
            cancelButtonColor: '#FF2929',
            confirmButtonText: aceptar,
            cancelButtonText: cancelar
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
            title: '¿Realizar cambios?',
            text: "",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#8ED813',
            cancelButtonColor: '#FF2929',
            confirmButtonText: aceptar,
            cancelButtonText: cancelar
        }).then((result) => {
            if (result.isConfirmed) {
                $("#theForm2").submit();
            }
        });
    });

})