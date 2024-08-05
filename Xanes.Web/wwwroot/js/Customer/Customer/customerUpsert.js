let typeNumeral, inputTypeNumeral, typeNumerals;
let personName = "";
let selectBusinessExecutive;

let labelType, divNaturalPerson, divLegalPerson;
document.addEventListener("DOMContentLoaded", () => {

    labelType = document.querySelector("#labelType");
    divNaturalPerson = document.querySelector(".naturalPerson");
    divLegalPerson = document.querySelector(".legalPerson");
    selectBusinessExecutive = document.querySelector("#selectBusinessExecutive");

    $(selectBusinessExecutive).select2(select2Options);

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
    inputCommercialName.addEventListener("change", () => {
        inputFirstName.value = inputCommercialName.value;
        inputLastName.value = inputCommercialName.value;
    });
    inputBusinessName.addEventListener("change", () => {
        inputFirstName.value = inputBusinessName.value;
        inputLastName.value = inputBusinessName.value;
    });



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
    personType_onClick(inputTypeNumeral);

    // Evento enviar form para upsert
    const formUpsert = document.getElementById("formUpsert");
    formUpsert.addEventListener("submit", fnCreateFormSubmit);

});


const fnCreateFormSubmit = async (event) => {

    try {

        document.querySelector("#sectorId").value = document.querySelector("#sector-select").value;

        fntoggleLoading();

        event.preventDefault();
        const formObject = event.currentTarget;

        const url =
            `${formObject.action}`;
        const formData = new FormData(formObject);

        const response = await fetch(url,
            {
                method: 'POST',
                body: formData
            });

        if (!response.ok) {
            const errorMessage = await response.json();
            Swal.fire({
                icon: 'error',
                text: errorMessage.errorMessages
            });
        } else {
            const jsonResponse = await response.json();
            if (!jsonResponse.isSuccess) {
                Swal.fire({
                    icon: 'error',
                    text: jsonResponse.errorMessages
                });
            } else {
                if (jsonResponse.urlRedirect) {
                    window.location.href = jsonResponse.urlRedirect;
                }
            }
        }

    } catch (error) {
        Swal.fire({
            icon: 'error',
            text: error
        });

    } finally {
        fntoggleLoading();
    }
}

const identificationType_onClick = async (objElem) => {
    let identificationTypeCode = document.querySelector("#identificationTypeCode");
    let identificationTypeNumber = document.querySelector("#identificationTypeNumber");


    let url = `/Customer/Customer/GetIdentificationType?id=${objElem.value}`;
    const response = await fetch(url, {
        method: "POST"
    });

    const jsonResponse = await response.json();

    if (jsonResponse.isSuccess) {

        identificationTypeCode.value = jsonResponse.data.code;
        identificationTypeNumber.value = jsonResponse.data.numeral;
    }
}


const personType_onClick = async (objElem) => {

    let currentValue = objElem.value;
    if (currentValue == PersonType.Natural) {
        personNaturalDiv.forEach((item) => {
            item.hidden = false;
        });
        personLegalDiv.style.display = styleHide;
        labelType.hidden = false;
        divNaturalPerson.hidden = false;
        divLegalPerson.hidden = true;

    }
    else if (currentValue == PersonType.Legal) {
        labelType.hidden = false;
        divNaturalPerson.hidden = true;
        divLegalPerson.hidden = false;

        personLegalDiv.style.display = styleShow;
        personNaturalDiv.forEach((item) => {
            item.hidden = true;
            //inputFirstName.value = ".";
            //inputLastName.value = ".";
        });
    }

}


function naturalNames_onChange(e) {
    //Hay dato y tipo de persona es natural
    if (e.target.value && personNaturalRad.checked) {
        //Concatenar los 4 campos

        if (inputFirstName.value) {
            personName = inputFirstName.value.trim();
        }

        if (inputSecondName.value) {
            personName = `${personName.trim()} ${inputSecondName.value.trim()}`;
        }

        if (inputLastName.value) {
            personName = `${personName.trim()} ${inputLastName.value.trim()}`;
        }

        if (inputSecondSurname.value) {
            personName = `${personName.trim()} ${inputSecondSurname.value.trim()}`;
        }

        document.querySelector("#commercialName").value = personName;
        document.querySelector("#businessName").value = personName;
        //console.log(personName);
    }
}
