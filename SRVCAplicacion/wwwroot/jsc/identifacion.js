async function cargarIdentificaciones() {
    try {
        const response = await fetch('/api/inquilino/identificacion');
        if (!response.ok) {
            throw new Error('Error en la respuesta de la API: ' + response.statusText);
        }
        const identificaciones = await response.json();
        const select = document.getElementById('identificacion');

        select.innerHTML = '<option value="" selected disabled>Seleccione identificación</option>';
        identificaciones.forEach(ident => {
            const option = document.createElement('option');
            option.value = ident.identificacion;
            option.textContent = ident.identificacion;
            select.appendChild(option);
        });
    } catch (error) {
        console.error('Error al cargar identificaciones:', error);
    }
}


async function cargarMotivos() {
    try {
        const response = await fetch("api/motivo/obtener");
        if (!response.ok) {
            throw new Error('Error al obtener los motivos. ' + response.statusText);
        }
        const motivos = await response.json();
        const motivosSelecc = document.getElementById('motivo');

        motivosSelecc.innerHTML = '<option value="" selected disabled>Seleccione motivo</option>';

        motivos.forEach(motivo => {
            const option = document.createElement('option');
            option.value = motivo.nombre_motivo;
            option.textContent = motivo.nombre_motivo;
            motivosSelecc.appendChild(option);

        });
    } catch (error) {
        console.error('Error al cargar motivo. ', error);
    }
}

async function cargarDepartamentos() {
    try {
        const response = await fetch('/api/departamento/obtener');
        if (!response.ok) {
            throw new Error('Error al obtener departamentos: ' + response.statusText);
        }

        const departamentos = await response.json();
        const departamentoSelect = document.getElementById('depto_visita');
            
        departamentoSelect.innerHTML = '<option value="" selected disabled>Seleccione Departamento</option>';

        departamentos.forEach(departamento => {
            const option = document.createElement('option');
            option.value = departamento.Descripcion; // Asegúrate de usar el id correcto
            option.textContent = departamento.Descripcion; // El texto que se verá
            departamentoSelect.appendChild(option);
        });

    } catch (error) {
        console.error('Error al cargar departamentos:', error);
    }
}



document.addEventListener('DOMContentLoaded', async () => {
    await cargarIdentificaciones();
    await cargarMotivos();
    await cargarDepartamentos();
});
