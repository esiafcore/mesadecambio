let dataTable, containerMain, inputDateInitial, inputDateFinal;
let dateInitial, dateFinal;

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
});

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
}


const fnAdjustmentFilterDataTable = () => {
    let wrapper = document.querySelector("#tblData_wrapper");

    let rowIzq = wrapper.querySelector(".col-md-auto.me-auto");
    let rowDer = wrapper.querySelector(".col-md-auto.ms-auto");
    let filterLength = wrapper.querySelector(".dt-length");
    rowIzq.classList.remove('col-md-auto', 'me-auto');
    rowIzq.classList.add('row', 'col-xl-9', "col-sm-8");
    filterLength.classList.add("col-xl-2", "col-sm-4");
    rowDer.classList.remove('col-md-auto', 'ms-auto');
    rowDer.classList.add('row', 'col-xl-3', "col-sm-4");

    // Crear los elementos del filtro de fecha y botón de búsqueda
    let filterDate = document.createElement('div');
    filterDate.className = 'dt-filtro-fecha col-8 d-flex gap-4';

    let initialValue, finalValue;
    if (dateInitial != undefined && dateFinal != undefined) {
        initialValue = dateInitial;
        finalValue = dateFinal;
    } else {
        initialValue = processingDate;
        finalValue = processingDate;
    }
    filterDate.innerHTML =
        `  <div class="row">
                            <div class="row col-xl-5">
                                <div class="col-4">
                                    Fecha inicial:
                                </div>
                                <div class="col-8">
                                    <input type="date" id="dateInitial" value="${initialValue}">
                                </div>
                            </div>
                            <div class="row col-xl-5">
                                <div class="col-4">
                                    Fecha final:
                                </div>
                                <div class="col-8">
                                    <input type="date" id="dateFinal" value="${finalValue}" min="${initialValue}">
                                </div>
                            </div>
                            <div class="row col-2">
                                <button onclick="fnLoadDatatable()" class="btn btn-sm btn-secondary boder-outline col-12" id="btnFilter">
                                    <i class="bi bi-funnel-fill"></i>  Filtrar
                                </button>
                            </div>
                        </div>`;

    filterLength.parentNode.insertBefore(filterDate, filterLength.nextSibling);
    inputDateInitial = document.querySelector("#dateInitial");
    inputDateFinal = document.querySelector("#dateFinal");

    //Ajustamos las fechas
    fnAdjustmentDates();

    inputDateInitial.addEventListener("change", () => {
        fnAdjustmentDates();
    });
};

const fnVoid = async (id) => {

    try {

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

    if (fetchResponse.ok) {
        let jsonResponse = await fetchResponse.json();

        if (jsonResponse.isSuccess) {
            dataTable.ajax.reload();
            toastr.success(jsonResponse.successMessages);
        } else {
            toastr.success(jsonResponse.errorMessages);
        }
    }
    else {
        console.log(fetchResponse);
    }
}

const fnLoadDatatable = () => {
    if (inputDateInitial != undefined && inputDateFinal != undefined) {
        dateInitial = inputDateInitial.value;
        dateFinal = inputDateFinal.value;
    } else {
        dateInitial = processingDate;
        dateFinal = processingDate;
    }
    //let dateFinal
    if (dataTable) dataTable.destroy();
    dataTable = new DataTable("#tblData", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/exchange/quotation/getall?dateInitial=${dateInitial}&dateFinal=${dateFinal}`,
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

                    let btnUpdate = `<a href="/exchange/quotation/CreateDetail?id=${row.id}" class="btn btn-primary py-1 px-3 my-0 mx-1"
                                        data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
                                         <i class="bi bi-pencil-square fs-5"></i>
                                     </a>`;

                    let btnReClosed = `<a href="/exchange/quotation/CreateDetail?id=${row.id}" class="btn btn-warning py-1 px-3 my-0 mx-1"
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