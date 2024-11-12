//async function agregarForm() {
//    const formData = {
//        nombre: document.getElementById('nombre').value,
//        apellido: document.getElementById('apellido').value,
//        identificacion: document.getElementById('identificacion').value,
//        telefono: document.getElementById('telefono').value,
//        estado: parseInt(document.getElementById('estado').value),
//        id_punto_control: parseInt(document.getElementById('id_punto_control').value)
//    };

//    try {
//        const response = await fetch('https://localhost:7285/api/Inquilino/CrearInquilino', {
//            method = 'POST',
//            headers: {
//                'Content-type': 'application/json'
//            },
//            body: JSON.stringify(formData)
//        });

//        const result = await response.json();
//        if (response.ok) {
//            alert("Usuario creado exitosamente.");
//            document.getElementById('formUsuario').reset();
//        }
//        else {
//            alert("Error" + result.message);
//        }

//    }
//    catch (error) {
//        console.error('Error al enviar el formulario', error);
//        alert("Error al enviar el formulario.");
//    }
//}

document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('btnConfirmar').addEventListener('click', agregarForm);
});

async function agregarForm() {
    // Desactivar el botón para evitar múltiples clics
    document.getElementById('btnConfirmar').disabled = false;

    const token = document.getElementById('token').value;
    const nombre = document.getElementById('nombre').value;
    const estado = document.getElementById('estado').value;

    // Validar campos (Ejemplo: nombre y apellido no pueden estar vacíos)
    if (!token || !nombre || !estado ||) {
        alert("Todos los campos son obligatorios.");
        document.getElementById('btnConfirmar').disabled = false; // Rehabilitar el botón
        return;
    }

    const formData = {
        token: token,
        nombre: nombre,
        estado: estado,
    };

    try {
        const response = await fetch('https://localhost:7285/api/Puntos/AgregarPunto', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        const result = await response.json();

        if (response.ok) {
            alert("Punto creado exitosamente.");
            document.getElementById('formPunto').reset();  // Limpiar formulario
        } else {
            // Si hay un error en el backend, mostrarlo
            alert("Error: " + result.message || "Error desconocido");
        }
    } catch (error) {
        console.error('Error al enviar el formulario', error);
        alert("Error al enviar el formulario.");
    } finally {
        // Rehabilitar el botón después de que se haya completado la acción
        document.getElementById('btnConfirmar').disabled = false;
    }
}
