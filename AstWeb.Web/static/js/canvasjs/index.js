var panParam = {
    canvas: document.getElementById('canvas'),
    width: window.innerWidth <= 375 ? window.innerWidth * 0.9 : window.innerWidth * 0.35,
    configItem: [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],//显示十大星体+两大虚点
    type: 0,//单盘
    panStyle: '文字',//文字版
    data1: '',
    data2: '54.824037,162.084464,46.221525,48.722376,244.212557,163.305140,254.484226,22.391717,341.801546,287.311022,355.070374,11.535184,335.308492,215.078236,62.043008,168.446871,166.859327,194.835691,225.314212,256.636296,287.675276,317.977888,346.859327,14.835691,45.314212,76.636296,107.675276,137.977888',
};

// //星体配置 十大星体+两大虚点+六小行星
// panParam.configItem = [1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1]
// //盘类型 0：单盘 1：双盘 2：配对盘
// panParam.type = 1
// //盘样式 '文字' 'A32' '专业' '清新' '温暖' '黑夜'
// panParam.panStyle = 'A32'
// //数据源1 单盘 双盘内环
// panParam.data1 = '194.674316,94.272731,218.814682,170.034598,217.374837,202.779696,323.976941,288.274194,288.392703,233.890682,155.577126,33.067805,323.900684,179.516167,333.000046,245.600707,203.400323,232.403188,262.728936,294.044746,325.726090,356.094653,23.400323,52.403188,82.728936,114.044746,145.726090,176.094653'
// //数据源2 双盘外环
// panParam.data2 = '54.824037,162.084464,46.221525,48.722376,244.212557,163.305140,254.484226,22.391717,341.801546,287.311022,355.070374,11.535184,335.308492,215.078236,62.043008,168.446871,166.859327,194.835691,225.314212,256.636296,287.675276,317.977888,346.859327,14.835691,45.314212,76.636296,107.675276,137.977888'

//绘盘
//var longitude,latitude;
//getLocation()
//function getLocation() {
//    if (navigator.geolocation) {
//        navigator.geolocation.getCurrentPosition(showPosition);
//    } else {
//        console.log("Geolocation is not supported by this browser.")
//    }
//}

//function showPosition(position) {
//    longitude=position.coords.longitude
//    latitude=position.coords.latitude
//    // x.innerHTML="Latitude: " + position.coords.latitude + "<br />Longitude: " + position.coords.longitude;
//    console.log("Latitude: " + position.coords.latitude + "Longitude: " + position.coords.longitude)
//}
//getXingpan()
//function getXingpan(){
//    var time=new Date()
//    var data=JSON.parse(sessionStorage.getItem('xpdata'))

//    var content={
//        action:'xingPan',
//        year:data.year,
//        month:data.month,
//        day:data.day,
//        hour:data.hour,
//        min:data.min,
//        second:data.second,
//        zone:-8,
//        longitude:longitude||'99.10E',
//        latitude:latitude||'25.10N',
//        refertime:'undefined',
//        referdegree:'undefined',
//    }
//    $post('/api/threeapi/GetAstrolabeData', content, function (data) {
//        if(data.Status==1){
//          panParam.data1=data.Result.Result
//          drawXingPan(panParam);

//          let iconFontLoad = new FontFaceObserver('iconfont');
//          iconFontLoad.load().then(function() {
//              drawXingPan(panParam);
//          })
//        }
//    })
//}

