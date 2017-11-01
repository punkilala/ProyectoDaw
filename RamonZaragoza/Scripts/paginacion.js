var enlace;
var clase;
var funcion;
$(document).ready(function () {

    //quiero ejecutar una funcion cuando acabe ajax
    if ($('#EjecutarFuncion') != undefined) {
        funcion = $('#EjecutarFuncion').val();
    }
     /////////////////
    //escuchar botones
    $('body').on('click', 'button', function () {
        var form = $(this).closest('form');
        if (form.attr('id') == "adminRegistro") return;

        var url;
        var boton = $(this).attr('id');

        //saber si tengo que ejecutar alguna funcion al acabar ajax
        if ($(this).data('funcion') != undefined) funcion = $(this).data('funcion');

        //si se pulso el boton para borrar algun registro
        if (boton == "btnEliminar") {
            //comprobar si se seleccionaron registros
            if ($(".seleccion:checked").length == 0) {
                alert('Debe seleccionar algún registro');
                return false;
            }
            if (!confirm('¿Esta seguro de realizar esta acción?')) return false;
            else $('#Eliminar').val(1); //para detectar que he pulsado el boton eliminar
            
        } else {
            //es el boton para borrar el valor del  input que utilizo para filtrar
            var elInput = $(this).attr('id').substring(4);
            $('#' + elInput).val('');
        }
              inicioAjax(form);
    });

     /////////////////
    //escuchar  select
    $('body').on('change', 'select', function () {
        var form = $(this).closest('form');
        //es el select del formulario registro
        if (form.attr('id') != "adminRegistro") {
            $('#pagina').val('1');
            inicioAjax(form);
        }
 
    });

     ///////////////
    //escuchar input
    $('body').on('change', 'input', function () {
        var form = $(this).closest('form');
        if (form.attr('id') == "adminRegistro") return;
        //evito que se envie  si se clickea los checkbox... estos van por el boton eliminar
        if ($(this).attr('name') == "idEliminar" || ($(this).attr('name') == "todo")) return false;

        // para front Filtro-Salario --- si filtro salario =0 no ajax
        if ($(this).attr('id') == "porNombre") {
            if ($('#Salario').val() == 0) return false;
        }

        inicioAjax(form);
    });

     //////////////////////////////////////////////////////////////////////
    //escuchar enlaces solo envio por ajax las ordenaciones y la paginacion
    $('body').on('click', 'a', function () {
        var form = $(this).closest('form');
        if (form.attr('id') == "adminRegistro") return;
        //es paginacion
        //la paginacion su ul tiene una clase llamada pagination
        var ul = $(this).closest('ul');
        if (ul.attr('class') == "pagination") {
            var href = $(this).attr('href');
            if (href != undefined) {
                url = href.split('&')
                //para evitar que me cambie de pagina por postback
                $(this).attr('href', '#');
                pagina = url[0].split('=');
                //pagina donde quiero ir
                $('#pagina').val(pagina[1]);

                inicioAjax(form);
            }
        } else if ($(this).hasClass("filtro")) {
            //es ordenación
            // para enviar por ajax el enlace tiene que tener una clase llamada filtro
            //componer el nombre del input oculto relacionado con cada enlace que acabo de clickear
            var campo = $(this).attr('id') + "By";
            enlace = $(this).attr('id');
            switch ($('#' + campo).val()) {
                case "":
                    $('#' + campo).val('Desc')
                    clase = "fa fa-arrow-down";
                    break;
                case "Desc":
                    $('#' + campo).val('Asc')
                    clase = "fa fa-arrow-up"
                    break;
                default:
                    $('#' + campo).val('');
                    clase = "";
                    break;
            }

            QuitarOrdenacion();
            inicioAjax(form);

        // para la vista OfertaEmpleo
        } else if ($(this).hasClass("cambiarEstado")) {
            //obtener valor de la celda 0 de la fila seleccionada
            id = $(this).parents("tr").find("td").eq(0).text();
            $('#CambiarEstado').val(id);
            var form = $(this).closest('form');
            $(this).parents("tr").find("td").eq(5).html('<img src="../content/images/mini-load.gif"/>');
            inicioAjax(form); 
        }
    });

    ///////////////////////////
    //Para Front Filtro Salario
    $('#filtro-Salario').mouseup(function () {
        $("#pagina").val(1);
        var form = $(this).closest('form');
        inicioAjax(form);
    });

});

var inicioAjax = function (form) {

    // Creo un div que bloqueara todo el formulario
    var block = $('<div class="block-porAjax" />');
    form.prepend(block)
    url = form.attr('action');

    //empiezo ajax
    $.ajax({
        type: 'POST',
        url: url,
        data: form.serialize(),
        success: function (respuesta) {
            block.remove();
            $('#parcial').html(respuesta);
            $('#' + enlace).addClass(clase);

            //ejecutar funcion si procede
            if (funcion != undefined) setTimeout(funcion, 0);

            //inicializar el detector de eliminar
            if ($('#Eliminar').val() == 1) $('#Eliminar').val(0);
            if ($('#CambiarEstado').val() > 0) $('#CambiarEstado').val(0);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus + ' ' + errorThrown);
        }
    });
}
//solo ordeno por el concepto que hago click , el resto lo borro por si tiene algo;
var QuitarOrdenacion = function () {
    //en enlace que acabo de hacer click
    var enlaceClick = enlace + "By";
    //recorrro todos los enlaces que hay dentro de mi tabla que son los enlaces de ordenacion
    $("#miTabla a").each(function () {
        var suId = ($(this).attr('id'));
        //obtengo el campo  oculto relacionado con el enlace
        var campoOculto = $('#' + suId + "By").attr('id');
        //borro todo la ordenacion que pueda haber en los campos ocultos menos el que acabo de hacer click
        if (enlaceClick != campoOculto) $('#' + campoOculto).val('');
    });
}