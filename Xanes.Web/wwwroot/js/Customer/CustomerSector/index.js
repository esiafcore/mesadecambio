let dataTable;

document.addEventListener("DOMContentLoaded", function () {
    fnLoadDatatable();
    //Habilitar Tooltip
    fnEnableTooltip();

    // Evento enviar form para crear
    //const formPrint = document.getElementById("formPrint");
    //formPrint.addEventListener("submit", fnPrintFormSubmit);

    searchValue = document.querySelector("#dt-search-0");
    searchValue.addEventListener('change', fnSetSessionSearchInput);
    searchValue.addEventListener('blur', fnSetSessionSearchInput);

});

const fnSetSessionSearchInput = () => {
    sessionStorage.setItem('searchValue', searchValue.value);
};

const fnCleanFilter = () => {
    btnFilter = document.querySelector("#btnFilter");

    searchValue.value = "";
    fnSetSessionSearchInput();
    clean = true;
    btnFilter.click();
}

const fnDeleteRow = async (url, rowFullName) => {
    try {
        let fetchOptions;

        let result = await Swal.fire({
            title: `&#191;Está seguro de eliminar el sector?`,
            html: `${rowFullName}<br>Este registro no se podrá recuperar`,
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

        fntoggleLoading();

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

        fntoggleLoading();
    }
    catch (e) {
        Swal.fire({
            icon: 'error',
            title: "Error en la conexión",
            text: e
        });
    }
}

const fnLoadDatatable = () => {
    dataTable = new DataTable("#tblData", {
        "layout": {
            topEnd: {
                search: true,
                buttons: [
                    "reload"
                ]
            }
        },

        "dataSrc": 'data',
        "ajax": {
            "url": `/customer/customersector/getall`,
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

                let sessionSearchValue = sessionStorage.getItem('searchValue');

                if (sessionSearchValue) {
                    fnSetSearchValue(sessionSearchValue);
                }
            }
        },
        "columns": [
            {
                data: 'code', "width": "10%"
                ,"render": (data, __, row) => {
                    return `<div style="margin-left:${row.depthnumber}rem" class="align-middle mb-0 mt-0 me-0">${data}</div>`;
                }
                , orderable: true
            },
            {
                data: 'name', "width": "30%"
                , render: DataTable.render.ellipsis(34, false)
                , orderable: true
            },
            {
                data: 'isActive', "width": "10%"
                , render: function (data, type, row) {
                    return `${data ? "ACT" : "INA"}`;
                }
                , orderable: false
            },
            {
                "data": "depthnumber", "width": "10%", "className": "text-end align-middle"
            },
            {
                data: 'createdDate', "width": "10%"
                , render: DataTable.render.datetime(defaultFormatDate)
                , orderable: true
            },
            {
                data: null, "width": "30%", orderable: false
                , "render": function (data) {

                    let urlSub = `/customer/customersector/upsert?parentId=${data.id}`;
                    let urlUpdate = `/customer/customersector/upsert?id=${data.id}`;
                    let urlView = `/customer/customersector/detail?id=${data.id}`;
                    let urlDelete = `/customer/customersector/delete?id=${data.id}`;

                    let btnSub = `<a href="${urlSub}" class="btn btn-warning py-1 px-3 my-0 mx-1" onclick="fnredirectBtnIndex(event)"
                                        data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Sub">
                                         <i class="bi bi-node-plus fs-5"></i>
                                     </a>`;

                    let btnUpdate = `<a href="${urlUpdate}" class="btn btn-primary py-1 px-3 my-0 mx-1" onclick="fnredirectBtnIndex(event)"
                                        data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Editar">
                                         <i class="bi bi-pencil-square fs-5"></i>
                                     </a>`;

                    let btnView = `<a href="${urlView}" class="btn btn-success py-1 px-3 my-0 mx-1" onclick="fnredirectBtnIndex(event)"
                                     data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Ver">
                                     <i class="bi bi-eye fs-5"></i>
                                   </a>`;

                    let btnDelete = `<a href="${urlDelete}" class="btn btn-danger py-1 px-3 my-0 mx-1" onclick="fnredirectBtnIndex(event)"
                                         data-bs-toggle="tooltip" data-bs-trigger="hover" data-bs-placement="top" data-bs-title="Eliminar">
                                          <i class="bi bi-trash-fill fs-5"></i>
                                     </a> `;

                    let buttons = `<div class="btn-group" role="group">`;
                    buttons += `
                            ${btnView}
                            ${btnSub}
                            ${btnUpdate}
                            ${btnDelete}
                                       `;
                    buttons += `</div>`;
                    return buttons;
                }
            }
        ],
        "searching": true,
        "select": selectOptions,
        "autoWidth": false,
        "pageLength": 15
    });

    dataTable.on("responsive-display", fnadjustDataTableResposive);
    dataTable.on('draw', function () {
        fnEnableTooltip(); // Reactivar tooltips tras cada redibujado
    });

    fninputDatatableMarkBorder();

}