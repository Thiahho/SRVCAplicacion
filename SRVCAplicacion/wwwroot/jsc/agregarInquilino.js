async function agregarForm() {
    // Obtener los valores del formulario
    const nombre = document.getElementById('nombre').value.trim();
    const apellido = document.getElementById('apellido').value.trim();
    const identificacion = document.getElementById('identificacion').value.trim();
    const activo = parseInt(document.getElementById('activo').value);
    const telefono = document.getElementById('telefono').value.trim();
    const estado = parseInt(document.getElementById('estado').value);
    const id_punto_control = parseInt(document.getElementById('id_punto_control').value);

    // Validar campos
    if (!nombre || !apellido || !identificacion || !telefono || isNaN(estado) || isNaN(activo) || isNaN(id_punto_control)) {
        alert("Todos los campos son obligatorios y deben ser válidos.");
        return;
    }

    const formData = {
        nombre,
        apellido,
        identificacion,
        activo,
        telefono,
        estado,
        id_punto_control
    };
    console.log("Datos antes de enviar:", formData);

    try {
        // Enviar la solicitud al backend
        const response = await fetch('https://localhost:7285/api/Inquilino/CrearInquilino', {
            method: 'POST',
            headers: { 'Content-type': 'application/json' },
            body: JSON.stringify(formData)
        });

        // Obtener la respuesta como JSON
        const result = await response.json();

        if (!response.ok) {
            throw new Error(`Error ${response.status}: ${result.error || "Error desconocido"}`);
        } else {
            alert("Visitante creado exitosamente.");
            document.getElementById('formInquilino').reset(); // Limpiar formulario
            window.location.href = "/listaInquilino";
        }
    } catch (error) {
        console.log(error);  // Para inspeccionar el error completo
        alert(`Error al enviar el formulario: ${error.message}`);
    }
}