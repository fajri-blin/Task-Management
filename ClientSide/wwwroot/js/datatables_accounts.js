$(document).ready(function () {
    var table = $('#tableAccounts').DataTable({
        searchBuilder: true,
        searchBuilder: {
            columns: [1, 2],
            conditions: {
                string: {
                    'starts': null,
                    '!starts': null,
                    'ends': null,
                    '!ends': null,
                }
            },
        },
        language: {
            searchBuilder: {
                title: {
                    0: 'Advanced Search',
                    _: 'Advanced Search (%d)'
                },
            }
        }
    });
    table.searchBuilder.container().prependTo(table.table().container());
});