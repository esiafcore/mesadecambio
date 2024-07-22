const FormatDateView = "dd/mm/yy";
const FormatDateInternal = "yy-mm-dd";

document.addEventListener('DOMContentLoaded', () => {
    const processingDate = document.querySelector("#processingDate");

    document.querySelector('#btnUpdateDate').addEventListener('click', async () => {
        await fnUpdateDate(processingDate);
    });

    //processingDate.addEventListener("keydown", function (event) {
    //    // Verificar si se presionó la tecla "Delete" o "Backspace"
    //    if (event.key === "Delete" || event.key === "Backspace") {
    //        // Limpiar el valor del input de fecha
    //        processingDate.value = "";
    //    }
    //});

    $(processingDate).datepicker({
        dateFormat: FormatDateView,
        changeMonth: true,
        changeYear: true,
        defaultDate: new Date(currentDateDefault + LocalTimeDefault),
    });

    // Habiltar tooltip
    fnEnableTooltip();
});

const fnUpdateDate = async (date) => {
    try {


        let dateValue = (date.value === "") ? null : date.value;

        if (dateValue) {
            // Validar que la fecha sea correcta
            if (!fnValidateDatePicker(dateValue)) {
                Swal.fire({
                    icon: 'error',
                    title: "Se produjo un error",
                    text: ACJS.DateInvalid
                });
                return;
            }


            dateValue = $.datepicker.formatDate(FormatDateInternal, $.datepicker.parseDate(FormatDateView, dateValue));
        }

        let url = `/exchange/quotation/ProcessingDate?processingDate=${dateValue}`;

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