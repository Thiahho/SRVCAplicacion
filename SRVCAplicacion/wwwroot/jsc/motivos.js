async function cargarMotivos() {
    try {
        const response = await fetch('/Motivo/motivos'); // Asegúrate de que la URL sea correcta
        const motivos = await response.json();
        const select = document.getElementById('motivo');

        select.innerHTML = '<option value="" selected disabled>Seleccione un motivo</option>';
        motivos.forEach(motivo => {
            const option = document.createElement('option');
            option.value = motivo.value; // Usa el valor del objeto SelectListItem
            option.text = motivo.text;    // Usa el texto del objeto SelectListItem
            select.appendChild(option);
        });
    } catch (error) {
        console.error('Error al cargar motivos:', error);
    }
}
