var currentCopies = [];
var selectedCopies = [];
var isEditMode = false;
// The 'maxAllowedCopies' variable will be initialized globally by the Razor view.
// var maxAllowedCopies = 0; // Removed - relying on global from Razor

$(document).ready(function () {
    // REMOVED: maxAllowedCopies = parseInt($('#MaxAllowedCopies').val()) || 0;
    // maxAllowedCopies is now expected to be set by the Razor view's <script> tag.
    // For explicit access (optional): maxAllowedCopies = window.maxAllowedCopies;

    // Initialize existing copies if in edit mode
    if ($('.js-copy').length > 0) {
        prepareInputs();
        currentCopies = [...selectedCopies]; // Create a copy of the array
        isEditMode = true;
        console.log('Edit mode detected. isEditMode =', isEditMode); // For debugging
    } else {
        console.log('Add mode detected. isEditMode =', isEditMode); // For debugging
    }

    // Handle search button click
    $('.js-search').on('click', function (e) {
        e.preventDefault();
        handleSearch();
    });

    // Remove/re-add handlers using event delegation
    $(document)
        .on('click', '.js-remove', handleRemoveCopy)
        .on('click', '.js-readd', handleReaddCopy);
});

function handleSearch() {
    var serial = $('#SearchValue').val().trim();

    // Client-side validation
    if (!serial) {
        showErrorMessage('Please enter a serial number');
        return;
    }

    if (selectedCopies.some(c => c.serial === serial)) {
        showErrorMessage('This copy is already added');
        return;
    }

    if (selectedCopies.length >= maxAllowedCopies) {
        showErrorMessage(`You cannot add more than ${maxAllowedCopies} book(s)`);
        return;
    }

    // Submit the form via AJAX
    $.ajax({
        url: "/Rentals/GetCopyDetails",
        type: "POST",
        data: $('#SearchForm').serialize(), // FIX: Correctly serialize the form
        beforeSend: onSearchBegin, // Manually trigger begin function
        success: function (result) {
            // REMOVE THESE LINES as onSearchComplete handles them:
            // indicator.removeClass("d-none");
            // $(".js-search").removeProp("disable");
            console.log(selectedCopies);
            onAddCopySuccess(result); // Manually call onAddCopySuccess
        },
        error: function (xhr, status, error) {
            console.error("Error loading table:", error);
            showErrorMessage('Error searching for copy. Please try again.'); // Provide user feedback
        },
        complete: onSearchComplete // Manually trigger complete function
    });
}

function onSearchBegin() {
    $('.js-search').prop('disabled', true);
    $('.indicator-progress').removeClass('d-none');
}

function onSearchComplete() {
    $('.js-search').prop('disabled', false);
    $('.indicator-progress').addClass('d-none');
}

function onAddCopySuccess(response) {
    // Clear the search field
    $('#SearchValue').val('');

    // Extract book ID from the response HTML
    var $response = $(response);
    var bookId = $response.find('.js-copy').data('book-id');

    // Check for duplicate books
    if (selectedCopies.some(c => c.bookId === bookId)) {
        showErrorMessage('You already have a copy of this book');
        return;
    }

    // Add the new copy to the form
    $('#CopiesForm').prepend(response);

    // Update the selected copies array
    prepareInputs();

    // Show the submit button
    $('#CopiesForm').find(':submit').removeClass('d-none');
}

function handleRemoveCopy() {
    var btn = $(this);
    var container = btn.closest('.js-copy-container');

    // Add a safety check for the container
    if (!container.length) {
        console.error("Error: '.js-copy-container' not found for the clicked button.");
        return;
    }

    if (isEditMode) {
        // In edit mode, we mark as removed but don't delete from DOM
        btn.toggleClass('btn-light-danger btn-light-success js-remove js-readd')
            .text('Re-Add');
        container.find('img').css('opacity', '0.5');
        container.find('h4').css('text-decoration', 'line-through');
        container.find('.js-copy') // Target the hidden input
            .toggleClass('js-copy js-removed')
            .removeAttr('name')
            .removeAttr('id');
    } else {
        // In add mode, completely remove the element from DOM
        container.remove();
    }

    updateFormState();
}

function handleReaddCopy() {
    var btn = $(this);
    var container = btn.closest('.js-copy-container');

    // Add a safety check for the container
    if (!container.length) {
        console.error("Error: '.js-copy-container' not found for the clicked button.");
        return;
    }

    btn.toggleClass('btn-light-danger btn-light-success js-remove js-readd')
        .text('Remove');
    container.find('img').css('opacity', '1');
    container.find('h4').css('text-decoration', 'none');
    container.find('.js-removed') // Target the hidden input
        .toggleClass('js-copy js-removed');

    updateFormState();
}

function prepareInputs() {
    selectedCopies = [];
    $('.js-copy').each(function (index) {
        var $input = $(this);
        selectedCopies.push({
            serial: $input.val(),
            bookId: $input.data('book-id')
        });
        $input.attr('name', `SelectedCopies[${index}]`)
            .attr('id', `SelectedCopies_${index}_`);
    });
}

function updateFormState() {
    prepareInputs();

    // In edit mode, compare with original copies
    if (isEditMode) {
        var hasChanges = !arraysEqual(selectedCopies, currentCopies);
        $('#CopiesForm').find(':submit').toggleClass('d-none', !hasChanges);
    }
    // In add mode, just check if we have any copies
    else {
        $('#CopiesForm').find(':submit').toggleClass('d-none', selectedCopies.length === 0);
    }
}

function arraysEqual(a, b) {
    if (a === b) return true;
    if (a == null || b == null) return false;
    if (a.length !== b.length) return false;

    for (var i = 0; i < a.length; ++i) {
        if (a[i].serial !== b[i].serial || a[i].bookId !== b[i].bookId) {
            return false;
        }
    }
    return true;
}

function showErrorMessage(message) {
    console.log(message)
    // Use your preferred error display method
    // Swal.fire({
    //     icon: 'error',
    //     title: 'Error',
    //     text: message,
    //     timer: 3000
    // });
}