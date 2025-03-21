
$(document).ready(function () {
    $("#btnAgregar").click(function () {

        var filasSeleccionadas = $('#tbBeneficiario tbody').find('tr.selected');

        var datos = [];

        filasSeleccionadas.each(function () {
            var fila = $(this);
            var identificacionBeneficiario = fila.find('td:eq(0)').text(); // Obtener el valor de la primera columna
            var nombreBeneficiario = fila.find('td:eq(1)').text(); // Obtener el valor de la segunda columna
            var parentesco = fila.find('td:eq(2)').text(); // Obtener el valor de la segunda columna
            var edad = fila.find('td:eq(3)').text(); // Obtener el valor de la segunda columna

            // Agregar los datos de la fila seleccionada al arreglo
            datos.push({
                identificacion: identificacionBeneficiario,
                nombre: nombreBeneficiario,
                parentesco: parentesco,
                edad: edad
                // Agrega aquí los demás datos que necesites de cada fila
            });
        });

        console.log(datos);

        // Actualizar el valor del campo oculto con los datos seleccionados
        $('#beneficiariosSeleccionados').val(JSON.stringify(datos));
        var idAfiliado = $("#idAfiliado").val();
        //var nombreAfiliado = $("#nombreAfiliado").val();
        var idAuxilio = $("#idAuxilio").val();
        var valorSolicitado = $("#valorSolicitado").val();
        var valorAprobado = $("#valorAprobado").val();
        var numFactura = $("#numFactura").val();
        var fechaFactura = $("#fechaFactura").val();
        var fechaLiquidacion = $("#fechaLiquidacion").val();
        var comiteAprobador = $("#comiteAprobador").val();
        var itemEstado = $("#itemEstado").val();

        // Obtener el checkbox
        var checkbox = document.getElementById('nombre');

        // Verificar si está seleccionado
        //if (checkbox.checked) {
        //    console.log('El checkbox está marcado');
        //} else {
        //    Swal.fire({
        //        icon: 'warning',
        //        html: `<label class="fuenteSweetAlert2">Por favor, Seleccione un beneficiario</label>`,
        //    });
        //}


        if (idAfiliado != "" && idAuxilio != "" && valorSolicitado != "" && valorAprobado != "" && numFactura != "" && fechaFactura && fechaLiquidacion != "") {
            if (itemEstado == 'B') {
                if (comiteAprobador != '') {
                    $("#theForm").submit();

                } else {
                    Swal.fire({
                        icon: 'warning',
                        html: `<label class="fuenteSweetAlert2">Por favor, Ingrese todos los campos</label>`,
                    });
                }
                

            } else {
                $("#theForm").submit();
            }

        } else {
            Swal.fire({
                icon: 'warning',
                html: `<label class="fuenteSweetAlert2">Por favor, Ingrese todos los campos</label>`,
            });
        }


    });//fin btnAgregar




});