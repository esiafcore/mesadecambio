let dateInitial, dateFinal;
let inputDateInitial, inputDateFinal;
document.addEventListener("DOMContentLoaded", function () {

    inputDateInitial = document.querySelector("#dateInitial");
    inputDateFinal = document.querySelector("#dateFinal");

});

const fnExportExcel = async () => {
    try {

        dateInitial = inputDateInitial.value;
        dateFinal = inputDateFinal.value;

        fntoggleLoading("Generando...");

        let url =
            `/exchange/quotationlegacy/exportexcel?dateInitial=${dateInitial}&dateFinal=${dateFinal}`;
        const response = await fetch(url,
            {
                method: 'GET'
            });

        if (response.status === 204) {
            Swal.fire({
                icon: 'warning',
                title: "No hay registros",
                text: "No se encontraron registros para exportar en el rango de fechas especificado."
            });
            return;
        } else if (response.status == 400) {
            window.location.href = window.location.href;
            return;
        }

        // Convertir la respuesta a un blob
        const blob = await response.blob();

        // Crear una URL para el blob
        const blobUrl = window.URL.createObjectURL(blob);


        // Crear un enlace temporal
        const link = document.createElement('a');
        link.href = blobUrl;

        link.download = 'ListadoCotizacionesLegacy.xlsx';

        // Simular clic en el enlace
        link.click();

        window.URL.revokeObjectURL(blobUrl);
    } catch (e) {
        Swal.fire({
            icon: 'error',
            title: "Error en la conexión",
            text: e
        });
    } finally {
        fntoggleLoading();
    }
};