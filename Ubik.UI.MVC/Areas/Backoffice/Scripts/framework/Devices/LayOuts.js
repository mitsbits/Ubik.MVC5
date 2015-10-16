$(function() {
    var sectionstable = $("#sections-table").dataTable({
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bSort": false,
        "bInfo": false,
        "bAutoWidth": true
    });

    //$('#sections-table tbody').on('click', 'tr', function () {
    //    if ($(this).hasClass('btn-info')) {
    //        $(this).removeClass('btn-info');
    //    }
    //    else {
    //        sectionstable.$('tr.btn-info').removeClass('btn-info');
    //        $(this).addClass('btn-info');
    //    }
    //});
});