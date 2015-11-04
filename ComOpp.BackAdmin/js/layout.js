/****
创建人：wyl 
创建时间：20140711 
修改时间：20140716
******************/

$(function () {
    $(window).resize(function () {
        west_top();
        center_autoH();
        center_northW();

    })
    west_top();
    center_autoH();
})
function center_autoH() {
    setTimeout(function () {
        $('.layout_box').each(function () {
            var h = $(this).parent().height();
            $(this).height(h);
        });
    }, 200);
}

function westTop() {
    west_top();

    center_northW_close();

    $('.layout-expand-west .panel-body').css('display', 'none');

}

function west_top() {
    var top = (($(window).height() - 36)) / 2 - 31;

    $('.west_btn').css('top', top);
    $('.layout-expand-west .panel-header').css('top', top);
    $('.layout-expand-west .panel-header').css('display', 'block');
}
function center_northW_close() {
    $('.layout-panel-center').css('left', '0px').css('width', '100%');
    $('.layout-panel-center .layout-body').css('width', '100%');
}
function center_northW() {
    var w = $(window).width();

    ////判断左则栏是否展开
    var leftDis = $('.layout-split-west .panel-body').css('display');
    setTimeout(function () {
        if (leftDis == 'none') {/////假如是折叠回来的,则center扩展宽度
            //center_northW_close();
            $('.layout-panel-center').css('left', '0px').css('width', w);
            $('.layout-panel-center .layout-body').css('width', w);
        }
    }, 200);
}
