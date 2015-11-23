// JavaScript Document
var cpcxDevice = {
    box: {
        "device": false, //true-启动成功 false-启动失败
        "accuracy": false, //信号准确 false-模糊 true-精确
        "record": true, //电话录音 false-关闭 true-开启
        "recorddir": 'E:/baidu download', //电话录音文件夹
        "recordpath": "", //录音文件地址
        "calltype": '0', //0-空闲 1-主叫中, 2-被叫中
        "state": '0', //0-未接，1-接通
        "callout": false, //电话外呼 存在号码按了后，没外呼，直接挂机
        "pre": "0", //内线->外线加拨号
        "phone": '', //主叫/被叫号码
        "otherkey": '', //电话接通后,本地按键其他值
        "starttime": "", //通话开始时间
        "endtime": "", //通话结束时间
        "log.link": ""
    },
    loading: function () {
        if (! +[1, ]) {//IE浏览器
            try { var obj = new ActiveXObject("CompanyName.CC301Plugin"); return true; } catch (e) { }
        } else {
            for (x = 0; x < navigator.plugins.length; x++) {
                if (navigator.plugins[x].name.toLowerCase() == "CC301Plugin".toLowerCase()) return true;
            }
        }
        return false;
    },
    config: function (name, value) {
        switch (name) {
            case "loader": case 'init':
                cpcxDevice.box["callout"] = false;
                cpcxDevice.box["calltype"] = '0'; cpcxDevice.box["state"] = '0'; cpcxDevice.box["recordpath"] = "";
                cpcxDevice.box["phone"] = ""; cpcxDevice.box["otherkey"] = ""; cpcxDevice.box["starttime"] = ""; cpcxDevice.box["endtime"] = "";
                if (typeof (value) == 'undefined') return;
                for (key in value) {
                    if (typeof (cpcxDevice.box[key]) == 'undefined') continue;
                    cpcxDevice.box[key] = value[key];
                }
                break; 
            default:
                if (typeof (cpcxDevice.box[name]) == 'undefined') return;
                cpcxDevice.box[name] = value;
                break;
        }
    },
    phone: function (method, params) {
        switch (method) {
            case 'call.in.start.loader': //来电-> 被叫初始化加载
                //if(!$("#Commonphone").length) clickphone();
                if ($.inArray(cpcxDevice.box['calltype'], ['1', '2']) != -1) return;
                cpcxDevice.config("loader", { "calltype": '2', "starttime": cpcxDevice.time() });
                break; case 'call.in.start': //来电-> 被叫 开始
                clickphone(params["phone"]);
                cpcxDevice.config("loader", { "calltype": '2', "phone": params["phone"], "starttime": cpcxDevice.time() }); //重置
                cpcxDevice.recordfile('start'); //开始录音
                break; case 'call.in.end': //来电-> 被叫 结束
                cpcxDevice.recordfile('end'); //结束录音
                cpcxDevice.config("endtime", cpcxDevice.time()); //重置
                cpcxDevice.log();
                //cpcxDevice.cpclog("来电结束:"+cpcxDevice.box["phone"]+" 接通："+(cpcxDevice.box['state']!='1' ? "未接" : "接通")+" 录音："+cpcxDevice.box["recordpath"]+" 时间(S:"+(new Date(cpcxDevice.box["starttime"]*1000)).toLocaleString()+" E:"+(new Date(cpcxDevice.box["endtime"]*1000)).toLocaleString()+")");
                cpcxDevice.config("loader"); //初始化
                break; case 'call.out.start.loader': //外呼-> 主叫初始化加载
                if ($.inArray(cpcxDevice.box['calltype'], ['1', '2']) != -1) return;
                cpcxDevice.config("loader", { "calltype": '1', "starttime": cpcxDevice.time(), "callout": false });
                break; case 'call.out.start': //外呼-> 主叫开始
                try { if (!Core.rule.isPhone(cpcxDevice.box["phone"])) { cpcxDevice.box["phone"] = $(":text#callInp").val(); } } catch (e) { }
                cpcxDevice.config("loader", { "callout": true, "calltype": '1', "phone": cpcxDevice.box["phone"], "starttime": cpcxDevice.time() }); //重置
                cpcxDevice.recordfile('start'); //开始录音
                break; case 'call.out.end':
                cpcxDevice.recordfile('end'); //结束录音
                if (cpcxDevice.box["phone"] != '' && cpcxDevice.box["phone"] != cpcxDevice.box["pre"]) {
                    cpcxDevice.config("endtime", cpcxDevice.time()); //重置
                    cpcxDevice.log();
                    //cpcxDevice.cpclog("外呼结束:"+cpcxDevice.box["phone"]+" 接通："+(cpcxDevice.box['state']!='1' ? "未接" : "接通")+" 录音："+cpcxDevice.box["recordpath"]+" 时间(S:"+(new Date(cpcxDevice.box["starttime"]*1000)).toLocaleString()+" E:"+(new Date(cpcxDevice.box["endtime"]*1000)).toLocaleString()+")");
                }
                cpcxDevice.config("loader"); //初始化
                break; case 'loca.hook': //本地摘机
                if (cpcxDevice.box["calltype"] == "2") {//来电-> 被叫
                    cpcxDevice.box['state'] = "1"; //接通 启动录音
                }
                break; case 'remote.hook': //外呼-> 主叫远程摘机
                cpcxDevice.config("accuracy", true); //精确信号
                if (cpcxDevice.box["calltype"] == "1") {
                    cpcxDevice.box['state'] = "1"; //接通 启动录音
                }
                break; case "local.hang": case 'remote.hang': case "busy": //本地/对方挂机 或忙音,线路断开
                if ($.inArray(method, ['remote.hang']) != -1) {
                    cpcxDevice.config("accuracy", true); //精确信号
                }
                if (cpcxDevice.box["calltype"] == '1') {//主叫
                    cpcxDevice.phone('call.out.end');
                } else if (cpcxDevice.box["calltype"] == '2') {//被叫
                    cpcxDevice.phone('call.in.end');
                } else {
                    cpcxDevice.config("loader"); //初始化
                }
                break; case 'state.dtmf': //非正常状态下拨号
                if ($.inArray(cpcxDevice.box["calltype"], ['1', '2']) != -1) {//呼叫成功后后拨号

                } else {//设置一个本地挂机动作后再设置信号异常提示
                    cpcxDevice.phone("local.hang");
                    Core.ccPhone.Status('alarm', "挂机后2-3秒后再拨!"); //信号异常
                    //cpcxDevice.cpclog("信号异常,请挂机后2-3秒后再拨号!");
                }
                break;
        }
    },
    recordfile: function (method) {//录音
        if (!cpcxDevice.box["record"]) return false;
        switch (method) {
            case 'start':
                var now = new Date();
                cpcxDevice.box["recordpath"] = cpcxDevice.box["recorddir"] + '/' + now.getFullYear() + (now.getMonth() + 1) + now.getDate() + "_" + now.getHours() + now.getMinutes() + now.getSeconds() + "_" + Math.ceil(Math.random() * 100) + ".wav".replace(/\\/g, "/");
                TV_StartRecordFile(cpcxDevice.box["recordpath"]);
                break; case 'stop': case 'end':
                cpcxDevice.cpclog("结束录音信号!");
                if (cpcxDevice.box["recordpath"] == "") return;
                TV_StopRecordFile(0);
                break;
        }
    },
    keydown: function (key) {//按键事件
        if (cpcxDevice.box['state'] != 1 && cpcxDevice.box["calltype"] == 1) {//在主叫未接通的情况下,按键为呼叫号码
            if (cpcxDevice.box["callout"]) return;
            cpcxDevice.box["phone"] += key;
            try { Core.ccPhone.keyboard('driver', key); } catch (e) { };
            //$(".phonediv").html(cpcxDevice.box["phone"]);
        } else {
            if (cpcxDevice.box['state'] == 1 && parseInt(cpcxDevice.box["calltype"]) == 2) return; //被叫,接通中后续信号不处理			
            cpcxDevice.box["otherkey"] += key;
            try { Core.ccPhone.keyboard('driver', key); } catch (e) { };
        }
    },
    EventListener: function (channel, type, handle, result, data) {
        var vValue = " type=" + type + " Handle=" + handle + " Result=" + result + " szdata=" + data;
        switch (parseInt(type)) {
            case BriEvent_PhoneHook: // 本地电话机摘机事件
                AppendStatusEx(channel, "本地电话机摘机" + vValue);
                cpcxDevice.phone('loca.hook');
                break;
            case BriEvent_PhoneDial: // 只有在本地话机摘机，没有调用软摘机时，检测到DTMF拨号
                AppendStatusEx(channel, "本地话机拨号" + vValue);
                break;
            case BriEvent_PhoneHang: // 本地电话机挂机事件
                AppendStatusEx(channel, "本地电话机挂机" + vValue);
                cpcxDevice.phone("local.hang");
                break;
            case BriEvent_CallIn: // 外线通道来电响铃事件
                AppendStatusEx(channel, "外线通道来电响铃事件" + vValue);
                cpcxDevice.phone('call.in.start.loader');
                break;
            case BriEvent_GetCallID: //得到来电号码
                AppendStatusEx(channel, "得到来电号码" + vValue);
                cpcxDevice.phone('call.in.start', { "phone": data }); //开始被叫事件
                break;
            case BriEvent_StopCallIn: // 对方停止呼叫(产生一个未接电话)
                AppendStatusEx(channel, "对方停止呼叫(产生一个未接电话)" + vValue);
                cpcxDevice.phone('call.in.end'); //结束被叫事件
                break;
            case BriEvent_DialEnd: // 调用开始拨号后，全部号码拨号结束
                AppendStatusEx(channel, "调用开始拨号后，全部号码拨号结束" + vValue);
                break; case BriEvent_PlayFileEnd: // 播放文件结束事件AppendStatusEx(channel,"播放文件结束事件"+vValue);
                break; case BriEvent_PlayMultiFileEnd: // 多文件连播结束事件AppendStatusEx(channel,"多文件连播结束事件"+vValue);
                break; case BriEvent_PlayStringEnd: //播放字符结束AppendStatusEx(channel,"播放字符结束"+vValue);
                break; case BriEvent_RepeatPlayFile: // 播放文件结束准备重复播放AppendStatusEx(channel,"播放文件结束准备重复播放"+vValue);
                break; case BriEvent_SendCallIDEnd: // 给本地设备发送震铃信号时发送号码结束AppendStatusEx(channel,"给本地设备发送震铃信号时发送号码结束"+vValue);
                break; case BriEvent_RingTimeOut: //给本地设备发送震铃信号时超时AppendStatusEx(channel,"给本地设备发送震铃信号时超时"+vValue);
                break; case BriEvent_Ringing: //正在内线震铃AppendStatusEx(channel,"正在内线震铃"+vValue);
                break; case BriEvent_Silence: // 通话时检测到一定时间的静音.默认为5秒
                AppendStatusEx(channel, "通话时检测到一定时间的静音" + vValue);
                break;
            case BriEvent_GetDTMFChar: //线路接通时收到DTMF码事件
                AppendStatusEx(channel, "线路接通时收到DTMF码事件" + vValue);
                cpcxDevice.phone('state.dtmf');
                break;
            case BriEvent_RemoteHook: // 拨号后,被叫方摘机事件
                AppendStatusEx(channel, "拨号后,被叫方摘机事件" + vValue);
                cpcxDevice.config("remote.hook"); //有精确信号
                break;
            case BriEvent_RemoteHang: //对方挂机事件
                AppendStatusEx(channel, "对方挂机事件" + vValue);
                cpcxDevice.phone('remote.hang'); //有精确信号
                break;
            case BriEvent_Busy: // 检测到忙音事件,表示PSTN线路已经被断开
                AppendStatusEx(channel, "检测到忙音事件,表示PSTN线路已经被断开" + vValue);
                cpcxDevice.phone("busy");
                TV_HangUpCtrl(0); //强制软挂机
                break;
            case BriEvent_DialTone: // 本地摘机后检测到拨号音
                AppendStatusEx(channel, "本地摘机后检测到拨号音" + vValue);
                cpcxDevice.phone('call.out.start.loader');
                break;
            case BriEvent_RingBack: // 电话机拨号结束呼出事件。
                AppendStatusEx(channel, "电话机拨号结束呼出事件" + vValue);
                cpcxDevice.phone('call.out.start');
                break; case BriEvent_MicIn: // MIC插入状态AppendStatusEx(channel,"MIC插入状态"+vValue);
                break; case BriEvent_MicOut: // MIC拔出状态AppendStatusEx(channel,"MIC拔出状态"+vValue);
                break; case BriEvent_FlashEnd: // 拍插簧(Flash)完成事件，拍插簧完成后可以检测拨号音后进行二次拨号AppendStatusEx(channel,"拍插簧(Flash)完成事件，拍插簧完成后可以检测拨号音后进行二次拨号"+vValue);
                break; case BriEvent_RefuseEnd: // 拒接完成
                AppendStatusEx(channel, "拒接完成" + vValue);
                break; case BriEvent_SpeechResult: // 语音识别完成 AppendStatusEx(channel,"语音识别完成"+vValue);
                break; case BriEvent_FaxRecvFinished: // 接收传真完成AppendStatusEx(channel,"接收传真完成"+vValue);
                break; case BriEvent_FaxRecvFailed: // 接收传真失败AppendStatusEx(channel,"接收传真失败"+vValue);
                break; case BriEvent_FaxSendFinished: // 发送传真完成AppendStatusEx(channel,"发送传真完成"+vValue);
                break; case BriEvent_FaxSendFailed: // 发送传真失败AppendStatusEx(channel,"发送传真失败"+vValue);
                break; case BriEvent_OpenSoundFailed: // 启动声卡失败AppendStatusEx(channel,"启动声卡失败"+vValue);
                break; case BriEvent_UploadSuccess: //远程上传成功AppendStatusEx(channel,"远程上传成功"+vValue);
                break; case BriEvent_UploadFailed: //远程上传失败AppendStatusEx(channel,"远程上传失败"+vValue);
                break; case BriEvent_EnableHook: // 应用层调用软摘机/软挂机成功事件
                AppendStatusEx(channel, "应用层调用软摘机/软挂机成功事件" + vValue);
                break; case BriEvent_EnablePlay: // 喇叭被打开或者/关闭AppendStatusEx(channel,"喇叭被打开或者/关闭"+vValue);
                break; case BriEvent_EnableMic: // MIC被打开或者关闭AppendStatusEx(channel,"MIC被打开或者关闭"+vValue);
                break; case BriEvent_EnableSpk: // 耳机被打开或者关闭AppendStatusEx(channel,"耳机被打开或者关闭"+vValue);
                break; case BriEvent_EnableRing: // 电话机跟电话线(PSTN)断开/接通AppendStatusEx(channel,"电话机跟电话线(PSTN)断开/接通"+vValue);
                break; case BriEvent_DoRecSource: // 修改录音源AppendStatusEx(channel,"修改录音源"+vValue);
                break; case BriEvent_DoStartDial: // 开始软件拨号
                AppendStatusEx(channel, "开始软件拨号" + vValue);
                break;
            case BriEvent_RecvedFSK: // 接收到FSK信号，包括通话中FSK/来电号码的FSK
                AppendStatusEx(channel, "接收到FSK信号，包括通话中FSK/来电号码的FSK" + vValue);
                break;
            case BriEvent_DevErr: //设备错误
                AppendStatusEx(channel, "设备错误" + vValue);
                break;
            default:
                if ($.inArray((channel + 1) + "" + type, ['1194', '1110']) != -1) {
                    AppendStatusEx(channel, "按键:ID=" + type + vValue);
                    if (type == '194') cpcxDevice.keydown(data);
                } else if (type < BriEvent_EndID) {
                    AppendStatusEx(channel, "忽略其它事件发生:ID=" + type + vValue);
                }
                break;
        }
    },
    time: function () {
        return Math.round(new Date().getTime() / 1000);
    },
    recorddir: function (path) {
        cpcxDevice.box["recorddir"] = path;
        $("span.recorddir").html("录音目录:" + path);
    },
    log: function (params) {
        if (cpcxDevice.box["log.link"] == "") return;
        $.get(cpcxDevice.box["log.link"] + '?r=' + Math.random(), { "accuracy": cpcxDevice.box['accuracy'] ? 1 : 0, "phone": cpcxDevice.box["phone"], "calltype": cpcxDevice.box['calltype'], "state": cpcxDevice.box['state'], "recordpath": cpcxDevice.box["recordpath"], "starttime": cpcxDevice.box["starttime"], "endtime": cpcxDevice.box["endtime"] });
    },
    cpclog: function (msg) {
        //$(".cpclog").prepend(msg+'<br/>');
    }
}