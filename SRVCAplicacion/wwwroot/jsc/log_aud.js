//Funcion para mostrar Cambios(admin)
async function mostrarLog() {

    try {
        const response = await fetch('api');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const log_aud = await response.json();
        console.log('Log recibidos:', log_aud);

        const tabla = document.getElementById('logTable');
        // tabla.innerHTML = ''; Limpia la tabla

        log_aud.forEach(log => {
            const fila = document.createElement('tr');
            fila.innerHTML = `
                <td>${log.id_log_aud}</td>
                <td>${log.id_usuario}</td>
                <td>${log.accion}</td>
                <td>${log.hora}</td>
                <td>${log.valor_original}</td>
                <td>${log.valor_nuevo}</td>
                <td>${log.id_punto_control}</td>
            `;
            tabla.querySelector('tbody').appendChild(fila);
        });
    } catch (error) {
        console.error('Error al mostrar los cambios:', error);
    }
}