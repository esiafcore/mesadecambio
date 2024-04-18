document.addEventListener("DOMContentLoaded", () => {

    let inputId = document.getElementById("DataModel_Id");
    let typesList = document.querySelector('#DataModel_TypeId');
    $("#DataModel_CategoryId").select2({
        theme: "bootstrap-5",
        allowClear: true,
        selectionCssClass: "select2--small",
        dropdownCssClass: "select2--small",
        placeholder: $(this).data('placeholder'),
    });

    $("#DataModel_SectorId").select2({
        theme: "bootstrap-5",
        allowClear: true,
        selectionCssClass: "select2--small",
        dropdownCssClass: "select2--small",
        placeholder: $(this).data('placeholder'),
    });

    $(document).on('select2:open', function (e) {
        document.querySelector(`[aria-controls="select2-${e.target.id}-results"]`).focus();
    });


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

    //Es crear
    if (inputId.valueAsNumber == 0) {
        //Seleccionar el primer elemento del Listado de Categoria y Tipo
        typesList.querySelectorAll('option')[1].selected = 'selected'
        document.querySelector('#DataModel_CategoryId').querySelectorAll('option')[0].selected = 'selected'
    }

    //setear el foco al crear o editar
    document.getElementById("DataModel_Code").focus();


});
