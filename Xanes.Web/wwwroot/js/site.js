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

const defaultDecimalTransa = 2;
const defaultDecimalExchange = 4;
const localLanguage = "es-NI";
const formatterAmount = (fractionDigits = defaultDecimalTransa) => {
    return new Intl.NumberFormat(localLanguage, { style: 'decimal', maximumFractionDigits: fractionDigits });
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
    className: 'bg-info bg-opacity-75 bg-gradient'
}

const select2Options = {
    theme: "bootstrap-5",
    allowClear: true,
    selectionCssClass: "select2--small",
    dropdownCssClass: "select2--small",
    placeholder: $(this).data('placeholder'),
    width: '100%',
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

const QuotationDetailType = {
    Deposit: 1,
    Transfer: 2
}

const QuotationType = {
    Buy: 1,
    Sell: 2,
    Transfer: 4
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