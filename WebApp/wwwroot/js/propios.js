function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

LoadData = function (Id, pControler) {
    var jqxhr = $.get(pControler + Id, function (data) {
        $('#dData').html(data);
    });
}

function ValidarEliminar(url) {
    Swal.fire({
        title: '¿Está Seguro(a)?',
        text: "No podrá recuperar la información!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#30a5ff',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar definitivamente!',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = url;
            //Confirmar('Registro eliminado!');
            //setTimeout(() => { window.location.href = url; }, 1000);
        }
    })
}

function Confirmar(text, time = 1000) {
    Swal.fire({
        icon: 'success',
        title: text,
        showConfirmButton: false,
        timer: time
    })
}

function MostrarError(error) {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        text: error,
        confirmButtonColor: '#30a5ff'
    })
}

function wait(ms) {
    var start = new Date().getTime();
    var end = start;
    while (end < start + ms) {
        end = new Date().getTime();
    }
}