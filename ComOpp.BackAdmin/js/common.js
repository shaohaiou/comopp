/****
创建人：wyl 
创建时间：20140711 
修改时间：20140715
******************/
/////效果一 

function winWarn(w, h, t, c) {
    $('body').append('<div id="winDialog"></div>');
    $('#winDialog').dialog({
        title: t,
        width: w,
        height: h,
        content: c,
        modal: true,
        //iconCls:'messager-warning',
        buttons: [{
            text: '立即下载安装',
            handler: function () {
                window.open(Sphone.driver, '_blank')

            }
        }, {
            text: '暂不下载',
            handler: function () {
                $('#winDialog').dialog('close');
            }
        }]
    });
    //$('#dialog').dialog('open');
}
/////弹窗效果
function dialog(w, h, t, url, modal) {
    $('body').append('<div id="dialog"></div>');
    $('#dialog').dialog({
        title: t,
        width: w,
        height: h,
        //closed: true,
        cache: false,
        modal: typeof (modal) == 'undefined' ? true : modal,
        href: url,
        onClose: function () {
            $(this).dialog('destroy');
            $('#dialog').remove();
        }
    });
    slidedown($('#dialog'), 500)
    //$('#dialog').dialog('open');
}

/////快速拨号弹窗//////n是传进来的电话号码，是否自动拨号的关键
function clickphone(n) {
    dialog('800', '385', '快捷拨号', Sphone.phone + (Sphone.phone.indexOf('?') != -1 ? "&" : "?") + "phone=" + (n ? n : ''), false);
}
//////////弹窗向下滑动效果
/////slidedown(对象， 对象的高度， 滑动时间）;
function slidedown(obj, s, offeset) {
    var box = $(obj).parent('.window');
    var boxShaw = $(obj).parent().next('.window-shadow');
    var h = $(obj).height();
    var w = $(obj).width();

    $(box).css('top', -h);
    $(boxShaw).css('top', -h);
    $(box).css('opacity', 0);
    $(boxShaw).css('opacity', 0);

    if (offeset) {
        var t = ($(window).height() - h) / 2 - 60;
        var l = ($(window).width() - w) / 2 + 90;
    } else {
        var t = ($(window).height() - h) / 2;
    }
    setTimeout(function () {
        if (offeset) {
            $(box).css('left', l);
            $(boxShaw).css('left', l);
        }
        $(box).animate(
					{ top: t + 'px', opacity: 1 }, s
				);
        $(boxShaw).animate(
					{ top: t + 'px', opacity: 1 }, s
				);
    }, 100);

}
$(function () {
    //////input按钮
    $("input[type='button']").mouseover(function () {
        $(this).css('background', '#14CEBC')
    });
    $("input[type='button']").mouseout(function () {
        $(this).css('background', '#2BB8AA')
    })
    $("input[type='submit']").mouseover(function () {
        if (!$(this).hasClass("hsSubmit")) {
            $(this).css('background', '#14CEBC')
        }
    });
    $("input[type='submit']").mouseout(function () {
        if (!$(this).hasClass("hsSubmit")) {
            $(this).css('background', '#2BB8AA')
        }
    })
    $("input[type='reset']").mouseover(function () {
        $(this).css('background', '#14CEBC')
    });
    $("input[type='reset']").mouseout(function () {
        $(this).css('background', '#2BB8AA')
    })
})



/****
创建人：zsx 
创建时间：20140714
修改时间：20140714
******************/
$(function () {
    $("#ltree").find("dt").click(function () {
        $("#ltree dt").removeClass("open");
        $(this).addClass("open");
        if (!$(this).attr("data-key")) {
            if ($(this).next("dd").css("display") == 'none') {
                $(this).next("dd").slideDown();
            } else {
                $(this).next("dd").slideUp();
            }
        } else {
            $(".achose").removeClass('achose');
            window.frames["riframe"].location.href = $(this).attr("data-key");
        }
    });
    $("#ltree dd p").find("a").click(function () {
        $("#ltree dt").removeClass("open");
        $(this).parents("dd").prev("dt").addClass("open");

        $(".achose").removeClass("achose");
        $(this).parent("p").addClass("achose");
        try { huashuClose(); } catch (e) { }
    });


    /*表格工具栏宽度变化*/
    $("#toolb").css("width", $("body").width());
    $(window).resize(function () {
        $("#toolb").css("width", $("body").width());
    })
    /*今天 昨天 近7天 时间选择*/
    $(".mylinkBtn").click(function () {
        $(this).siblings("a").removeClass("bechose");
        $("#riqi").hide();
        $(this).addClass("bechose");
        if ($(this).attr("id") == "diySelf") {
            $("#riqi").show();
            return false;
        }
    })
    /*普通搜索 高级搜索切换*/
    $("#diySearch").click(function (event) {
        $("#schlist").stop(true, false);
        if ($(this).hasClass("shouqi")) {
            $("#schlist").slideUp(function () {
                $('#diySearch').removeClass("shouqi").text("搜索");
            });
            return false;
        } else {
            $("#schlist").slideDown(function () {
                $('#diySearch').addClass("shouqi").text("收起");
            });
            return false;
        }
    })

})
/*点击商机管理分类下的综合查询 弹出数据页面*/
function showData(url,titletext,width,height) {
    //dialog('800','400','潜客搜索','html/show_data.html');
    $('body').append('<div id="dialogSearch"></div>');
    $('#dialogSearch').dialog({
        title: titletext,
        width: width,
        height: height,
        cache: false,
        modal: false,
        href: url + "?d=" + new Date(),
        onClose: function () {
            $(this).dialog('destroy');
            $('#dialogSearch').remove();
        }
    });
    slidedown($('#dialogSearch'), 300, true)
}
function player(mp3) {
    alert(1);
}
/*营销工具下的短信查询 客户号码字段下的值被点击则页面跳转到短信所发的客户信息*/
function showMsg() {
    location.href = 'messg_detail.html'; //'');
}	