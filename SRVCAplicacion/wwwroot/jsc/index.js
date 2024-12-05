$(document).ready(function () {
    $('#example').DataTable({
        "language": {
            "decimal": "",
            "emptyTable": "No hay datos disponibles en la tabla",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
            "infoEmpty": "Mostrando 0 a 0 de 0 registros",
            "infoFiltered": "(filtrado de _MAX_ registros totales)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ registros",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "No se encontraron coincidencias",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            "aria": {
                "sortAscending": ": activar para ordenar la columna ascendente",
                "sortDescending": ": activar para ordenar la columna descendente"
            }
        }
    });
});


(function () {
    'use strict'

    var forms = document.querySelectorAll('.needs-validation')


    Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault()
                event.stopPropagation()


                document.getElementById('formAlert').classList.remove('d-none');
            } else {
                document.getElementById('formAlert').classList.add('d-none');
            }

            form.classList.add('was-validated')
        }, false)
    })
})


/**
 
Función para actualizar el contador en tiempo real.*/
async function ContadorInquilinos() {
    try {
        const response = await fetch("https://localhost:7285/api/Inquilino/ConteoInquilino"); // Ruta del endpoint
        if (!response.ok) throw new Error("Error al obtener datos");

        const data = await response.json();
        const countElement = document.getElementById("totalInquilinos");

        if (countElement) {
            countElement.textContent = data.activeCount || 0; // Mostrar el número de activos
        }
    } catch (error) {
        console.error("Error al actualizar el contador:", error);
    }
}

// Actualizar el contador cada 5 segundos
setInterval(ContadorInquilinos, 5000);

// Llamar la función al cargar la página
document.addEventListener("DOMContentLoaded", ContadorInquilinos);