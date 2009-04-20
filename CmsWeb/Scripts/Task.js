﻿$(function() {
    SearchClicked = RefreshList;
    SelectTab();
    StripeList();
    var c = $.cookie("tasklast");
    $('#tasks > thead a.sortable').click(function(ev) {
        $("#tasks #Sort").val($(this).text());
        RefreshList();
    });
});
function RefreshList() {
    var q = $('#form').formSerialize2();
    $.navigate("/Task/List",q);
}
function GotoPage(pg) {
    var q = $('#form').formSerialize2();
    q = q.appendQuery("Page=" + pg);
    $.navigate("/Task/List", q);
}
function SetPageSize(sz) {
    var q = $('#form').formSerialize2();
    q = q.appendQuery("PageSize=" + sz);
    $.navigate("/Task/List", q);
}
function SelectTab(tab) {
    var tselected = "ui-tabs-selected";
    $('#tabs li').removeClass(tselected);
    if (tab)
        $('#CurTab').val(tab);
    $('#' + $("#CurTab").val()).addClass(tselected);
}
function ClickTab(tab) {
    SelectTab(tab);
    $.cookie('CurTaskTab', tab, { expires: 360 });
    RefreshList();
    return false;
}
function StripeList() {
    $('#tasks > tbody tr:even').addClass('alt');
}
function DeleteList(qs) {
    $.post('/Task/Action/', qs, function(ret) {
        var a = ret.split("<---------->");
        $('#tabs').html(a[0]);
        $("#actions").html(a[1]);
        $('#tasks > tbody').html(a[2]).ready(StripeList);
    });
}
function DoAction() {
    var v = $('#actions option:selected').val();
    var ai = $(".actionitem:checked").getCheckboxVal().join(",");
    var qs = "option=" + v + "&curtab=" + $('#CurTab').val() + "&items=" + ai;
    $('#actions').attr('selectedIndex', 0);
    if (ai = "")
        return;
    switch (v) {
        case '':
        case '-':
            return;
        case 'delegate':
        case 'sharelist':
            alert('not implemented yet');
            return;
        case 'deletelist':
            if (confirm('Are you sure you want to delete the list?')) {
                $.block();
                DeleteList(qs);
                $.unblock();
            }
            return;
        default:
            $.block();
            $.post('/Task/Action/', qs, function(ret) {
                $('#tasks > tbody').html(ret).ready(StripeList);
                $.unblock();
            });
            return;
    }
}
function AddListEnter(e) {
    var key = window.event ? e.keyCode : e.which;
    if (key != 13)
        return true;
    AddListClick()
    return false;
}
function AddListClick() {
    var qs = "ListName=" + $("#ListName").val();
    $.post('/Task/AddList/', qs, function(ret) {
        var a = ret.split("<---------->");
        $('#tabs').html(a[0]);
        $("#actions").html(a[1]);
        $('#ListName').val('');
    });
}
function AddTaskEnter(e) {
    var key = window.event ? e.keyCode : e.which;
    if (key != 13)
        return true;
    AddTaskClick();
    return false;
}
function AddTaskClick() {
    var qs = "TaskDesc=" + $("#TaskDesc").val() + "&CurTab=" + $("#CurTab").val();
    $.post('/Task/AddTask/', qs, function(ret) {
        $("#nomatch").remove();
        var alt = !($('#tasks > tbody tr:visible:first').hasClass("alt") || false);
        $('#tasks > tbody tr:first').before(ret);
        if (alt)
            $('#tasks > tbody tr:first').addClass("alt");
        $('#TaskDesc').val('');
    });
}
function ShowDetail(id) {
    var drid = $("#TaskId").val();
    if (drid) {
        $.post("/Task/Detail/" + id + "/Row/" + drid, function(ret) {
            var a = ret.split("<---------->");
            $('#r' + drid).html(a[0]);
            $('#r' + drid).removeClass("detailrow");
            $('#r' + id).addClass("detailrow").html(a[1]);
        });
    }
    else {
        $.post('/Task/Detail/' + id, function(ret) {
            $('#r' + id).addClass("detailrow");
            $('#r' + id).html(ret);
        });
    }
}
function Deselect() {
    var id = $("#TaskId").val();
    $.post('/Task/Columns/' + id, function(ret) {
        $('#r' + id).removeClass("detailrow").html(ret);
    });
}
function SetPriority(id, priority) {
    $.getJSON('/Task/Priority/'+id+'?priority='+priority, null, function(ret) {
        $('#Priority').text(ret.Priority);
    });
}
function SetComplete(id) {
    $.post('/Task/SetComplete/' + id, null, function(ret) {
        $('#r' + id).removeClass('detailrow').html(ret);
    });
}
function Accept(id) {
    $.post('/Task/Accept/'+id, null, function(ret) {
        $('#r' + id).html(ret);
    });
}
$(function() {
    $("#dialogbox").dialog({
        overlay: { background: "#000", opacity: 0.8 },
        bgiframe: true,
        modal: true,
        autoOpen: false,
        closeOnEscape: true,
        width: 600,
        height: 525,
        close: function(event, ui) {
            $('#dialogbox').empty();
            SearchClicked = RefreshList;
        }
    });
});

var queryString = "";
function ChangePage() { }
function SearchClicked() { }
function SelectPerson(id) { }

function SearchContacts() {
    SearchClicked = SearchContactClicked;
    ChangePage = ChangeContactPage;
    $('#dialogbox').dialog("option", "title", "Select Contact");
    $('#dialogbox').load("/Task/SearchContact/", null, function() {
        queryString = $('#searchform').formSerialize2();
        $(".datepicker").datepicker();
        $("#contacts").initPager();
        $('#contacts > thead a.sortable').click(function(ev) {
            $("#contacts #Sort").val($(ev.target).text());
            queryString = $('#searchform').formSerialize2();
            $.post('/Task/SearchContact/0', queryString, function(ret) {
                $('#contacts > tbody').html(ret).ready(function() { $("#contacts").initPager(); });
            });
            return false;
        });
    });
    $('#dialogbox').dialog("open");
}
function SearchPeople(SelectFunc) {
    SelectPerson = SelectFunc;
    SearchClicked = SearchPeopleClicked;
    ChangePage = ChangePeoplePage;
    $('#dialogbox').dialog("option", "title", "Select Person");
    $('#dialogbox').load("/Task/SearchPeople/", null, function() {
        queryString = $('#searchform').formSerialize2();
        $("#people").initPager();
        $('#people > thead a.sortable').click(function(ev) {
            $("#people #Sort").val($(ev.target).text());
            queryString = $('#searchform').formSerialize2();
            $.post('/Task/SearchPeople/0', queryString, function(ret) {
                $('#people > tbody').html(ret).ready(function() { $("#people").initPager(); });
            });
            return false;
        });
    });
    $('#dialogbox').dialog("open");
}
function AddSourceContact(contactid) {
    var taskid = $('#TaskId').val();
    $.post('/Task/AddSourceContact/' + taskid + "?contactid=" + contactid, null, function(ret) {
        $('#r' + taskid).html(ret);
    });
    $('#dialogbox').dialog("close");
}
function CompleteWithContact() {
    var taskid = $('#TaskId').val();
    $.post('/Task/CompleteWithContact/' + taskid, null, function(ret) {
        window.location = "/Contact.aspx?edit=1&id=" + ret.ContactId;
    }, "json");
}
function ActOnPerson(action, peopleid) {
    var taskid = $('#TaskId').val();
    $.post(action + taskid + "?peopleid=" + peopleid, null, function(ret) {
        $('#r' + taskid).html(ret);
    });
    $('#dialogbox').dialog("close");
}
function ChangeOwnerPerson(peopleid) {
    ActOnPerson('/Task/ChangeOwner/', peopleid);
}
function AddDelegatePerson(peopleid) {
    ActOnPerson('/Task/Delegate/', peopleid);
}
function AddAboutPerson(peopleid) {
    ActOnPerson('/Task/ChangeAbout/', peopleid);
}
function ChangeContactPage(page, pager) {
    $.post('/Task/SearchContact/' + page, queryString, function(ret) {
        $('#contacts > tbody').html(ret);
    });
    return false;
}
function ChangePeoplePage(page, pager) {
    $.post('/Task/SearchPeople/' + page, queryString, function(ret) {
        $('#people > tbody').html(ret);
    });
    return false;
}
$.fn.initPager = function() {
    this.each(function() {
        $(".pagination", this).pagination($("#Count", this).val(), {
            items_per_page: $("#PageSize", this).val(),
            num_display_entries: 5,
            num_edge_entries: 1,
            current_page: 0,
            callback: ChangePage
        });
        $('#NumItems', this).text($('#Count', this).val().addCommas() + " items");
    });
    return this;
};
function SearchContactClicked() {
    queryString = $('#searchform').formSerialize2();
    $.post('/Task/SearchContact/0', queryString, function(ret) {
        $('#contacts > tbody').html(ret).ready(function() { $("#contacts").initPager(); });
    });
    return false;
}
function SearchPeopleClicked() {
    queryString = $('#searchform').formSerialize2();
    $.post('/Task/SearchPeople/0', queryString, function(ret) {
        $('#people > tbody').html(ret).ready(function() { $("#people").initPager(); });
    });
    return false;
}
function Edit() {
    var id = $("#TaskId").val();
    $.post('/Task/Edit/' + id, function(ret) {
        $('#r' + id).html(ret);
        $(".datepicker").datepicker();
    });
} 
function Update() {
    var id = $("#TaskId").val();
    var qs = $("#Edit").formSerialize2();
    $.post('/Task/Update/' + id, qs, function(ret) {
        $('#r' + id).html(ret);
    }, "html");
}
