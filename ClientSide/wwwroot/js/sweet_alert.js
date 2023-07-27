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
                url: `/Assignment/DeepDeleteAssignments?guid=${guid}`,
                type: 'POST',
                success: function (data) {
                    Swal.fire({
                        title: 'Success',
                        text: 'Assignment deleted successfully',
                        icon: 'success',
                    }).then(() => {
                        // Optionally, refresh the page or perform other actions after success
                        window.location.reload(); // For example, reload the page
                    });
                },
                error: function (xhr, status, error) {
                    console.log(xhr.responseText);
                    Swal.fire({
                        title: 'Error',
                        text: 'Failed to delete assignment',
                        icon: 'error',
                    });
                }
            });
        }
    });
}
