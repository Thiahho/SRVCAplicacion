async function agregarForm() {
    // Obtener los valores del formulario
    const nombre = document.getElementById('nombre').value.trim();
    const apellido = document.getElementById('apellido').value.trim();
    const identificacion = document.getElementById('identificacion').value.trim();
    const activo = parseInt(document.getElementById('activo').value);
    const telefono = document.getElementById('telefono').value.trim();
    //let imgPath = document.getElementById('imgPath').value.trim();
    
    //if (imgPath == "") {
    //    imgPath = null;
    //}
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
       /* imgPath,*/
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

        // Verifica si la respuesta es OK y obtiene el JSON solo si lo es
        const result= await response.json(); // Obtén la respuesta como texto

        if (response.ok) {
            //const result = JSON.parse(responseText); // Intenta parsear como JSON solo si la respuesta es válida
            alert("Usuario creado exitosamente.");
            document.getElementById('formUsuario').reset(); // Limpiar formulario
        } else {
            // Si no es 200 OK, muestra el error
            alert("Error: " + (result.message || " Error desconocido"));
        }
    } catch (error) {
        console.error('Error al enviar el formulario:', error);
        alert("Error al enviar el formulario. Por favor, inténtalo de nuevo.");
    }
}
