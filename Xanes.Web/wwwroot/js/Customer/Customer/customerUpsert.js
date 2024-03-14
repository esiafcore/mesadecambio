document.addEventListener("DOMContentLoaded", () => {

    let inputId = document.getElementById("DataModel_Id");
    let typesList = document.querySelector('#DataModel_TypeId');
    //Es crear
    if (inputId.valueAsNumber == 0) {
        //Seleccionar el primer elemento del Listado de Categoria y Tipo
        typesList.querySelectorAll('option')[1].selected = 'selected'
        document.querySelector('#DataModel_CategoryId').querySelectorAll('option')[1].selected = 'selected'
    }
    //setear el foco al crear o editar
    document.getElementById("DataModel_Code").focus();
});
