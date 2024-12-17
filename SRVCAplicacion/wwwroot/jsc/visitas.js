async function CrearVisitante() {
    try {
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

        const nombre = document.getElementById('inputNombreNV').value.trim();
        const apellido = document.getElementById('inputApellidoNV').value.trim();
        const identificacion = document.getElementById('inputDNINV').value.trim();
        const telefono = document.getElementById('inputTelefonoNV').value.trim();
        const activo = 0; 
        const estado = 2; 
        const estado_actualizacion = 1; 

        if (!nombre || !apellido || !identificacion || !telefono) {
            alert("Todos los campos son obligatorios.");
            return;
        }

        const formData = {
            nombre: nombre,
            apellido: apellido,
            identificacion: identificacion,
            telefono: telefono,
            estado: estado,
            activo: activo,
            id_punto_control: id_punto_control,
            estado_actualizacion: estado_actualizacion
        };

        const response = await fetch('http://localhost:5000/api/Inquilino/CrearVisitante', {
            method: 'POST',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        const result = await response.json();

        if (response.ok) {
            alert("Visitante creado exitosamente.");
            document.getElementById('formCrearVisitante').reset(); // Limpiar formulario
        } else {
            alert("Error: " + (result.message || "El número de DNI ya está registrado en la base de datos."));
        }
    } catch (error) {
        console.error('Error al procesar la solicitud', error);
        alert("Ocurrió un error: " + error.message);
    } finally {
        document.getElementById('inputNombreNV').value = '';
        document.getElementById('inputApellidoNV').value = '';
        document.getElementById('inputDNINV').value = '';
        document.getElementById('inputTelefonoNV').value = '';
    }
}




async function BuscarPorDNIVisita() {
    var dni = document.getElementById('inputDNIingreso').value;
 
    console.log(dni);
    try {
        const response = await fetch(`http://localhost:5000/api/inquilino/obtener/${dni}`);
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const datosPorDNI = await response.json();
        console.log('Registros recibidos:', datosPorDNI);


        if (datosPorDNI.nombre && datosPorDNI.apellido && datosPorDNI.id_visitante_inquilino) {
            inputIdInquilinoIngreso.value = datosPorDNI.id_visitante_inquilino; 
            inputNombreIngreso.value = datosPorDNI.nombre; 
            inputApellidoIngreso.value = datosPorDNI.apellido;
            alert("El inquilono se encuentra en el registro.");
        }

    } catch (error) {
        alert("el numero de dni no esta asociado a ningun visitante registrado.");
        console.error('Error al mostrar los registros:', error);
        inputIdInquilinoIngreso.value = "";
        inputNombreIngreso.value = "";
        inputApellidoIngreso.value = "";
    }
}

async function BuscarPorDNIVisitaFILTRO() {
    var dni = document.getElementById('inputDNIingreso').value;

    console.log(dni); 
    try {
        const response = await fetch(`http://localhost:5000/api/inquilino/obtener/${dni}`);
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const datosPorDNI = await response.json();
        console.log('Registros recibidos:', datosPorDNI);

       
        if (datosPorDNI.estado === 1) {
            alert("Este DNI pertenece a un inquilino, no a un visitante.");
            inputIdInquilinoIngreso.value = "";
            inputNombreIngreso.value = "";
            inputApellidoIngreso.value = "";
        } else if (datosPorDNI.estado === 2) {
            inputIdInquilinoIngreso.value = datosPorDNI.id_visitante_inquilino;
            inputNombreIngreso.value = datosPorDNI.nombre; 
            inputApellidoIngreso.value = datosPorDNI.apellido;
            alert("El visitante se encuentra en el registro.");
        } else {
            alert("El estado del registro es desconocido.");
            inputIdInquilinoIngreso.value = "";
            inputNombreIngreso.value = "";
            inputApellidoIngreso.value = "";
        }
    } catch (error) {
        alert("El número de DNI no está asociado a ningún visitante registrado.");
        console.error('Error al mostrar los registros:', error);
        inputIdInquilinoIngreso.value = "";
        inputNombreIngreso.value = "";
        inputApellidoIngreso.value = "";
    }
}


async function guardarRegistroVisitanteActivar() {
    try {
        const responseClaims = await fetch('http://localhost:5000/api/Usuario/obtener-claims');
        const dataClaims = await responseClaims.json();

        var id_usuario = dataClaims.idUsarioLog;
        var nombre_encargado = dataClaims.nombre_encargadoLog;
        var id_punto_control = dataClaims.id_punto_controlLog;

        console.log('ID Usuario:', id_usuario);
        console.log('Nombre Encargado:', nombre_encargado);
        console.log('ID Punto Control:', id_punto_control);

        const identificacion_visita = document.getElementById('inputDNIingreso').value;
        const id_visitante_inquilino = document.getElementById('inputIdInquilinoIngreso').value;
        const nombre = document.getElementById('inputNombreIngreso').value.trim();
        const apellido = document.getElementById('inputApellidoIngreso').value.trim();
        const nombre_visitante_inquilino = (nombre && apellido) ? nombre + ' ' + apellido : nombre + apellido;

        const fechaingreso = new Date();
        //fechaingreso.setHours(fechaingreso.getHours() - 3);
        const hora_ingreso = fechaingreso.toISOString();
        console.log('Fecha y Hora de Ingreso:', hora_ingreso);

        const motivo = document.getElementById('inputMotivoIngreso').value;
        const motivo_personalizado = document.getElementById('motivoPersonalizado').value;
        const depto_visita = document.getElementById('inputSectorDepto').value;
        const estado_visita = 3;
        const nombre_punto_control = document.getElementById('inputNombrePControl').value;
        const estado_actualizacion = 1;

        if (!nombre || !apellido || !identificacion_visita || !motivo || !motivo_personalizado || !depto_visita || !nombre_punto_control) {
            alert("Todos los campos son obligatorios.");
            document.getElementById('crearNuevoVisitante').disabled = false;
            return;
        }

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
            } else {
                const activarError = await activarResponse.json();
                alert("Error al activar al visitante: " + (activarError.mensaje || "Error desconocido"));
            }
        } else {
            alert("Error: " + result.message || "Error desconocido");
        }

    } catch (error) {
        console.error('Error al obtener los claims o al enviar el formulario', error);
        alert("Error al obtener los claims o al enviar el formulario.");
    } finally {
        // limpio los campos
        document.getElementById('inputDNIingreso').value = '';
        document.getElementById('inputIdInquilinoIngreso').value = '';
        document.getElementById('inputNombreIngreso').value = '';
        document.getElementById('inputApellidoIngreso').value = '';
        document.getElementById('inputMotivoIngreso').value = '';
        document.getElementById('motivoPersonalizado').value = '';
        document.getElementById('inputSectorDepto').value = '';
        document.getElementById('inputNombrePControl').value = '';
        document.getElementById('crearNuevoVisitante').disabled = false;
    }
}


//
async function marcarSalidaVisitaDesactivar(idRegistro) {

    try {
        const response = await fetch(`http://localhost:5000/api/busqueda/ActualizarHoraSalidaVisita?idRegistro=${idRegistro}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
        });

        if (!response.ok) {
            const errorData = await response.json();
            console.error('Error al marcar salida:', errorData.mensaje);
            alert('Hubo un error al intentar marcar la salida.');
            return;
        }
        console.log('Primer fetch completado con éxito');

        const desactivarResponse = await fetch(`http://localhost:5000/api/Inquilino/desactivarActivoPRUEBA/${idRegistro}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!desactivarResponse.ok) {
            const activarError = await desactivarResponse.json();
            console.error('Error al desactivar activo:', activarError.mensaje);
            alert("Error al desactivar al inquilino: " + (activarError.mensaje || "Error desconocido"));
            return; 
        }

        alert('Se marcó la salida y se desactivó correctamente.');
        console.log('Salida marcada y activo desactivado.');
    } catch (error) {
        console.error('Error en la solicitud:', error);
    } finally {
        location.reload();
    }
}
//

async function mostrarRegistrosVisitasActivosTEXBOX() {
    try {
        const response = await fetch('http://localhost:5000/api/Busqueda/obtener-todos');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const registros = await response.json();
        console.log('Registros recibidos:', registros);

        const tbody = document.querySelector('#tablaRegistrosV tbody');
        tbody.innerHTML = '';

        //registros.filter(registro => registro.estado_actualizacion === 2)
        registros//estado_visita 3=visitante, 4=inquilino
            .filter(registro => registro.estado_visita === 3)
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
                            ${registro.hora_salida === null ? `<button type="button" id="marcaSalida" onclick="marcarSalidaVisitaDesactivar(${registro.id_registro_visitas})">salió</button>` : ''}
                        </td>
                    </tr>
                `;
                tbody.insertAdjacentHTML('beforeend', fila);
            });

    } catch (error) {
        console.error('Error al mostrar los registros:', error);
    }
}

async function mostrarRegistrosVisitantesHistorialTEXBOX() {
    try {
        const response = await fetch('http://localhost:5000/api/Busqueda/obtener-todos');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const registros = await response.json();
        console.log('Registros recibidos:', registros);

        const tbody = document.querySelector('#tablaHistorialRegistrosVI tbody');
        tbody.innerHTML = ''; 

        //registros.filter(registro => registro.estado_actualizacion === 2)
        registros//estado_visita 3=visitante, 4=inquilino
            .filter(registro => registro.estado_visita === 1)
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
async function mostrarTotalVisitasActivos() {
    try {
        const response = await fetch('http://localhost:5000/api/Inquilino/contarVisitantesActivos',
            { method: 'GET' });

        if (!response.ok) {
            throw new Error('Error al obtener los datos');
        }

        const totalVisitantesActivos = await response.json();

        document.getElementById('totalVisitantes').textContent = totalVisitantesActivos;
    } catch (error) {
        console.error('Error al mostrar el total de visitas activos:', error);
        document.getElementById('totalVisitantes').textContent = 'Error';
    }
}