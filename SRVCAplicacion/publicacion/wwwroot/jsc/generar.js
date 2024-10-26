document.addEventListener("DOMContentLoaded", function () {
    const registroForm = document.querySelector('formRegistro');

    registroForm.addEventListener('submit', function (event) {
        const usuario = document.getElementById('usuario').value.trim();
        const email = document.getElementById('email').value.trim();
        const contraseña = document.getElementById('contraseña').value.trim();
        const confirmarContraseña = document.getElementById('confirmarContraseña').value.trim();

        if (!usuario || !email || !contraseña || !confirmarContraseña) {
            event.preventDefault();
            alert('Por favor, complete todos los campos.');
            return;
        }
        if (contraseña !== confirmarContraseña) {
            event.preventDefault();
            alert('Las contraseñas no coinciden.');
            return;
        }

        const confirmarRegistro = confirm("Desea registrar el usuario?");
        if (!confirmarRegistro) {
            event.preventDefault();
        }
    });
});