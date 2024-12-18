﻿async function editarxDNI() {
    // Obtener el valor del input
    var dni = document.getElementById('inputEditarxDNI').value;

    // Llamar a la función que utilizará ese valor
    console.log(dni); // Aquí puedes realizar lo que necesites con el valor
    try {
        const response = await fetch(`http://localhost:5000/api/inquilino/obtener/${dni}`);
        console.log('Respuesta del servidor:', response);

        if (!response.ok) {
            throw new Error('Hubo un error al obtener los datos');
        }

        const editarPorDNI = await response.json();
        console.log('Registros recibidos:', editarPorDNI);


        if (editarPorDNI.nombre && editarPorDNI.apellido && editarPorDNI.id_visitante_inquilino && editarPorDNI.telefono) {
            inputIdxEditar.value = editarPorDNI.id_visitante_inquilino; // Asigna el id 
            inputNombrexEditar.value = editarPorDNI.nombre; // Asigna el nombre 
            inputApellidoxEditar.value = editarPorDNI.apellido; // Asigna el apellido
            inputTelefonoxEditar.value = editarPorDNI.telefono; // Asigna el telefono
            var toast = document.getElementById("toast");
            toast.innerHTML = "Se encontró un registro con ese DNI.";
            toast.className = "show";
            setTimeout(function () { toast.className = toast.className.replace("show", ""); }, 3000);
        }

    } catch (error) {
        alert("el numero de dni no esta asociado a ningun registro.");
        console.error('Error al mostrar los registros:', error);
        inputEditarxDNI.value = "";
        inputIdxEditar.value = "";
        inputNombrexEditar.value = "";
        inputApellidoxEditar.value = "";
        inputTelefonoxEditar.value = "";
    }
}
//prueba



//fin prueba
async function actualizarDatos() {
    const idxAct = document.getElementById('inputIdxEditar').value;
    const nombrexAct = document.getElementById('inputNombrexEditar').value.trim();
    const apellidoxAct = document.getElementById('inputApellidoxEditar').value.trim();
    const telefonoxAct = document.getElementById('inputTelefonoxEditar').value.trim();

    console.log('Datos :', idxAct, nombrexAct, apellidoxAct, telefonoxAct );

    // Asegúrate de que todos los campos estén completos
    if (!idxAct || !nombrexAct || !apellidoxAct || !telefonoxAct) {
        alert("Por favor complete todos los campos.");
        return;
    }

    try {
        const response = await fetch(`http://localhost:5000/api/inquilino/ActualizarInquilinoVisita/${idxAct}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                id_visitante_inquilino: idxAct,
                nombre: nombrexAct,
                apellido: apellidoxAct,
                telefono: telefonoxAct,

            })
        });

        if (!response.ok) {
            throw new Error('Hubo un error al actualizar los datos');
        }

        const result = await response.json();
        console.log('Datos actualizados:', result);

        // Mostrar un mensaje de éxito
        var toast = document.getElementById("toast");
        toast.innerHTML = "Datos actualizados correctamente.";
        toast.className = "show";
        setTimeout(function () { toast.className = toast.className.replace("show", ""); }, 3000);

    } catch (error) {
        console.error('Error al actualizar los datos:', error);
        alert("Hubo un problema al actualizar los datos.");
    } finally {
        document.getElementById('inputIdxEditar').value = '';
        document.getElementById('inputNombrexEditar').value = '';
        document.getElementById('inputApellidoxEditar').value = '';
        document.getElementById('inputTelefonoxEditar').value = '';
    }
}