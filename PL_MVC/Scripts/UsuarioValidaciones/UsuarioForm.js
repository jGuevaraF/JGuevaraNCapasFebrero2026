function SoloLetras(input, event) {
    let res = /^[a-zA-Z]+$/.test(event.key);

    let padreDiv = $(input).closest('div');
    let span = $(padreDiv[0]).children('span')
    console.log(padreDiv)
    console.log(span)
    if (res) {
        $(input).css('border-color', 'green');
        $(input).css('box-shadow', '1px 1px 5px green');
        $(span).text('Es un dato correcto')
        $(span).css('color', 'green')
        //$(input).addClass("inptCorrecto");

        $(input).removeClass('inptError').addClass('intpCorrecto');



    } else {
        event.preventDefault();
        $(input).css('border-color', 'red');
        $(input).css('box-shadow', '1px 1px 5px red');
        $(span).text('Solo acepto letras!!!')
        $(span).css('color', 'red')
        $(input).removeClass('intpCorrecto').addClass('inptError');

    }

}

function ValidarEnvio(event) {
    //event => onSubmit

    let inputs = $('form input')

    let validar = ValidarInputs(inputs);

    if (!validar) {
        event.preventDefault(); //evitando que el formulario se envie
        alert("Tienes errores, revisalo")
    }


    console.log(inputs)
}

function ValidarInputs(inputs) {

    let validar = true;

    $.each(inputs, function (i, valor) {
        if ($(valor).hasClass('inptError')) {
            //hay errores
            validar = false;
        } 

    }
    )

    return validar;
}
