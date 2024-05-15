let typeNumeral, inputTypeNumeral, typeNumerals;
document.addEventListener("DOMContentLoaded", () => {

    let inputId = document.getElementById("DataModel_Id");
    //$("#DataModel_CategoryId").select2(select2Options);
    $("#sector-select").select2(select2Options);
    $("#sector-select").select2('focus');


    //Setear enfoque en el search input
    $(document).on('select2:open', function (e) {
        document.querySelector(`[aria-controls="select2-${e.target.id}-results"]`).focus();
    });

    //Setear form-floating para Select2
    select2Floating();

    //Setear Event Handler a utilizar
    inputFirstName.addEventListener("change", naturalNames_onChange);
    inputSecondName.addEventListener("change", naturalNames_onChange);
    inputLastName.addEventListener("change", naturalNames_onChange);
    inputSecondSurname.addEventListener("change", naturalNames_onChange);

    personType_onClick(personLegalRad);

    inputTypeNumeral = document.querySelector("#typeNumeral");
    typeNumerals = document.querySelectorAll(".typeNumerals");

    typeNumerals.forEach((item) => {
        if (item.checked) typeNumeral = item.value;

        item.addEventListener("change", () => {
            typeNumeral = item.value;
            inputTypeNumeral.value = typeNumeral;
        });
    });

    inputTypeNumeral.value = typeNumeral;

});

function personType_onClick(objElem) {
    let currentValue = objElem.value;

    if (currentValue == PersonType.Natural) {

        personNaturalDiv.forEach((item) => {
            item.style.display = styleShow;
        });
        personLegalDiv.style.display = styleHide;
    }
    else if (currentValue == PersonType.Legal) {
        personLegalDiv.style.display = styleShow;
        personNaturalDiv.forEach((item) => {
            item.style.display = styleHide;
        });
    }
}


function naturalNames_onChange(e) {
    //Hay dato y tipo de persona es natural
    if (e.target.value && personNaturalRad.checked) {
        //Concatenar los 4 campos
        let personName = "";
        if (inputFirstName.value) {
            personName = inputFirstName.value.trim();
        }

        if (inputSecondName.value) {
            personName = `${personName.trim()} ${inputSecondName.value.trim() }`;
        }

        if (inputLastName.value) {
            personName = `${personName.trim()} ${inputLastName.value.trim()}`;
        }

        if (inputSecondSurname.value) {
            personName = `${personName.trim()} ${inputSecondSurname.value.trim()}`;
        }

        console.log(personName);
    }
}
