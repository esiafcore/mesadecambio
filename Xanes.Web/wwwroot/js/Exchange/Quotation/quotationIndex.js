﻿let dataTable;
document.addEventListener("DOMContentLoaded", function () {
    loadDatatable();

    //Habilitar Tooltip
    fnEnableTooltip();
});


function loadDatatable() {
    dataTable = new DataTable("#tblData", {
        dataSrc: 'data',
        "ajax": { url: '/exchange/quotation/getall' },
        "columns": [
            {
                data: 'dateTransa', "width": "5%"
                , render: DataTable.render.date(defaultFormatDate, defaultFormatDate)
            },
            {
                data: 'numeral', "width": "5%"
                , render: function (data, type, row) {
                    return `${row.typeTrx.code}-${row.numeral.toString().padStart(paddingLength, paddingChar)}`;
                }
            },
            {
                data: 'customerTrx.businessName', "width": "30%"
            },
            { data: 'currencyOriginExchangeTrx.code', "width": "5%" },
            { data: 'currencyTransaTrx.code', "width": "5%" },
            {
                data: 'exchangeRateBuyTransa', "width": "5%"
                , render: DataTable.render.number(null, null, decimalExchange)
            },
            {
                data: 'exchangeRateSellTransa', "width": "5%"
                , render: DataTable.render.number(null, null, decimalExchange)
            },
            {
                data: 'amountTransa', "width": "10%"
                , render: DataTable.render.number(null, null, decimalTransa)
            },
            {
                data: 'amountRevenue', "width": "10%"
                , render: DataTable.render.number(null, null, decimalTransa)
            },
            {
                data: 'amountCost', "width": "10%"
                , render: DataTable.render.number(null, null, decimalTransa)
            },
            {
                data: null, "width": "10%"
                ,"render": (data, type, row) => {
                    return `<div class="btn-group" role="group">
                        <a href="/exchange/quotation/detail?id=${row.id}" class="btn btn-success py-1 px-3 my-0 mx-2"
                            data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Ver">
                            <i class="bi bi-eye fs-5"></i>
                        </a>
                        <a href="/exchange/quotation/upsert?id=${row.id}" class="btn btn-primary py-1 px-3 my-0 mx-2"
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
                            <i class="bi bi-pencil-square fs-5"></i>
                        </a>
                        <a onClick="fnDeleteRow('/exchange/quotation/delete?id=${row.id}','${row.dateTransa}','${row.transaFullName}')" class="btn btn-danger py-1 px-3 my-0 mx-2" 
                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Eliminar">
                            <i class="bi bi-trash-fill fs-5"></i>
                        </a>

                    </div>`
                },
                "width": "15%"
            }
        ],
        "info": false,
        "ordering": true,
        "paging": true,
        "select": true,

    });
}

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