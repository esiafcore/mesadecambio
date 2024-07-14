
let containerMain;

document.addEventListener("DOMContentLoaded", async () => {
    containerMain = document.querySelector("#containerMain");
    containerMain.classList = "container-fluid p-0";

    // buscar la etiqueta MAIN y quitar las clases
    const main = containerMain.querySelector("main");
    main.classList = "";
    main.classList = "p-0";
});
