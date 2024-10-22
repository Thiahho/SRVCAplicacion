document.addEventListener("DOMContentLoaded", function () {
    const loginForm = document.querySelector('formLogin');

    loginForm.addEventListener('submit', function (event) {
        const usuario = document.getElementById('usuario').value.trim();
        const contraseña = document.getElementById('contraseña').value.trim();

        if (!usuario || contraseña) {
            event.preventDefault();
            alert('Por favor, complete los campos.');
            return;
        }

        const confirmarLogin = confirm("Desea iniciar sesión?");
        if (!confirmarLogin) {
            event.preventDefault();
        }
    });
});