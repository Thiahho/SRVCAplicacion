//Funcion para mostrar usuarios(admin)
async function mostrarInquilino() {

    try {
        const response = await fetch('https://localhost:7285/api/Inquilino/ObtenerTodos');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const inquilinos = await response.json();
        console.log('Inquilinos recibidos:', inquilinos);

        const tabla = document.getElementById('InquilinoTable');
        // tabla.innerHTML = ''; Limpia la tabla

        inquilinos.forEach(inquilino => {
            const fila = document.createElement('tr');
            fila.innerHTML = `
                <td>${inquilino.id_visitante_inquilino}</td>
                <td>${inquilino.nombre}</td>
                <td>${inquilino.apellido}</td>
                <td>${inquilino.identificacion}</td>
                <td>${inquilino.activo}</td>
                <td>${inquilino.telefono}</td>
                <td>${inquilino.estado}</td>
                <td>${inquilino.id_punto_control}</td>
            `;
            tabla.querySelector('tbody').appendChild(fila);
        });
        $('#InquilinoTable').DataTable({
            destroy: true,
            //scrollY: '400px',
            scrollCollapse: true,
            paging: true
        });

    } catch (error) {
        console.error('Error al mostrar inquilinos:', error);
    }
}

document.addEventListener('DOMContentLoaded', mostrarInquilino);