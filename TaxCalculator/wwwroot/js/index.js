$(document).ready(function () {
    // Delete record when the bin icon is clicked
    $(".delete-icon").on("click", function () {
        var id = $(this).data("id"); // Get the record ID from data attribute
        var confirmation = confirm("Are you sure you want to delete this record?");

        if (confirmation) {
            $.ajax({
                url: '/Home/DeleteRate/' + id,
                type: 'POST',
                success: function (result) {
                    // Handle success - reload grid
                    if (result.success) {
                        location.reload();
                    } else {
                        alert('Failed to delete the record.');
                    }
                },
                error: function () {
                    alert('An error occurred while processing the request.');
                }
            });
        }
    });

    $('.numeric-input').keypress(function (event) {
        var charCode = event.which ? event.which : event.keyCode;

        // Allow only digits (0-9) and certain special keys like Backspace, Delete, etc.
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    });
});