window.togglePasswordVisibility = function (elementId) {
    const passwordInput = document.getElementById(elementId);
    if (passwordInput.type === "password") {
        passwordInput.type = "text";
    } else {
        passwordInput.type = "password";
    }
}
