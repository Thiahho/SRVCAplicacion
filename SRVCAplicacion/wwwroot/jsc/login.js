//document.addEventListener("DOMContentLoaded", function () {
//    const loginForm = document.querySelector('formLogin');

//    loginForm.addEventListener('submit', function (event) {
//        const usuario = document.getElementById('usuario').value.trim();
//        const contraseña = document.getElementById('contraseña').value.trim();

//        if (!usuario || contraseña) {
//            event.preventDefault();
//            alert('Por favor, complete los campos.');
//            return;
//        }

//        const confirmarLogin = confirm("Desea iniciar sesión?");
//        if (!confirmarLogin) {
//            event.preventDefault();
//        }
//    });
//});


async function login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('contraseña').value;

    const response = await fetch('https://localhost:7285/api/acceso/login', {
        method: 'POST',
        headers{
        'Content-Type': 'application/json'
    },
        body: JSON.stringify({ usuario:username, contraseña:password})
    });

    const data = await response.json();

    if (response.ok) {
        localStorage.setItem('token', data.token);
        window.location.href = '/home';
    }
    else {
        alert(data.mensaje);
    }
}