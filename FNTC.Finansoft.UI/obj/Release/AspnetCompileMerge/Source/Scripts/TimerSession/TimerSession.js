$(document).everyTime(3000, function () {

        $.ajax({
            type: "POST",
            url: "/TimerSession/ValidarSession/KeepActiveSession",
            data: { nit: "1085905490"},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: VerifySessionState,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus + ": " + XMLHttpRequest.responseText);
            }
        });

});


var cantValidaciones = 0;

function VerifySessionState(result) {
    //alert("correcto");
    //if (result.d) {
    //    $("#EstadoSession").text("activo");
    //}
    //else
    //    $("#EstadoSession").text("expiro");

    //$("#cantValidaciones").text(cantValidaciones);
    //cantValidaciones++;

}

//function SessionAbandon() {

//    $.ajax({
//        type: "POST",
//        url: "ValidarSession1.aspx/SessionAbandon",
//        data: {},
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: true,
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            alert(textStatus + ": " + XMLHttpRequest.responseText);
//        }
//    });

//}