// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function disableSubmitButton() {
    $('body :submit').attr('disabled', 'disabled');
    $('.indicator-label').toggleClass('d-none');
    $('.indicator-progress').toggleClass('d-none');
}

$(document).ready(function () {
    // Disabled Submit Button
    $('form').on('submit', function () {
        if ($('.js-tinymce').length > 0) {
            $('.js-tinymce').each(function () {
                var input = $(this);

                var content = tinyMCE.get(input.attr('id')).getContent();
                input.val(content);
            });
        }
        var isValid = $(this).valid();
        if (isValid) disableSubmitButton();
    });
    // select2
    $('.js-select2').select2({
        theme: 'bootstrap-5',
    });
    $('.js-select2').on('select2:select', function (e) {
        $('form').validate().element('#' + $(this).attr('id'));
    });
    // date using flatpickr
    $(".js-flatpicker").flatpickr({
        enableTime: true,
        maxDate: new Date(),
    });
    // textarea using tinymce
    if ($('.js-tinymce').length > 0) {
        tinymce.init({
            selector: '.js-tinymce',
            height: 400,
            plugins: [
                'advlist', 'autolink', 'lists', 'preview',
                'anchor', 'searchreplace', 'fullscreen', 'table',
            ],
            content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:16px }'
        });
    }
});