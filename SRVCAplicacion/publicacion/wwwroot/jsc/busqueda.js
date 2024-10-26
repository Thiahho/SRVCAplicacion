document.getElementById('buscar').addEventListener('click', async function () {
    const condicion = document.getElementById('condicion').value;
    const desde = document.getElementById('desde').value;
    const hasta = document.getElementById('hasta').value;

    // Validación de fechas
    const fechaDesde = desde ? new Date(desde).toISOString() : null;
    const fechaHasta = hasta ? new Date(hasta).toISOString() : null;

    // Construir la URL con parámetros de consulta
    let url = `/api/visitas/buscar?condicion=${encodeURIComponent(condicion)}`;
    if (fechaDesde && fechaHasta) {
        url += `&desde=${fechaDesde}&hasta=${fechaHasta}`;
    }

    try {
        const response = await fetch(url);

        if (response.ok) {
            const registros = await response.json();
            mostrarRegistrosEnTabla(registros); // Mostrar los resultados en la tabla
        } else {
            const errorData = await response.json();
            alert(`Error: ${errorData}`);
            limpiarTabla(); // Limpia la tabla si no se encuentran registros
        }
    } catch (error) {
        console.error('Error en la búsqueda:', error);
    }
});

function mostrarRegistrosEnTabla(registros) {
    const tbody = document.getElementById('tbody-resultados');
    tbody.innerHTML = ''; // Limpia la tabla antes de agregar nuevos datos

    registros.forEach(registro => {
        const fila = document.createElement('tr');

        fila.innerHTML = `
            <td>${registro.identificacion_visita}</td>
            <td>${registro.nombre_encargado}</td>
            <td>${registro.depto_visita}</td>
            <td>${registro.nombre_visitante_inquilino}</td>
            <td>${registro.motivo}</td>
            <td>${registro.motivo_personalizado}</td>
            <td>${registro.hora_ingreso ? new Date(registro.hora_ingreso).toLocaleString() : ''}</td>
            <td>${registro.hora_salida ? new Date(registro.hora_salida).toLocaleString() : ''}</td>
        `;

        tbody.appendChild(fila); // Añade la fila a la tabla
    });
}