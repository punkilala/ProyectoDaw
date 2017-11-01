//Configuracin colores para el Menu Top
var colorFondo = ['#ccc', '#51B6DB', '#68B93D', '#DBBB51', '#906292', '#B93D68', '#3ACBCB', '#D8BFD8'];

function Colorea(menuActivo) {

    //cambio a activo el li correspondiente
    $('#' + menuActivo).addClass('activo' + menuActivo);
    //cambio color de fondo de Menu Top
    $('.navbar-inverse').css('background-color', colorFondo[menuActivo]);
    $('.navbar-inverse').css('box-shadow', '-5px 2px 5px ' + colorFondo[menuActivo]);
    $('#secPrincipal').css('border-top', '5px solid ' + colorFondo[menuActivo]);
}
