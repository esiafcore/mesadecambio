﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Enable tooltip
const fnEnableTooltip = () => {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
};

// Función para destruir el tooltip
const fnDisabledTooltip = (element) => {
    const tooltip = bootstrap.Tooltip.getInstance(element);
    if (tooltip) {
        tooltip.dispose();
    }
};

// Funcion para mover modales
function fnmakeModalDraggable(modalId) {
    $("#" + modalId).draggable({
        handle: ".modal-header",
        cursor: "move"
    });
}


const LocalTimeDefault = "T00:00:00";

const defaultDecimalTransa = 2;
const defaultDecimalExchange = 6;
const localLanguage = "es-NI";
const formatterAmount = (fractionDigits = defaultDecimalTransa) => {
    return new Intl.NumberFormat(localLanguage, {
        style: 'decimal'
        , maximumFractionDigits: fractionDigits
        , minimumFractionDigits: fractionDigits
    });
}
//formatterAmount(2).format(10000000000.123456)

const fnremoveSession = () => {
    //Filtros en Quotation
    sessionStorage.removeItem('objFilter');
    sessionStorage.removeItem('searchValue');
}

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

const fnredirectBtnIndex = async (event) => {
    if (!event.ctrlKey) {
        fntoggleLoading();
    }
}

const ButtonsText = {
    Confirm: "Confirmar",
    Cancel: "Cancelar",
    Closed: "Cerrar",
    Delete: "Eliminar",
    Create: "Crear",
    Link: "Relacionar",
    Add: "Agregar",
    Adjustment: "Ajustar",
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
    Auxiliary: "Auxiliar",
    Yes: "Si",
    No: "No",
    Copy: "Copiar",
    CopiedAlert: "Copiado!!"
}

const fnSweetAlertOkay = async ({ title, text, icon, time, html }) => {
    return new Promise(resolve => {
        Swal.fire({
            title: title,
            text: text,
            icon: icon,
            html: html,
            showCancelButton: false,
            showDenyButton: false,
            confirmButtonText: ButtonsText.Accept,
            buttonsStyling: false,
            customClass: {
                confirmButton: 'app-bg-primary',
                footer: 'custom-footer-bg'
            },
            footer: `<label class="fs-labelform text-primary">${time}</label>`
        }).then(() => resolve());
    });
};

const fnShowModalMessages = async (data, titulo = "") => {
    let icono = "error";

    if (data.isInfo) {
        icono = "info";
    } else if (data.isWarning) {
        icono = "warning";
    }

    return new Promise(resolve => {
        Swal.fire({
            title: titulo,
            text: data.errorMessages,
            icon: icono,
            showCancelButton: false,
            showDenyButton: false,
            confirmButtonText: ButtonsText.Accept,
            buttonsStyling: false,
            customClass: {
                confirmButton: 'app-bg-primary'
            }
        }).then(() => resolve());
    });
};

const SystemInformationReportType = {
    // Transacciones
    Operation: 1,
    Deposit: 2,
    Transfer: 4,
    Transport: 8
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

const ToggleLoadingText = {
    Printing: "Imprimiendo...",
    GeneratePdf: "Generado PDF...",
    GenerateExcel: "Generado Excel..."


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
    Transport: 4
}


const fnparseFloat = (valueInput, onlyNumbersPositive = false) => {
    // Verifica si valueInput es un número válido
    if (!isNaN(valueInput) && typeof valueInput !== "string") {
        // Si es un número válido, conviértelo a flotante y devuélvelo
        return parseFloat(valueInput);
    }

    // Verifica si valueInput es una cadena antes de intentar reemplazar las comas
    if (typeof valueInput === 'string') {

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

const fnremoveClassBtnExporDataTable = (dt, node, config) => {
    $(node).removeClass("btn-secondary"); // Remueve la clase 'btn-secondary'
};

const fncreateBtnCopyToClipboard = (text) => {
    const btnCopyToClipBoard =
        `<a onclick="copyToClipboard('${text}', this)"
            data-bs-toggle="tooltip" data-bs-placement="top" title="${ButtonsText.Copy}"
            class="btn btn-warning-outline border text-info py-1 px-3 my-0 mx-1">
            <i class="bi bi-copy"></i>
        </a>`;

    return btnCopyToClipBoard;
};

// Copiar UID al clipboard
const copyToClipboard = async (text, btnElement) => {
    try {
        await navigator.clipboard.writeText(text);

        // Cambiar el título del tooltip a "Copiado!"
        const tooltip = bootstrap.Tooltip.getInstance(btnElement);
        btnElement.setAttribute("data-bs-original-title", ButtonsText.CopiedAlert);
        tooltip.show();

        // Desactivar tooltip y restaurar el texto del tooltip original
        btnElement.setAttribute("data-bs-original-title", ButtonsText.Copy);

    } catch (error) {
        Swal.fire({
            icon: 'error',
            title: ACJS.NoStatus,
            text: error
        });
    }
};

const LengthMenuDatatable = [15, 25, 50];
const SearchInputDatatableId = "dt-search-0";
const LenghtSelectDatatableId = "dt-length-0";
const SearchInputContainerDatatableId = "dt-search";
const LenghtSelectContainerDatatableId = "dt-length";
// Funcion para remarcar borde de input search y el lenght en el datatable
const fninputDatatableMarkBorder = () => {
    const searchInputContainerDatatableAll = document.querySelectorAll(`.${SearchInputContainerDatatableId}`);
    if (searchInputContainerDatatableAll) {
        searchInputContainerDatatableAll.forEach((searchInputContainerDatatable) => {
            const searchInput = searchInputContainerDatatable.querySelector("input");
            if (searchInput) {
                searchInput.classList.add(
                    "border",
                    "border-primary",
                    "border-opacity-25"
                );
            }
        });
    }

    const lengthSelectContainerDatatableAll = document.querySelectorAll(`.${LenghtSelectContainerDatatableId}`);
    if (lengthSelectContainerDatatableAll) {
        lengthSelectContainerDatatableAll.forEach((lengthSelectContainerDatatable) => {
            const lengthSelect = lengthSelectContainerDatatable.querySelector("select");
            if (lengthSelect) {
                lengthSelect.classList.add(
                    "border",
                    "border-primary",
                    "border-opacity-25",
                    "py-1"
                );
            }
        });
    }
};

const fnadjustDataTableResposive = (e, datatable, row, showHide, update) => {
    const childNode = row.child();
    if (childNode && childNode.length > 0) {
        // Busca todos los elementos UL con la clase "dtr-details" dentro del primer hijo
        const hiddenList = $(row.child()[0]).find("ul.dtr-details");
        const hiddenListChild = hiddenList.children();

        for (let i = 0; i < hiddenListChild.length; i++) {
            hiddenListChild[i].classList.remove(
                "text-center",
                "text-end",
                "text-start"
            );
            // Agrega la clase "text-start" para alinear el texto a la izquierda
            hiddenListChild[i].classList.add("text-start");
        }
    }
};
