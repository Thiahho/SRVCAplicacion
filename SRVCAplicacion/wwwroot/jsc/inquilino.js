﻿async function CrearInquilino() {
    try {
        // Obtener el ID del punto de control
        const responseID = await fetch('http://localhost:5000/api/Inquilino/obtenerPunto', {
            method: 'GET',
            headers: {
                'Content-type': 'application/json'
            }
        });

        if (!responseID.ok) {
            throw new Error('Error al obtener el ID del punto de control');
        }

        const resultadoIdPuntoControl = await responseID.json();

        if (!resultadoIdPuntoControl || resultadoIdPuntoControl.length === 0) {
            throw new Error('No se obtuvo un ID válido para el punto de control');
        }

        const id_punto_control = resultadoIdPuntoControl[0];

        const nombre = document.getElementById('inputNombreNI').value.trim();
        const apellido = document.getElementById('inputApellidoNI').value.trim();
        const identificacion = document.getElementById('inputDNINI').value.trim();
        const telefono = document.getElementById('inputTelefonoNI').value.trim();

        if (!nombre || !apellido || !identificacion || !telefono) {
            alert("Todos los campos son obligatorios.");
            return;
        }

        const formData = {
            nombre: nombre,
            apellido: apellido,
            identificacion: identificacion,
            telefono: telefono,
            estado: 1, // Inquilino
            activo: 0, // Salida por defecto
            id_punto_control: id_punto_control,
            estado_actualizacion: 1
        };

        const response = await fetch('http://localhost:5000/api/Inquilino/CrearInquilino', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        const result = await response.json();

        if (response.ok) {
            alert("Inquilino creado correctamente.");
        } else {
            alert("Error: " + (result.message || "El número de DNI ya está registrado en la base de datos."));
        }
    } catch (error) {
        console.error('Error al procesar la solicitud', error);
        alert("Ocurrió un error: " + error.message);
    } finally {
        document.getElementById('inputNombreNI').value = '';
        document.getElementById('inputApellidoNI').value = '';
        document.getElementById('inputDNINI').value = '';
        document.getElementById('inputTelefonoNI').value = '';
    }
}


async function BuscarPorDNIInquilinoFILTRO() {
    var dni = document.getElementById('inputDNIingresoI').value;

    console.log(dni); // Log del valor ingresado
    try {
        const response = await fetch(`http://localhost:5000/api/inquilino/obtener/${dni}`);
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const datosPorDNI = await response.json();
        console.log('Registros recibidos:', datosPorDNI);

        if (datosPorDNI.estado === 2) {
            alert("Este DNI pertenece a un visitante, no a un inquilino.");
            inputIdInquilinoIngresoI.value = "";
            inputNombreIngresoI.value = "";
            inputApellidoIngresoI.value = "";
        } else if (datosPorDNI.estado === 1) {
            inputIdInquilinoIngresoI.value = datosPorDNI.id_visitante_inquilino;
            inputNombreIngresoI.value = datosPorDNI.nombre;
            inputApellidoIngresoI.value = datosPorDNI.apellido;
            alert("El inquilino se encuentra en el registro.");
        } else {
            alert("El estado del registro es desconocido.");
            inputIdInquilinoIngresoI.value = "";
            inputNombreIngresoI.value = "";
            inputApellidoIngresoI.value = "";
        }
    } catch (error) {
        alert("El número de DNI no está asociado a ningún inquilino registrado.");
        console.error('Error al mostrar los registros:', error);
        inputIdInquilinoIngresoI.value = "";
        inputNombreIngresoI.value = "";
        inputApellidoIngresoI.value = "";
    }
}

async function guardarRegistroInquilinoActivar() {
    try {
        const responseClaims = await fetch('http://localhost:5000/api/Usuario/obtener-claims');
        const dataClaims = await responseClaims.json();

        var id_usuario = dataClaims.idUsarioLog;
        var nombre_encargado = dataClaims.nombre_encargadoLog;
        var id_punto_control = dataClaims.id_punto_controlLog;

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
        //estado_visita 3=visitante, 4=inquilino
        const estado_visita = 4;
        const nombre_punto_control = document.getElementById('inputNombrePControlI').value;
        const estado_actualizacion = 1;

        // Valida que los campos esten todos llenos
        if (!nombre || !apellido || !identificacion_visita || !motivo || !motivo_personalizado || !depto_visita || !nombre_punto_control) {
            alert("Todos los campos son obligatorios.");
            //document.getElementById('crearNuevoVisitante').disabled = false; 
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
        const response = await fetch('http://localhost:5000/api/Busqueda/CrearRegistro', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        const result = await response.json();

        if (response.ok) {

            const activarResponse = await fetch(`http://localhost:5000/api/Inquilino/activarActivo/${identificacion_visita}`, {
                method: 'PUT',
                headers: {
                    'Content-type': 'application/json'
                }
            });

            if (activarResponse.ok) {
                alert("Registro creado exitosamente.");
                //alert("Inquilino activado exitosamente.");
            } else {
                const activarError = await activarResponse.json();
                alert("Error al activar al inquilino: " + (activarError.mensaje || "Error desconocido"));
            }
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

async function marcarSalidaInquilinoDesactivar(idRegistro) {
    try {
        // Primer endpoint: ActualizarHoraSalida
        const response = await fetch(`http://localhost:5000/api/busqueda/ActualizarHoraSalidaInquilino?idRegistro=${idRegistro}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        if (!response.ok) {
            // Manejo de error del primer endpoint
            const errorData = await response.json();
            console.error('Error al marcar salida:', errorData.mensaje);
            alert('Hubo un error al intentar marcar la salida.');
            return; // Salir de la función si el primer endpoint falla
        }
        console.log('Primer fetch completado con éxito');

        // Si el primer endpoint es exitoso, seguimos con el segundo
        const desactivarResponse = await fetch(`http://localhost:5000/api/Inquilino/desactivarActivoPRUEBA/${idRegistro}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!desactivarResponse.ok) {
            // Manejo de error del segundo endpoint
            const activarError = await desactivarResponse.json();
            console.error('Error al desactivar activo:', activarError.mensaje);
            alert("Error al desactivar al inquilino: " + (activarError.mensaje || "Error desconocido"));
            return; // Salir si el segundo endpoint falla
        }

        // Si ambos endpoints son exitosos
        alert('Se marcó la salida y se desactivó correctamente.');
        console.log('Salida marcada y activo desactivado.');
    } catch (error) {
        console.error('Error en la solicitud:', error);
    } finally {
        // Recargar la página
        location.reload();
    }
}

async function mostrarRegistrosInquilinosActivosTEXBOX() {
    try {
        const response = await fetch('http://localhost:5000/api/Busqueda/obtener-todos');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const registros = await response.json();
        console.log('Registros recibidos:', registros);

        const tbody = document.querySelector('#tablaRegistrosIN tbody');
        tbody.innerHTML = ''; // Limpiar contenido previo

        //registros.filter(registro => registro.estado_actualizacion === 1)
        registros //estado_visita 3=visitante, 4=inquilino
            .filter(registro => registro.estado_visita === 4)
            .forEach(registro => {
                const fila = `
                    <tr>
                        <td><input type="text" class="form-control" value="${registro.id_registro_visitas}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.motivo}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.depto_visita}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.nombre_visitante_inquilino}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.identificacion_visita}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.hora_ingreso}" readonly></td>
                        <td>
                            ${registro.hora_salida === null ? `<button type="button" id="marcaSalida" onclick="marcarSalidaInquilinoDesactivar(${registro.id_registro_visitas})">salió</button>` : ''}
                        </td>
                    </tr>
                `;
                tbody.insertAdjacentHTML('beforeend', fila);
            });

    } catch (error) {
        console.error('Error al mostrar los registros:', error);
    }
}

async function mostrarRegistrosInquilinosHistorialTEXBOX() {
    try {
        const response = await fetch('http://localhost:5000/api/Busqueda/obtener-todos');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const registros = await response.json();
        console.log('Registros recibidos:', registros);

        const tbody = document.querySelector('#tablaHistorialRegistrosIN tbody');
        tbody.innerHTML = ''; // Limpiar contenido previo

        //registros.filter(registro => registro.estado_actualizacion === 1)
        registros//estado_visita 3=visitante, 4=inquilino
            .filter(registro => registro.estado_visita === 2)
            .forEach(registro => {
                const fila = `
                    <tr>
                        <td><input type="text" class="form-control" value="${registro.id_registro_visitas}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.motivo}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.depto_visita}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.nombre_visitante_inquilino}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.identificacion_visita}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.hora_ingreso}" readonly></td>
                        <td><input type="text" class="form-control" value="${registro.hora_salida}" readonly></td>
                    </tr>
                `;
                tbody.insertAdjacentHTML('beforeend', fila);
            });

    } catch (error) {
        console.error('Error al mostrar los registros:', error);
    }
}
/*
async function mostrarTotalInquilinosActivos() {
    try {
        console.log('Llamada a mostrarTotalInquilinosActivos');
        // Realizar la solicitud al endpoint
        const response = await fetch('http://localhost:5000/api/Inquilino/contarInquilinosActivos',
            { method: 'GET' });

        if (!response.ok) {
            throw new Error('Error al obtener los datos');
        }

        // Obtener los datos como JSON
        const totalInquilinosActivos = await response.json();
        console.log('Respuesta obtenida:', totalInquilinosActivos);

        // Actualizar el contenido del elemento con el ID "totalInquilinos"
        document.getElementById('totalInquilinos').textContent = totalInquilinosActivos;
    } catch (error) {
        console.error('Error al mostrar el total de inquilinos activos:', error);
        document.getElementById('totalInquilinos').textContent = 'Error';
    }
}
*/
async function mostrarTotalInquiActivos() {
    try {
        // Realizar la solicitud al endpoint
        const response = await fetch('http://localhost:5000/api/Inquilino/contarInquilinosActivos',
            { method: 'GET' });

        if (!response.ok) {
            throw new Error('Error al obtener los datos');
        }

        // Obtener los datos como JSON
        const totalInquiActivos = await response.json();

        // Actualizar el contenido del elemento con el ID "totalInquilinos"
        document.getElementById('totalInquilinos').textContent = totalInquiActivos;
    } catch (error) {
        console.error('Error al mostrar el total de visitas activos:', error);
        document.getElementById('totalInquilinos').textContent = 'Error';
    }
}

