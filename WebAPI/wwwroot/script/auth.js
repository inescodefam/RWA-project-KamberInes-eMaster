
function logout() {
    localStorage.removeItem("JWT");
    window.location.href = "auth.html";
}

$(document).ready(function () {
    $('#loginForm').submit(function (e) {
        e.preventDefault();

        let loginUrl = "/api/auth/login";
        let loginData = {
            "email": $("#email").val(),
            "password": $("#password").val()
        }
        $.ajax({
            method: "POST",
            url: loginUrl,
            data: JSON.stringify(loginData),
            contentType: 'application/json'
        }).done(function (response) {
            localStorage.setItem('jwt', response.token);
            window.location.href = 'logs.html';
        }).fail(function (err) {
            $('#error').text('Invalid username or password!');
            localStorage.removeItem('jwt');
        });
    });
})