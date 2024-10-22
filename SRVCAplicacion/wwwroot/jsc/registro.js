document.getElementById('guardar').addEventListener('click', async function () {
    const motivo = document.getElementById('motivo').value;
    const departamento = document.getElementById('departamento').value;

    if (motivo && departamento) {
        const registro = {
            motivo: motivo,
            departamento: departamento,
            identificacion_visita: document.getElementById('identificacion').value
            // Agrega los demás campos según sea necesario
        };

        try {
            const response = await fetch('/api/Busqueda/CrearRegistro', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(registro)
            });

            if (response.ok) {
                const result = await response.json();
                alert(result.Message); // Mensaje de éxito
            } else {
                alert('Error al guardar el registro');
            }
        } catch (error) {
            console.error('Error al guardar:', error);
        }
    } else {
        alert('Por favor, complete todos los campos obligatorios.');
    }
});
