let dataTable;

document.addEventListener("DOMContentLoaded", () => {
    fnLoadDatatable();
    //Habilitar Tooltip
    fnEnableTooltip();
});


const fnLoadDatatable = () => {
    dataTable = new DataTable("#tblData", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/customer/customer/getall`,
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
                data: 'code', "width": "5%"
                , orderable: true
            },
            {
                data: 'identificationNumber', "width": "10%"
                , orderable: true
            },
            {
                data: 'businessName', "width": "40%"
                , orderable: true
            },
            {
                data: 'typeTrx.code', "width": "10%"
                , orderable: true
            },
            {
                data: 'sectorTrx.name', "width": "10%"
                , orderable: true
            },
            {
                data: 'businessExecutiveTrx.code', "width": "5%"
                , orderable: true
            },
            {
                data: 'isActive', "width": "5%"
                , render: function (data, type, row) {
                    return data ? YesNo.Yes : YesNo.No;
                }
                , orderable: false
            },
            {
                data: null, "width": "15%", orderable: false
                , "render": (data, type, row) => {

                    let btnUpdate = `<a href="/customer/customer/Upsert?id=${row.id}" class="btn btn-primary py-1 px-3 my-0 mx-1"
                                        data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Editar">
                                         <i class="bi bi-pencil-square fs-5"></i>
                                     </a>`;

                    let btnView = `<a href="/customer/customer/Detail?id=${row.id}" class="btn btn-success py-1 px-3 my-0 mx-1"
                                     data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Ver">
                                     <i class="bi bi-eye fs-5"></i>
                                   </a>`;

                    let btnDelete = `<a href="/customer/customer/Delete?id=${row.id}" class="btn btn-danger py-1 px-3 my-0 mx-1" 
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