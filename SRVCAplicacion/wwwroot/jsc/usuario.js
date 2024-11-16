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

        usuarios.forEach(usuario => {
            const fila = document.createElement('tr');
            fila.innerHTML = `
                <td>${usuario.id_usuario}</td>
                <td>${usuario.usuario}</td>
                <td>${usuario.contraseña}</td>
                <td>${usuario.email}</td>
                <td>${usuario.telefono}</td>
                <td>${usuario.dni}</td>
                <td>${usuario.estado}</td>
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
                <td>${usuario.id_usuario}</td>
                <td><input type="text" value="${usuario.usuario}" data-id="${usuario.id_usuario}" data-field="usuario"/></td>
                <td><input type="text" value="${usuario.contraseña}" data-id="${usuario.id_usuario}" data-field="contraseña"/></td>
                <td><input type="text" value="${usuario.email}" data-id="${usuario.id_usuario}" data-field="email"/></td>
                <td><input type="text" value="${usuario.telefono}" data-id="${usuario.id_usuario}" data-field="telefono"/></td>
                <td><input type="text" value="${usuario.dni}" data-id="${usuario.id_usuario}" data-field="dni"/></td>
                <td><input type="text" value="${usuario.estado}" data-id="${usuario.id_usuario}" data-field="estado"/></td>
                <td><button onclick="editUsuario(${usuario.id_usuario})">Guardar Cambios</button></td>
            `;
        tableBody.appendChild(row);
    });
}

async function editUsuario(id) {
    // Obtiene todos los inputs del usuario específico basándose en el `id`
    const inputs = document.querySelectorAll(`input[data-id="${id}"]`);
    const usuarioData = {};

    // Recorre los inputs para construir el objeto `usuarioData` con los valores
    inputs.forEach(input => {
        const field = input.getAttribute('data-field');
        usuarioData[field] = input.value;
    });
    usuarioData["estado"] = parseInt(usuarioData["estado"], 10);
    console.log('Datos enviados:', usuarioData);

    try {
        const response = await fetch(`https://localhost:7285/api/Usuario/Actualizar/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(usuarioData)
        });

        if (response.ok) {
            alert('Usuario actualizado con éxito');
        } else {
            alert('Error al actualizar el usuarios');
        }
    } catch (error) {
        console.error('Error al editar el usuario:', error);
    }
}

//Funcion para crear un nuevo usuario(admin)

async function crearNuevoUsuario() {

    const usuarioNuevo = {
        usuario: document.getElementById('usuario').value,
        contraseña: document.getElementById('contraseña').value,
        email: document.getElementById('email').value,
        telefono: document.getElementById('telefono').value,
        dni: document.getElementById('dni').value,
        estado: document.getElementById('estado').value,
        idPuntoControl: document.getElementById('idPuntoControl').value
    };

    try {
        const response = await fetch('/api/Usuario/Crear', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(usuarioNuevo)
        });

        if (response.ok) {
            alert('El usuario se creo correctamente');

        } else {
            alert('Error al intentar crear el usuario');
        }

    } catch (error) {
        console.error('Error al intentar crear el usuario', error);
    }
}

