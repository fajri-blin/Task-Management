    function showDeleteConfirmation(guid) {
        Swal.fire({
            title: 'Confirm Delete',
            text: 'Are you sure you want to delete this assignment?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel',
            dangerMode: true,
        }).then((result) => {
            if (result.isConfirmed) {
                // If the user confirms deletion, send the delete request to the server
                fetch(`/Assignment/DeepDeleteAssignments?guid=${guid}`, {
                    method: 'POST',
                }).then((response) => {
                    if (response.ok) {
                        // The deep delete was successful (HTTP 200 status)
                        Swal.fire(
                            'Deleted!',
                            'The assignment has been deleted.',
                            'success'
                        ).then(() => {
                            // Redirect to the GetAllAssignment page after successful delete
                            window.location.href = '/Assignment/GetAllAssignment';
                        });
                    } else {
                        // The deep delete encountered an error
                        Swal.fire(
                            'Error!',
                            'An error occurred during deletion.',
                            'error'
                        );
                    }
                });
            }
        });
    }