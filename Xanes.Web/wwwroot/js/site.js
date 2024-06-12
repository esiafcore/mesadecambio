// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Enable tooltip
const fnEnableTooltip = () => {
    let buttons = document.querySelectorAll('[data-bs-toggle="tooltip"]');

    // Inicializa el tooltip para cada botón
    buttons.forEach(function (button) {
        new bootstrap.Tooltip(button); // Se usa 'new bootstrap.Tooltip' para inicializar el tooltip
    });
};

const LocalTimeDefault = "T00:00:00";

const defaultDecimalTransa = 2;
const defaultDecimalExchange = 4;
const localLanguage = "es-NI";
const formatterAmount = (fractionDigits = defaultDecimalTransa) => {
    return new Intl.NumberFormat(localLanguage, {
        style: 'decimal'
        , maximumFractionDigits: fractionDigits
        , minimumFractionDigits: fractionDigits
    });
}
//formatterAmount(2).format(10000000000.123456)

const defaultFormatDate = 'dd/MM/yyyy';
const paddingLength = 3;
const paddingChar = '0';
const selectOptions = {
    info: false,
    items: 'row',
    style: 'os',
    blurable: true,
    className: 'selectedinfo'
}

function fntoggleLoading(textContent = "Cargando...") {
    const loading = document.getElementById("loading");
    const loadingText = document.getElementById("loading-text");
    loadingText.innerHTML = textContent;
    loading.classList.toggle("loading-show");
}

const fnShowModalMessages = (data, titulo = "") => {
    let icono = "error";

    if (data.isInfo) {
        icono = "info";
    } else if (data.isWarning) {
        icono = "warning";
    }

    Swal.fire({
        icon: icono,
        title: titulo,
        text: data.errorMessages
    });
};

const SystemInformationReportType = {
    // Transacciones
    Operation: 1,
    Deposit: 2,
    Transfer: 4
};

const customMessagesSelect = {
    noResults: function () {
        return "No se encontraron resultados.";
    }
};


// Validar que la fecha sea correcta
const fnValidateDatePicker = (dateChangeValue) => {

    try {
        const parsedDate = $.datepicker.parseDate(FormatDateView, dateChangeValue);
        if (parsedDate instanceof Date) {
            return true;
        } else {
            return false;
        }
    } catch (error) {
        return false;
    }

};

const ACJS = {
    GuidEmpty: '00000000-0000-0000-0000-000000000000',
    BadRequest: "Error en la solicitud",
    CharDefaultEmpty: 0,
    PlaceHolderSelect: "-- Seleccionar --",
    DefaultDateMinValue: "1900-01-01",
    DefaultDateMaxValue: "9999-12-31",
    DefaultDateFormatWeb: "yyyy-MM-dd",
    DateInvalid: "La fecha ingresada no es válida."
}

const select2Options = {
    theme: "bootstrap-5",
    language: customMessagesSelect,
    allowClear: true,
    selectionCssClass: "select2--small",
    dropdownCssClass: "select2--small",
    placeholder: ACJS.PlaceHolderSelect,
    width: '100%'
}

//Constantes para STYLES / CSS
const styleHide = "none";
const styleShow = "block";
const styleShowInline = "inline-block";

const ButtonsText = {
    Confirm: "Confirmar",
    Cancel: "Cancelar",
    Closed: "Cerrar",
    Delete: "Eliminar",
    Create: "Crear",
    Link: "Relacionar",
    Add: "Agregar",
    CreateVersion: "Crear Nueva Versión",
    Update: "Actualizar",
    Save: "Guardar",
    View: "Visualizar",
    Approved: "Aprobar",
    DisAccount: "Descontabilizar",
    Bill: "Facturar",
    Void: "Anular",
    Print: "Imprimir",
    Balance: "Saldos",
    Active: "Activar",
    Inactive: "Inactivar",
    Accept: "Aceptar",
    Authorize: "Autorizar",
    Auxiliary: "Auxiliar"
}

const ButtonsColor = {
    Confirm: "#3085d6",
    Cancel: "#d33"
}

const YesNo = {
    Yes: 'Si',
    No: 'No'
}

const CurrencyType = {
    Base: 1,
    Foreign: 2,
    Additional: 4
}

const PersonType = {
    Natural: 1,
    Legal: 2
}


const IdentificationType = {
    RUC: 1,
    CEDU: 2,
    DIMEX: 4,
    NITE: 8,
    DIDI: 16,
    PASS: 32
}

const QuotationDetailType = {
    Deposit: 1,
    Transfer: 2,
    CreditTransfer: 4,
    DebitTransfer: 8
}

const QuotationType = {
    Buy: 1,
    Sell: 2,
    Transfer: 4
}


const fnparseFloat = (valueInput, onlyNumbersPositive = false) => {
    // Verifica si valueInput es un número válido
    if (!isNaN(valueInput) && typeof valueInput !== "string") {
        // Si es un número válido, conviértelo a flotante y devuélvelo
        return parseFloat(valueInput);
    }

    // Verifica si valueInput es una cadena antes de intentar reemplazar las comas
    if (typeof valueInput === 'string') {
        // Utiliza una expresión regular para reemplazar todas las comas
        //if (!onlyNumbersPositive) {
        //    valueInput = valueInput.replace(/,/g, "");
        //} else {
        //    valueInput = valueInput.replace(/[,-]/g, "");
        //}

        if (!onlyNumbersPositive) {
            // Eliminar comas y caracteres no numéricos
            valueInput = valueInput.replace(/[^0-9.-]/g, "");
        } else {
            // Eliminar comas, signos negativos y caracteres no numéricos
            valueInput = valueInput.replace(/[^0-9.]/g, "");
        }
        // Verifica si el valor del input es un número después de reemplazar las comas
        if (!isNaN(valueInput.trim()) && valueInput.trim() !== "") {
            return parseFloat(valueInput.trim());
        }
    }

    // Si no es un número válido, retornar 0
    return 0;
}

const select2Floating = () => {
    $('.select2me')
        .parent('div')
        .children('span')
        .children('span')
        .children('span')
        .css('height', ' calc(3.5rem + 2px)');

    //document.querySelectorAll('.select2me').forEach((element) => {
    //    let parentDiv = element.parentElement;
    //    if (parentDiv.tagName.toLowerCase() === 'div') {
    //        let spans = parentDiv.querySelectorAll('span');
    //        spans.forEach(function (span) {
    //            span.style.height = 'calc(3.5rem + 2px)';
    //            span.style.marginTop = '18px';
    //        });
    //    }
    //});

    //document.querySelectorAll('.select2me').forEach((element) => {
    //    let parentDiv = element.parentElement;
    //    if (parentDiv.tagName.toLowerCase() === 'div') {
    //        let labels = parentDiv.querySelectorAll('label');
    //        labels.forEach(function (label) {
    //            label.style.zIndex = '1';
    //        });
    //    }
    //});

    $('.select2me')
        .parent('div')
        .children('span')
        .children('span')
        .children('span')
        .children('span')
        .css('margin-top', '18px');

    $('.select2me')
        .parent('div')
        .find('label')
        .css('z-index', '1');
}