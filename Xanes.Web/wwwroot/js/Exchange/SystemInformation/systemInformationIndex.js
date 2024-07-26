// Reporte seleccionadoo
let selectReportValue;

let dateInitialSave, dateFinalSave;

document.addEventListener("DOMContentLoaded", async () => {
    
    const selectGroupReport = document.getElementById("select-group");
    selectGroupReport.addEventListener("change", async (event) => {
        const selectElement = event.target;

        // Solo cuando en selects
        if (selectElement.tagName.toLocaleLowerCase() === "select") {

            // Solo habilitar una opcion
            const selectGroups = document.querySelectorAll(".select-report");
            selectGroups.forEach(otherSelect => {
                if (otherSelect !== selectElement) {
                    otherSelect.value = '';
                }
            });

            // Cargar vista parcial seleccionada
            selectReportValue = parseInt(selectElement.value);
            await fnloadPartialsView(selectReportValue);

            fnSetEventChange();

            fnSetProcessingDate();

            if (dateInitialSave) {
                document.getElementById('dateInitial').value = dateInitialSave;
            }

            if (dateFinalSave) {
                document.getElementById('dateFinal').value = dateFinalSave;
            }
        }
    });

    await fnloadPartialsView(SystemInformationReportType.Operation);
    fnSetProcessingDate();

    fnSetEventChange();
});

const fnSetEventChange = () => {
    document.getElementById('dateInitial').addEventListener("change", () => {
        fnAdjustmentDates();
        dateInitialSave = document.getElementById('dateInitial').value;
    });

    document.getElementById('dateFinal').addEventListener("change", () => {
        dateFinalSave = document.getElementById('dateFinal').value;
    });
};

const fnSetProcessingDate = () => {
    document.getElementById('dateInitial').value = processingDate;
    document.getElementById('dateFinal').value = processingDate;
}

// Función para ajustar las fechas según los criterios
const fnAdjustmentDates = () => {
    let dateInitialValue = new Date(document.getElementById('dateInitial').value);
    let dateFinalValue = new Date(document.getElementById('dateFinal').value);

    // Validar si la fecha final es menor que la fecha inicial
    if (dateFinalValue < dateInitialValue) {
        document.getElementById('dateFinal').value = document.getElementById('dateInitial').value;
    }

    // Establecer el mínimo de la fecha final como la fecha inicial
    document.getElementById('dateFinal').min = document.getElementById('dateInitial').value;    
}


// Validaciones en formularios ====>
// Evento antes de enviar formularios para agregar validaciones  
const fnsendFormValidate = async (event) => {
    try {
        event.preventDefault();

        let resultResponse = {
            isSuccess: true
        };

        const formObject = event.currentTarget;
        const formData = new FormData(formObject);
        const plainFormData = Object.fromEntries([...formData.entries()].filter(([key, _]) => !key.startsWith("__")));

        // Obtener tipo de reporte
        const typeReport = parseInt(plainFormData.ReportType);
        if (!typeReport) {
            plainFormData.ReportType = SystemInformationReportType[plainFormData.ReportType];
        } else {
            plainFormData.ReportType = parseInt(plainFormData.ReportType);
        }


        if (plainFormData.ReportType == SystemInformationReportType.Operation) {

            resultResponse = await fnvalidateOperation(plainFormData);
        }

        if (plainFormData.ReportType == SystemInformationReportType.Deposit) {
            resultResponse = await fnvalidateDeposit(plainFormData);
        }

        if (plainFormData.ReportType == SystemInformationReportType.Transfer) {
            resultResponse = await fnvalidateTransfer(plainFormData);
        }

        // Continuar flujo normal del formulario
        if (resultResponse.isSuccess) {
            formObject.submit();
        } else if (resultResponse.message) {
            Swal.fire({
                icon: 'error',
                title: "Error",
                text: resultResponse.message
            });
        }

    } catch (error) {
        Swal.fire({
            icon: 'error',
            title: "Error",
            text: error
        });
    }
};

const fnvalidateOperation = async (plainFormData) => {
    const resultResponse = {
        isSuccess: true
    };

    // Validar que existan registros disponibles
    const url = `${window.location.origin}/Exchange/SystemInformation/VerificationDataForOperation`;
    const formDataJsonString = JSON.stringify(plainFormData);
    let fetchOptions = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        },
        body: formDataJsonString
    };

    const response = await fetch(url, fetchOptions);

    if (!response.ok) {
        const errorMessage = await response.text();
        Swal.fire({
            icon: "error",
            title: "Error",
            text: errorMessage
        });
    } else {
        let jsonResponse = await response.json();
        if (!jsonResponse.isSuccess) {
            fnShowModalMessages(jsonResponse);
            resultResponse.isSuccess = false;
            return resultResponse;
        }
    }

    return resultResponse;
};

const fnvalidateDeposit = async (plainFormData) => {
    const resultResponse = {
        isSuccess: true
    };

    // Validar que existan registros disponibles
    const url = `${window.location.origin}/Exchange/SystemInformation/VerificationDataForDeposit`;
    const formDataJsonString = JSON.stringify(plainFormData);
    let fetchOptions = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        },
        body: formDataJsonString
    };

    const response = await fetch(url, fetchOptions);

    if (!response.ok) {
        const errorMessage = await response.text();
        Swal.fire({
            icon: "error",
            title: "Error",
            text: errorMessage
        });
    } else {
        let jsonResponse = await response.json();
        if (!jsonResponse.isSuccess) {
            fnShowModalMessages(jsonResponse);
            resultResponse.isSuccess = false;
            return resultResponse;
        }
    }

    return resultResponse;
};

const fnvalidateTransfer = async (plainFormData) => {
    const resultResponse = {
        isSuccess: true
    };

    // Validar que existan registros disponibles
    const url = `${window.location.origin}/Exchange/SystemInformation/VerificationDataForTransfer`;
    const formDataJsonString = JSON.stringify(plainFormData);
    let fetchOptions = {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        },
        body: formDataJsonString
    };

    const response = await fetch(url, fetchOptions);

    if (!response.ok) {
        const errorMessage = await response.text();
        Swal.fire({
            icon: "error",
            title: "Error",
            text: errorMessage
        });
    } else {
        let jsonResponse = await response.json();
        if (!jsonResponse.isSuccess) {
            fnShowModalMessages(jsonResponse);
            resultResponse.isSuccess = false;
            return resultResponse;
        }
    }

    return resultResponse;
};

// Renderizar vistas parciales
const fnloadPartialsView = async (selectValue) => {
    try {

        // Renderizar vista parcial correspondiente ====>
        const url = `${window.location.origin}/Exchange/SystemInformation/GetPartialView?reportType=${selectValue}`;
        const fetchOptions = {
            method: "POST"
        };

        const response = await fetch(url, fetchOptions);

        if (!response.ok) {
            const errorMessage = await response.text();
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: errorMessage
            });
        } else {

            const partialView = await response.text();

            // Mostral la vista parcial
            const containerPartialView = document.getElementById("report-filters");
            containerPartialView.classList.remove("d-none");
            containerPartialView.innerHTML = partialView;
            
            // Abrir panel de criterios
            const criterias = document.querySelector(".criteria");
            criterias.classList.remove("d-none");
        }

    } catch (error) {
        Swal.fire({
            icon: 'error',
            title: "Error",
            text: error
        });
    }
};