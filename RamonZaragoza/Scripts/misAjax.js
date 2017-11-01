$(document).ready(function () {
    $("body").on('click', 'button', function () {
       
        // Si el boton no tiene el atributo ajax no hacemos nada
        if ($(this).data('ajax') == undefined) return;

        // El metodo .data identifica la entrada y la castea al valor más correcto
        if ($(this).data('ajax') != true) return;
        var form = $(this).closest("form");
        var button = $(this);
        
        var url = form.attr('action');

        if (button.data('confirm') != undefined) {
            if (button.data('confirm') == '') {
                if (!confirm('¿Esta seguro de realizar esta acción?')) return false;
            } else {
                if (!confirm(button.data('confirm'))) return false;
            }
        }

        // Creamos un div que bloqueara todo el formulario
        $('#' + button.attr('id')).hide();
        $('#cargando').show();

        // En caso de que haya habido un mensaje de alerta
        $(".alert", form).remove();

        // Para los formularios que tengan CKupdate
        if (form.hasClass('CKupdate')) CKupdate();

        form.ajaxSubmit({
            dataType: 'JSON',
            type: 'POST',
            url: url,
            success: function (r) {
                $('#' + button.attr('id')).show();
                $('#cargando').hide();

                // Mostrar mensaje
                if (r.mensaje != null) {
                    if (r.mensaje.length > 0) {
                        var css = "";
                        if (r.respuesta) css = "alert-info";
                        else css = "alert-danger";

                        var mensaje = '<div class="alert ' + css + ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' + r.mensaje + '</div>';
                        if ($('#mensaje').length) $('#mensaje').html(mensaje);
                        else form.prepend(mensaje); 
                    }
                }

                // Ejecutar funciones
                if (r.funcion != null) {
                    setTimeout(r.funcion, 0);
                }
                // Redireccionar
                if (r.href != null) {
                    if (r.href == 'self') window.location.reload(true);
                    else window.location.href = r.href;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
             //   block.remove();
                $('#' + button.attr('id')).show();
                $('#cargando').hide();
                form.prepend('<div class="alert alert-warning alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>' + errorThrown + ' | <b>' + textStatus + '</b></div>');
            }
        });

        return false;
    })
})

function onBegin() {
    $('#btn-enviar').css('display', 'none');
}

function onSuccess(r) {
    if ($('#btn-enviar') != undefined) $('#btn-enviar').css('display', 'initial');
    if (r.mensaje.length > 0) {
        if (r.respuesta) {
            var css = "alert-info";
        } else {
            css = "alert-danger";
        }
        var alerta = '<div class="alert ' + css + ' alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>'
            + r.mensaje + '</div>';
        $('#mensaje').html(alerta);
    }

    if (r.funcion != null) {
        setTimeout(r.funcion, 0);
    }

    if (r.href != null) {
        $('#cargando').css('display', "inline")
        window.location = r.href;
    }
    //si hay algun form modal abierto lo cierro
    if ($('#myModal').is(':visible')) {
        $('#myModal').modal('hide')
        $('.modal-backdrop').remove();
    }
}
function CheckElementos() {
    if ($(".seleccion:checked").length == 0) {
        alert('Debe seleccionar algún registro');
        return false;
    }
}

