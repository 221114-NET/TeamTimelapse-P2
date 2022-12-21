function setFormMessage(formElement, type, message){
    const messageElement = formElement.querySelector(".formMessage");

    messageElement.textContent = message;
    messageElement.classList.remove("formSuccessMessage", "formErrorMessage");
    messageElement.classList.add(`formMessage--${type}`);
}
//setFormMessage(loginForm, "success", "You're logged in!")

function setInputError(inputElement, message){
    inputElement.classList.add("formInput--error")
    inputElement.parentElement.querySelector(".inputErrorMessage").textContent = message;
}

function clearInputError(inputElement) {
    inputElement.classList.remove("formInput--error");
    inputElement.parentElement.querySelector(".inputErrorMessage").textContent = message;
}


document.addEventListener("DOMContentLoaded", () => {
    const loginForm = document.querySelector("#login");;
    const createAccontForm = document.querySelector("#createAccount");

    document.querySelector("#linkCreateAccount").addEventListener("click", () => {
        loginForm.classList.add("formHidden");
        createAccontForm.classList.remove("formHidden");
    });
    document.querySelector("#linkLogin").addEventListener("click", () => {
        loginForm.classList.remove("formHidden");
        createAccontForm.classList.add("formHidden");
    });

    loginForm.addEventListener("submit", e => {
        e.preventDefault();

        //Ajax/fetch login
        setFormMessage(loginForm, "error", "Invalid Email/password combination");
    });

    document.querySelectorAll("formInput").forEach(inputElement => {
        inputElement.addEventListener("blur", e => {
            if(e.target.id === "signupPassword" && e.target.value.length > 0 && e.target.value.length < 6) {
                setInputError(inputElement, "Password must be at least 6 charachters in length")
            }
        });

        inputElement.addEventListener("input", e => {
            clearInputError(inputElement);
        });
    });
});