//Funcion para mostrar usuarios(admin)
async function mostrarPuntos() {

    try {
        const response = await fetch('https://localhost:7285/api/Puntos/ObtenerPuntos');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const Puntos_de_controles = await response.json();
        console.log('Puntos recibidos:', Puntos_de_controles);

        const tabla = document.getElementById('PuntosTable');
        // tabla.innerHTML = ''; Limpia la tabla

        Puntos_de_controles.forEach(Puntos_de_controle => {
            const fila = document.createElement('tr');
            fila.innerHTML = `
                <td>${Puntos_de_controle.id_punto_control}</td>
                <td>${Puntos_de_controle.token}</td>
                <td>${Puntos_de_controle.nombre_punto_control}</td>
                <td>${Puntos_de_controle.estado}</td>
                <td>${Puntos_de_controle.id}</td>
              
            `;
            tabla.querySelector('tbody').appendChild(fila);
        });
        $('#PuntosTable').DataTable({
            destroy: true,
            //scrollY: '400px',
            scrollCollapse: true,
            paging: true
        });

    } catch (error) {
        console.error('Error al mostrar puntos:', error);
    }
}

document.addEventListener('DOMContentLoaded', mostrarPuntos);
