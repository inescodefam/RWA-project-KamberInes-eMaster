// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getYourServices() {
    return JSON.parse(localStorage.getItem('yourServices') || '[]');
}

function saveYourServices(services) {
    localStorage.setItem('yourServices', JSON.stringify(services));
}

$(document).on('click', '.add-to-your-services', function () {
    var service = {
        id: $(this).data('id'),
        name: $(this).data('name'),
        desc: $(this).data('desc'),
        price: $(this).data('price')
    };
    var services = getYourServices();

    if (!services.some(s => s.id === service.id)) {
        services.push(service);
        saveYourServices(services);
        alert('Service added!');
    } else {
        alert('Service already in your list.');
    }
});

$(document).on('click', '.remove-from-your-services', function () {
    var id = $(this).data('id');
    var services = getYourServices().filter(s => s.id !== id);
    saveYourServices(services);
    renderYourServices();
});
