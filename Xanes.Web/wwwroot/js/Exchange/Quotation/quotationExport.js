let dateInitial, dateFinal;
let inputDateInitial, inputDateFinal;
document.addEventListener("DOMContentLoaded", function () {

    inputDateInitial = document.querySelector("#dateInitial");
    inputDateFinal = document.querySelector("#dateFinal");
    inputDateInitial.addEventListener("change", () => {
        fnAdjustmentDates();
    });
});

const fnAdjustmentDates = () => {
    let dateInitialValue = new Date(document.getElementById('dateInitial').value);
    let dateFinalValue = new Date(document.getElementById('dateFinal').value);

    // Validar si la fecha final es menor que la fecha inicial
    if (dateFinalValue < dateInitialValue) {
        document.getElementById('dateFinal').value = document.getElementById('dateInitial').value;
    }

    // Establecer el mínimo de la fecha final como la fecha inicial
    document.getElementById('dateFinal').min = document.getElementById('dateInitial').value;
}

const fnExportExcel = async () => {
    try {

        dateInitial = inputDateInitial.value;
        dateFinal = inputDateFinal.value;

        fntoggleLoading();

        let url =
            `/exchange/quotation/exportexcel?dateInitial=${dateInitial}&dateFinal=${dateFinal}`;
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

        link.download = 'ListadoCotizaciones.xlsx';

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