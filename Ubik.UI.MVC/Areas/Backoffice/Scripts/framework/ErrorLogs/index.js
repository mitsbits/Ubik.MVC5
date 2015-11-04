$(function () {
    $('input#row-selector').on('ifChanged', function (event) {
        var selected = $(this).prop('checked');
        var row = $(this).closest('tr');

        if (selected) {
            $('input.selector').iCheck('check');
        } else {
            $('input.selector').iCheck('uncheck');
        }
    });

    $('input.selector').on('ifChanged', function(event) {
        var selected = $(this).prop('checked');
        var row = $(this).closest('tr');

        if (selected) {
            $(row).addClass("active");
        } else {
            (row).removeClass("active");
        }
    });

    $('button#btn-delete-selectd').click(function (event) {
        if (!anySelected()) {
            event.preventDefault();
            return false;
        }
        var redirectUrl = window.location.href;
        var url = "/api/backoffice/errorlogs/clearlogs/";
        $.post(url, { '': selectdIds() })
            .done(function (data) {
                window.location.href = redirectUrl;
            });
    });

    function anySelected() {
        return $('input.selector:checked').length > 0;
    }

    function selectdIds() {
        var ids = new Array();
        $('input.selector:checked').each(function (index, element) {
            ids.push($(this).data("id"));
        });
        return ids;
    }

    $('#deletelogs-range').daterangepicker(
        {
            format: 'DD/MM/YYYY',
            showDropdowns: true
            //startDate: moment().subtract('days', 29),
            //endDate: moment()
        },
        function (start, end) {
            $('input#RangeStart').val(start.format());
            $('input#RangeEnd').val(end.format());
        });
    $('#deletelogs-range').on('cancel.daterangepicker', function (ev, picker) {
        $('input#rangeStart').val('');
        $('input#rangeEnd').val('');
        $('#deletelogs-range').val('');
    });
});