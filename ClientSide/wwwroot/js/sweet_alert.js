$(document).ready(() => {
 

});
function showDeleteConfirmation(guid) {
    Swal.fire({
        title: 'Confirm Delete',
        text: 'Are you sure you want to delete this assignment?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel',
    }).then((result) => {
        if (result.isConfirmed) {
            // If the user confirms deletion, send the delete request to the server
            $.ajax({
                type: 'POST',
                url: `/Assignment/DeepDeleteAssignments?guid=${guid}`,
                dataType: 'json',
                success: function (data) {
                    if (data.code === 200) {
                        Swal.fire({
                            title: 'Success',
                            text: 'Assignment deleted successfully',
                            icon: 'success',
                        }).then(() => {
                            // Optionally, refresh the page or perform other actions after success
                            window.location.reload(); // For example, reload the page
                        });
                    } else if (data.code === 500) {
                        Swal.fire({
                            title: 'Error',
                            text: data.message,
                            icon: 'error',
                        });
                    } else {
                        Swal.fire({
                            title: 'Error',
                            text: 'Unknown error occurred',
                            icon: 'error',
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        title: 'Error',
                        text: 'Failed to delete assignment',
                        icon: 'error',
                    });
                },
            });
        }
    });
}