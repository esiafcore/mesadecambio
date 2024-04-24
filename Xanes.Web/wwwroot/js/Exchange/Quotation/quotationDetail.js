let containerMain;
let dataTableTransfer, dataTableDeposit;
document.addEventListener("DOMContentLoaded", function () {
    containerMain = document.querySelector("#containerMain");
    containerMain.className = "container-fluid";
    loadDatatable();

    //Habilitar Tooltip
    fnEnableTooltip();
});

//function loadDatatable() {
//    dataTableDeposit = new DataTable("#tblDeposit", {
//        dataSrc: 'data',
//        ajax: { url: '/exchange/quotation/GetDepositByParent' },
//        columns: [
//            {
//                data: 'lineNumber', "width": "5%", orderable: true
//            },
//            {
//                data: 'numeral', "width": "8%"
//                , render: function (data, type, row) {
//                    return `${row.typeTrx.code}-${row.numeral.toString().padStart(paddingLength, paddingChar)}`;
//                }
//                , orderable: false
//            },
//            {
//                data: 'customerTrx.businessName', "width": "38%"
//                , render: DataTable.render.ellipsis(28, false)
//                , orderable: true
//            },
//            {
//                data: 'currencyOriginExchangeTrx.code', "width": "10%"
//                , render: function (data, type, row) {
//                    return `${row.currencyOriginExchangeTrx.code} - ${row.currencyTransaTrx.code}`;
//                }
//                , orderable: false
//            },
//            {
//                data: 'exchangeRateBuyTransa', "width": "5%"
//                , render: DataTable.render.number(null, null, decimalExchange)
//                , orderable: false
//            },
//            {
//                data: 'exchangeRateSellTransa', "width": "5%"
//                , render: DataTable.render.number(null, null, decimalExchange)
//                , orderable: false
//            },
//            {
//                data: 'amountTransa', "width": "10%"
//                , render: DataTable.render.number(null, null, decimalTransa)
//                , orderable: true
//            },
//            {
//                data: 'amountRevenue', "width": "5%"
//                , render: DataTable.render.number(null, null, decimalTransa)
//                , orderable: false
//            },
//            {
//                data: 'amountCost', "width": "5%"
//                , render: DataTable.render.number(null, null, decimalTransa)
//                , orderable: false
//            },
//            {
//                data: 'isClosed', "width": "2%"
//                , render: function (data, type, row) {
//                    return data ? YesNo.Yes : YesNo.No;
//                }
//                , orderable: false
//            },
//            {
//                data: 'isPosted', "width": "2%"
//                , render: function (data, type, row) {
//                    return data ? YesNo.Yes : YesNo.No;
//                }
//                , orderable: false
//            },
//            {
//                data: null, "width": "5%", orderable: false
//                , "render": (data, type, row) => {
//                    return `<div class="btn-group" role="group">
//                        <a href="/exchange/quotation/detail?id=${row.id}" class="btn btn-success py-1 px-3 my-0 mx-1"
//                            data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Ver">
//                            <i class="bi bi-eye fs-5"></i>
//                        </a>
//                        <a href="/exchange/quotation/upsert?id=${row.id}" class="btn btn-primary py-1 px-3 my-0 mx-1"
//                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Editar">
//                            <i class="bi bi-pencil-square fs-5"></i>
//                        </a>
//                        <a onClick="fnDeleteRow('/exchange/quotation/delete?id=${row.id}','${row.dateTransa}','${row.transaFullName}')" class="btn btn-danger py-1 px-3 my-0 mx-1" 
//                           data-bs-toggle="tooltip" data-bs-placement="top" data-bs-title="Eliminar">
//                            <i class="bi bi-trash-fill fs-5"></i>
//                        </a>

//                    </div>`;
//                }
//            }
//        ],
//        searching: true,
//        select: selectOptions

//    });
//}
