$(document).ready(function () {


    $("#btnExcel").click(function () {

        var proveedor = $('#proveedor').val();
        var clase_proveedor = $('#Clase_Proveedor').val();
        var fecha_desde = $('#Fecha_desde').val();
        var fecha_hasta = $('#Fecha_hasta').val();

        if (clase_proveedor =="") {
            clase_proveedor = "0"
            $('#Clase_Proveedor').val(clase_proveedor)
        }

        if (!fecha_desde) {
            var fechaMinima = new Date();
            fechaMinima.setFullYear(fechaMinima.getFullYear() - 100); // Restar 100 años
            fecha_desde = "0001-01-01";;
            $('#Fecha_desde').val(fecha_desde)
        }
        if (!fecha_hasta) {
            fecha_hasta = "0001-01-01";
            $('#Fecha_hasta').val(fecha_hasta)
        }

        var chkExcel = document.getElementById('chkExcel').checked;
        var chkPDF = document.getElementById('chkPDF').checked;

        if (chkExcel) {
            $("#Form_reporte").submit();
        }
        else if (chkPDF) {

            generarReportePDF()
        }
        else {
            mostrarAlerta("Por favor seleccione el tipo de reporte");
        }
    });

    function mostrarAlerta(mensaje) {
        Swal.fire({
            icon: 'warning',
            html: '<label class="fuenteSweetAlert2">' + mensaje + '</label>',
        });
    }

    function generarReportePDF() {
        // Envía el formulario a una nueva ventana
        var form = document.getElementById('Form_reporte');
        form.target = '_blank';
        form.submit();
    }

});