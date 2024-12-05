
$('#guardarCambios').click(function () {
    var formData = $('#formInstitucion').serialize(); // Recoger los datos del formulario

    $.ajax({
        url: 'guardar_institucion.php',
        type: 'POST',
        data: formData,
        success: function (response) {
            var result = JSON.parse(response);
            alert(result.message); // Mostrar un mensaje de éxito o error

            // Reiniciar el formulario
            $('#formInstitucion')[0].reset();

            // Cerrar el modal
            $('#edit2').modal('hide');
        },
        error: function () {
            alert('Error al guardar la institución.');
        }
    });
});
$(document).ready(function () {
    // Cargar los datos en el DataTable
    $('#tablaInstituciones').DataTable({
        ajax: {
            url: 'obtener_instituciones.php', // URL del archivo PHP
            dataSrc: ''
        },
        columns: [
            { data: 'id' },
            { data: 'dni' },
            { data: 'nombre' },
            { data: 'apellido' },
            { data: 'parentesco' },
            {
                data: null,
                render: function (data, type, row) {
                    return `<button class="btn btn-danger" onclick="eliminarInstitucion(${row.id})">Eliminar</button>`;
                }
            }
        ]
    });
});

// Función para eliminar institución
function eliminarInstitucion(id) {
    if (confirm("¿Estás seguro de que deseas eliminar esta institución?")) {
        $.ajax({
            url: 'eliminar_institucion.php', // Archivo para manejar la eliminación
            type: 'POST',
            data: { id: id },
            success: function (response) {
                alert('Institución eliminada correctamente.');
                $('#tablaInstituciones').DataTable().ajax.reload(); // Recargar la tabla
            },
            error: function () {
                alert('Error al eliminar la institución.');
            }
        });
    }
}