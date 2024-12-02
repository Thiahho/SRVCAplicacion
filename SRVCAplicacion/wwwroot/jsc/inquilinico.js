// visitante.js

const { toLocaleString } = require("../fontawesome-free/js/v4-shims");

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
        //limpia los campos
        document.getElementById('inputNombreNV').value = '';
        document.getElementById('inputApellidoNV').value = '';
        document.getElementById('inputDNINV').value = '';
        document.getElementById('inputTelefonoNV').value = '';
    }

}
// fin crear visitante

async function formatearFecha(fecha) {
    const year = fecha.getFullYear();
    const month = String(fecha.getMonth() + 1).padStart(2, '0'); // Mes con dos dígitos
    const day = String(fecha.getDate()).padStart(2, '0'); // Día con dos dígitos
    const hours = String(fecha.getHours()).padStart(2, '0'); // Hora con dos dígitos
    const minutes = String(fecha.getMinutes()).padStart(2, '0'); // Minutos con dos dígitos
    const seconds = String(fecha.getSeconds()).padStart(2, '0'); // Segundos con dos dígitos

    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
}

async function BuscarPorDNI() {
    // Obtener el valor del input
    var dni = document.getElementById('inputDNIingreso').value;

    // Llamar a la función que utilizará ese valor
    console.log(dni); // Aquí puedes realizar lo que necesites con el valor
    try {
        const response = await fetch(`https://localhost:7285/api/inquilino/obtener/${dni}`);
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const datosPorDNI = await response.json();
        console.log('Registros recibidos:', datosPorDNI);


        if (datosPorDNI.nombre && datosPorDNI.apellido) {
            inputNombreIngreso.value = datosPorDNI.nombre; // Asigna el nombre 
            inputApellidoIngreso.value = datosPorDNI.apellido; // Asigna el apellido 
            alert("El inquilono se encuentra en el registro.");
        }

    } catch (error) {
        alert("el numero de dni no esta asociado a ningun inquilono registrado.");
        console.error('Error al mostrar los registros:', error);
        inputNombreIngreso.value = "";
        inputApellidoIngreso.value = "";
    }
}

async function guardarRegistro() {
    const identificacion_visita = document.getElementById('inputDNIingreso').value;
    
    const nombre = document.getElementById('inputNombreIngreso').value.trim();
    const apellido = document.getElementById('inputApellidoIngreso').value.trim();
    const nombre_visitante_inquilino = (nombre && apellido) ? nombre + ' ' + apellido : nombre + apellido;

    const fechaingreso = new Date();
    fechaingreso.setHours(fechaingreso.getHours() - 3);
    const hora_ingreso = fechaingreso.toISOString();
    console.log('Respuesta del servidor:', fechaingreso);
    console.log('Respuesta del servidor:', hora_ingreso);

    const motivo = document.getElementById('inputMotivoIngreso').value;
    const motivo_personalizado = document.getElementById('motivoPersonalizado').value;
    const depto_visita = document.getElementById('inputSectorDepto').value;
    const estado_visita = parseInt("1");
    const id_usuario = parseInt("1969");
    const id_visitante_inquilino = parseInt("1");
    const id_punto_control = parseInt("1");
    const nombre_punto_control = document.getElementById('inputNombrePControl').value;

    if (!nombre || !apellido || !identificacion_visita || !motivo || !motivo_personalizado || !depto_visita || !nombre_punto_control) {
        alert("Todos los campos son obligatorios.");
        document.getElementById('crearNuevoVisitante').disabled = false;
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
        //Limpia los campos al terminar
        document.getElementById('inputDNIingreso').value = '';
        document.getElementById('inputNombreIngreso').value = '';
        document.getElementById('inputApellidoIngreso').value = '';
        document.getElementById('inputMotivoIngreso').value = '';
        document.getElementById('motivoPersonalizado').value = '';
        document.getElementById('inputSectorDepto').value = '';
        document.getElementById('inputNombrePControl').value = '';

    }
}

async function mostrarRegistros() {
    try {
        const response = await fetch('https://localhost:7285/api/Busqueda/obtener');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const registros = await response.json();
        console.log('Registros recibidos:', registros);

        const tabla = document.getElementById('tablaRegistrosI');
        // tabla.innerHTML = ''; Limpia la tabla

        registros.forEach(registro => {
            const fila = document.createElement('tr');
            fila.innerHTML = `
                <td>${registro.id_registro_visitas}</td>
                <td>${registro.motivo}</td>
                <td>${registro.depto_visita}</td>
                <td>${registro.nombre_visitante_inquilino}</td>
                <td>${registro.identificacion_visita}</td>
                <td>${registro.hora_ingreso}</td>
                <td>${registro.hora_salida}</td>
                <td>
                    ${registro.hora_salida === null ? `<button type="button" id="marcaSalida" onclick="horaSalidas(${registro.identificacion_visita})">salida</button>` : ''  }
                </td>
                `;
            tabla.querySelector('tbody').appendChild(fila);
        });
    } catch (error) {
        console.error('Error al mostrar los registros:', error);
    }
}

//asdasdsa


//prueba
async function horaSalidas(dni) {
    const fechaSalida = new Date();
    const hora_formateada = fechaSalida.toISOString();
    console.log('Respuesta fecha salida:', fechaSalida);
    console.log('Respuesta formateada:', hora_formateada);

    try {
        const response = await fetch(`https://localhost:7285/api/busqueda/actualizarHorarioSalida/${dni}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(hora_formateada) // Enviamos el valor como una cadena ISO
        });

        // Verificar si la respuesta es exitosa
        if (!response.ok) {
            // Si la respuesta no es exitosa, procesamos el error
            const errorData = await response.json();  // Parsear la respuesta como JSON
            alert('hubo un error al intentar marcar la salida');
            console.error('Error:', errorData.mensaje);  // Mostrar el mensaje de error
        } else {
            // Si la respuesta es exitosa, procesamos la respuesta (aunque NoContent no tiene cuerpo)
            alert('Se marco la salida correctamente');
            console.log('Se marco la salida correctamente');
        }
    } catch (error) {
        console.error('Error en la solicitud:', error);
    } finally {
        // recarga de pagina
        location.reload();
    }
}




