//Funcion para mostrar Cambios(admin)
async function mostrarLog() {

    try {
        const response = await fetch('http://localhost:5000/api/LogAud/obtener');
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



//MostrarLOG mejorado
async function mostrarLogMejorado() {
    try {
        const response = await fetch('http://localhost:5000/api/LogAud/obtener');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const log_aud = await response.json();
        console.log('Registros recibidos:', log_aud);

        const tbody = document.querySelector('#tablaLogAud tbody');
        tbody.innerHTML = ''; // Limpiar contenido previo

         log_aud
                .forEach(log => {
                    const fila = `
                        <tr>
                            <td><input type="text" class="form-control" value="${log.id_log_aud}" readonly></td>
                            <td><input type="text" class="form-control" value="${log.id_usuario}" readonly></td>
                            <td><input type="text" class="form-control" value="${log.accion}" readonly></td>
                            <td><input type="text" class="form-control" value="${log.hora}" readonly></td>
                            <td>
                                ${log.valor_original != null
                                    ? `<textarea class="form-control" readonly>${log.valor_original}</textarea>`
                                    : `<input type="text" class="form-control" value=" " readonly>`}
                            </td>
                            <td><textarea class="form-control" readonly>${log.valor_nuevo}</textarea></td>
                        </tr>
                    `;
                    tbody.insertAdjacentHTML('beforeend', fila);
                });

    } catch (error) {
        console.error('Error al mostrar los registros:', error);
    }
}

async function sincronizar() {
    const statusDiv = document.getElementById("status");

    try {
        const response = await fetch("http://localhost:5000/api/Sincronizacion/GenerarJson", {
            method: "POST"
        });

        if (response.ok) {
            const result = await response.json();
            statusDiv.innerHTML = `<span style="color: green;">${result.Message}: ${result.Path}</span>`;
        } else {
            const error = await response.json();
            statusDiv.innerHTML = `<span style="color: red;">Error: ${error.Message}</span>`;
        }
    } catch (error) {
        console.error('Error al mostrar los registros:', error);
    }
}