$(document).ready(function () {
    var table = $('#tableAdditionals').DataTable({
    });
    table.searchBuilder.container().prependTo(table.table().container());
});