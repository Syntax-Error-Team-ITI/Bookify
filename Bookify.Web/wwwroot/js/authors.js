// Wait for the DOM to be fully loaded before running the script
document.addEventListener('DOMContentLoaded', function () {
    $(document).ready(function () {
        $('.table').DataTable();
    });

    // Confirmation for Delete forms
    const deleteForms = document.querySelectorAll('.delete-form');
    deleteForms.forEach(form => {
        form.addEventListener('submit', function (e) {
            e.preventDefault(); // Prevent default form submission
            const currentForm = this; // Store the form element

            bootbox.confirm({
                message: 'Are you sure you want to mark this author as deleted?',
                buttons: {
                    confirm: {
                        label: 'Yes, Delete',
                        className: 'btn-danger'
                    },
                    cancel: {
                        label: 'No, Cancel',
                        className: 'btn-secondary'
                    }
                },
                callback: function (result) {
                    if (result) {
                        currentForm.submit(); // If confirmed, submit the form
                    }
                }
            });
        });
    });

    // Confirmation for Retrieve forms
    const retrieveForms = document.querySelectorAll('.retrieve-form');
    retrieveForms.forEach(form => {
        form.addEventListener('submit', function (e) {
            e.preventDefault(); // Prevent default form submission
            const currentForm = this; // Store the form element

            bootbox.confirm({
                message: 'Are you sure you want to retrieve this author?',
                buttons: {
                    confirm: {
                        label: 'Yes, Retrieve',
                        className: 'btn-success'
                    },
                    cancel: {
                        label: 'No, Cancel',
                        className: 'btn-secondary'
                    }
                },
                callback: function (result) {
                    if (result) {
                        currentForm.submit(); // If confirmed, submit the form
                    }
                }
            });
        });
    });
});