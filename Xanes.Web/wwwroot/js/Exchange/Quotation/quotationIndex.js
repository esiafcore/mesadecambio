let dataTable, containerMain, inputDateInitial, inputDateFinal;
let dateInitial, dateFinal;
let dateFinalReport = document.querySelector("#dateFinalReport");
let dateInitialReport = document.querySelector("#dateInitialReport");
let inputIncludeVoid, includeVoid;
document.addEventListener("DOMContentLoaded", function () {
    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid";
    fnLoadDatatable();
    //Habilitar Tooltip
    fnEnableTooltip();

    //// Crear los elementos del filtro de fecha y botón de búsqueda
    //var filtroFecha = $('<div class="dt-filtro-fecha col-8 d-flex gap-4">Fecha de inicio: <input type="date" id="fechaInicio"> Fecha fin: <input type="date" id="fechaFin"> <button id="btnBuscar">Buscar</button></div>');

    //// Insertar los elementos antes de la barra de búsqueda estándar
    //wrapper.find('.dt-length').after(filtroFecha);

    // Evento enviar form para crear
    const formPrint = document.getElementById("formPrint");
    formPrint.addEventListener("submit", fnPrintFormSubmit);
});


const fnPrintFormSubmit = async (event) => {

    try {
        let resultResponse = {
            isSuccess: true
        };
        event.preventDefault();
        const formObject = event.currentTarget;

        const url = `${formObject.action}`;
        const formData = new FormData(formObject);
        //const plainFormData = Object.fromEntries(formData.entries());
        const plainFormData = Object.fromEntries([...formData.entries()].filter(([key, _]) => !key.startsWith("__")));
        plainFormData.ReportType = parseInt(plainFormData.ReportType);

        resultResponse = await fnvalidateOperation(plainFormData);

        // Continuar flujo normal del formulario
        if (resultResponse.isSuccess) {
            formObject.submit();
        }

    } catch (error) {
        Swal.fire({
            icon: 'error',
            text: error
        });

    }
}

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


// Función para ajustar las fechas según los criterios
const fnAdjustmentDates = () => {
    let dateInitialValue = new Date(inputDateInitial.value);
    let dateFinalValue = new Date(inputDateFinal.value);

    // Validar si la fecha final es menor que la fecha inicial
    if (dateFinalValue < dateInitialValue) {
        inputDateFinal.value = inputDateInitial.value;
    }

    // Establecer el mínimo de la fecha final como la fecha inicial
    inputDateFinal.min = inputDateInitial.value;

    dateInitialReport.value = inputDateInitial.value;
    dateFinalReport.value = inputDateFinal.value;
}


const fnAdjustmentFilterDataTable = () => {
    let wrapper = document.querySelector("#tblData_wrapper");
    let rowIzq = wrapper.querySelector(".col-md-auto.me-auto");
    let rowCen = document.createElement("div");
    rowCen.className = "row col-xl-8 col-lg-7";
    let rowDer = wrapper.querySelector(".col-md-auto.ms-auto");
    rowIzq.classList.remove('col-md-auto', 'me-auto');
    rowIzq.classList.add('row', 'col-lg-2', "col-xl-2", "col-sm-4");
    rowDer.classList.remove('col-md-auto', 'ms-auto');
    rowDer.classList.add('row', 'col-lg-2', "col-xl-2", "col-sm-4");

    // Crear los elementos del filtro de fecha y botón de búsqueda
    let filterDate = document.createElement('div');
    filterDate.className = 'dt-filtro-fecha';

    let initialValue, finalValue, includeVoidValue;
    if (dateInitial != undefined && dateFinal != undefined && includeVoid != undefined) {
        initialValue = dateInitial;
        finalValue = dateFinal;
        if (includeVoid) {
            includeVoidValue = "checked";
        } else {
            includeVoidValue = "";
        }
    } else {
        initialValue = processingDate;
        finalValue = processingDate;
        includeVoidValue = "";
    }

    filterDate.innerHTML =
        `  <div class="row">
                <div class="row col-5 m-0 col-md-5 col-xl-5 col-xxl-3 mb-1">
                    <div class="col-5 col-sm-6 col-xxl-6 pe-0">
                        Fecha inicial:
                    </div>
                    <div class="col-7 col-sm-6 col-lg-6">
                        <input type="date" id="dateInitial" value="${initialValue}">
                    </div>
                </div>
                <div class="row col-5 m-0 col-md-6 col-xl-5 col-xxl-3 mb-1">
                    <div class="col-5 col-xl-6">
                        Fecha final:
                    </div>
                    <div class="col-7 col-lg-6">
                        <input type="date" id="dateFinal" value="${finalValue}" min="${initialValue}">
                    </div>
                </div>
                 <div class="row col-5 m-0 col-md-5 col-xl-5 col-xxl-2 mb-1">
                     <div class="col-8">
                         Anulados
                     </div>
                     <div class="col-1">
                         <input type="checkbox" id="includeVoid" ${includeVoidValue}>
                     </div>
                 </div>
                <div class="row col-4 col-sm-3 col-xl-2 mb-1">
                    <button onclick="fnLoadDatatable()" data-bs-toggle="tooltip" data-bs-placement="top"
                    data-bs-title="Filtrar"
                    class="btn btn-sm btn-secondary boder-outline col-10 col-xl-12" id="btnFilter">
                        <i class="bi bi-funnel-fill"></i>  Filtrar
                    </button>
                </div>
            </div>`;

    rowCen.appendChild(filterDate);
    let container = rowIzq.parentElement;
    container.insertBefore(rowCen, rowDer);

    inputDateInitial = document.querySelector("#dateInitial");
    inputDateFinal = document.querySelector("#dateFinal");
    inputIncludeVoid = document.querySelector("#includeVoid");
    //Ajustamos las fechas
    fnAdjustmentDates();

    inputDateInitial.addEventListener("change", () => {
        fnAdjustmentDates();
    });

    dateInitialReport.value = initialValue;
    dateFinalReport.value = finalValue;

    fnEnableTooltip();
};

const fnVoid = async (id) => {

    try {

        let result = await Swal.fire({
            title: `&#191;Está seguro de anular la cotización?`,
            icon: "warning",
            showCancelButton: true,
            reverseButtons: true,
            focusConfirm: false,
            confirmButtonText: "Aceptar",
            cancelButtonText: "Cancelar",
            customClass: {
                confirmButton: "btn btn-primary px-3 mx-2",
                cancelButton: "btn btn-danger px-3 mx-2"
            },
            buttonsStyling: false
        });

        if (!result.isConfirmed) {
            return;
        }

        let url = `/exchange/quotation/Void?id=${id}`;

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

const fnPrintReport = async (id) => {
    try {
        const url = `/exchange/quotation/ValidateDataToPrint?id=${id}`;

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
            if (jsonResponse.isSuccess) {
                window.open(`${jsonResponse.data.urlRedirectTo}`, "_blank");
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

const fnDeleteRow = async (url, dateTransa, transaFullName) => {
    try {
        let fetchOptions;

        let result = await Swal.fire({
            title: `&#191;Está seguro de eliminar la transacción?`,
            html: `${dateTransa} ${transaFullName}<br>Este registro no se podrá recuperar`,
            icon: "warning",
            showCancelButton: true,
            reverseButtons: true,
            focusConfirm: false,
            confirmButtonText: ButtonsText.Delete,
            cancelButtonText: ButtonsText.Cancel,
            customClass: {
                confirmButton: "btn btn-danger px-3 mx-2",
                cancelButton: "btn btn-primary px-3 mx-2"
            },
            buttonsStyling: false
        });

        if (!result.isConfirmed) {
            return;
        }

        fetchOptions = {
            method: "DELETE"
        };

        const fetchResponse = await fetch(url, fetchOptions);
        let jsonResponse = await fetchResponse.json();

        if (jsonResponse.isSuccess) {
            dataTable.ajax.reload();
            toastr.success(jsonResponse.successMessages);
        } else {
            toastr.success(jsonResponse.errorMessages);
        }
    }
    catch (e) {
        Swal.fire({
            icon: 'error',
            title: "Error en la conexión",
            text: e
        });
    }
}


const fnExportExcel = async () => {
    try {
        let url =
            `/exchange/quotation/exportexcel?dateInitial=${dateInitial}&dateFinal=${dateFinal}&includeVoid=${includeVoid}`;
        const response = await fetch(url, {
            method: 'GET'
        });

        // Convertir la respuesta a un blob
        const blob = await response.blob();

        // Crear una URL para el blob
        const blobUrl = window.URL.createObjectURL(blob);


        // Crear un enlace temporal
        const link = document.createElement('a');
        link.href = blobUrl;

        link.download = 'ListadoCotizaciones.xlsx';

        // Simular clic en el enlace
        link.click();

        window.URL.revokeObjectURL(blobUrl);
    }
    catch (e) {
        Swal.fire({
            icon: 'error',
            title: "Error en la conexión",
            text: e
        });
    }
};

const fnLoadDatatable = () => {
    if (inputDateInitial != undefined && inputDateFinal != undefined && inputIncludeVoid != undefined) {
        dateInitial = inputDateInitial.value;
        dateFinal = inputDateFinal.value;
        includeVoid = inputIncludeVoid.checked;
    } else {
        dateInitial = processingDate;
        dateFinal = processingDate;
        includeVoid = false;
    }
    //let dateFinal
    if (dataTable) dataTable.destroy();
    dataTable = new DataTable("#tblData", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/exchange/quotation/getall?dateInitial=${dateInitial}&dateFinal=${dateFinal}&includeVoid=${includeVoid}`,
            "dataSrc": function (data) {
                if (data.isSuccess) {
                    return data.data;
                } else {
                    fnShowModalMessages(data);
                    return [];
                }
            },
            "complete": function () {
                fnEnableTooltip();
                fnAdjustmentFilterDataTable();
            }
        },
        "columns": [
            {
                data: 'dateTransa', "width": "5%"
                , render: DataTable.render.date(defaultFormatDate, defaultFormatDate)
                , orderable: true
            },
            {
                data: 'numeral', "width": "8%"
                , render: function (data, type, row) {
                    return `${row.typeTrx.code}-${row.numeral.toString().padStart(paddingLength, paddingChar)}`;
                }
                , orderable: false
            },
            {
                data: 'customerTrx.businessName', "width": "30%"
                , render: DataTable.render.ellipsis(28, false)
                , orderable: true
            },
            {
                data: 'currencyTransaTrx.code', "width": "5%"
                , render: function (data, type, row) {
                    return `${row.currencyTransaTrx.code} - ${row.currencyTransferTrx.code}`;
                }
                , orderable: false
            },
            {
                data: 'exchangeRateBuyTransa', "width": "5%"
                , render: DataTable.render.number(null, null, decimalExchange)
                , orderable: false
            },
            {
                data: 'exchangeRateSellTransa', "width": "5%"
                , render: DataTable.render.number(null, null, decimalExchange)
                , orderable: false
            },
            {
                data: 'amountTransaction', "width": "10%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: true
            },
            {
                data: 'amountRevenue', "width": "10%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: false
            },
            {
                data: 'amountCost', "width": "10%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: false
            },
            {
                data: 'businessExecutiveTrx.code', "width": "5%"
                , render: function (data, type, row) {
                    return `${data}`;
                }
                , orderable: false
            },
            {
                data: 'isClosed', "width": "5%"
                , render: function (data, type, row) {
                    return data ? YesNo.Yes : YesNo.No;
                }
                , orderable: false
            },
            {
                data: 'isPosted', "width": "5%"
                , render: function (data, type, row) {
                    return data ? YesNo.Yes : YesNo.No;
                }
                , orderable: false
            },
            {
                data: null, "width": "10%", orderable: false
                , "render": (data, type, row) => {

                    let urlUpdate = `/exchange/quotation/CreateDetail?id=${row.id}`;
                    let urlReClosed = urlUpdate;

                    if (row.isLoan || row.isPayment) {
                        urlUpdate = `/exchange/quotation/Upsert?id=${row.id}`;
                        urlReClosed = urlUpdate;
                    }

                    let btnUpdate = `<a href="${urlUpdate}" class="btn btn-primary py-1 px-3 my-0 mx-1"
                                        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
                                         <i class="bi bi-pencil-square fs-5"></i>
                                     </a>`;

                    let btnReClosed = `<a href="${urlReClosed}" class="btn btn-warning py-1 px-3 my-0 mx-1"
                                        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Re-Cerrar">
                                         <i class="bi bi-check2-square fs-5"></i>
                                     </a>`;

                    let btnVoid = `<a onclick="fnVoid(${row.id})" class="btn btn-outline-info py-1 px-3 my-0 mx-1"
                                        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Anular">
                                         <i class="bi bi-x-square-fill fs-5"></i>
                                     </a>`;

                    let btnView = `<a href="/exchange/quotation/Detail?id=${row.id}" class="btn btn-success py-1 px-3 my-0 mx-1"
                                     data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Ver">
                                     <i class="bi bi-eye fs-5"></i>
                                   </a>`;

                    let btnDelete = `<a href="/exchange/quotation/Delete?id=${row.id}" class="btn btn-danger py-1 px-3 my-0 mx-1" 
                                         data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Eliminar">
                                          <i class="bi bi-trash-fill fs-5"></i>
                                     </a> `;

                    let btnPrint = `<a onclick="fnPrintReport('${row.id}')" class="btn btn-outline-primary py-1 px-3 my-0 mx-1" 
                                       data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Imprimir">
                                        <i class="bi bi-printer fs-5"></i>
                                    </a> `;

                    let buttons = `<div class="btn-group" role="group">`;


                    if (row.isVoid) {
                        buttons += `
                            ${btnView}
                            ${btnPrint}
                        `;
                    } else if (row.isClosed && row.isPosted && !row.isVoid) {
                        buttons += `
                            ${btnView}
                            ${btnPrint}
                            ${btnVoid}
                        `;
                    } else if (row.isClosed && !row.isPosted && !row.isVoid) {
                        buttons += `
                            ${btnView}
                            ${btnPrint}
                            ${btnReClosed}
                            ${btnVoid}
                        `;
                    }
                    //else if (row.isClosed && !row.isPosted && row.isVoid) {
                    //    buttons += `
                    //        ${btnView}
                    //        ${btnPrint}
                    //    `;
                    //}
                    else {
                        buttons += `
                            ${btnView}
                            ${btnUpdate}
                            ${btnDelete}
                            ${btnPrint}
                        `;
                    }

                    buttons += `</div>`;

                    return buttons;
                }
            }
        ],
        "createdRow": function (row, data) {
            if (data.isVoid) {
                $(row).addClass('table-danger bg-danger bg-opacity-50');
            }
        },
        "searching": true,
        "select": selectOptions
    });
}