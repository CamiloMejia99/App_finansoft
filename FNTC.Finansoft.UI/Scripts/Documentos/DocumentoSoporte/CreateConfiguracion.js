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
        window.location.href = "/Documentos/DocumentoSoporte/IndexConfiguracion";
    });

    $("#btnEditar").click(function () {
        Swal.fire({
            title: '¿Realizar Cambios?',
            text:"",
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

    $('.number').on('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

})