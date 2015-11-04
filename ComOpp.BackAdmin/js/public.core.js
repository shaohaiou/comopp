// JavaScript Document
var Sys = {};
var ua = navigator.userAgent.toLowerCase();
var s;
(s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
(s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
(s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
(s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
(s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;
if (Sys.ie) document.documentElement.addBehavior("#default#userdata");

var Core = {
    of: function (o) { return typeof (o); },
    submit: function (o) {
        if (typeof (o) != 'object') o = $(":submit");
        o.attr('disabled', 'disabled'); return true;
    },
    Easyui: {
        box: { "state": false, 'hidden': [], 'resize': true, 'fixed': false }, W: 0, H: 0, T: 0, columns: [], link: '',
        Init: function (o, h, columns, conf) {
            Core.Easyui.T = h || 0;
            Core.Easyui.o = $(o);
            if (!Core.rule.isNumber('p.integer', Core.Easyui.W)) Core.Easyui.W = $("body").width();
            if (!Core.rule.isNumber('p.integer', Core.Easyui.H)) Core.Easyui.H = $("body", window.parent.document).height();
            //storage
            var columnskey = Core.Easyui.o.attr('data-columnskey');
            if (Core.rule.iskey(columnskey) && Core.loadUserdata(columnskey + ':hidden')) {
                Core.Easyui.box['hidden'] = Core.loadUserdata(columnskey + ':hidden').split(',');
            }
            Core.Easyui.columns(columns);
            try {
                Core.Easyui.columns(conf['frozenColumns'], 'frozen');
            } catch (e) { }
            Core.Easyui.config(conf);
        },
        columns: function (columns, method) {
            switch (method) {
                case 'frozen':
                    Core.Easyui.box['frozenColumns'] = [];
                    try {
                        if (typeof (columns) == 'undefined' || typeof (columns[0]) == 'undefined') return;
                    } catch (e) { return; }
                    Core.Easyui.box['frozenColumns'] = columns[0];
                    for (key in Core.Easyui.box['frozenColumns']) {
                        if ($.isArray(Core.Easyui.box['hidden']) && $.inArray(Core.Easyui.box['frozenColumns'][key]['field'], Core.Easyui.box['hidden']) != -1) {
                            Core.Easyui.box['frozenColumns'][key]['hidden'] = true;
                        }
                    }
                    break; default:
                    Core.Easyui.box['columns'] = typeof (columns) != 'undefined' ? columns : [];
                    if (!Core.Easyui.box['columns'].length) return; //hidden
                    //本地存储
                    var w = Core.Easyui.W; var box = [];
                    for (var i = 0; i < Core.Easyui.box['columns'].length; i++) {
                        if (Core.rule.isNumber('p.integer', Core.Easyui.box['columns'][i]['width'])) {
                            w -= Core.Easyui.box['columns'][i]['width'];
                        } else {
                            box.push(i);
                        }
                        if ($.isArray(Core.Easyui.box['hidden']) && $.inArray(Core.Easyui.box['columns'][i]['field'], Core.Easyui.box['hidden']) != -1) {
                            Core.Easyui.box['columns'][i]['hidden'] = true;
                        }
                    }
                    if (!box.length) return;
                    for (var i = 0; i < box.length; i++) {
                        Core.Easyui.box['columns'][box[i]]['width'] = w / box.length;
                    }
                    break;
            }
        },
        config: function (conf) {
            Core.Easyui.box['config'] = {
                method: 'get', //数据方式
                width: Core.Easyui.W, //宽度
                height: Core.Easyui.H - Core.Easyui.T, //高度
                striped: 'true', //条纹行
                fitColumns: true, //宽度自适横向滚动条
                singleSelect: false, //选中单行
                rownumbers: true,
                collapsible: true,
                autoRowHeight: true, //自动行高
                pagination: true, //是否分页
                remoteSort: true, //是否允许排序等待,false本页排序,true后台排序
                multiSort: false, //多个排序
                //toolbar      :'#Toolb', //出现工具栏
                pageSize: 100, //初始化页内行数
                pageList: [50, 100, 200],
                loadMsg: '数据加载中，请稍候...',
                checkOnSelect: true,
                columns: [Core.Easyui.box['columns']]
            };
            Core.Easyui.box['config'] = typeof (conf) != 'undefined' ? Core.Easyui.Params(conf, Core.Easyui.box['config']) : Core.Easyui.box['config'];
        },
        sortStyle: function () {
            if (typeof (Core.Easyui.box['config'].onLoadSuccess) == 'undefined' && Core.Easyui.box['columns'] != 'undefined') {
                Core.Easyui.box['config'].onLoadSuccess = function () {
                    var Style = [];
                    for (key in Core.Easyui.box['columns']) {
                        if (Core.Easyui.box['columns'][key]['sortable'] != true) continue;
                        Style.push(Core.Easyui.box['columns'][key]['field']);
                    }
                    if (!Style.length) return;

                    Core.Easyui.o.datagrid('getPanel').find('div.datagrid-header tr td').each(function () {
                        if ($.inArray($(this).attr("field"), Style) != -1) {
                            $(this).find("span").eq(1).addClass("lic");
                        }
                    });
                }
            }
        },
        Params: function (conf, params) {
            for (key in params) {
                conf[key] = (typeof (conf[key]) != 'undefined' ? conf[key] : params[key]);
            }
            return conf;
        },
        load: function (params) {
            Core.Easyui.o.datagrid('load', params);
        },
        reload: function () {
            if (!Core.Easyui.box['state']) return;
            Core.Easyui.o.datagrid('reload');
        },
        get: function (url) {
            Core.Easyui.sortStyle();
            Core.Easyui.box['config']['url'] = Core.Easyui.link = url
            Core.Easyui.box['state'] = true; //已启用
            Core.Easyui.o.datagrid(Core.Easyui.box['config']); //*/
        },
        resize: function (o, w, h) {
            $(window).resize(function () {
                if (!Core.Easyui.box['state'] || !Core.Easyui.box['resize']) return;
                var w = w || (!Core.Easyui.box['fixed'] ? $("body").width() : Core.Easyui.W);
                try {
                    Core.Easyui.o.datagrid('resize', {
                        width: w >= Core.Easyui.W ? w : Core.Easyui.W, //宽度
                        height: (h || $("body", window.parent.document).height()) - Core.Easyui.T //高度					  
                    })
                } catch (e) { }
            })
        },
        diyColumns: {
            show: function (o) {
                if (!Core.Easyui.box['state'] || !$(o).length) return;
                Core.Easyui.box['showdiy'] = $(o);

                var w = '';
                for (key in Core.Easyui.box['frozenColumns']) {
                    if ($.inArray(Core.Easyui.box['frozenColumns'][key].field, ['ck', '_expander']) != -1 || !Core.Easyui.box['frozenColumns'][key].title) continue;
                    w += '<P><input id="_pg_' + Core.Easyui.box['frozenColumns'][key].field + '" type="checkbox" ' + ($.isArray(Core.Easyui.box['hidden']) && $.inArray(Core.Easyui.box['frozenColumns'][key].field, Core.Easyui.box['hidden']) != -1 ? " " : " checked='checked' ") + ' name="_columns_[]" value="' + Core.Easyui.box['frozenColumns'][key].field + '"/><label for="_pg_' + Core.Easyui.box['frozenColumns'][key].field + '">' + Core.Easyui.box['frozenColumns'][key].title + '</label></p>';
                }
                for (key in Core.Easyui.box['columns']) {
                    if ($.inArray(Core.Easyui.box['columns'][key].field, ['ck']) != -1 || !Core.Easyui.box['columns'][key].title) continue;
                    w += '<P><input id="_pg_' + Core.Easyui.box['columns'][key].field + '" type="checkbox" ' + ($.isArray(Core.Easyui.box['hidden']) && $.inArray(Core.Easyui.box['columns'][key].field, Core.Easyui.box['hidden']) != -1 ? " " : " checked='checked' ") + ' name="_columns_[]" value="' + Core.Easyui.box['columns'][key].field + '"/><label for="_pg_' + Core.Easyui.box['columns'][key].field + '">' + Core.Easyui.box['columns'][key].title + '</label></p>';
                }
                w += '<div style="padding-top:6px;"><div class="cksure fl" id="diysurebtn">确定</div><div id="ColumnsCheckbox" class="ckqx fr">全选</div></div>';
                try {
                    Core.Easyui.box['showdiy'].html(w).toggle("fast");
                    Core.Easyui.box['showdiy'].find("#ColumnsCheckbox").live("click", function () {
                        if ($(this).attr('data-state') != 1) {
                            $(":checkbox[name='_columns_[]']").removeAttr("checked");
                            $(this).attr('data-state', 1);
                        } else {
                            $(":checkbox[name='_columns_[]']").attr("checked", "checked");
                            $(this).attr('data-state', 0);
                        }
                    });
                    Core.Easyui.box['showdiy'].find("#diysurebtn").live("click", function () {
                        Core.Easyui.diyColumns.hide();
                    });
                } catch (e) { }
            },
            hide: function () {
                if (!Core.Easyui.box['state'] || !Core.Easyui.box['showdiy']) return;
                Core.Easyui.box['hidden'] = [];

                $(Core.Easyui.box['showdiy']).find(":checkbox[name='_columns_[]']").each(function (index, element) {
                    if ($(this).attr("checked")) {
                        Core.Easyui.o.datagrid('showColumn', $(this).attr('value'));
                    } else {
                        Core.Easyui.o.datagrid('hideColumn', $(this).attr('value'));
                        Core.Easyui.box['hidden'].push($(this).attr('value'));
                    }
                });
                $(Core.Easyui.box['showdiy']).hide();
                Core.Easyui.o.datagrid("resize");
                var columnskey = Core.Easyui.o.attr('data-columnskey');
                if (Core.rule.iskey(columnskey)) {
                    Core.saveUserdata(columnskey + ':hidden', Core.Easyui.box['hidden'].join(','));
                }
            },
            recover: function () {
                var columnskey = Core.Easyui.o.attr('data-columnskey');
                if (!Core.rule.iskey(columnskey)) return;
                Core.delUserdata(columnskey + ':hidden');
            }
        }
    },
    UnionCombobox: function (method, layer) {
        if (!layer['layer1'] || !layer['layer2'] || !layer['layer3']) return;
        switch (method) {
            case 'district': case 'brand':
                layer['layer2']['column'] = {
                    disabled: false,
                    url: layer['layer2'][2].replace("@pid", layer['layer1'][1]).replace("@id", layer['layer2'][1]),
                    valueField: 'id',
                    textField: 'name',
                    onLoadSuccess: function () {
                        if (Core.rule.isNumber('p.integer', layer['layer2'][1])) {
                            $(layer['layer2'][0]).combobox('setValue', layer['layer2'][1]);
                            return;
                        }
                        $(layer['layer2'][0]).combobox('setValue', '').combobox('setText', layer['layer2'][3]);
                    }
                }
                layer['layer3']['column'] = {
                    disabled: false,
                    url: layer['layer3'][2].replace("@pid", layer['layer2'][1]).replace("@id", layer['layer3'][1]),
                    valueField: 'id',
                    textField: 'name',
                    onLoadSuccess: function () {
                        if (Core.rule.isNumber('p.integer', layer['layer3'][1])) {
                            $(layer['layer3'][0]).combobox('setValue', layer['layer3'][1]);
                            return;
                        }
                        $(layer['layer3'][0]).combobox('setValue', '').combobox('setText', layer['layer3'][3]);
                    }
                }
                if (layer['layer3'][4] == true) layer['layer3']['column'].groupField = 'group';
                if (layer['layer2'][4] == true) layer['layer2']['column'].groupField = 'group';


                $(layer['layer1'][0]).combobox({
                    method: 'get',
                    url: layer['layer1'][2].replace("@id", layer['layer1'][1]),
                    editable: false,
                    valueField: 'id',
                    textField: 'name',
                    onLoadSuccess: function () {
                        if (Core.rule.isNumber('p.integer', layer['layer1'][1])) {
                            $(layer['layer1'][0]).combobox('setValue', layer['layer1'][1]);
                            return;
                        }
                        $(layer['layer1'][0]).combobox('setValue', '').combobox('setText', layer['layer1'][3]);
                    },
                    onSelect: function (option) {
                        $(layer['layer3'][0]).combobox('setValue', '').combobox('setText', layer['layer3'][3]);
                        $(layer['layer3'][0]).combobox('disable');

                        if (!option || !option.id) return;
                        layer['layer1.2.column'] = layer['layer2']['column']
                        layer['layer1.2.column'].disabled = false;
                        layer['layer1.2.column'].url = layer['layer2'][2].replace("@pid", option.id).replace("@id", '');
                        layer['layer1.2.column'].onLoadSuccess = function () {
                            $(layer['layer2'][0]).combobox('setValue', '').combobox('setText', layer['layer2'][3]);
                        }
                        $(layer['layer2'][0]).combobox(layer['layer1.2.column']).combobox('clear');
                    }
                });
                layer['layer2']['column'].onSelect = function (option) {
                    if (!option || !option.id) return;
                    layer['layer2.3.column'] = layer['layer3']['column']
                    layer['layer2.3.column'].url = layer['layer3'][2].replace("@pid", option.id).replace("@id", '');
                    layer['layer2.3.column'].onLoadSuccess = function () {
                        $(layer['layer3'][0]).combobox('setValue', '').combobox('setText', layer['layer3'][3]);
                    }
                    $(layer['layer3'][0]).combobox(layer['layer2.3.column']).combobox('clear');
                }
                if (!Core.rule.isNumber('p.integer', layer['layer1'][1])) {
                    layer['layer2']['column'].disabled = true;
                    layer['layer2']['column'].url = null;
                }
                $(layer['layer2'][0]).combobox(layer['layer2']['column']);
                $(layer['layer3'][0]).combobox(Core.rule.isNumber('p.integer', layer['layer2'][1]) ? layer['layer3']['column'] : {});
                break;
        }
    },
    rule: {
        common: function (method, a) {
            switch (method) {
                case '*':
                    var r = /[\w\W]+/;
                    break; case 'pwd':
                    var r = /^([a-zA-Z0-9]){5,20}$/;
                    break;
            }
            return r.test(a);
        },
        iskey: function (a) {
            return /^([a-zA-Z0-9]|[._-]){2,32}$/.test(a);
        },
        isMobile: function (a) {
            return /^13[0-9]{9}$|14[0-9]{9}|15[0-9]{9}$|17[0-9]{9}$|18[0-9]{9}$/.test(a);
        },
        isPhone: function (a) {
            return (/^1[3578]{1}[\d]{9}$/.test(a) || /^([0-9]{3,4}-)?[0-9]{7,8}$/.test(a) || /^([0-9]{3,4})?[0-9]{7,8}$/.test(a));
        },
        isDatetime: function (a) {
            var d = new Date(Date.parse(a.toString().replace(/-/g, "/"))); //"2005-12-15 09:41:30"
            return /^[0-9]*[1-9][0-9]*$/.test(d.getFullYear());
        },
        isNumber: function (method, a) {
            switch (method) {
                case 'integer': //正-负整数+0
                    var r = /^-?\d+$/;
                    break;
                case 'p.integer': //正整数
                    var r = /^[0-9]*[1-9][0-9]*$/;
                    break;
                case 'p.integer+': //正整数+0
                    var r = /^\d+$/;
                    break;
                case 'n.integer': //负整数
                    var r = /^-[0-9]*[1-9][0-9]*$/;
                    break;
                case 'n.integer+': //负整数+0
                    var r = /^((-\d+)|(0+))$/;
                    break;
                case 'double': case 'float': //浮点数
                    var r = /^(-?\d+)(\.\d+)?$/;
                    break;
                case 'p.double': //正浮点数
                    var r = /^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$/;
                    break;
                case 'p.double+': //正浮点数 + 0
                    var r = /^\d+(\.\d+)?$/;
                    break;
                case 'n.double': //负浮点数
                    var r = /^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$/;
                    break;
                case 'n.double+': //负浮点数 + 0
                    var r = /^((-\d+(\.\d+)?)|(0+(\.0+)?))$/;
                    break;
                case 'boolean': //0|1 布尔值
                    var r = /^[0-1]{1}$/;
                    break;
                case 'boolean+': //1|2 布尔值
                    var r = /^[1-2]{1}$/;
                    break;
                case 'numeric':
                    var r = /^[0-9]*$/;
                    break;
                case 'money':
                    var r = /^([1-9][\d]{0,9}|0)(\.[\d]{1,2})?$/;
                    break;
                default:
                    var r = /^[+\-]?\d+(\.\d+)?$/;
                    break;
            }
            return r.test(a);
        }
    },
    saveUserdata: function (name, data) {
        if (Sys.ie) {
            if (data.length < 54889) {
                with (document.documentElement) {
                    setAttribute("value", data);
                    save('Dv__' + name);
                }
            }
        } else if (window.localStorage) {
            localStorage.setItem('Dv__' + name, data);
        } else if (window.sessionStorage) {
            sessionStorage.setItem('Dv__' + name, data);
        }
    },
    loadUserdata: function (name) {
        if (Sys.ie) {
            with (document.documentElement) {
                load('Dv__' + name);
                return getAttribute("value");
            }
        } else if (window.localStorage) {
            return localStorage.getItem('Dv__' + name);
        } else if (window.sessionStorage) {
            return sessionStorage.getItem('Dv__' + name);
        }
    },
    delUserdata: function (name) {
        if (Sys.ie) {
            with (document.documentElement) {
                removeAttribute('value');
                save('Dv__' + name);
            }
        } else if (window.localStorage) {
            localStorage.removeItem('Dv__' + name);
        } else if (window.sessionStorage) {
            sessionStorage.removeItem('Dv__' + name);
        }
        return true;
    }
}