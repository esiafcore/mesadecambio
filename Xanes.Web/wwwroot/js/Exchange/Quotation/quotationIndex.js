let dataTable, containerMain;
document.addEventListener("DOMContentLoaded", function () {
    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid";
    loadDatatable();
    //Habilitar Tooltip
    fnEnableTooltip();
});

function loadDatatable() {
    dataTable = new DataTable("#tblData", {
        dataSrc: 'data',
        ajax: {
            "url": '/exchange/quotation/getall',
            "dataSrc": function (data) {
                if (data.isSuccess) {
                    return data.data;
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Error",
                        text: data.errorMessages
                    });
                    return [];
                }
            },
            "complete": function () {
                fnEnableTooltip();
            }
        },
        columns: [
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
                data: 'customerTrx.businessName', "width": "38%"
                , render: DataTable.render.ellipsis(28, false)
                , orderable: true
            },
            {
                data: 'currencyTransaTrx.code', "width": "10%"
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
                data: 'amountRevenue', "width": "5%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: false
            },
            {
                data: 'amountCost', "width": "5%"
                , render: DataTable.render.number(null, null, decimalTransa)
                , orderable: false
            },
            {
                data: 'isClosed', "width": "2%"
                , render: function (data, type, row) {
                    return data ? YesNo.Yes : YesNo.No;
                }
                , orderable: false
            },
            {
                data: 'isPosted', "width": "2%"
                , render: function (data, type, row) {
                    return data ? YesNo.Yes : YesNo.No;
                }
                , orderable: false
            },
            {
                data: null, "width": "5%", orderable: false
                , "render": (data, type, row) => {
                    return `<div class="btn-group" role="group">
                        <a href="/exchange/quotation/Detail?id=${row.id}" class="btn btn-success py-1 px-3 my-0 mx-1"
                            data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Ver">
                            <i class="bi bi-eye fs-5"></i>
                        </a>
                        <a href="/exchange/quotation/CreateDetail?id=${row.id}" class="btn btn-primary py-1 px-3 my-0 mx-1"
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
                            <i class="bi bi-pencil-square fs-5"></i>
                        </a>
                        <a href="/exchange/quotation/Delete?id=${row.id}" class="btn btn-danger py-1 px-3 my-0 mx-1" 
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Eliminar">
                            <i class="bi bi-trash-fill fs-5"></i>
                        </a>
                          <a onclick="fnPrintReport('${row.id}')" class="btn btn-outline-primary py-1 px-3 my-0 mx-1" 
                             data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Imprimir">
                              <i class="bi bi-printer fs-5"></i>
                          </a>
                    </div>`;
                }
            }
        ],
        searching: true,
        select: selectOptions
    });
}


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