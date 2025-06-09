// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // select2
    $(".js-select2").select2({
        theme: 'bootstrap-5',
    });
    // date using flatpickr
    $(".js-flatpicker").flatpickr({
        enableTime: true,
    });
    // textarea using tinymce
    tinymce.init({
        selector: '.js-tinymce',
        height: 400,
        plugins: [
            'advlist', 'autolink', 'lists', 'preview',
            'anchor', 'searchreplace', 'fullscreen', 'table',
        ],
        content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:16px }'
    });
});