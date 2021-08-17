
function validatePassword(e) {

    var regularExpression = /^[0-9]/;
    var inputValue = e.target.value;
    var parentNode = e.target.parentNode;

    if (inputValue) {

        if (!regularExpression.test(inputValue)) {
            e.srcElement.classList.add('is-invalid');
            if (parentNode.querySelector('.invalid-feedback') === null)
                parentNode.appendChild(CreateValidationMessageNode('La contraseña no cumple con los requisitos minimos'));

        } else {
            e.srcElement.classList.remove('is-invalid');
        }
    }
}

function validateEmail(e) {

    var regularExpression = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    var inputValue = e.target.value;
    var parentNode = e.target.parentNode;

    if (inputValue) {

        if (!regularExpression.test(inputValue)) {
            e.srcElement.classList.add('is-invalid');

            if (parentNode.querySelector('.invalid-feedback') === null)
                parentNode.appendChild(CreateValidationMessageNode('Ingrese un correo válido'));

        } else {
            e.srcElement.classList.remove('is-invalid');
        }
    }
}

function CreateValidationMessageNode(message) {

    var node = document.createElement('span');
    node.classList.add('error');
    node.classList.add('invalid-feedback');
    node.innerText = message;

    return node;

}