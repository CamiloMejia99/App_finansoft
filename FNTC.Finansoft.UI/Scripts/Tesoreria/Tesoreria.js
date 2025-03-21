
$(document).ready(function () {
    $("#btnAgregar").click(function () {
        var cuenta = $("#cuenta").val();

        if (cuenta != "") {
            $("#theForm").submit();

        } else {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor, seleccione una cuenta</label>`,
            });
        }


    });//fin btnAgregar


});