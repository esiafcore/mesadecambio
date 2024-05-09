document.addEventListener('DOMContentLoaded', () => {
    const processingDate = document.querySelector("#processingDate");

    document.querySelector('#btnUpdateDate').addEventListener('click', async () => {
        await fnUpdateDate(processingDate);
    });

    processingDate.addEventListener("keydown", function (event) {
        // Verificar si se presionó la tecla "Delete" o "Backspace"
        if (event.key === "Delete" || event.key === "Backspace") {
            // Limpiar el valor del input de fecha
            processingDate.value = "";
        }
    });

});

const fnUpdateDate = async (date) => {
    try {

        let url = `/exchange/quotation/ProcessingDate?processingDate=${date.value}`;

        const response = await fetch(url, {
            method: 'POST'
        });

        const jsonResponse = await response.json();
        if (!jsonResponse.isSuccess) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: jsonResponse.errorMessages
            });
        } else {
            if (jsonResponse.urlRedirect) {
                window.location.href = jsonResponse.urlRedirect;
            }
        }

    } catch (e) {
        Swal.fire({
            icon: 'error',
            title: "Error en la conexión",
            text: e
        });
    }
};