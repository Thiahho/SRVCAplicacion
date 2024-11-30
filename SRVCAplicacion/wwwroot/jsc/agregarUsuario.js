
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

//ORIGINAL
//ORIGINAL
//ORIGINAL
//ORIGINAL
//ORIGINAL
//document.addEventListener('DOMContentLoaded', function () {
//    document.getElementById('btnConfirmar').addEventListener('click', agregarForm);
//});

//async function agregarForm() {
//    // Desactivar el botón para evitar múltiples clics
//    document.getElementById('btnConfirmar').disabled = false;

//    const nombre = document.getElementById('nombre').value;
//    const apellido = document.getElementById('apellido').value;
//    const identificacion = document.getElementById('identificacion').value;
//    const telefono = document.getElementById('telefono').value;
//    const estado = parseInt(document.getElementById('estado').value);
//    const id_punto_control = parseInt(document.getElementById('id_punto_control').value);

//    // Validar campos (Ejemplo: nombre y apellido no pueden estar vacíos)
//    if (!nombre || !apellido || !identificacion || !telefono || isNaN(estado) || !id_punto_control) {
//        alert("Todos los campos son obligatorios.");
//        document.getElementById('btnConfirmar').disabled = false; // Rehabilitar el botón
//        return;
//    }

//    const formData = {
//        nombre: nombre,
//        apellido: apellido,
//        identificacion: identificacion,
//        telefono: telefono,
//        estado: estado,
//        id_punto_control: id_punto_control
//    };

//    try {
//        const response = await fetch('https://localhost:7285/api/Inquilino/CrearInquilino', {
//            method: 'POST',
//            headers: {
//                'Content-type': 'application/json'
//            },
//            body: JSON.stringify(formData)
//        });

//        const result = await response.json();

//        if (response.ok) {
//            alert("Usuario creado exitosamente.");
//            document.getElementById('formUsuario').reset();  // Limpiar formulario
//        } else {
//            // Si hay un error en el backend, mostrarlo
//            alert("Error: " + result.message || "Error desconocido");
//        }
//    } catch (error) {
//        console.error('Error al enviar el formulario', error);
//        alert("Error al enviar el formulario.");
//    } finally {
//        // Rehabilitar el botón después de que se haya completado la acción
//        document.getElementById('btnConfirmar').disabled = false;
//    }
//}

//FINORIGINAL
//FINORIGINAL
//FINORIGINAL
//FINORIGINAL
//FINORIGINAL
//FINORIGINAL





document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('btnConfirmar').addEventListener('click', agregarForm);
});

async function agregarForm() {
    const boton= document.getElementById('btnConfirmar').boton.disabled = false;

    const formData = {
        usuario: document.getElementById('usuario').value.trim(),
        apellido : document.getElementById('apellido').value.trim(),
        identificacion : document.getElementById('identificacion').value.trim(),
        telefono : document.getElementById('telefono').value,
        estado : parseInt(document.getElementById('estado').value),
        id_punto_control : parseInt(document.getElementById('id_punto_control').value)
    };
    if (Object.values(formData).some(value => !value || isNaN(formData.estado) || isNaN(formData.id_punto_control))) {
        mostrarAlerta("Todos los campos son obligatorios.");
        boton.disabled = false;
        return;
    }

    try
    {
        const response = await fetch('https://localhost:7285/api/Usuario/Crear', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(formData)
        });


        if (response.ok) {
            alert("Usuario creado exitosamente.");
            document.getElementById('formUsuario').reset();
        } else {
            const error = await response.json();
            mostrarAlerta(error.error || "Error desconocido.");
        }
    } catch (error) {
        console.error("Error:", error);
        mostrarAlerta("Error al conectar con el servidor.");
    } finally {
        boton.disabled = false; // Reactivar botón
    }
}
function mostrarAlerta(mensaje) {
    const alerta = document.getElementById('formAlert');
    alerta.textContent = mensaje;
    alerta.classList.remove('d-none');
    setTimeout(() => alerta.classList.add('d-none'), 5000);
}