var host = 'http://api.xingzuoxingyu.com';

var Astro = {};
Astro.agotime = function (time) {
    layui.use(['util'], function () {
        var util = layui.util;
        var str = util.timeAgo(time);
        return str;
    });
}
//转成html字符
Astro.HtmlDecode = function (str) {
    return $('<div/>').html(str).text();
}

Astro.GetQueryString = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]); return null;
}
Astro.getAstro = function (birthday) {
    var time = new Date(birthday);
    var month = time.getMonth() + 1;
    var date = time.getDate();
    var value = '';
    if (month == 1 && date >= 20 || month == 2 && date <= 18) { value = "水瓶座"; }
    if (month == 2 && date >= 19 || month == 3 && date <= 20) { value = "双鱼座"; }
    if (month == 3 && date >= 21 || month == 4 && date <= 19) { value = "白羊座"; }
    if (month == 4 && date >= 20 || month == 5 && date <= 20) { value = "金牛座"; }
    if (month == 5 && date >= 21 || month == 6 && date <= 21) { value = "双子座"; }
    if (month == 6 && date >= 22 || month == 7 && date <= 22) { value = "巨蟹座"; }
    if (month == 7 && date >= 23 || month == 8 && date <= 22) { value = "狮子座"; }
    if (month == 8 && date >= 23 || month == 9 && date <= 22) { value = "处女座"; }
    if (month == 9 && date >= 23 || month == 10 && date <= 22) { value = "天秤座"; }
    if (month == 10 && date >= 23 || month == 11 && date <= 21) { value = "天蝎座"; }
    if (month == 11 && date >= 22 || month == 12 && date <= 21) { value = "射手座"; }
    if (month == 12 && date >= 22 || month == 1 && date <= 19) { value = "摩羯座"; }
    return value;
}

Astro.LayDate = function (e) {
    layui.use('laydate', function () {
        var mydata = new Date();
        var laydate = layui.laydate;
        if (e == null) {
            lay('.LayDate').each(function () {
                laydate.render({
                    elem: this
                    , trigger: 'click',
                    format: 'yyyy-MM-dd',
                    calendar: true,
                    theme: "#f84b9c",
                    type: 'datetime'
                });
            });
        } else if (e == 2) {
            lay('.LayDate').each(function () {
                laydate.render({
                    elem: this,
                    format: 'yyyy-MM-dd',
                    calendar: true,
                    theme: "molv",
                });
            });
        } else {
            lay('.LayDate').each(function () {
                laydate.render({
                    elem: this,
                    format: 'yyyy-MM-dd',
                    calendar: true,
                    theme: "molv",
                    min: 0,
                });
            });
        }
        //执行一个laydate实例
    });
    layui.use('form', function () {
        var form = layui.form;
        form.verify({
            select: function (value) {
                if (value == "0") {
                    return '请选择下拉选项！！！';
                }
            },
        });
    });
}

Astro.ThreeApi = function (url, postdata, callback) {
    $.ajax({
        url: host + url,
        type: 'get',
        // crossDomain: true,
        data: postdata,
        // async: false,
        dataType: 'json',
        success: function (data) {
            callback(data)
        }
    });
}

Astro.bandplace = function (x, y) {
    bandplaceevent.first(x, y);
}
var bandplaceevent = {
    first: function (x, y) {
        var jsons = apArr;
        var proHtml = '';
        $.each(jsons, function (x, y) {
            proHtml += '<option value="' + x + '">' + x + '</option>';
        });
        $("#" + x).html(proHtml);
        layui.use('form', function () {
            var form = layui.form;
            form.render();
            form.on('select(' + x + ')', function (data) {
                if (data.value == "云南省") {
                    bandplaceevent.two("云南省", y);
                }
                else {
                    bandplaceevent.two(data.value, y);
                    form.render();
                }
            });
        });
    },
    two: function (id, y) {
        var cityHtml = '';
        cityData = apArr;
        $.each(cityData, function (a, c) {
            if (a == id) {
                $.each(c, function (i, j) {
                    cityHtml += '<option value="' + j + '" name="' + i + '">' + i + '</option>';
                });
            }
        });
        $("#" + y).html(cityHtml);
    }
};

