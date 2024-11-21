document.addEventListener("DOMContentLoaded", function () {
    const loginForm = document.querySelector('#formLogin');

    loginForm.addEventListener('submit', async function (event) {
        event.preventDefault();

        const usuario = document.getElementById('usuario').value.trim();
        const contraseña = document.getElementById('contraseña').value.trim();

        if (!usuario || contraseña) {
            event.preventDefault();
            alert('Por favor, complete los campos.');
            return;
        }

        const confirmarLogin = confirm("Desea iniciar sesión?");
        if (!confirmarLogin) return;
        //{
        //    event.preventDefault();
        //}

        try {
            const response = await fetch('api/Acceso/Login', {
                method: 'POST',
                headers: { 'Content-type': 'application/json' },
                body: JSON.stringify({ usuario, contraseña })
            });

            const result = await response.json();
            alert('Inicio de sesion exitoso.');
            window.location.href = result.redirectUrl || '/';
        }cath(error){
            console.error('Error:', error);
        }
    });
});