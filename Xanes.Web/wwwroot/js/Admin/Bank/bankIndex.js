document.addEventListener("DOMContentLoaded", function () {
    loadDatatable();
});

function loadDatatable() {
    let dataTable = new DataTable("#tblData", {
        "ajax": { url: '/admin/bank/getall' },
        "columns": [
            { data: 'code', "width": "15%" },
            { data: 'name', "width": "15%" },
            { data: 'bankingCommissionPercentage', "width": "15%" },
            { data: 'isCompany', "width": "15%" },
            { data: 'orderPriority', "width": "15%" }
        ]
    });

    //dataTable = $("#tblData").DataTable({
    //    "ajax": { url: '/admin/bank/getall' },
    //    "columns": [
    //        { data: 'code', "width": "15%" },
    //        { data: 'name', "width": "15%" },
    //        { data: 'bankingCommissionPercentage', "width": "15%" },
    //        { data: 'isCompany', "width": "15%" },
    //        { data: 'orderPriority', "width": "15%" }
    //    ]
    //});
}