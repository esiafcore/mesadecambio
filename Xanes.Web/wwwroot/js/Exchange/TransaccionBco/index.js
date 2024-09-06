let dataTable;


document.addEventListener("DOMContentLoaded", function () {
 
    fnLoadDatatable();
    //Habilitar Tooltip
    fnEnableTooltip();


});

const fnLoadDatatable = () => {
  
    if (dataTable) dataTable.destroy();
    dataTable = new DataTable("#tblData", {
        "dataSrc": 'data',
        "ajax": {
            "url": `/exchange/transaccionbco/getall`,
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
            }
        },
        "columns": [
            {
                data: 'fechaTransa', "width": "5%"
                , render: DataTable.render.date(defaultFormatDate, defaultFormatDate)
                , orderable: true
            },
            {
                data: 'numeroTransaccion', "width": "5%"
                , orderable: false
            }               
        ],
        "searching": true,
        "select": selectOptions,
        "lengthMenu": [10],
        "autoWidth": false,
        "language": {
            info: "Mostrando _START_ a _END_ de _TOTAL_ entradas",
            infoFiltered: "(Filtrado de _MAX_ total de entradas)",
            infoEmpty: "No hay datos para mostrar",
            lengthMenu: "",
            zeroRecords: "No se encontraron coincidencias",
            processing: "Procesando...",
            loadingRecords: "Cargando...",
            emptyTable: "No hay datos disponibles en la tabla",
            search: "Buscar",
            select: {
                cells: {
                    "1": "1 celda seleccionada",
                    "_": "%d celdas seleccionadas"
                },
                columns: {
                    "1": "1 columna seleccionada",
                    "_": "%d columnas seleccionadas"
                },
                rows: {
                    "1": "1 fila seleccionada",
                    "_": "%d filas seleccionadas"
                }
            }
        }
    });
}
