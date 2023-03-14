// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const currentUrl = window.location.href;
const navLinks = document.querySelectorAll('.nav-link');

navLinks.forEach(link => {
    if (link.href === currentUrl) {
        link.classList.add('active'); // добавляем класс для подсветки
    }
});
