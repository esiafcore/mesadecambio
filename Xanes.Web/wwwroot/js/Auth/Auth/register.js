
let containerMain;
document.addEventListener("DOMContentLoaded", () => {

    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid";
    const form = document.querySelector('form');
    const password = document.getElementById('password');
    const confirmPassword = document.getElementById('confirmPassword');

    form.addEventListener('submit', function (e) {
        if (password.value !== confirmPassword.value) {
            e.preventDefault();
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Las contraseñas no coinciden',
                confirmButtonText: 'OK'
            });
        }
    });


    fnEnableTooltip();
});