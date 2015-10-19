﻿$(function () {
    $('input#row-selector').on('ifChanged', function (event) {
        var selected = $(this).prop('checked');
        if (selected) {
            $('input.selector').iCheck('check');
        } else {
            $('input.selector').iCheck('uncheck');
        }
    });

    $('button#btn-delete-selectd').click(function(event) {
        if (!anySelected()) {
            event.preventDefault();
            return false;
        }
        alert(selectdIds());
    });

    function anySelected() {
        return $('input.selector:checked').length > 0;
    }

    function selectdIds() {
        var ids = new Array();
        $('input.selector:checked').each(function(index, element) {
            ids.push($(this).data("id"));
        });
        return ids;
    }
});