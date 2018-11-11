/**
 * @author guangtian| longxi1@staff.sina.com.cn
 * @date 2016/11/07
 *
 */
function checkDevice(){
        var u = navigator.userAgent;
        var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1;
        var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/);
        var ua = u.toLowerCase();
        if(ua.match(/MicroMessenger/i)=="micromessenger") {
            nav = 'weixin';
        }else if (ua.match(/WeiBo/i) == "weibo") {
            nav  = 'weibo';
       }else{
            nav = 'normal';
       }
       if(isiOS){
            device = 'ios';
        } else if(isAndroid){
            device =  'android';
        } else{
            device = 'normal';
        }
        var info = new Array();
        info[0] = device;
        info[1] = nav;
        return info;
    }
var info = checkDevice();
    $('.button').on('click', function(){
        if(info[1] == 'weixin'){
             $('.shadow').css('display','block');
        }else if(info[0] == 'ios'){
            if(info[1] == 'weibo'){
                $('.shadow').css('display','block');
            }else{
                //https://itunes.apple.com/cn/app/id545599065
                //https://itunes.apple.com/cn/app/xin-lang-bo-ke-xie-zuo-yue/id466711718
                window.location.href = 'https://itunes.apple.com/cn/app/id545599065';
            }
        }else if(info[0] == 'android'){
            window.location.href = apk;
        }else{
            window.location.href = 'https://itunes.apple.com/cn/app/id545599065';
        }
});


