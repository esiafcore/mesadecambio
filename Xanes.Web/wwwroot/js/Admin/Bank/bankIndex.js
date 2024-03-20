document.addEventListener("DOMContentLoaded", function () {
    loadDatatable();
});

function loadDatatable() {
    let dataTable = $("#tblData");

    dataTable.DataTable({
        "ajax": { url: '/admin/bank/getall' },
        "columns": [
            { data: 'code', "width": "5%" },
            { data: 'name', "width": "15%" },
            { data: 'bankingCommissionPercentage', "width": "5%" },
            { data: 'isCompany', "width": "5%" },
            { data: 'orderPriority', "width": "5%" }
        ],
        "info": false,
        "ordering": true,
        "paging": true,
        "order": [[1, 'asc'], [0, 'desc']],
        "columnDefs": [
            {
                target: 0,
                visible: true,
                searchable: true,
                orderable: true
            },
            {
                target: 1,
                orderable: false
            },
            {
                target: 2,
                orderable: false
            },
            {
                target: 3,
                orderable: false
            },
            {
                target: 4,
                orderable: false
            },

        ]
    });
}