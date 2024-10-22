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
async function crearVisitante() {
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
