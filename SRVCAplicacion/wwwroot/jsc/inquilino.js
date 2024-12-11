async function CrearInquilino() {

    const nombre = document.getElementById('inputNombreNI').value;
    const apellido = document.getElementById('inputApellidoNI').value;
    const identificacion = document.getElementById('inputDNINI').value;
    const telefono = document.getElementById('inputTelefonoNI').value;
    //1 ingreso, 0 salio
    const activo = 0;
    //1 inquilino, 2 visitante
    const estado = 1;
    // const estado = parseInt(document.getElementById('estado').value);
    const id_punto_control = 1;
    //const id_punto_control = parseInt(document.getElementById('id_punto_control').value);

    // Validar campos (Ejemplo: nombre y apellido no pueden estar vacíos)
    if (!nombre || !apellido || !identificacion || !telefono) {
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
            alert("Inquilino creado exitosamente.");
            document.getElementById('formCrearVisitante').reset();  // Limpiar formulario
        } else {
            // Si hay un error en el backend, mostrarlo
            alert("Error: " + result.message || "el numero de dni ya esta registrado en la base de datos");
        }
    } catch (error) {
        console.error('Error al enviar el formulario', error);
        alert("Error al enviar el formulario.");
    } finally {
        //limpia los campos
        document.getElementById('inputNombreNI').value = '';
        document.getElementById('inputApellidoNI').value = '';
        document.getElementById('inputDNINI').value = '';
        document.getElementById('inputTelefonoNI').value = '';
    }

}

// fin crear visitante

async function BuscarPorDNIInquilino() {
    // Obtener el valor del input
    var dni = document.getElementById('inputDNIingresoI').value;

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


        if (datosPorDNI.nombre && datosPorDNI.apellido && datosPorDNI.id_visitante_inquilino) {
            inputIdInquilinoIngresoI.value = datosPorDNI.id_visitante_inquilino; // Asigna el id 
            inputNombreIngresoI.value = datosPorDNI.nombre; // Asigna el nombre 
            inputApellidoIngresoI.value = datosPorDNI.apellido; // Asigna el apellido 
            alert("El inquilono se encuentra en el registro.");
        }

    } catch (error) {
        alert("el numero de dni no esta asociado a ningun inquilono registrado.");
        console.error('Error al mostrar los registros:', error);
        inputDNIingresoI.value = "";
        inputIdInquilinoIngresoI.value = "";
        inputNombreIngresoI.value = "";
        inputApellidoIngresoI.value = "";
    }
}

async function guardarRegistroInquilino() {
    try {
        // Realizar la llamada al endpoint para obtener los claims
        const responseClaims = await fetch('https://localhost:7285/api/Usuario/obtener-claims');
        const dataClaims = await responseClaims.json();

        // Acceder a los valores de los claims 
        var id_usuario = dataClaims.idUsarioLog;
        var nombre_encargado = dataClaims.nombre_encargadoLog;
        var id_punto_control = dataClaims.id_punto_controlLog;

        // prueba para ver por consola si trae los valores correctos
        console.log('ID Usuario:', id_usuario);
        console.log('Nombre Encargado:', nombre_encargado);
        console.log('ID Punto Control:', id_punto_control);

        const identificacion_visita = document.getElementById('inputDNIingresoI').value;
        const id_visitante_inquilino = document.getElementById('inputIdInquilinoIngresoI').value;
        const nombre = document.getElementById('inputNombreIngresoI').value.trim();
        const apellido = document.getElementById('inputApellidoIngresoI').value.trim();
        const nombre_visitante_inquilino = (nombre && apellido) ? nombre + ' ' + apellido : nombre + apellido;

        const fechaingreso = new Date();
        //fechaingreso.setHours(fechaingreso.getHours() - 3);
        const hora_ingreso = fechaingreso.toISOString();
        console.log('Fecha y Hora de Ingreso:', hora_ingreso);

        const motivo = document.getElementById('inputMotivoIngresoI').value;
        const motivo_personalizado = document.getElementById('motivoPersonalizadoI').value;
        const depto_visita = document.getElementById('inputSectorDeptoI').value;
        const estado_visita = 1;
        const nombre_punto_control = document.getElementById('inputNombrePControlI').value;
        const estado_actualizacion = 1;

        // Valida que los campos esten todos llenos
        if (!nombre || !apellido || !identificacion_visita || !motivo || !motivo_personalizado || !depto_visita || !nombre_punto_control) {
            alert("Todos los campos son obligatorios.");
            document.getElementById('crearNuevoVisitante').disabled = false; // Asegúrate de que el botón se habilite
            return;
        }

        // crea una nueva const con los datos ingresados
        const formData = {
            id_usuario: id_usuario,
            nombre_encargado: nombre_encargado,
            nombre_visitante_inquilino: nombre_visitante_inquilino,
            identificacion_visita: identificacion_visita,
            motivo: motivo,
            motivo_personalizado: motivo_personalizado,
            depto_visita: depto_visita,
            hora_ingreso: hora_ingreso,
            estado_visita: estado_visita,
            id_punto_control: id_punto_control,
            id_visitante_inquilino: id_visitante_inquilino,
            nombre_punto_control: nombre_punto_control,
            estado_actualizacion: estado_actualizacion
        };
        console.log('datos:', formData);

        // mando los datos al servidor para crear el registro
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
            document.getElementById('formCrearVisitante').reset();
        } else {
            // para mostrar si hay errores en el back
            alert("Error: " + result.message || "Error desconocido");
        }

    } catch (error) {
        console.error('Error al obtener los claims o al enviar el formulario', error);
        alert("Error al obtener los claims o al enviar el formulario.");
    } finally {
        // limpio los campos
        document.getElementById('inputDNIingresoI').value = '';
        document.getElementById('inputIdInquilinoIngresoI').value = '';
        document.getElementById('inputNombreIngresoI').value = '';
        document.getElementById('inputApellidoIngresoI').value = '';
        document.getElementById('inputMotivoIngresoI').value = '';
        document.getElementById('motivoPersonalizadoI').value = '';
        document.getElementById('inputSectorDeptoI').value = '';
        document.getElementById('inputNombrePControlI').value = '';
        document.getElementById('crearNuevoVisitante').disabled = false; // Habilitar el botón al finalizar
    }
}

async function mostrarRegistrosInquilinos() {
    try {
        const response = await fetch('https://localhost:7285/api/Busqueda/obtener-todos');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const registros = await response.json();
        console.log('Registros recibidos:', registros);

        const tabla = document.getElementById('tablaRegistrosI');

        registros.filter(registro => registro.estado_actualizacion === 1)
            .forEach(registro => {
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
                        ${registro.hora_salida === null ? `<button type="button" id="marcaSalida" onclick="marcarSalidaInquilino(${registro.id_registro_visitas})">salida</button>` : ''}
                    </td>
                    `;
                tabla.querySelector('tbody').appendChild(fila);
            });

    } catch (error) {
        console.error('Error al mostrar los registros:', error);
    }
}

async function marcarSalidaInquilino(idRegistro) {

    try {
        const response = await fetch(`https://localhost:7285/api/busqueda/ActualizarHoraSalida?idRegistro=${idRegistro}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        // Verificar si la respuesta es exitosa
        if (!response.ok) {
            // Si la respuesta no es exitosa, procesamos el error
            const errorData = await response.json();  // Parsear la respuesta como JSON
            alert('hubo un error al intentar marcar la salida');
            console.error('Error:', errorData.mensaje);  // Mostrar el mensaje de error
        } else {
            // Si la respuesta es exitosa, procesamos la respuesta
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