$(document).ready(function () {

    $().ready(function () {
        document.getElementById("btnGenerar").disabled = true;
        
    });
    

    $("#btnPYZ").click(function () {
        
        $("#viewLiquidacion").hide();
        $("#viewPYZ").show();
    });

    $("#btnLiquidacion").click(function () {

        $("#viewPYZ").hide();
        $("#viewLiquidacion").show();
    });

    
    

    $("#tercero").change(function () {
        
        var data = $("#tercero").val();

        $("#contenido").empty();
        if (data != 0) {
            if ($("#contenido").is(":empty")) {
                var buttonPYZ = '<li><a target="_blank" href="/Creditos/Certificados/pazYsalvo?id=' + data + '" class="tabsnota"> <i class="fa fa-file-text" aria-hidden="true"></i> Paz y Salvo </a></li>';
                $('#contenido').append(buttonPYZ);
                
            }
        }

    });

    $("#tercerol").change(function () {

        //$('btnLiquidacion2').click();


        if ($(this).val() == "0" || $("#linea").val() == "0") {
            document.getElementById("btnGenerar").disabled = true;
        } else {
            document.getElementById("btnGenerar").disabled = false;
        }
        
    });
    $("#linea").change(function () {

        if ($(this).val() == "0" || $("#tercerol").val() == "0") {
            document.getElementById("btnGenerar").disabled = true;
        } else {
            document.getElementById("btnGenerar").disabled = false;
        }

    });

    $("#btnGenerar").click(function () {
        var data1 = $("#tercerol").val();
        var data2 = $("#linea").val();
        $('#btnLiquidacion2').hide();
        

        $.ajax({
            url: '/Creditos/Certificados/verificaCredito',
            datatype: "Json",
            data: {
                id: data1,
                idd: data2
            },//solo para enviar datos
            type: 'post',
        }).done(function (data) {
            if (data.status == true) {
               
                $('#btnLiquidacion2').attr('href', '/Creditos/Certificados/liquidacion?id=' + data1 + '&idd=' + data2);
                $('#btnLiquidacion2').show();
            }
            else if (data.status == false) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'El usuario no registra créditos con esa línea!'
                    
                })
                
            }

        });

       
        
    });

    $('#btnLiquidacion2').click(function () {
        $('#btnLiquidacion2').hide();
    });


});