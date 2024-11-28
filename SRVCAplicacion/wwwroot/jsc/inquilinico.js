﻿// visitante.js

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
/*
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
*/

// Puedes agregar más funciones aquí...

// Función para crear un nuevo visitante axxxx
async function CrearVisitante() {

    const nombre = document.getElementById('inputNombreNV').value;
    const apellido = document.getElementById('inputApellidoNV').value;
    const identificacion = document.getElementById('inputDNINV').value;
    const telefono = document.getElementById('inputTelefonoNV').value;
    const activo = parseInt("1");
    const estado = parseInt("1");
    // const estado = parseInt(document.getElementById('estado').value);
    const id_punto_control = parseInt("1"); 
    //const id_punto_control = parseInt(document.getElementById('id_punto_control').value);

    // Validar campos (Ejemplo: nombre y apellido no pueden estar vacíos)
    if (!nombre || !apellido || !identificacion || !telefono ) {
        alert("Todos los campos son obligatorios.");
        document.getElementById('crearNuevoVisitante').disabled = false; // Rehabilitar el botón
        return;
    }

    const formData = {
        nombre: nombre,
        apellido: apellido,
        identificacion: identificacion,
        telefono: telefono,
        estado: estado,
        activo: activo,
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
            alert("Visitante creado exitosamente.");
            document.getElementById('formCrearVisitante').reset();  // Limpiar formulario
        } else {
            // Si hay un error en el backend, mostrarlo
            alert("Error: " + result.message || "Error desconocido");
        }
    } catch (error) {
        console.error('Error al enviar el formulario', error);
        alert("Error al enviar el formulario.");
    } finally {
        // Rehabilitar el botón después de que se haya completado la acción
        //
        document.getElementById('inputNombreNV').value = '';
        document.getElementById('inputApellidoNV').value = '';
        document.getElementById('inputDNINV').value = '';
        document.getElementById('inputTelefonoNV').value = '';
    }

}
// fin crear visitante

async function buscarPorDNI() {

}

async function guardarRegistro() {
    const identificacion_visita = document.getElementById('inputDNIingreso').value;
    //const nombre_visitante_inquilino = document.getElementById('inputNombreNV').value + ' ' + document.getElementById('inputApellidoNV').value;
    //const apellido = document.getElementById('inputApellidoNV').value;
    
    const nombre = document.getElementById('inputNombreIngreso').value.trim();
    const apellido = document.getElementById('inputApellidoIngreso').value.trim();
    const nombre_visitante_inquilino = (nombre && apellido) ? nombre + ' ' + apellido : nombre + apellido;

    //const motivo = document.getElementById('selectMotivo').value;
    const selectMotivo = document.getElementById('selectMotivo');
    const motivoC = selectMotivo;
    // Se ejecutará cada vez que cambie la selección
    selectMotivo.addEventListener('change', function () {
        motivoC = selectMotivo.value;
        console.log(motivo); // Para verificar el valor seleccionado
    });
    const motivo = motivoC;

    const motivo_personalizado = document.getElementById('motivoPersonalizado').value;
    const depto_visita = document.getElementById('inputSectorDepto').value;
    const hora_ingreso = new Date();
    const estado_visita = parseInt("1");
    const id_usuario = parseInt("1");
    const id_visitante_inquilino = parseInt("1");
    // const estado = parseInt(document.getElementById('estado').value);
    const id_punto_control = parseInt("1");
    //const id_punto_control = parseInt(document.getElementById('id_punto_control').value);
    const nombre_punto_control = document.getElementById('inputNombrePControl').value;

    // Validar campos (Ejemplo: nombre y apellido no pueden estar vacíos)
    if (!nombre || !apellido || !identificacion_visita /*|| !motivo */|| !motivo_personalizado || !depto_visita || !nombre_punto_control) {
        alert("Todos los campos son obligatorios.");
        document.getElementById('crearNuevoVisitante').disabled = false; // Rehabilitar el botón
        return;
    }

    const formData = {
        nombre_visitante_inquilino: nombre_visitante_inquilino,
        identificacion_visita: identificacion_visita,
        motivo: motivo,
        motivo_personalizado: motivo_personalizado,
        depto_visita: depto_visita,
        hora_ingreso: hora_ingreso,
        estado_visita: estado_visita,
        id_punto_control: id_punto_control,
        id_usuario: id_usuario,
        id_visitante_inquilino: id_visitante_inquilino,
        nombre_punto_control: nombre_punto_control
    };

    try {
        const response = await fetch('https://localhost:7285/api/Busqueda/CrearRegistro', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        const result = await response.json();

        if (response.ok) {
            alert("Registro creado exitosamente.");
            document.getElementById('formCrearVisitante').reset();  // Limpiar formulario
        } else {
            // Si hay un error en el backend, mostrarlo
            alert("Error: " + result.message || "Error desconocido");
        }
    } catch (error) {
        console.error('Error al enviar el formulario', error);
        alert("Error al enviar el formulario.");
    } finally {
        // Rehabilitar el botón después de que se haya completado la acción
        //
        document.getElementById('inputNombreIngreso').value = '';
        document.getElementById('inputApellidoIngreso').value = '';
        document.getElementById('inputDNIingreso').value = '';
        document.getElementById('motivoPersonalizado').value = '';
        document.getElementById('inputNombrePControl').value = '';
    }

}
