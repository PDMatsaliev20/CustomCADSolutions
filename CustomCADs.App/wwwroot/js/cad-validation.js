$(function () {
    $('#cad-folder').hide();
    $('#cad-file').show();
});

$('#switch').click(function () {
    $('#cad-file').toggle();
    $('#cad-folder').toggle();
});