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

const defaultFormatDate = 'dd/MM/yyyy';
const defaultDecimalTransa = 2;
const defaultDecimalExchange = 4;

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