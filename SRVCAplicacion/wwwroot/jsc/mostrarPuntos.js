//Funcion para mostrar Puntos(admin)
async function mostrarPuntos() {

    try {
        const response = await fetch('https://localhost:7285/api/Puntos/ObtenerPuntos');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const Puntos = await response.json();
        console.log('Puntos recibidos:', Puntos);

        const tabla = document.getElementById('puntosTable');
        // tabla.innerHTML = ''; Limpia la tabla

        Puntos.forEach(Puntos => {
            const fila = document.createElement('tr');
            fila.innerHTML = `
                <td>${Puntos.id_punto_control}</td>
                <td>${Puntos.token}</td>
                <td>${Puntos.nombre_punto_control}</td>
                <td>${Puntos.estado}</td>
            `;
            tabla.querySelector('tbody').appendChild(fila);
        });
    } catch (error) {
        console.error('Error al mostrar Puntos:', error);
    }
}