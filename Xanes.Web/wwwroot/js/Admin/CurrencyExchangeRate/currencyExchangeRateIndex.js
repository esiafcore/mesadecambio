let dataTable, currencies;

document.addEventListener("DOMContentLoaded", function () {
    currencies = document.querySelectorAll(".currencies");

    fnLoadDatatable();
    //Habilitar Tooltip
    fnEnableTooltip();
});

const fnGetCurrency = () => {
    let currency = CurrencyType.Foreign;
    currencies.forEach((item) => {
        if (item.checked) {
            currency = item.value;
        }
    });

    return currency;

};

const fnExportExcel = async () => {
    try {
        let url =
            `/admin/currencyexchangerate/exportexcel?currencyType=${fnGetCurrency()}`;
        const response = await fetch(url, {
            method: 'GET'
        });

        if (response.status === 204) {
            Swal.fire({
                icon: 'warning',
                title: "No hay registros",
                text: "No se encontraron registros para exportar en la moneda especificada."
            });
            return;
        }

        if (!response.ok) {
            throw new Error('Error en la respuesta del servidor');
        }


        // Convertir la respuesta a un blob
        const blob = await response.blob();

        // Crear una URL para el blob
        const blobUrl = window.URL.createObjectURL(blob);


        // Crear un enlace temporal
        const link = document.createElement('a');
        link.href = blobUrl;

        link.download = 'ListadoTiposDeCambio.xlsx';

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
    if (dataTable) dataTable.destroy();
    dataTable = new DataTable("#tblData", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/admin/currencyexchangerate/getall?currencyType=${fnGetCurrency()}`,
            "dataSrc": function (data) {
                if (data.isSuccess) {
                    return data.data;
                } else {
                    return [];
                }
            },
            "complete": function () {
                fnEnableTooltip();
            }
        },
        "columns": [
            {
                data: 'dateTransa', "width": "15%"
                , render: DataTable.render.date(defaultFormatDate, defaultFormatDate)
                , orderable: true
            },
            {
                data: 'currencyTrx.abbreviation', "width": "15%"
                , render: function (data, type, row) {
                    return `${data}`;
                }
                , orderable: false
            },
            {
                data: 'officialRate', "width": "15%"
                , render: DataTable.render.number(null, null, decimalExchange)
                , orderable: true
            },
            {
                data: 'createdDate', "width": "15%"
                , render: function (data, type, row) {
                    if (!data) {
                        return '';
                    }

                    let dateSplit = data.split(".")[0];
                    return `${dateSplit.replace("T", " ")}`;
                }
                , orderable: true
            },
            {
                data: null, "width": "30%", orderable: false
                , "render": (data, type, row) => {

                    let btnUpdate = `<a href="/admin/currencyexchangerate/Upsert?id=${row.id}" class="btn btn-primary py-1 px-3 my-0 mx-1"
                                        data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Editar">
                                         <i class="bi bi-pencil-square fs-5"></i>
                                     </a>`;

                    let btnView = `<a href="/admin/currencyexchangerate/Detail?id=${row.id}" class="btn btn-success py-1 px-3 my-0 mx-1"
                                     data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Ver">
                                     <i class="bi bi-eye fs-5"></i>
                                   </a>`;

                    let btnDelete = `<a href="/admin/currencyexchangerate/Delete?id=${row.id}" class="btn btn-danger py-1 px-3 my-0 mx-1" 
                                         data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Eliminar">
                                          <i class="bi bi-trash-fill fs-5"></i>
                                     </a> `;

                    let buttons = `<div class="btn-group" role="group">`;


                    buttons += `
                            ${btnView}
                            ${btnUpdate}
                            ${btnDelete}
                        `;

                    buttons += `</div>`;

                    return buttons;
                }
            }
        ],
        "searching": true,
        "select": selectOptions
    });
}

const fnupdateLinkParameter = (selectedRadio) => {
    const radioValue = parseInt(selectedRadio.value);
    window.location.href = `${window.location.origin}/Admin/CurrencyExchangeRate/Index?currencyType=${radioValue}`;
};