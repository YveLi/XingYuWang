<template>
    <div>
        <canvas ref="astrolabe"></canvas>
    </div>
</template>

    //<!--该组件共四个参数-->
    //<!-- width:canvas图片尺寸 -->
    //<!-- panStyle:星盘样式 专业 A32 文字 清新 温暖 黑夜-->
    //<!-- type:星盘类型 0单盘 1双盘 2配对 -->
    //<!-- data1:双盘内圈/单盘 数据源 -->
    //<!-- data2:双盘外圈数据源 -->
    <script>
        import {dealSingleData, dealDoubleData, dealSinglePlanet, dealDoublePlanet} from '@/js/dealNatalData.js'
  import {drawCircleAngle, drawCircleOrigin, drawIconAngle, drawRingOrigin, drawTextAngle, drawLineConn, drawLineAngle} from '@/js/utilCanvas.js'
  export default {
            name: 'canvas-astrolabe',
    props: ['width', 'configItem', 'panStyle', 'type', 'data1', 'data2'],
    computed: {
        },
    created: function() {
            this.canvasInit();
        },
    mounted: function() {
            this.drawXingPan();
        },
    watch: {//监听数据，若参数改变则重绘
            width: function() {//画布宽度
            this.drawXingPan();
        },
      configItem: function() {
            this.drawXingPan();
        },
      panStyle: function() {
            this.drawXingPan();
        },
      type: function() {//绘盘类型——0单盘1双盘2配对盘
            this.drawXingPan();
        },
      data1: function() {
            this.drawXingPan();
        },
      data2: function() {
            this.drawXingPan();
        }
    },
    methods: {
            //初始化绘盘库函数drawAstrolabe
            canvasInit: function() {
            //若库函数已存在则直接返回——注：库函数有修改时要注释掉
            // if('undefined' != typeof drawAstrolabe){return;}
            (function (window) {
                var iconPlanet = [  //行星图标-文字
                    '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', ''];
                var textPlanet = [  //行星名称-文字
                    "日", "月", "水", "金", "火", "木", "土", "天", "海", "冥", "升", "顶", '凯', '谷', '智', '婚', '灶', '北'];
                var iconStar = [    //星座图标-文字
                    "", "", "", "", "", "", "", "", "", "", "", ""];
                var textStar = [    //星座名称-文字
                    "白羊", "金牛", "双子", "巨蟹", "狮子", "处女", "天秤", "天蝎", "射手", "摩羯", "水瓶", "双鱼"];
                var rOutOuter;
                var sizeDashed;       //虚线尺寸
                var colorStar = [];   //星座标识相关的配色方案
                var colorPalace = []; //宫位标识相关的配色方案
                var colorPlanet = []; //行星标识相关的配色方案
                function setFlagColor(baseColor) {//设置星体、宫位、星座颜色(基准是基色baseColor)
                    for (var i = 0; i < 12; i++) {
                        colorStar[i] = baseColor[i % 4];
                        colorPalace[i] = baseColor[i % 4];
                    }
                    var colorLittle = '#EC58E7';//小行星配色——粉色
                    var colorNorth = '#57A8B0';//北交点配色——淡蓝色
                    colorPlanet = [
                        colorStar[0], colorStar[3], colorStar[2], colorStar[1],
                        colorStar[0], colorStar[0], colorStar[1], colorStar[2],
                        colorStar[3], colorStar[3], colorStar[0], colorStar[1],
                        colorLittle, colorLittle, colorLittle, colorLittle, colorLittle, colorNorth];
                }
                /**白底版配色
                **/
                function getWhiteColor() {
                    setFlagColor(["red", "#6A5A0C", "#046A4A", "#003366"]);
                    return {
                        // colorOutCircle: "#656565",//外环颜色
                        colorOutCircle: "#B9B9B9",//外环颜色
                        colorInCircle: "#B9B9B9",//内环颜色
                        colorMidCircle: "#878787",//中圈颜色
                        colorPalaceLine: "#99cccc",//宫位线颜色
                        colorXudian: "#ff9999",//虚点宫位线颜色
                        colorDu: "#000000",//专业版度颜色
                        colorFen: "#757575",//专业版分颜色
                    }
                }
                /**黑底版配色
                **/
                function getBlackColor() {
                    setFlagColor(["red", '#DF9210', '#39E03E', '#5695C5']);
                    return {
                        colorOutCircle: "#DADADA",
                        colorPalaceLine: "#404D4C",
                        colorXudian: "#DADADA",
                    }
                }
                /**清新版配色
                **/
                function getFreshColor() {
                    setFlagColor(["red", '#DF9210', '#39E03E', '#5695C5']);
                    return {
                        colorCircle: '#46D3E2',
                        colorBackground: '#D5FBFF',
                        colorXudian: '#46D3E2',
                        colorPalaceLine: "#A7E6ED",
                    }
                }
                /**温暖版配色
                **/
                function getWramColor() {
                    setFlagColor(["red", '#DF9210', '#39E03E', '#5695C5']);
                    return {
                        colorCircle: '#FFADB7',
                        colorBackground: '#FFDDE1',
                        colorXudian: '#FFADB7',
                        colorPalaceLine: "#FED1D6",
                    }
                }
                /**黑夜版配色
                **/
                function getDarkColor() {
                    setFlagColor(["red", '#DF9210', '#39E03E', '#5695C5']);
                    return {
                        colorRingOut: '#464646',
                        colorRingMid: '#4A5A73',
                        colorRingIn: '#3C3D44',
                        colorCircle: 'white',
                        colorBackground: '#FFDDE1',
                        colorXudian: '#C3C067',
                        colorPalaceLine: "#605F4D",
                    }
                }
                var ctx = null;
                var configItem = [];
                var drawAstrolabe = function (_ctx, _width, _configItem, pType, style) {
                    //设置画布
                    var width = _width;
                    ctx = _ctx;
                    ctx.lineWidth = 2;
                    configItem = _configItem;
                    ctx.translate(width, width);
                    rOutOuter = width - 1;
                    sizeDashed = rOutOuter * 0.02;
                    //返回画图函数
                    var handler = {
                        '单盘': {
                            '专业': drawProSingle,
                            '文字': drawTextSingle,
                            'A32': drawA32Single,
                            '清新': drawFreshSingle,
                            '温暖': drawWarmSingle,
                            '黑夜': drawDarkSingle,
                        },
                        '双盘': {
                            '专业': drawProDouble,
                            '文字': drawTextDouble,
                            'A32': drawA32Double,
                            '清新': drawFreshDouble,
                            '温暖': drawWarmDouble,
                            '黑夜': drawDarkDouble,
                        },
                        '法达': {
                            '专业': drawProFirdaria,
                        }
                    };
                    var draw = handler[pType][style];
                    return draw;
                }
                //单盘专业版
                function drawProSingle(objSingle) {
                    //1、单盘数据
                    var { planetSingle, palace, phaseSingle } = dealSinglePlanet(objSingle, 0.015, configItem);
                    //2、专业版配色
                    var proColor = getWhiteColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.909;
                    var rInOuter = rOutOuter * 0.489;
                    var rInInner = rOutOuter * 0.396;
                    var rOutMid = (rOutInner + rOutOuter) / 2;
                    var rInMid = (rInInner + rInOuter) / 2;
                    var rPlanetFlag = rInOuter + (rOutInner - rInOuter) * 0.8;
                    var rPhasePoint = rInInner * 0.96;
                    var rMidDu = rInOuter + (rOutInner - rInOuter) * 0.565;
                    var rMidStar = rInOuter + (rOutInner - rInOuter) * 0.379;
                    var rMidFen = rInOuter + (rOutInner - rInOuter) * 0.207;
                    var sizeStarFlag = rOutOuter * 0.058;
                    var sizePalaceNum = rOutOuter * 0.05;
                    var sizePlanetFlag = rOutOuter * 0.077;
                    var sizeDu = rOutOuter * 0.047;//度字号
                    var sizeFen = rOutOuter * 0.04;//分字号
                    var sizeMidStar = rOutOuter * 0.045;//中圈星座字号
                    //4、开始画图
                    drawProSinglePan();
                    function drawProSinglePan() {
                        //1、画底图：四个圈
                        drawCircleOrigin(ctx, rOutOuter, proColor.colorOutCircle);
                        drawCircleOrigin(ctx, rOutInner, proColor.colorOutCircle);
                        drawCircleOrigin(ctx, rInOuter, proColor.colorInCircle);
                        drawCircleOrigin(ctx, rInInner, proColor.colorInCircle);
                        //2、画宫位相关：宫位线、宫位编号、对应星座、度、分
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            var starNum = palace[i][2];
                            var du = palace[i][3];
                            var fen = palace[i][4];
                            var colorLine = (i % 3 == 0) ? proColor.colorXudian : proColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInInner, -rOutInner, angle, colorLine);
                            var angleNum = (angle + angleNext) / 2;
                            drawTextAngle(ctx, rInMid, angleNum, i + 1, colorPalace[i], sizePalaceNum);
                            drawIconAngle(ctx, rOutMid, angle, iconStar[starNum], colorStar[starNum], sizeStarFlag);
                            drawTextAngle(ctx, rOutMid, angle + 1 / 70, du, proColor.colorDu, sizeDu);
                            drawTextAngle(ctx, rOutMid, angle - 1 / 70, fen, proColor.colorFen, sizeFen);
                            var starNext = palace[(i + 1) % 12][2];
                            var starGap = (starNext - starNum + 12) % 12;
                            if (starGap > 1) {
                                var angleRefer = angle + 1 / 70;
                                var angleGap = (angleNext - angle - 1 / 35) / starGap;
                                var starJieDuo;
                                for (var j = 1; j < starGap; j++) {
                                    starJieDuo = (starNum + j) % 12;
                                    drawIconAngle(ctx, rOutMid, angleRefer + j * angleGap, iconStar[starJieDuo], colorStar[starJieDuo], sizeStarFlag);
                                }
                            }
                        }
                        //3、画行星相关(用调整后的角度):行星、对应星座、度、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planetSingle[i][5];
                            var starNum = planetSingle[i][2];
                            var du = planetSingle[i][3];
                            var fen = planetSingle[i][4];
                            drawIconAngle(ctx, rPlanetFlag, planetSingle[i][5], iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawIconAngle(ctx, rMidStar, angleAdjust, iconStar[starNum], colorPlanet[i], sizeMidStar);
                            drawTextAngle(ctx, rMidDu, angleAdjust, du, proColor.colorDu, sizeDu);
                            drawTextAngle(ctx, rMidFen, angleAdjust, fen, proColor.colorFen, sizeFen);
                        }
                        //4、画相位
                        phaseSingle.forEach(function (item) {
                            drawLineConn(ctx, rPhasePoint, planetSingle[item[0]][1], rPhasePoint, planetSingle[item[1]][1], "#" + item[2], item[5], sizeDashed);
                        })
                    }
                }
                //单盘文字版
                function drawTextSingle(objSingle) {
                    //1、单盘数据
                    var { planetSingle, palace, phaseSingle } = dealSinglePlanet(objSingle, 0.024, configItem);
                    //2、文字版颜色
                    var textColor = getBlackColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.792;
                    var rInOuter = rOutOuter * 0.744;
                    var rInInner = rOutOuter * 0.652;
                    var rOutMid = (rOutOuter + rOutInner) / 2;
                    var rInMid = (rInOuter + rInInner) / 2;
                    var rPlanetFlag = rInInner * 0.887;
                    var rPhasePoint = rInInner * 0.796;
                    var sizePalaceNum = rOutOuter * 0.055;
                    var sizeStarFlag = rOutOuter * 0.07;
                    var sizePlanetFlag = rOutOuter * 0.075;
                    var sizePhasePoint = rInInner * 0.008;
                    //4、开始画图
                    drawTextSinglePan();
                    function drawTextSinglePan() {
                        drawRingOrigin(ctx, rOutOuter, "#000000");//星盘底色
                        //1、画底图：四个圈、刻度线
                        drawCircleOrigin(ctx, rOutOuter, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rOutInner, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rInOuter, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rInInner, textColor.colorOutCircle);
                        for (var i = 0; i < 360; i++) {
                            var color = (i % 5 == 0) ? textColor.colorOutCircle : textColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInOuter, -rOutInner, i / 360, color);
                        }
                        //2、画宫位相关：宫位线、宫位编号、星座边界线、星座
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            if (i % 3 == 0) {
                                drawLineAngle(ctx, 0, -rOutOuter, angle, textColor.colorXudian);
                            } else { drawLineAngle(ctx, 0, -rInOuter, angle, textColor.colorPalaceLine); }
                            drawTextAngle(ctx, rInMid, (angle + angleNext) / 2, i + 1, colorPalace[i], sizePalaceNum);
                            var angleStar = -palace[0][0] / 360 + i / 12;//星座边界的角度(-x轴逆时针方向)
                            drawLineAngle(ctx, -rOutInner, -rOutOuter, angleStar, textColor.colorOutCircle);
                            drawTextAngle(ctx, rOutMid, angleStar + 1 / 24, textStar[i], colorStar[i], sizeStarFlag);
                            drawIconAngle(ctx, rOutMid, angleStar + 1 / 24 - 1 / 48, iconStar[i], colorStar[i], sizeStarFlag);
                        }
                        //3、画行星相关:行星名称、行星点位、名称与点位连线、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planetSingle[i][5];
                            var angleOriginal = planetSingle[i][1];
                            drawTextAngle(ctx, rPlanetFlag, angleAdjust, textPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angleOriginal, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angleOriginal, rPlanetFlag - sizePlanetFlag / 2, angleAdjust, colorPlanet[i], 0);
                        }
                        //4、画相位
                        phaseSingle.forEach(function (item) {
                            drawLineConn(ctx, rPhasePoint, planetSingle[item[0]][1], rPhasePoint, planetSingle[item[1]][1], "#" + item[2], item[5], sizeDashed);
                        })
                    }
                }
                //单盘A32版
                function drawA32Single(objSingle) {
                    //1、单盘数据
                    var { planetSingle, palace, phaseSingle } = dealSinglePlanet(objSingle, 0.024, configItem);
                    //2、文字版颜色
                    var textColor = getBlackColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.792;
                    var rInOuter = rOutOuter * 0.744;
                    var rInInner = rOutOuter * 0.652;
                    var rOutMid = (rOutOuter + rOutInner) / 2;
                    var rInMid = (rInOuter + rInInner) / 2;
                    var rPlanetFlag = rInInner * 0.887;
                    var rPhasePoint = rInInner * 0.796;
                    var sizePalaceNum = rOutOuter * 0.055;
                    var sizeStarFlag = rOutOuter * 0.07;
                    var sizePlanetFlag = rOutOuter * 0.075;
                    var sizePhasePoint = rInInner * 0.008;
                    //4、开始画图
                    drawA32SinglePan();
                    function drawA32SinglePan() {
                        drawRingOrigin(ctx, rOutOuter, "#000000");//星盘底色
                        //1、画底图：四个圈、刻度线
                        drawCircleOrigin(ctx, rOutOuter, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rOutInner, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rInOuter, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rInInner, textColor.colorOutCircle);
                        for (var i = 0; i < 360; i++) {
                            var color = (i % 5 == 0) ? textColor.colorOutCircle : textColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInOuter, -rOutInner, i / 360, color);
                        }
                        //2、画宫位相关：宫位线、宫位编号、星座边界线、星座
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            if (i % 3 == 0) {
                                drawLineAngle(ctx, 0, -rOutOuter, angle, textColor.colorXudian);
                            } else { drawLineAngle(ctx, 0, -rInOuter, angle, textColor.colorPalaceLine); }
                            drawTextAngle(ctx, rInMid, (angle + angleNext) / 2, i + 1, colorPalace[i], sizePalaceNum);
                            var angleStar = -palace[0][0] / 360 + i / 12;//星座边界的角度(-x轴逆时针方向)
                            drawLineAngle(ctx, -rOutInner, -rOutOuter, angleStar, textColor.colorOutCircle);
                            drawIconAngle(ctx, rOutMid, angleStar + 1 / 24, iconStar[i], colorStar[i], sizeStarFlag);
                        }
                        //3、画行星相关:行星名称、行星点位、名称与点位连线、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planetSingle[i][5];
                            var angleOriginal = planetSingle[i][1];
                            drawIconAngle(ctx, rPlanetFlag, angleAdjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angleOriginal, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angleOriginal, rPlanetFlag - sizePlanetFlag / 2, angleAdjust, colorPlanet[i], 0);
                        }
                        //4、画相位
                        phaseSingle.forEach(function (item) {
                            drawLineConn(ctx, rPhasePoint, planetSingle[item[0]][1], rPhasePoint, planetSingle[item[1]][1], "#" + item[2], item[5], sizeDashed);
                        })
                    }
                }
                //单盘清新版
                function drawFreshSingle(objSingle) {
                    //1、单盘数据
                    var { planetSingle, palace, phaseSingle } = dealSinglePlanet(objSingle, 0.024, configItem);
                    //2、文字版颜色
                    var freshColor = getFreshColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.792;
                    var rInOuter = rOutOuter * 0.744;
                    var rInInner = rOutOuter * 0.652;
                    var rOutMid = (rOutOuter + rOutInner) / 2;
                    var rInMid = (rInOuter + rInInner) / 2;
                    var rPlanetFlag = rInInner * 0.887;
                    var rPhasePoint = rInInner * 0.796;
                    var sizePalaceNum = rOutOuter * 0.055;
                    var sizeStarFlag = rOutOuter * 0.07;
                    var sizePlanetFlag = rOutOuter * 0.075;
                    var sizePhasePoint = rInInner * 0.008;
                    //4、开始画图
                    drawFreshSinglePan();
                    function drawFreshSinglePan() {
                        drawRingOrigin(ctx, rOutOuter, "#000000");//星盘底色
                        //1、画底图：四个圈、刻度线
                        drawRingOrigin(ctx, rOutOuter, freshColor.colorBackground);
                        drawCircleOrigin(ctx, rOutOuter, freshColor.colorCircle);
                        drawCircleOrigin(ctx, rOutInner, freshColor.colorCircle);
                        drawRingOrigin(ctx, rInOuter, 'white');
                        drawCircleOrigin(ctx, rInOuter, freshColor.colorCircle);
                        drawCircleOrigin(ctx, rInInner, freshColor.colorCircle);
                        for (var i = 0; i < 360; i++) {
                            var color = (i % 5 == 0) ? freshColor.colorCircle : freshColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInOuter, -rOutInner, i / 360, color);
                        }
                        //2、画宫位相关：宫位线、宫位编号、星座边界线、星座
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            if (i % 3 == 0) {
                                drawLineAngle(ctx, 0, -rOutOuter, angle, freshColor.colorXudian);
                            } else { drawLineAngle(ctx, 0, -rInOuter, angle, freshColor.colorPalaceLine); }
                            drawTextAngle(ctx, rInMid, (angle + angleNext) / 2, i + 1, colorPalace[i], sizePalaceNum);
                            var angleStar = -palace[0][0] / 360 + i / 12;//星座边界的角度(-x轴逆时针方向)
                            drawLineAngle(ctx, -rOutInner, -rOutOuter, angleStar, freshColor.colorCircle);
                            drawIconAngle(ctx, rOutMid, angleStar + 1 / 24, iconStar[i], colorStar[i], sizeStarFlag);
                        }
                        //3、画行星相关:行星名称、行星点位、名称与点位连线、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planetSingle[i][5];
                            var angleOriginal = planetSingle[i][1];
                            drawIconAngle(ctx, rPlanetFlag, angleAdjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angleOriginal, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angleOriginal, rPlanetFlag - sizePlanetFlag / 2, angleAdjust, colorPlanet[i], 0);
                        }
                        //4、画相位
                        phaseSingle.forEach(function (item) {
                            drawLineConn(ctx, rPhasePoint, planetSingle[item[0]][1], rPhasePoint, planetSingle[item[1]][1], "#" + item[2], item[5], sizeDashed);
                        })
                    }
                }
                //单盘温暖版
                function drawWarmSingle(objSingle) {
                    //1、单盘数据
                    var { planetSingle, palace, phaseSingle } = dealSinglePlanet(objSingle, 0.024, configItem);
                    //2、文字版颜色
                    var wramColor = getWramColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.792;
                    var rInOuter = rOutOuter * 0.744;
                    var rInInner = rOutOuter * 0.652;
                    var rOutMid = (rOutOuter + rOutInner) / 2;
                    var rInMid = (rInOuter + rInInner) / 2;
                    var rPlanetFlag = rInInner * 0.887;
                    var rPhasePoint = rInInner * 0.796;
                    var sizePalaceNum = rOutOuter * 0.055;
                    var sizeStarFlag = rOutOuter * 0.07;
                    var sizePlanetFlag = rOutOuter * 0.075;
                    var sizePhasePoint = rInInner * 0.008;
                    //4、开始画图
                    drawFreshSinglePan();
                    function drawFreshSinglePan() {
                        drawRingOrigin(ctx, rOutOuter, "#000000");//星盘底色
                        //1、画底图：四个圈、刻度线
                        drawRingOrigin(ctx, rOutOuter, wramColor.colorBackground);
                        drawCircleOrigin(ctx, rOutOuter, wramColor.colorCircle);
                        drawCircleOrigin(ctx, rOutInner, wramColor.colorCircle);
                        drawRingOrigin(ctx, rInOuter, 'white');
                        drawCircleOrigin(ctx, rInOuter, wramColor.colorCircle);
                        drawCircleOrigin(ctx, rInInner, wramColor.colorCircle);
                        for (var i = 0; i < 360; i++) {
                            var color = (i % 5 == 0) ? wramColor.colorCircle : wramColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInOuter, -rOutInner, i / 360, color);
                        }
                        //2、画宫位相关：宫位线、宫位编号、星座边界线、星座
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            if (i % 3 == 0) {
                                drawLineAngle(ctx, 0, -rOutOuter, angle, wramColor.colorXudian);
                            } else { drawLineAngle(ctx, 0, -rInOuter, angle, wramColor.colorPalaceLine); }
                            drawTextAngle(ctx, rInMid, (angle + angleNext) / 2, i + 1, colorPalace[i], sizePalaceNum);
                            var angleStar = -palace[0][0] / 360 + i / 12;//星座边界的角度(-x轴逆时针方向)
                            drawLineAngle(ctx, -rOutInner, -rOutOuter, angleStar, wramColor.colorCircle);
                            drawIconAngle(ctx, rOutMid, angleStar + 1 / 24, iconStar[i], colorStar[i], sizeStarFlag);
                        }
                        //3、画行星相关:行星名称、行星点位、名称与点位连线、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planetSingle[i][5];
                            var angleOriginal = planetSingle[i][1];
                            drawIconAngle(ctx, rPlanetFlag, angleAdjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angleOriginal, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angleOriginal, rPlanetFlag - sizePlanetFlag / 2, angleAdjust, colorPlanet[i], 0);
                        }
                        //4、画相位
                        phaseSingle.forEach(function (item) {
                            drawLineConn(ctx, rPhasePoint, planetSingle[item[0]][1], rPhasePoint, planetSingle[item[1]][1], "#" + item[2], item[5], sizeDashed);
                        })
                    }
                }
                //单盘黑夜版
                function drawDarkSingle(objSingle) {
                    //1、单盘数据
                    var { planetSingle, palace, phaseSingle } = dealSinglePlanet(objSingle, 0.024, configItem);
                    //2、文字版颜色
                    var darkColor = getDarkColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.792;
                    var rInOuter = rOutOuter * 0.744;
                    var rInInner = rOutOuter * 0.652;
                    var rOutMid = (rOutOuter + rOutInner) / 2;
                    var rInMid = (rInOuter + rInInner) / 2;
                    var rPlanetFlag = rInInner * 0.887;
                    var rPhasePoint = rInInner * 0.796;
                    var sizePalaceNum = rOutOuter * 0.055;
                    var sizeStarFlag = rOutOuter * 0.07;
                    var sizePlanetFlag = rOutOuter * 0.075;
                    var sizePhasePoint = rInInner * 0.008;
                    //4、开始画图
                    drawDarkSinglePan();
                    function drawDarkSinglePan() {
                        //1、画底图：四个圈、刻度线
                        drawRingOrigin(ctx, rOutOuter, darkColor.colorRingOut);
                        drawCircleOrigin(ctx, rOutOuter, darkColor.colorCircle);
                        drawCircleOrigin(ctx, rOutInner, darkColor.colorCircle);
                        drawRingOrigin(ctx, rInOuter, darkColor.colorRingMid);
                        drawCircleOrigin(ctx, rInOuter, darkColor.colorCircle);
                        drawRingOrigin(ctx, rInInner, darkColor.colorRingIn);
                        drawCircleOrigin(ctx, rInInner, darkColor.colorCircle);
                        for (var i = 0; i < 360; i++) {
                            var color = (i % 5 == 0) ? darkColor.colorCircle : darkColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInOuter, -rOutInner, i / 360, color);
                        }
                        //2、画宫位相关：宫位线、宫位编号、星座边界线、星座
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            if (i % 3 == 0) {
                                drawLineAngle(ctx, 0, -rOutOuter, angle, darkColor.colorXudian);
                            } else { drawLineAngle(ctx, 0, -rInOuter, angle, darkColor.colorPalaceLine); }
                            drawTextAngle(ctx, rInMid, (angle + angleNext) / 2, i + 1, colorPalace[i], sizePalaceNum);
                            var angleStar = -palace[0][0] / 360 + i / 12;//星座边界的角度(-x轴逆时针方向)
                            drawLineAngle(ctx, -rOutInner, -rOutOuter, angleStar, darkColor.colorCircle);
                            drawIconAngle(ctx, rOutMid, angleStar + 1 / 24, iconStar[i], colorStar[i], sizeStarFlag);
                        }
                        //3、画行星相关:行星名称、行星点位、名称与点位连线、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planetSingle[i][5];
                            var angleOriginal = planetSingle[i][1];
                            drawIconAngle(ctx, rPlanetFlag, angleAdjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angleOriginal, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angleOriginal, rPlanetFlag - sizePlanetFlag / 2, angleAdjust, colorPlanet[i], 0);
                        }
                        //4、画相位
                        phaseSingle.forEach(function (item) {
                            drawLineConn(ctx, rPhasePoint, planetSingle[item[0]][1], rPhasePoint, planetSingle[item[1]][1], "#" + item[2], item[5], sizeDashed);
                        })
                    }
                }
                //双盘专业版
                //为什么不画0°黄色he相位线？
                function drawProDouble(objDouble) {
                    //1、双盘数据
                    var { palace, phaseDouble, planet1, planet2 } = dealDoublePlanet(objDouble, 0.015, configItem);
                    //2、专业版配色
                    var proColor = getWhiteColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.909;
                    var rInOuter = rOutOuter * 0.304;
                    var rInInner = rOutOuter * 0.256;
                    var rIOMid = (rInOuter + rOutInner) / 2;
                    var rOutMid = (rOutInner + rOutOuter) / 2;
                    var rInMid = (rInInner + rInOuter) / 2;
                    var rPhasePoint = rInInner * 0.96;
                    var rPlanetNatal = rInOuter + (rIOMid - rInOuter) * 0.8;
                    var rPlanetTransit = rIOMid + (rOutInner - rIOMid) * 0.8;
                    var rNatalDu = rInOuter + (rIOMid - rInOuter) * 0.565;
                    var rNatalStar = rInOuter + (rIOMid - rInOuter) * 0.379;
                    var rNatalFen = rInOuter + (rIOMid - rInOuter) * 0.207;
                    var rTransitDu = rIOMid + (rOutInner - rIOMid) * 0.565;
                    var rTransitStar = rIOMid + (rOutInner - rIOMid) * 0.379;
                    var rTransitFen = rIOMid + (rOutInner - rIOMid) * 0.207;
                    var sizeNatalDu = rOutOuter * 0.037;//本命度字号
                    var sizeNatalFen = rOutOuter * 0.03;//本命分字号
                    var sizeTransitDu = rOutOuter * 0.037;//行运度字号
                    var sizeTransitFen = rOutOuter * 0.03;//行运分字号
                    var sizeStarFlag = rOutOuter * 0.058;
                    var sizePalaceNum = rOutOuter * 0.04;
                    var sizePlanetFlag = rOutOuter * 0.05;
                    var sizeDu = rOutOuter * 0.047;//度字号
                    var sizeFen = rOutOuter * 0.04;//分字号
                    var sizeMidStar = rOutOuter * 0.035;//中圈星座字号
                    //4、开始画图
                    drawProDoublePan();
                    function drawProDoublePan() {
                        //1、画底图：五个圈
                        drawCircleOrigin(ctx, rOutOuter, proColor.colorOutCircle);
                        drawCircleOrigin(ctx, rOutInner, proColor.colorOutCircle);
                        drawCircleOrigin(ctx, rIOMid, proColor.colorMidCircle);
                        drawCircleOrigin(ctx, rInOuter, proColor.colorInCircle);
                        drawCircleOrigin(ctx, rInInner, proColor.colorInCircle);
                        //2、画宫位相关：宫位线、宫位编号、对应星座、度、分
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            var starNum = palace[i][2];
                            var du = palace[i][3];
                            var fen = palace[i][4];
                            var colorLine = (i % 3 == 0) ? proColor.colorXudian : proColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInInner, -rOutInner, angle, colorLine);
                            var angleNum = (angle + angleNext) / 2;
                            drawTextAngle(ctx, rInMid, angleNum, i + 1, colorPalace[i], sizePalaceNum);
                            drawIconAngle(ctx, rOutMid, angle, iconStar[starNum], colorStar[starNum], sizeStarFlag);
                            drawTextAngle(ctx, rOutMid, angle + 1 / 70, du, proColor.colorDu, sizeDu);
                            drawTextAngle(ctx, rOutMid, angle - 1 / 70, fen, proColor.colorFen, sizeFen);
                            var starNext = palace[(i + 1) % 12][2];
                            var starGap = (starNext - starNum + 12) % 12;
                            if (starGap > 1) {
                                var angleRefer = angle + 1 / 70;
                                var angleGap = (angleNext - angle - 1 / 35) / starGap;
                                var starJieDuo;
                                for (var j = 1; j < starGap; j++) {
                                    starJieDuo = (starNum + j) % 12;
                                    drawIconAngle(ctx, rOutMid, angleRefer + j * angleGap, iconStar[starJieDuo], colorStar[starJieDuo], sizeStarFlag);
                                }
                            }
                        }
                        //3、画行星相关(用调整后的角度):行星、对应星座、度、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planet1[i][5];
                            var starNum = planet1[i][2];
                            var du = planet1[i][3];
                            var fen = planet1[i][4];
                            drawIconAngle(ctx, rPlanetNatal, planet1[i][5], iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawIconAngle(ctx, rNatalStar, angleAdjust, iconStar[starNum], colorPlanet[i], sizeMidStar);
                            drawTextAngle(ctx, rNatalDu, angleAdjust, du, proColor.colorDu, sizeNatalDu);
                            drawTextAngle(ctx, rNatalFen, angleAdjust, fen, proColor.colorFen, sizeNatalFen);
                            var angleAdjust = planet2[i][5];
                            var starNum = planet2[i][2];
                            var du = planet2[i][3];
                            var fen = planet2[i][4];
                            drawIconAngle(ctx, rPlanetTransit, planet2[i][5], iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawIconAngle(ctx, rTransitStar, angleAdjust, iconStar[starNum], colorPlanet[i], sizeMidStar);
                            drawTextAngle(ctx, rTransitDu, angleAdjust, du, proColor.colorDu, sizeTransitDu);
                            drawTextAngle(ctx, rTransitFen, angleAdjust, fen, proColor.colorFen, sizeTransitFen);
                        }
                        phaseDouble.forEach(function (item) {
                            if (item[4] != 'he') {
                                drawLineConn(ctx, rPhasePoint, planet2[item[0]][1], rPhasePoint, planet1[item[1]][1], "#" + item[2], item[5], sizeDashed);
                            }
                        });
                    }
                }
                //双盘文字版
                function drawTextDouble(objDouble) {
                    //1、双盘数据
                    var { palace, phaseDouble, planet1, planet2 } = dealDoublePlanet(objDouble, 0.024, configItem);
                    //2、文字版颜色
                    var textColor = getBlackColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.835;
                    var rInOuter = rOutOuter * 0.792;
                    var rInInner = rOutOuter * 0.720;
                    var rOutMid = (rOutInner + rOutOuter) / 2;
                    var rInMid = (rInInner + rInOuter) / 2;
                    var rPlanet2Flag = rInInner * 0.887;
                    var rPlanet1Flag = rInInner * 0.750;
                    var rPhasePoint = rInInner * 0.650;
                    var sizePalaceNum = rOutOuter * 0.055;
                    var sizeStarFlag = rOutOuter * 0.07;
                    var sizePlanetFlag = rOutOuter * 0.075;
                    var sizePhasePoint = rInInner * 0.008;
                    //4、开始画图
                    drawTextDoublePan();
                    function drawTextDoublePan() {
                        drawRingOrigin(ctx, rOutOuter, "#000000");//星盘底色
                        //1、画底图：四个圈、刻度线
                        drawCircleOrigin(ctx, rOutOuter, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rOutInner, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rInOuter, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rInInner, textColor.colorOutCircle);
                        for (var i = 0; i < 360; i++) {
                            var color = (i % 5 == 0) ? textColor.colorOutCircle : textColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInOuter, -rOutInner, i / 360, color);
                        }
                        //2、画宫位相关：宫位线、宫位编号、星座边界线、星座
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            if (i % 3 == 0) {
                                drawLineAngle(ctx, 0, -rOutOuter, angle, textColor.colorXudian);
                            } else { drawLineAngle(ctx, 0, -rInOuter, angle, textColor.colorPalaceLine); }
                            drawTextAngle(ctx, rInMid, (angle + angleNext) / 2, i + 1, colorPalace[i], sizePalaceNum);
                            var angleStar = -palace[0][0] / 360 + i / 12;//星座边界的角度(-x轴逆时针方向)
                            drawLineAngle(ctx, -rOutInner, -rOutOuter, angleStar, textColor.colorOutCircle);
                            drawTextAngle(ctx, rOutMid, angleStar + 1 / 24, textStar[i], colorStar[i], sizeStarFlag);
                            drawIconAngle(ctx, rOutMid, angleStar + 1 / 24 - 1 / 48, iconStar[i], colorStar[i], sizeStarFlag);
                        }
                        //3、画行星相关:行星名称、行星点位、名称与点位连线、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planet1[i][5];
                            var angleOriginal = planet1[i][1];
                            drawTextAngle(ctx, rPlanet1Flag, angleAdjust, textPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angleOriginal, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angleOriginal, rPlanet1Flag - sizePlanetFlag / 2, angleAdjust, colorPlanet[i], 0);
                            var angle2Adjust = planet2[i][5];
                            var angle2Original = planet2[i][1];
                            drawTextAngle(ctx, rPlanet2Flag, angle2Adjust, textPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angle2Original, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angle2Original, rPlanet2Flag - sizePlanetFlag / 2, angle2Adjust, colorPlanet[i], 0);
                        }
                        //4、画相位
                        phaseDouble.forEach(function (item) {
                            if (item[4] != 'he') {
                                drawLineConn(ctx, rPhasePoint, planet2[item[0]][1], rPhasePoint, planet1[item[1]][1], "#" + item[2], item[5], sizeDashed);
                            }
                        })
                    }
                }
                //双盘A32版
                function drawA32Double(objDouble) {
                    //1、双盘数据
                    var { palace, phaseDouble, planet1, planet2 } = dealDoublePlanet(objDouble, 0.024, configItem);
                    //2、文字版颜色
                    var textColor = getBlackColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.835;
                    var rInOuter = rOutOuter * 0.792;
                    var rInInner = rOutOuter * 0.720;
                    var rOutMid = (rOutInner + rOutOuter) / 2;
                    var rInMid = (rInInner + rInOuter) / 2;
                    var rPlanet2Flag = rInInner * 0.887;
                    var rPlanet1Flag = rInInner * 0.750;
                    var rPhasePoint = rInInner * 0.650;
                    var sizePalaceNum = rOutOuter * 0.055;
                    var sizeStarFlag = rOutOuter * 0.07;
                    var sizePlanetFlag = rOutOuter * 0.075;
                    var sizePhasePoint = rInInner * 0.008;
                    //4、开始画图
                    drawA32DoublePan();
                    function drawA32DoublePan() {
                        drawRingOrigin(ctx, rOutOuter, "#000000");//星盘底色
                        //1、画底图：四个圈、刻度线
                        drawCircleOrigin(ctx, rOutOuter, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rOutInner, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rInOuter, textColor.colorOutCircle);
                        drawCircleOrigin(ctx, rInInner, textColor.colorOutCircle);
                        for (var i = 0; i < 360; i++) {
                            var color = (i % 5 == 0) ? textColor.colorOutCircle : textColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInOuter, -rOutInner, i / 360, color);
                        }
                        //2、画宫位相关：宫位线、宫位编号、星座边界线、星座
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            if (i % 3 == 0) {
                                drawLineAngle(ctx, 0, -rOutOuter, angle, textColor.colorXudian);
                            } else { drawLineAngle(ctx, 0, -rInOuter, angle, textColor.colorPalaceLine); }
                            drawTextAngle(ctx, rInMid, (angle + angleNext) / 2, i + 1, colorPalace[i], sizePalaceNum);
                            var angleStar = -palace[0][0] / 360 + i / 12;//星座边界的角度(-x轴逆时针方向)
                            drawLineAngle(ctx, -rOutInner, -rOutOuter, angleStar, textColor.colorOutCircle);
                            drawIconAngle(ctx, rOutMid, angleStar + 1 / 24, iconStar[i], colorStar[i], sizeStarFlag);
                        }
                        //3、画行星相关:行星名称、行星点位、名称与点位连线、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planet1[i][5];
                            var angleOriginal = planet1[i][1];
                            drawIconAngle(ctx, rPlanet1Flag, angleAdjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angleOriginal, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angleOriginal, rPlanet1Flag - sizePlanetFlag / 2, angleAdjust, colorPlanet[i], 0);
                            var angle2Adjust = planet2[i][5];
                            var angle2Original = planet2[i][1];
                            drawIconAngle(ctx, rPlanet2Flag, angle2Adjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angle2Original, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angle2Original, rPlanet2Flag - sizePlanetFlag / 2, angle2Adjust, colorPlanet[i], 0);
                        }
                        //4、画相位
                        phaseDouble.forEach(function (item) {
                            if (item[4] != 'he') {
                                drawLineConn(ctx, rPhasePoint, planet2[item[0]][1], rPhasePoint, planet1[item[1]][1], "#" + item[2], item[5], sizeDashed);
                            }
                        })
                    }
                }
                //双盘清新版
                function drawFreshDouble(objDouble) {
                    //1、双盘数据
                    var { palace, phaseDouble, planet1, planet2 } = dealDoublePlanet(objDouble, 0.024, configItem);
                    //2、文字版颜色
                    var freshColor = getFreshColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.835;
                    var rInOuter = rOutOuter * 0.792;
                    var rInInner = rOutOuter * 0.720;
                    var rOutMid = (rOutInner + rOutOuter) / 2;
                    var rInMid = (rInInner + rInOuter) / 2;
                    var rPlanet2Flag = rInInner * 0.887;
                    var rPlanet1Flag = rInInner * 0.750;
                    var rPhasePoint = rInInner * 0.650;
                    var sizePalaceNum = rOutOuter * 0.055;
                    var sizeStarFlag = rOutOuter * 0.07;
                    var sizePlanetFlag = rOutOuter * 0.075;
                    var sizePhasePoint = rInInner * 0.008;
                    //4、开始画图
                    drawFreshDoublePan();
                    function drawFreshDoublePan() {
                        drawRingOrigin(ctx, rOutOuter, "#000000");//星盘底色
                        //1、画底图：四个圈、刻度线
                        drawRingOrigin(ctx, rOutOuter, freshColor.colorBackground);
                        drawCircleOrigin(ctx, rOutOuter, freshColor.colorCircle);
                        drawCircleOrigin(ctx, rOutInner, freshColor.colorCircle);
                        drawRingOrigin(ctx, rInOuter, 'white');
                        drawCircleOrigin(ctx, rInOuter, freshColor.colorCircle);
                        drawCircleOrigin(ctx, rInInner, freshColor.colorCircle);
                        for (var i = 0; i < 360; i++) {
                            var color = (i % 5 == 0) ? freshColor.colorCircle : freshColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInOuter, -rOutInner, i / 360, color);
                        }
                        //2、画宫位相关：宫位线、宫位编号、星座边界线、星座
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            if (i % 3 == 0) {
                                drawLineAngle(ctx, 0, -rOutOuter, angle, freshColor.colorXudian);
                            } else { drawLineAngle(ctx, 0, -rInOuter, angle, freshColor.colorPalaceLine); }
                            drawTextAngle(ctx, rInMid, (angle + angleNext) / 2, i + 1, colorPalace[i], sizePalaceNum);
                            var angleStar = -palace[0][0] / 360 + i / 12;//星座边界的角度(-x轴逆时针方向)
                            drawLineAngle(ctx, -rOutInner, -rOutOuter, angleStar, freshColor.colorCircle);
                            drawIconAngle(ctx, rOutMid, angleStar + 1 / 24, iconStar[i], colorStar[i], sizeStarFlag);
                        }
                        //3、画行星相关:行星名称、行星点位、名称与点位连线、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planet1[i][5];
                            var angleOriginal = planet1[i][1];
                            drawIconAngle(ctx, rPlanet1Flag, angleAdjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angleOriginal, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angleOriginal, rPlanet1Flag - sizePlanetFlag / 2, angleAdjust, colorPlanet[i], 0);
                            var angle2Adjust = planet2[i][5];
                            var angle2Original = planet2[i][1];
                            drawIconAngle(ctx, rPlanet2Flag, angle2Adjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angle2Original, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angle2Original, rPlanet2Flag - sizePlanetFlag / 2, angle2Adjust, colorPlanet[i], 0);
                        }
                        //4、画相位
                        phaseDouble.forEach(function (item) {
                            if (item[4] != 'he') {
                                drawLineConn(ctx, rPhasePoint, planet2[item[0]][1], rPhasePoint, planet1[item[1]][1], "#" + item[2], item[5], sizeDashed);
                            }
                        })
                    }
                }
                //双盘温暖版
                function drawWarmDouble(objDouble) {
                    //1、双盘数据
                    var { palace, phaseDouble, planet1, planet2 } = dealDoublePlanet(objDouble, 0.024, configItem);
                    //2、文字版颜色
                    var freshColor = getWramColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.835;
                    var rInOuter = rOutOuter * 0.792;
                    var rInInner = rOutOuter * 0.720;
                    var rOutMid = (rOutInner + rOutOuter) / 2;
                    var rInMid = (rInInner + rInOuter) / 2;
                    var rPlanet2Flag = rInInner * 0.887;
                    var rPlanet1Flag = rInInner * 0.750;
                    var rPhasePoint = rInInner * 0.650;
                    var sizePalaceNum = rOutOuter * 0.055;
                    var sizeStarFlag = rOutOuter * 0.07;
                    var sizePlanetFlag = rOutOuter * 0.075;
                    var sizePhasePoint = rInInner * 0.008;
                    //4、开始画图
                    drawWarmDoublePan();
                    function drawWarmDoublePan() {
                        drawRingOrigin(ctx, rOutOuter, "#000000");//星盘底色
                        //1、画底图：四个圈、刻度线
                        drawRingOrigin(ctx, rOutOuter, freshColor.colorBackground);
                        drawCircleOrigin(ctx, rOutOuter, freshColor.colorCircle);
                        drawCircleOrigin(ctx, rOutInner, freshColor.colorCircle);
                        drawRingOrigin(ctx, rInOuter, 'white');
                        drawCircleOrigin(ctx, rInOuter, freshColor.colorCircle);
                        drawCircleOrigin(ctx, rInInner, freshColor.colorCircle);
                        for (var i = 0; i < 360; i++) {
                            var color = (i % 5 == 0) ? freshColor.colorCircle : freshColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInOuter, -rOutInner, i / 360, color);
                        }
                        //2、画宫位相关：宫位线、宫位编号、星座边界线、星座
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            if (i % 3 == 0) {
                                drawLineAngle(ctx, 0, -rOutOuter, angle, freshColor.colorXudian);
                            } else { drawLineAngle(ctx, 0, -rInOuter, angle, freshColor.colorPalaceLine); }
                            drawTextAngle(ctx, rInMid, (angle + angleNext) / 2, i + 1, colorPalace[i], sizePalaceNum);
                            var angleStar = -palace[0][0] / 360 + i / 12;//星座边界的角度(-x轴逆时针方向)
                            drawLineAngle(ctx, -rOutInner, -rOutOuter, angleStar, freshColor.colorCircle);
                            drawIconAngle(ctx, rOutMid, angleStar + 1 / 24, iconStar[i], colorStar[i], sizeStarFlag);
                        }
                        //3、画行星相关:行星名称、行星点位、名称与点位连线、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planet1[i][5];
                            var angleOriginal = planet1[i][1];
                            drawIconAngle(ctx, rPlanet1Flag, angleAdjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angleOriginal, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angleOriginal, rPlanet1Flag - sizePlanetFlag / 2, angleAdjust, colorPlanet[i], 0);
                            var angle2Adjust = planet2[i][5];
                            var angle2Original = planet2[i][1];
                            drawIconAngle(ctx, rPlanet2Flag, angle2Adjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angle2Original, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angle2Original, rPlanet2Flag - sizePlanetFlag / 2, angle2Adjust, colorPlanet[i], 0);
                        }
                        //4、画相位
                        phaseDouble.forEach(function (item) {
                            if (item[4] != 'he') {
                                drawLineConn(ctx, rPhasePoint, planet2[item[0]][1], rPhasePoint, planet1[item[1]][1], "#" + item[2], item[5], sizeDashed);
                            }
                        })
                    }
                }
                //双盘黑夜版
                function drawDarkDouble(objDouble) {
                    //1、双盘数据
                    var { palace, phaseDouble, planet1, planet2 } = dealDoublePlanet(objDouble, 0.024, configItem);
                    //2、文字版颜色
                    var darkColor = getDarkColor();
                    //3、布局&尺寸，以rOutOuter为基准
                    var rOutInner = rOutOuter * 0.835;
                    var rInOuter = rOutOuter * 0.792;
                    var rInInner = rOutOuter * 0.720;
                    var rOutMid = (rOutInner + rOutOuter) / 2;
                    var rInMid = (rInInner + rInOuter) / 2;
                    var rPlanet2Flag = rInInner * 0.887;
                    var rPlanet1Flag = rInInner * 0.750;
                    var rPhasePoint = rInInner * 0.650;
                    var sizePalaceNum = rOutOuter * 0.055;
                    var sizeStarFlag = rOutOuter * 0.07;
                    var sizePlanetFlag = rOutOuter * 0.075;
                    var sizePhasePoint = rInInner * 0.008;
                    //4、开始画图
                    drawDarkDoublePan();
                    function drawDarkDoublePan() {
                        //1、画底图：四个圈、刻度线
                        drawRingOrigin(ctx, rOutOuter, darkColor.colorRingOut);
                        drawCircleOrigin(ctx, rOutOuter, darkColor.colorCircle);
                        drawCircleOrigin(ctx, rOutInner, darkColor.colorCircle);
                        drawRingOrigin(ctx, rInOuter, darkColor.colorRingMid);
                        drawCircleOrigin(ctx, rInOuter, darkColor.colorCircle);
                        drawRingOrigin(ctx, rInInner, darkColor.colorRingIn);
                        drawCircleOrigin(ctx, rInInner, darkColor.colorCircle);
                        for (var i = 0; i < 360; i++) {
                            var color = (i % 5 == 0) ? darkColor.colorCircle : darkColor.colorPalaceLine;
                            drawLineAngle(ctx, -rInOuter, -rOutInner, i / 360, color);
                        }
                        //2、画宫位相关：宫位线、宫位编号、星座边界线、星座
                        for (var i = 0; i < 12; i++) {
                            var angle = palace[i][1];
                            var angleNext = (i == 11) ? 1 : palace[i + 1][1];
                            if (i % 3 == 0) {
                                drawLineAngle(ctx, 0, -rOutOuter, angle, darkColor.colorXudian);
                            } else { drawLineAngle(ctx, 0, -rInOuter, angle, darkColor.colorPalaceLine); }
                            drawTextAngle(ctx, rInMid, (angle + angleNext) / 2, i + 1, colorPalace[i], sizePalaceNum);
                            var angleStar = -palace[0][0] / 360 + i / 12;//星座边界的角度(-x轴逆时针方向)
                            drawLineAngle(ctx, -rOutInner, -rOutOuter, angleStar, darkColor.colorCircle);
                            drawIconAngle(ctx, rOutMid, angleStar + 1 / 24, iconStar[i], colorStar[i], sizeStarFlag);
                        }
                        //3、画行星相关:行星名称、行星点位、名称与点位连线、分
                        for (var i = 0; i < 18; i++) {
                            if (configItem[i] === 0) { break; }
                            var angleAdjust = planet1[i][5];
                            var angleOriginal = planet1[i][1];
                            drawIconAngle(ctx, rPlanet1Flag, angleAdjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angleOriginal, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angleOriginal, rPlanet1Flag - sizePlanetFlag / 2, angleAdjust, colorPlanet[i], 0);
                            var angle2Adjust = planet2[i][5];
                            var angle2Original = planet2[i][1];
                            drawIconAngle(ctx, rPlanet2Flag, angle2Adjust, iconPlanet[i], colorPlanet[i], sizePlanetFlag);
                            drawCircleAngle(ctx, rPhasePoint, angle2Original, sizePhasePoint, colorPlanet[i]);
                            drawLineConn(ctx, rPhasePoint, angle2Original, rPlanet2Flag - sizePlanetFlag / 2, angle2Adjust, colorPlanet[i], 0);
                        }
                        //4、画相位
                        phaseDouble.forEach(function (item) {
                            if (item[4] != 'he') {
                                drawLineConn(ctx, rPhasePoint, planet2[item[0]][1], rPhasePoint, planet1[item[1]][1], "#" + item[2], item[5], sizeDashed);
                            }
                        })
                    }
                }
                function drawProFirdaria(objSingle) {
                    drawProSingle(objSingle);
                }
                window.drawAstrolabe = drawAstrolabe
            })(window);
        },
      drawXingPan: function(){
        var canvas = this.$refs.astrolabe;
        canvas.setAttribute('width', this.width*2);
        canvas.setAttribute('height', this.width*2);
        canvas.style.width = this.width + 'px';
        canvas.style.height = this.width + 'px';
        var ctx = canvas.getContext('2d');
        var draw;
        switch(this.type){
          case 0: {//单盘
            draw = drawAstrolabe(ctx, this.width, this.configItem, '单盘', this.panStyle);
        draw(dealSingleData(this.data1.split(',')));
            break;
          }case 1: {//双盘
            draw = drawAstrolabe(ctx, this.width, this.configItem, '双盘', this.panStyle);
        draw(dealDoubleData(this.data1.split(','), this.data2.split(',')));
            break;
          }case 2: {//配对盘
            draw = drawAstrolabe(ctx, this.width, this.configItem, '单盘', this.panStyle);
        var objSynastry1 = dealSingleData(this.data1.split(','));
            var objSynastry2 = dealSingleData(this.data2.split(','));
            var objSynastry = {
            palace: objSynastry1.palace,
              planet: objSynastry2.planet,
              phase: objSynastry2.phase,
            }
            draw(objSynastry);
            break;
          }case 3: {//法达盘
            draw = drawAstrolabe(ctx, this.width, this.configItem, '法达', '专业');
        draw(dealSingleData(this.data1.split(',')));
            break;
          }default: {
            break;
          }
        }
      },
    }
  }
</script>
