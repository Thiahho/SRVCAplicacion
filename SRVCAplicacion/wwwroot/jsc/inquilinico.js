// visitante.js

// Función para obtener todos los visitantes
async function obtenerVisitantes() {
    const response = await fetch('/api/VisitanteInquilino');
    const data = await response.json();

    // Mapeamos los datos para interpretar los valores de los enums
    data.forEach(visitante => {
        console.log(`Nombre: ${visitante.nombre}`);
        console.log(`Activo: ${visitante.activo === 1 ? 'Activo' : 'Inactivo'}`);
        console.log(`Estado: ${visitante.estado === 1 ? 'Dentro' : 'Fuera'}`);
    });
}

// Función para crear un nuevo visitante
async function crearVisitantee() {
    const nuevoVisitante = {
        nombre: "Juan",
        apellido: "Perez",
        identificacion: "12345678",
        activo: 1,  // Enum para Activo
        telefono: "1234567890",
        imgpath: "/images/juan.jpg",
        estado: 1,  // Enum para Estado (Dentro)
        id_punto_control: 1
    };

    const response = await fetch('/api/VisitanteInquilino', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(nuevoVisitante)
    });

    if (response.ok) {
        const data = await response.json();
        console.log('Visitante creado:', data);
    }
}

// Puedes agregar más funciones aquí...

// Función para crear un nuevo visitante axxxx
async function CrearVisitante() {
    // Desactivar el botón para evitar múltiples clics
    document.getElementById('btnConfirmar').disabled = false;

    const nombre = document.getElementById('nombre').value;
    const apellido = document.getElementById('apellido').value;
    const identificacion = document.getElementById('identificacion').value;
    const telefono = document.getElementById('telefono').value;
    const estado = parseInt(document.getElementById('estado').value);
    const id_punto_control = parseInt(document.getElementById('id_punto_control').value);

    // Validar campos (Ejemplo: nombre y apellido no pueden estar vacíos)
    if (!nombre || !apellido || !identificacion || !telefono || isNaN(estado) || !id_punto_control) {
        alert("Todos los campos son obligatorios.");
        document.getElementById('btnConfirmar').disabled = false; // Rehabilitar el botón
        return;
    }

    const formData = {
        nombre: nombre,
        apellido: apellido,
        identificacion: identificacion,
        telefono: telefono,
        estado: estado,
        id_punto_control: id_punto_control
    };

    try {
        const response = await fetch('https://localhost:7285/api/Inquilino/CrearInquilino', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        const result = await response.json();

        if (response.ok) {
            alert("Usuario creado exitosamente.");
            document.getElementById('formUsuario').reset();  // Limpiar formulario
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
