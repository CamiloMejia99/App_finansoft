$(document).ready(function() {

    $("#btnConsultar").click(function () {


        $("#lblfecha").text("");
        $("#lblnombre").text("");
        $("#lbldocumento").text("");
        var tercero = $("#tercero").val();
        var fechDesde = $("#fechaDesde").val();
        var fechHasta = $("#fechaHasta").val();
        $("#mitabla").empty();

        if (tercero == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Debe seleccionar un tercero'
            })
        } else if (fechDesde == "" || fechHasta == "") {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Debe seleccionar un periodo'
            })
        } else {
            $.ajax({
                url: '/Accounting/Informes/GetCertificadoRetencion',
                datatype: "Json",
                data: {
                    tercero: tercero,
                    fechaDesde: fechDesde,
                    fechaHasta: fechHasta
                },//solo para enviar datos
                type: 'post',
            }).done(function (data) {
                if (data.status == true) {
                    $("#lblanio").text(data.anio);
                    $("#lblfecha").text(data.fecha);
                    $("#lblnombre").text(data.nombre);
                    $("#lbldocumento").text(data.documento);
                    $("#lbldireccion").text(data.direccion);

                    $.each(data.datos, function (index, val) {
                        var tr = "<tr>";
                        tr += '<td>' + val[0] + '</td>';
                        tr += '<td style="text-align:center" >' + val[1] + '</td>';
                        tr += '<td style="text-align:center" >' + val[2] + '</td>';
                        tr += '<td style="text-align:center" >' + val[3] + '</td>';
                        tr += '</tr>';
                        $('#mitabla').append(tr);
                    });

                    var tr = "<tr>";
                    tr += '<td>' + "" + '</td>';
                    tr += '<td>' + "" + '</td>';
                    tr += '<td style="text-align:center" >' + '<b>TOTAL</b>' + '</td>';
                    tr += '<td style="text-align:center" >' + data.total + '</td>';
                    tr += '</tr>';
                    $('#mitabla').append(tr);
                    
                }
                else if (data.status == false) {
                    Swal.fire({
                        icon: 'info',
                        title: 'No se encontraron resultados',
                        text: ''
                    })
                }
            });
        }//fin else validar campos

    });//fin funcion btnConsultar


});