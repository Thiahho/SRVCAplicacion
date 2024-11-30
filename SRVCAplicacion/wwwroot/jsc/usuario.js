//Funcion para mostrar usuarios(admin)
async function mostrarUsuarios() {

    try {
        const response = await fetch('https://localhost:7285/api/Usuario/Obtener');
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const usuarios = await response.json();
        console.log('Usuarios recibidos:', usuarios);

        const tabla = document.getElementById('usuarioTable');
        tabla.querySelector('tbody').innerHTML = '';
        usuarios.forEach(usuario => {
            const fila = document.createElement('tr');
            fila.innerHTML = `
                <td>${usuario.id_usuario}</td>
                <td>${usuario.usuario}</td>
                <td>${usuario.contraseña}</td>
                <td>${usuario.email}</td>
                <td>${usuario.telefono}</td>
                <td>${usuario.dni}</td>
                <td>${usuario.Estado}</td>
                <td>${usuario.id_punto_control}</td>
            `;
            tabla.querySelector('tbody').appendChild(fila);
        });
    } catch (error) {
        console.error('Error al mostrar usuarios:', error);
    }
}

//cargaDatosParaEditar
async function mostrarParaEditar() {
    const response = await fetch('https://localhost:7285/api/Usuario/Obtener');
    const users = await response.json();
    const tableBody = document.getElementById('usuarioTableEdit').querySelector('tbody');
    tableBody.innerHTML = '';
    users.forEach(usuario => {
        const row = document.createElement('tr');
        //row.setAttribute('data-id', usuario.id_usuario);
        row.innerHTML = `
                <td><input type="text" value="${usuario.id_usuario}" data-id="${usuario.id_usuario}" data-field="id_usuario"/></td>
                <td><input type="text" value="${usuario.usuario}" data-id="${usuario.id_usuario}" data-field="usuario"/></td>
                <td><input type="text" value="${usuario.contraseña}" data-id="${usuario.id_usuario}" data-field="contraseña"/></td>
                <td><input type="text" value="${usuario.email}" data-id="${usuario.id_usuario}" data-field="email"/></td>
                <td><input type="text" value="${usuario.telefono}" data-id="${usuario.id_usuario}" data-field="telefono"/></td>
                <td><input type="text" value="${usuario.dni}" data-id="${usuario.id_usuario}" data-field="dni"/></td>
                <td><input type="text" value="${usuario.Estado}" data-id="${usuario.id_usuario}" data-field="Estado"/></td>
                <td><input type="text" value="${usuario.id_punto_control}" data-id="${usuario.id_usuario}" data-field="id_punto"/></td>
                 <td>
                  <button id="btnEditar" onclick="editUsuario(${usuario.id_usuario})" class="btn btn-primary btn-sm">Editar</button>
                 </td>
            `;
        //<td><button onclick="editUsuario(${usuario.id_usuario})">Guardar Cambios</button></td>
        tableBody.appendChild(row);

        const btnEditar = document.getElementById(`btnEditar`);
        btnEditar.addEventListener("click", function () {
            const usuarioActualizado = {
                id_usuario: document.getElementById(`id_usuario`).value,
                usuario: document.getElementById(`usuario`).value,
                contraseña: document.getElementById(`contraseña`).value,
                email: document.getElementById(`email`).value,
                telefono: document.getElementById(`telefono`).value,
                dni: document.getElementById(`dni`).value,
                Estado: parseInt(document.getElementById(`Estado`).value),
                id_punto: parseInt(document.getElementById(`id_punto`).value),
            };

            console.log('Datos antes de llamar a editUsuario:', usuarioActualizado);

            // Llamar a la función de edición
            editUsuario(usuario.id_usuario, usuarioActualizado);
        });
    });
}

//async function editUsuario(id,usuarioActualizado) {
//    // Obtiene todos los inputs del usuario específico basándose en el `id`
//    const inputs = document.querySelectorAll(`input[data-id="${id}"]`);
//    const usuarioData = {};

//    // Obtener el usuario actual desde el backend
//    let currentUser;
//    try {
//        if (!usuarioActualizado || !usuarioActualizado.usuario || !usuarioActualizado.dni || !usuarioActualizado.telefono || !usuarioActualizado.contraseña || !usuarioActualizado.estado || !usuarioActualizado.email) {
//            throw new Error("Los datos del usuario no son validos");
//        }

//        const userResponse = await fetch(`https://localhost:7285/api/Usuario/GetUsuarioActual/${id}`);
//        if (userResponse.ok) {
//            const user = await userResponse.json();
//            currentUser = user.usuario; // Asume que el modelo de respuesta tiene una propiedad `usuario`
//        } else {
//            console.error("Error al obtener el usuario actual");
//            alert("No se pudo obtener el usuario actual");
//            return;
//        }
//    } catch (error) {
//        console.error("Error al obtener el usuario actual:", error);
//        alert("Hubo un problema al obtener el usuario actual.");
//        return;
//    }

//    // Recorre los inputs para construir el objeto `usuarioData` con los valores
//    inputs.forEach(input => {
//        const field = input.getAttribute('data-field');
//        usuarioData[field] = input.value;
//    });

//    // Convierte el estado a número (si es necesario)
//    usuarioData["estado"] = parseInt(usuarioData["estado"], 10);

//    // Construye el objeto de auditoría
//    const auditoria = {
//        realizadoPor: currentUser, // Usuario que realiza la acción
//        accion: "Editar Usuario",
//        fecha: new Date().toISOString(), // ISO string para una mejor compatibilidad
//        detalles: `Usuario con ID ${id} fue editado. Cambios: ${JSON.stringify(usuarioData)}`
//    };

//    console.log('Datos enviados:', usuarioData, 'Auditoria:', auditoria);

//    try {
//        const response = await fetch(`https://localhost:7285/api/Usuario/Actualizar/${id}`, {
//            method: 'PUT',
//            headers: {
//                'Content-Type': 'application/json'
//            },
//            body: JSON.stringify({
//                usuario: usuarioData, // Datos del usuario a actualizar
//                auditoria: auditoria  // Registro de auditoría
//            })
//        });

//        if (response.ok) {
//            alert('Usuario actualizado con éxito');
//        } else {
//            const errorMessage = await response.text();
//            console.error('Error en la respuesta del servidor:', errorMessage);
//            alert('Error al actualizar el usuario');
//        }
//    } catch (error) {
//        console.error('Error al editar el usuario:', error);
//    }
//}


//---------------------------------------------------
//async function editUsuario(id, usuarioActualizado) {
//    console.log('Objeto usuario actualizado antes de la llamada:', usuarioActualizado);
// // Verifica los datos antes de validar
//    if (!usuarioActualizado || !usuarioActualizado.usuario || !usuarioActualizado.dni ||
//        !usuarioActualizado.telefono || !usuarioActualizado.contraseña || !usuarioActualizado.estado
//        || !usuarioActualizado.email) {
//        alert("Los datos del usuario no son válidos");
//        console.log('Datos inválidos:', usuarioActualizado);
//        return;
//    }

//    try {
//        const userResponse = await fetch(`https://localhost:7285/api/Usuario/GetUsuarioActual/${id}`);

//        if (!userResponse.ok) {
//            const errorMessage = await userResponse.text();
//            console.error('Error al obtener el usuario actual:', errorMessage);
//            alert("No se pudo obtener el usuario actual");
//            return;
//        }

//        const currentUser = await userResponse.json();

//        console.log('Usuario actual obtenido:', currentUser);

//        const response = await fetch(`https://localhost:7285/api/Usuario/Actualizar/${id}`, {
//            method: 'PUT',
//            headers: {
//                'Content-Type': 'application/json'
//            },
//            body: JSON.stringify(usuarioActualizado)  
//        });

//        if (response.ok) {
//            alert('Usuario actualizado con éxito');
//        } else {
//            const errorMessage = await response.text();
//            console.error('Error en la respuesta del servidor:', errorMessage);
//            alert('Error al actualizar el usuario');
//        }
//    } catch (error) {
//        console.error('Error al editar el usuario:', error);
//        alert("Hubo un problema al editar el usuario.");
//    }
//}

async function editUsuario(id) {
    const row = document.querySelector(`tr td input[data-id='${id}']`).closest('tr');
    const usuarioActualizado = {};

    const inputs = row.querySelectorAll('input');
    inputs.forEach(input => {
        const field = input.getAttribute('data-field');
        usuarioActualizado[field]
    })
    // Verificar si los datos son válidos
    if (!usuarioActualizado || !usuarioActualizado.usuario || !usuarioActualizado.dni ||
        !usuarioActualizado.telefono || !usuarioActualizado.contraseña || !usuarioActualizado.estado
        || !usuarioActualizado.email || !usuarioActualizado.id_punto_control) {
        alert("Los datos del usuario no son válidos");
        console.log('Datos inválidos:', usuarioActualizado);
        return;
    }

    try {
        // Obtén los datos del usuario actual
        const userResponse = await fetch(`https://localhost:7285/api/Usuario/GetUsuarioActual/${id}`);

        if (!userResponse.ok) {
            const errorMessage = await userResponse.text();
            console.error('Error al obtener el usuario actual:', errorMessage);
            alert("No se pudo obtener el usuario actual");
            return;
        }

        const currentUser = await userResponse.json();
        console.log('Usuario actual obtenido:', currentUser);

        // Hacer la solicitud PUT para actualizar el usuario
        const response = await fetch(`https://localhost:7285/api/Usuario/Actualizar/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(usuarioActualizado)
        });

        if (response.ok) {
            alert('Usuario actualizado con éxito');
        } else {
            const errorMessage = await response.text();
            console.error('Error en la respuesta del servidor:', errorMessage);
            alert('Error al actualizar el usuario');
        }
    } catch (error) {
        console.error('Error al editar el usuario:', error);
        alert("Hubo un problema al editar el usuario.");
    }
}

// Función para capturar los datos del formulario y llamar a `editUsuario`
document.getElementById("btnEditar").addEventListener("click", function () {
    // Obtener los valores del formulario
    const usuarioActualizado = {
        usuario: document.getElementById("usuario").value,
        dni: document.getElementById("dni").value,
        telefono: document.getElementById("telefono").value,
        contraseña: document.getElementById("contraseña").value,
        Estado: parseInt(document.getElementById("Estado").value), // Convertir a número
        email: document.getElementById("email").value,
        id_punto: document.getElementById("id_punto").value
    };

    // Verificar los datos antes de llamar a la función
    console.log('Datos antes de llamar a editUsuario:', usuarioActualizado);

    // Llamar a la función `editUsuario` con un ID y los datos del usuario
    const idUsuario = id; // Este ID debe ser dinámico, dependiendo del usuario que estés editando
    editUsuario(idUsuario, usuarioActualizado);
});



//Funcion para crear un nuevo usuario(admin)

//async function crearNuevoUsuario() {

//    const usuarioNuevo = {
//        usuario: document.getElementById('usuario').value,
//        contraseña: document.getElementById('contraseña').value,
//        email: document.getElementById('email').value,
//        telefono: document.getElementById('telefono').value,
//        dni: document.getElementById('dni').value,
//        estado: document.getElementById('estado').value,
//        idPuntoControl: document.getElementById('id_punto').value
//    };

//    try {
//        const response = await fetch('/api/Usuario/Crear', {
//            method: 'POST',
//            headers: {
//                'Content-Type': 'application/json'
//            },
//            body: JSON.stringify(usuarioNuevo)
//        });

//        if (response.ok) {
//            alert('El usuario se creo correctamente');

//        } else {
//            alert('Error al intentar crear el usuario');
//        }

//    } catch (error) {
//        console.error('Error al intentar crear el usuario', error);
//    }
//}