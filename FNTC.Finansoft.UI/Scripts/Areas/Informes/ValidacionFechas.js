$(document).ready(function () {
    $("#descargar").click(function () {

        fechaDesdeBC = $("#fechaDesdeBC").val();
        fechaHastaBC = $("#fechaHastaBC").val();
        fechaDesde = $("#fechaDesde").val();
        fechaHasta = $("#fechaHasta").val();
        informe = $("#informe").val();

        var prueba = 0;

        if (informe == 37) {
            if (fechaDesdeBC != "" && fechaHastaBC != "") {
                $('#theForm').submit();
            }
            else {
                
                Swal.fire({
                    icon: 'info',
                    title: 'Información!',
                    text: 'Debe ingresar fecha desde y fecha hasta para generar el informe'
                });
            }

        } else if (informe == 6) {
                    if (fechaDesde != "" || fechaHasta != "") {
                        $('#theForm').submit();
                    }
                    else {
                        
                        Swal.fire({
                            icon: 'info',
                            title: 'Información!',
                            text: 'Debe ingresar por lo menos una fecha para generar el informe'
                        });
                    }
        }
        else if (informe == 7) {
                if (fechaDesde != "" || fechaHasta != "") {
                    $('#theForm').submit();
                }
                else {

                    Swal.fire({
                        icon: 'info',
                        title: 'Información!',
                        text: 'Debe ingresar por lo menos una fecha para generar el informe'
                    });
                }
        } else if (informe == 2) {
                    if (fechaDesde != "" && fechaHasta != "") {
                        $('#theForm').submit();
                    }
                    else {

                        Swal.fire({
                            icon: 'info',
                            title: 'Información!',
                            text: 'Debe ingresar fecha desde y fecha hasta para generar el informe'
                        });
                    }
        } else if (informe == 22) {
                    if (fechaDesde != "" && fechaHasta != "") {
                        $('#theForm').submit();
                    }
                    else {

                        Swal.fire({
                            icon: 'info',
                            title: 'Información!',
                            text: 'Debe ingresar fecha desde y fecha hasta para generar el informe'
                        });
                    }
        } else if (informe == 3) {
                    if (fechaDesde != "" || fechaHasta != "") {
                        $('#theForm').submit();
                    }
                    else {

                        Swal.fire({
                            icon: 'info',
                            title: 'Información!',
                            text: 'Debe ingresar por lo menos una fecha para generar el informe'
                        });
                    }
        }
        else if (informe == 40) {
                if (fechaDesde != "" || fechaHasta != "") {
                    $('#theForm').submit();
                }
                else {

                    Swal.fire({
                        icon: 'info',
                        title: 'Información!',
                        text: 'Debe ingresar por lo menos una fecha para generar el informe'
                    });
                }
        }
        else if (informe == 41) {
                if (fechaDesde != "" || fechaHasta != "") {
                    $('#theForm').submit();
                }
                else {

                    Swal.fire({
                        icon: 'info',
                        title: 'Información!',
                        text: 'Debe ingresar por lo menos una fecha para generar el informe'
                    });
                }
        }
        else {
            $('#theForm').submit();
        }

    });

 
})