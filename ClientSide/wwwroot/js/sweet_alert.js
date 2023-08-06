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

function showDeleteAccount(guid) {
    Swal.fire({
        title: 'Confirm Delete',
        text: 'Are you sure you want to delete this account?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel',
    }).then((result) => {
        if (result.isConfirmed) {
            // If the user confirms deletion, send the delete request to the server
            $.ajax({
                type: 'POST',
                url: `/Account/Delete?guid=${guid}`,
                dataType: 'json',
                success: function (data) {
                    if (data.code === 200) {
                        Swal.fire({
                            title: 'Success',
                            text: 'Account deleted successfully',
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
                        text: 'Failed to delete account',
                        icon: 'error',
                    });
                },
            });
        }
    });
}

function showActivateAccount(guid) {
    Swal.fire({
        title: 'Confirm Activate',
        text: 'Are you sure you want to activate this account?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, activate it!',
        cancelButtonText: 'Cancel',
    }).then((result) => {
        if (result.isConfirmed) {
            // If the user confirms deletion, send the delete request to the server
            $.ajax({
                type: 'POST',
                url: `/Account/Activate?guid=${guid}`,
                dataType: 'json',
                success: function (data) {
                    if (data.code === 200) {
                        Swal.fire({
                            title: 'Success',
                            text: 'Account activated successfully',
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
                        text: 'Failed to activate account',
                        icon: 'error',
                    });
                },
            });
        }
    });
}

function showDeleteAdditional(guid) {
    Swal.fire({
        title: 'Confirm Delete',
        text: 'Are you sure you want to delete this additional?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel',
    }).then((result) => {
        if (result.isConfirmed) {
            // If the user confirms deletion, send the delete request to the server
            $.ajax({
                type: 'POST',
                url: `/Additional/Delete?guid=${guid}`,
                dataType: 'json',
                success: function (data) {
                    if (data.code === 200) {
                        Swal.fire({
                            title: 'Success',
                            text: 'Additional deleted successfully',
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
                        text: 'Failed to delete additional',
                        icon: 'error',
                    });
                },
            });
        }
    });
}