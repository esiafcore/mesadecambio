document.addEventListener("DOMContentLoaded", () => {

    let inputId = document.getElementById("DataModel_Id");
    $("#DataModel_CategoryId").select2(select2Options);
    $("#DataModel_SectorId").select2(select2Options);

    //Setear enfoque en el search input
    $(document).on('select2:open', function (e) {
        document.querySelector(`[aria-controls="select2-${e.target.id}-results"]`).focus();
    });

    //Setear form-floating para Select2
    select2Floating();

    //setear el foco al crear o editar
    document.getElementById("DataModel_Code").focus();
    personType_onClick(personLegalRad);
});

function personType_onClick(objElem) {
    let currentValue = Number(objElem.value);

    if (currentValue == PersonType.Natural) {
        personNaturalDiv.style.display = "" //styleShow;
        personLegalDiv.style.display = styleHide;
    }
    else if (currentValue == PersonType.Legal) {
        personLegalDiv.style.display = "" //styleShow;
        personNaturalDiv.style.display = styleHide;
    }
}

