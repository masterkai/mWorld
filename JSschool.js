var org_no;
var org_desc;
$(function () {
    getAddr();

    $('#BUT2').on('click', function () {
        $('#BUT2').hide();
        var JasonData2 = {
            "name": $('#s_name').val(),
            "stdno": $('#s_stdno').val(),
            "mobile": $('#s_mobile').val(),
            "school": $('#s_rQ1 :selected').val(),
            "numbers": $('#s_no').val(),
            "func": "sdeanntodaw"
        };

        if ($("input[name=per_chk1]:checked").length == 0) { alert('請勾選同意個資保護聲明。'); $('#BUT2').show(); return false; }
        if ($.trim($('#s_name').val()) == "" || $('#s_name').val() == "學員姓名") { alert('請填寫入姓名。'); $('#BUT2').show(); return false; }
        if ($.trim($('#s_stdno').val()) == "" || $('#s_stdno').val() == "學員編號") { alert('請填寫學員編號。'); $('#BUT2').show(); return false; }
        if (($('#s_stdno').val()).length > 16) { alert('請填寫學員編號。'); $('#BUT2').show(); return false; }
        if ($.trim($('#s_no').val()) == "" || $('#s_no').val() == "抽獎序號") { alert('請填寫抽獎序號。'); $('#BUT2').show(); return false; }

        if ($.trim($('#s_mobile').val()) == "" || $('#s_mobile').val() == "行動電話") { alert('請填寫行動電話。'); $('#BUT2').show(); return false; }
        if (isNaN($('#s_mobile').val()) == true) { alert('行動電話格式錯誤。'); $('#BUT2').show(); return false; }
        if (($('#s_mobile').val()).length != 10) { alert('行動電話格式錯誤。'); $('#BUT2').show(); return false; }

        if ($('#s_rQ1 :selected').val() == "") { alert('請選擇分校。'); $('#BUT2').show(); return false; }
        goSumbit_2(JasonData2, s2, f2);
    });
});

function getAddr() {
    var JSonData = {
        "func": "dgaetaaraa"
    }
    $.ajax({
        type: "post",
        url: "Default.aspx",
        data: JSonData,
        dataType: 'jsonp',             // xml/json/script/html
        cache: false,                   // 是否允許快取
        success: function (response) {
            org_no = response[0].org_no.split(';');
            org_desc = response[0].org_desc.split(';');
            $('#s_rQ1').html("");
            $('#s_rQ1').append('<option value="">上課分校</option>');
            for (i = 0; i < org_no.length; i++) {
                var valorg = org_no[i].split(',');
                var textorg = org_desc[i].split(',');
                $('#s_rQ1').append('<option value="' + valorg[0] + '">' + textorg[0] + '</option>');
            }

        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        },
        beforeSend: function (xhr) {     // 設定 RequestHeader
            xhr.setRequestHeader("Accept", "text/javascript");
        }
    });
}

function goSumbit_2(JSonData, func1, func2) {
    $.ajax({
        type: "post",
        url: "Default.aspx",
        data: JSonData,
        dataType: 'jsonp',             // xml/json/script/html
        cache: false,                   // 是否允許快取
        success: function (response) {
            func1(response);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            func2(thrownError);
        },
        beforeSend: function (xhr) {     // 設定 RequestHeader
            xhr.setRequestHeader("Accept", "text/javascript");
        }
    });
}

function s2(data) {
    if (data[0].result == "1") {
        $("#BUT2").show();
        alert('抽獎資料已送出，謝謝您的參加，祝您幸運中獎！');
        location.reload();
    } else {
        alert(data[0].errMsg);
        $("#BUT2").show();
    }
}

function f2(data) {
    alert(data);
    $("#sBut").show();
}  