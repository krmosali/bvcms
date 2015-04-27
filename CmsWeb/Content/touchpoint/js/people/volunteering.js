﻿$(function () {

    var getSubmitDialog = function() {
        return $('#dialogHolder');
    };

    $('.showSubmitDialog').click(function (ev) {
        ev.preventDefault();
        var id = $(this).attr('data-cid');
        var submitDialog = getSubmitDialog();
        $.post('/Volunteering/DialogSubmit/' + id, null, function (data) {
            $('.modal-content', submitDialog).html(data);
            submitDialog.modal();
        });
    });

    $('.showCreateDialog').click(function (ev) {
        ev.preventDefault();
        var id = $(this).attr('data-pid');
        var type = $(this).attr('data-ctype');
        var submitDialog = getSubmitDialog();
        $.post('/Volunteering/DialogType/' + id + '?type=' + type, null, function (data) {
            $('.modal-content', submitDialog).html(data);
            $('.modal-title', submitDialog).text('Select Check Type');
            submitDialog.modal();
        });
    });

    $('.showEditDialog').click(function (ev) {
        ev.preventDefault();
        var id = $(this).attr('data-cid');
        var submitDialog = getSubmitDialog();
        $.post('/Volunteering/DialogEdit/' + id, null, function (data) {
            $('.modal-content', submitDialog).html(data);
            submitDialog.modal();
        });
    });

    $('.showDeleteDialog').click(function (ev) {
        ev.preventDefault();
        var id = $(this).attr('data-cid');

        swal({
            title: 'Delete Check',
            type: 'warning',
            showCancelButton: true,
            confirmButtonClass: 'btn-danger',
            confirmButtonText: 'Yes, delete it!',
            closeOnConfirm: false
        },
        function() {
            $.post('/Volunteering/DeleteCheck/' + id, null, function(ret) {
                if (ret && ret.error)
                    swal('Error!', ret.error, 'error');
                else {
                    swal({
                        title: 'Deleted!',
                        type: 'success'
                    },
                    function () {
                        document.location.reload();
                    });
                }
            });
        });
    });

    $('a.editable').editable({
        mode: 'inline',
        type: 'text',
        url: '/Volunteering/EditForm/'
    });
});
