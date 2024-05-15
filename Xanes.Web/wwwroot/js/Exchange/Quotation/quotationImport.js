document.addEventListener("DOMContentLoaded", function () {

    // Evento enviar form para importar
    const formImport = document.getElementById("formImport");
    formImport.addEventListener("submit", fnimportSubmit);
});


// Importar 
const fnimportSubmit = async (event) => {

    try {

        event.preventDefault();
        const formObject = event.currentTarget;

        const url = formObject.action;
        const formData = new FormData(formObject);

        const response = await fetch(url, {
            method: 'POST',
            body: formData
        });

        const jsonResult = await response.json();
        if (!response.ok) {
            Swal.fire({
                icon: 'error',
                text: jsonResult.errorMessages
            });

        } else {
            if (!jsonResult.isSuccess) {
                Swal.fire({
                    icon: 'error',
                    text: jsonResult.errorMessages
                });

            } else {
                const result = await Swal.fire({
                    title: jsonResult.sucessMessages,
                    text: "El archivo se ha importado",
                    icon: "success",
                    showCancelButton: false,
                    showDenyButton: false,
                    confirmButtonText: "Aceptar",
                    buttonsStyling: false,
                    customClass: {
                        confirmButton: 'bg-primary text-white border'
                    }
                });

                if (result.isConfirmed) {
                    if (jsonResult.urlRedirect) {
                        window.location.href = jsonResult.urlRedirect;
                    }
                }
            }
        }

    } catch (error) {
        Swal.fire({
            icon: 'error',
            text: error
        });
    }
};