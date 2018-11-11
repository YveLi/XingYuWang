// import {delArrayItem, getAssert, getType} from './util.js'

const signName1 = ['羊','牛','子','蟹','狮','女','秤','蝎','射','羯','瓶','鱼']
const signName2 = ['白羊','金牛','双子','巨蟹','狮子','处女','天秤','天蝎','射手','摩羯','水瓶','双鱼']
const signName3 = ['白羊座','金牛座','双子座','巨蟹座','狮子座','处女座','天秤座','天蝎座','射手座','摩羯座','水瓶座','双鱼座']
const xingxing = ['太阳','月亮','水星','金星','火星','木星','土星','天王星','海王星','冥王星','上升','天顶']
var xingpanArr={};
const sunSignTable = [
  [[3,21],[4,19]], //白羊
  [[4,20],[5,20]], //金牛
  [[5,21],[6,21]], //双子
  [[6,22],[7,22]], //巨蟹
  [[7,23],[8,22]], //狮子
  [[8,23],[9,22]], //处女
  [[9,23],[10,23]], //天秤
  [[10,24],[11,22]], //天蝎
  [[11,23],[12,21]], //射手
  [[12,22],[1,19]], //摩羯
  [[1,20],[2,18]], //水瓶
  [[2,19],[3,20]], //双鱼
]


var assert = getAssert('NatalData')

/**2.1、单盘星体重叠处理
  入口参数：
    objSingle [待处理单盘数据对象]
    argleGap [处理后星体间隔角度]
    configItem [要显示的星体配置]
  出口参数：{palace, phaseSingle, planetSingle}
**/
const dealSinglePlanet = function(objSingle, argleGap, configItem) {
  var planetSingle = objSingle.planet.map(function(item){return item.concat();});
  var palace = objSingle.palace.map(function(item){return item.concat();});
  var phaseSingle = objSingle.phase.map(function(item){return item.concat();});
  //2、调整重叠行星角度
  var planetDeal = dealPlanet(planetSingle, argleGap, configItem);
  planetDeal.forEach(function(e){
    planetSingle[e[0]][5] = e[1];
  });
  if(planetSingle[10][0] != palace[0][0]){//上升点与第一宫不重叠
    var gapDegree = planetSingle[10][0] - palace[0][0];
    var gapAngle = gapDegree/360;
    planetSingle.forEach(function(item){
      item[1] += gapAngle;
      item[5] += gapAngle;
    })
  }
  return {planetSingle: planetSingle, phaseSingle: phaseSingle, palace: palace};
}
/**2.2、双盘星体重叠处理
  入口参数：
    objDouble [待处理双盘数据对象]
    argleGap [处理后星体间隔角度]
    configItem [要显示的星体配置]
  出口参数：{palace, phaseDouble, planet1, planet2}
**/
const dealDoublePlanet = function(objDouble, argleGap, configItem) {
  var palace = objDouble.palace.map(function(item){return item.concat();});
  var phaseDouble = objDouble.phase.map(function(item){return item.concat();});
  var planet1 = objDouble.planet1.map(function(item){return item.concat();});
  var planet2 = objDouble.planet2.map(function(item){return item.concat();});
  var angleGap = (planet2[10][0] - planet1[10][0])/360;//行运角度都要加上该角度差
  //2、调整重叠行星角度
  var planetDeal = dealPlanet(planet1, argleGap, configItem);
  planetDeal.forEach(function(e){
    planet1[e[0]][5] = e[1];
  });
  planetDeal = dealPlanet(planet2, argleGap, configItem);
  planetDeal.forEach(function(e){
    planet2[e[0]][5] = e[1];
  });
  planet2.forEach(function(item){
    item[1] += angleGap;
    item[5] += angleGap;
  });
  return {palace: palace, phaseDouble: phaseDouble, planet1:planet1, planet2: planet2};
}
/**a>处理行星重叠问题(行星重叠)
  入口参数：
    planetArr  星体数据数组
    configItem  [要显示的星体配置]
    minGap  [两行星之间最小角度间隔]
  出口参数：
    [(星体编号,调整后的角度),……]
  注意：在这里最小角度的星体必须是0°、上升点，故双盘的数据都传原始数据，去重叠后再偏转
**/
function dealPlanet(planetArr,minGap,configItem = [1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0]){
  //1、获取要处理的数据并排序
  var dealArr = planetArr.map(function(item,index){return [index, item[5]];});
  for(var i = configItem.length - 1; i >11; i--){
    if(configItem[i] != 1){
      dealArr = delArrayItem(dealArr,i);
    }
  }
  dealArr.sort(function(a,b){return ((a[1] > b[1]) ? 1 : -1);});
  //2、去重叠处理
  //2.1整体把控：以上升点为基点，所有点位限定取值范围。
  for(var i = 0; i < dealArr.length; i++){
    if(dealArr[i][1] < minGap * i){
      dealArr[i][1] = minGap * i;
    }//角度过小的调大
    else if(dealArr[i][1] > (1 - minGap * (dealArr.length - i))){
      for(var j = i; j < dealArr.length; j++){
        dealArr[j][1] = 1 - minGap * (dealArr.length - j);
      }
    }//角度过大的调小
  }
  //2.2获取仍然重叠的行星区段并处理重叠
  //调整区段：优先以区段中间偏大点为基点，调整区段内其他点位至最小间隔。
  //二重重叠：从小到大区段调整；若会与前面冲突，则以前面的为基点；若会与后面冲突，则后面被冲突的也同时调整。
  var arrOver = findOverLapp(dealArr, minGap);
  arrOver.forEach(function(e){
    //1、选基点——该区段中间偏大角，若会与前面调好的重叠 则以前面调好的为基点
    var mid = parseInt((e[0] + e[1] + 0.5)/2);
    if((dealArr[mid][1] - minGap * (mid - e[0]))
      < (dealArr[e[0] - 1][1] + minGap)){
      mid = e[0] - 1;
    }
    //2、基于基点调整该区段之标准间隔
    for(var i = e[0]; i <= e[1]; i++){
      dealArr[i][1] = dealArr[mid][1] + minGap * (i - mid);
    }
    //3、若该区段的调整导致与后面的重叠 则调整后面重叠的部分至标准间隔
    for(var i = e[1]; i < 11; i++){
      if(dealArr[i + 1][1] - dealArr[i][1] < minGap){
        dealArr[i + 1][1] = dealArr[i][1] + minGap;
      }
      else{break;}
    }
  })
  //处理完成返回处理成果
  return dealArr;
}
//检索数组arrSearch中间隔小于minGap的区段
function findOverLapp(arrSearch,minGap){
  var length = arrSearch.length;
  for(var i = 0; i < length; i++){//arrSearch[i][2]为true表示与前面的重叠，arrSearch[i][3]为true表示与后面的重叠
    arrSearch[i][2] = (minGap > Math.abs(arrSearch[i][1] - arrSearch[(i - 1 + length) % length][1]));
    arrSearch[i][3] = (minGap > Math.abs(arrSearch[i][1] - arrSearch[(i + 1) % length][1]));
  }
  var overLapp = [];
  var num = -1;
  var begin = false;
  for(var i = 0; i < length; i++){
    if(!begin){//如果还未重叠，寻找重叠起点，终点预设为-1
      if(arrSearch[i][2] == false && arrSearch[i][3] == true){
        begin = true;
        overLapp[++num] = [i, -1];
      }
    }
    else{//如果已经开始重叠，寻找重叠终点
      if(arrSearch[i][2] == true && arrSearch[i][3] == false){
        overLapp[num][1] = i;
        begin = false;
      }
    }
  }
  if(num == -1){return overLapp;}
  if(overLapp[num][1] == -1){//如果最后一段重叠未找到终点，则返回第一个点开始查找
    for(var i = 0; i < length; i++){
      if(arrOver[i][2] == true && arrOver[i][3] == false){
        overLapp[num][1] = i;
      }
    }
  }
  return overLapp;
}
/**1.1、单盘数据处理——字符串转对象
  入口参数：单盘数据源数组arrData。
    相位计算getPhaseDoubleStr(arrData, arrData).
  出口参数：{planet:planetCvs, palace:palaceCvs, phase:phaseCvs};
**/
const dealSingleData = function(arrData){
  assert(getType(arrData) === 'Array', 'dealSingleData传入的参数应该为数组')

  var planetUse = [];//[原始数据、理论偏转角度、对应星座、度、分、实际偏转角度]
  var palaceUse = [];//[原始数据、理论偏转角度、对应星座、度、分、实际偏转角度]
  var phaseUse = [];//[星体1、星体2、颜色、误差、线型]
  //获取后端返回的原始数据
  var planetData = [];
  var palaceData = [];
  var phaseData = [];
  for(var i = 0; i < 10; i++){
    planetData[i] = parseFloat(arrData[i]);
  }
  planetData[10] = parseFloat(arrData[16]);
  planetData[11] = parseFloat(arrData[25]);
  for(var i = 10; i < 16; i++) {
    planetData[i + 2] = parseFloat(arrData[i]);
  }
  for(var i = 16; i < 22; i++){
    palaceData[i - 16] = parseFloat(arrData[i]);
    palaceData[i - 16 + 6] = parseFloat(arrData[i]) + 180;
  }
  phaseData = getPhaseDoubleStr(arrData, arrData).split("|");
  //处理得到画图可用的数据
  for(var i = 0; i < 18; i++){//度数、理论偏转、星座、度、分[、实际偏转、宫位]
    planetUse[i] = calAngle(planetData[i], palaceData[0]);
    planetUse[i][5] = planetUse[i][1];
    if(i < 12){palaceUse[i] = calAngle(palaceData[i], palaceData[0]);}
  }
  var planetPalace = calPlanetPalace(planetUse, palaceUse);
  for(var i = 0; i < 18; i++){
    planetUse[i][6] = planetPalace[i];
  }
  for(var i = 0; i < phaseData.length; i++){
    var arrPhase = calLineWeight(phaseData[i]);
    if(arrPhase.length > 1){
      phaseUse.push(arrPhase);
    }
  }
  var objDan = {planet:planetUse, palace:palaceUse, phase:phaseUse};

  // 行星相位
  var phasedl=""; phasetitle="",phaseli="",phaselast=0,phasethis=0;
  for(var i=0;i<phaseUse.length;i++){ 
      var bgcolor='';
      switch (phaseUse[i][2]) {
        case 'FFFF00':
        bgcolor='d0ad7c'

          break;
        case '0000FF':
        bgcolor='c36a6a'
          break;
        case '03F5FE':
        bgcolor='729e82'
          break;
        case '09ff11':
        bgcolor='8da8d9'
          break;
        case 'ff1111':
        bgcolor='c36a6a'
          break;
      }
      if(i==0){
        phaselast=phaseUse[i][0];
        phasethis=phaseUse[i][0];
        phasetitle='<dt class="icon_cl1"><span>'+xingxing[phaseUse[i][0]]+'</span></dt>';
        phaseli='<dd style="background:#'+bgcolor+'"><span>'+phaseUse[i][4]+xingxing[phaseUse[i][1]]+'</span></dd>'
      }else{
        phasethis=phaseUse[i][0];
        if(phasethis==phaselast){
          phaseli+='<dd style="background:#'+bgcolor+'"><span>'+phaseUse[i][4]+xingxing[phaseUse[i][1]]+'</span></dd>'
          phaselast=phaseUse[i][0];
          
        }else{
          phasedl='<dl class="clear" >'+phasetitle+phaseli+'</dl>'
          $('#xxlist1').append(phasedl);
          phaseli="";
          phasetitle='<dt class="icon_cl1"><span>'+xingxing[phaseUse[i][0]]+'</span></dt>';
          phaseli+='<dd style="background:#'+bgcolor+'"><span>'+phaseUse[i][4]+xingxing[phaseUse[i][1]]+'</span></dd>'
          
          phaselast=phaseUse[i][0];
          
        }
      }
  }
  // 行星位置
  var planetli=""
  for(var i=0;i<12;i++){
    planetli+='<dl><dt class="icon_cl1"><span>'+xingxing[i]+'</span></dt><dd>'+signName3[planetUse[i][2]]+'<em>'+planetUse[i][3]+'°'+planetUse[i][4]+'′</em><small>第'+planetUse[i][6]+'宫</small></dd></dl>'
  }
  $('#xxlist2').append(planetli)

  //各宫位置
  var palaceli="",gong="";
  for(var i=0;i<12;i++){
    if(i==0){
      gong='<small>上升</small>'
    }else if(i==3){
      gong='<small>天底</small>'
    }else if(i==6){
      gong='<small>下降</small>'
    }else if(i==9){
      gong='<small>天顶</small>'
    }else{
      gong=''
    }
    palaceli+='<dl><dt class="icon_cl1"><span>第'+(i+1)+'宫</span></dt><dd>'+signName3[palaceUse[i][2]]+'<em>'+palaceUse[i][3]+'°'+palaceUse[i][4]+'′</em>'+gong+'</dd></dl>'
  }
  $('#xxlist3').append(palaceli)
  console.log(palaceUse);
  return objDan;
}

// 获得星座的名字
function getSignName(index, wordNum = 3){
  return [signName1, signName2, signName3][wordNum - 1][index]
}

// 获得太阳、月亮、上升星座
function getMainSign(natal_data, wordNum){
  let planet = dealSingleData(natal_data).planet
  let sunSign = planet[0][2]
  let moonSign = planet[1][2]
  let upSign = planet[10][2]
  
  return {
    sunSign: getSignName(sunSign, wordNum),
    moonSign: getSignName(moonSign, wordNum),
    upSign: getSignName(upSign, wordNum)
  }
}

// 解析日期字符串为对象
function parseDateString(string){
  assert(string, 'parseDateString未传入参数')
  assert(string.indexOf('T') > -1, 'parseDateString 传入的参数错误')
  var [ymd, hms] = string.split('T')
  var [year, month, date] = ymd.split('-').map(item=>+item)
  var [hour, minute, second] = hms.split(':').map(item=>+item)
  return {year, month, date, hour, minute, second}
}

// 比较有权重关系的两个数组
function compareArr(arr1, arr2){
  assert(Array.isArray(arr1) && Array.isArray(arr2), '方法compareArr的参数应该为数组')
  assert(arr1.length === arr2.length, '方法compareArr  数组arr1的长度 !== 数组arr2的长度' )

  //1大，0和，-1小
  var compareSingle = (a,b)=>a > b ? 1 : a === b ? 0 : -1

  // 0 平， 1大， -1小
  for(let i = 0;i < arr1.length;i++){
    var compareResult = compareSingle(arr1[i], arr2[i])
    if(compareResult !== 0 || i === arr1.length - 1){
      return compareResult
    }
  }
}

// 根据月和日，来获得太阳星座
function getSunSign(month, date, wordNum = 3){
  for(let i = 0;i < sunSignTable.length;i++){
    let beginDate = sunSignTable[i][0]
    let endDate = sunSignTable[i][1]
    let moveDate = [month, date]
    let signName = getSignName(i, wordNum)
    // 处理摩羯12.22-01.19
    if(beginDate[0] === 12){
      if(compareArr(moveDate, beginDate) >= 0 && compareArr(moveDate, [12,31]) <= 0){
        return signName
      }

      if(compareArr(moveDate, [1,1]) >= 0 && compareArr(moveDate, [12,31]) <= 0){
        return signName
      }
    }

    let moreThanBegin = compareArr(moveDate, beginDate) >= 0
    let lessThanEnd = compareArr(moveDate, endDate) <= 0
    if(moreThanBegin && lessThanEnd){
      return signName
    }
  }
}
/**1.2、双盘数据处理——字符串转对象
  入口参数：双盘数据源数组(arrData1, arrData2)、是否是本命内isInNatal
    相位计算getPhaseDoubleStr(arrData1, arrData2).
  出口参数：{planet1:planetCvs1, planet2:planetCvs2, palace:palaceCvs, phase:phaseCvs};
**/
const dealDoubleData = function(arrData1,arrData2){
  var planetUse1 = [];//[原始数据、理论偏转角度、对应星座、度、分、实际偏转角度]
  var planetUse2 = [];//[原始数据、理论偏转角度、对应星座、度、分、实际偏转角度]
  var palaceUse = [];//[原始数据、理论偏转角度、对应星座、度、分、实际偏转角度]
  var phaseUse = [];//[星体1、星体2、颜色、误差、线型]
  //获取后端返回的原始数据
  var planetData1 = [];
  var planetData2 = [];
  var palaceData = [];
  var phaseData = [];
  phaseData = getPhaseDoubleStr(arrData2, arrData1).split("|");
  for(var i = 0; i < 10; i++){
    planetData1[i] = parseFloat(arrData1[i]);
    planetData2[i] = parseFloat(arrData2[i]);
  }
  planetData1[10] = arrData1[16];
  planetData1[11] = arrData1[25];
  planetData2[10] = arrData2[16];
  planetData2[11] = arrData2[25];
  for(var i = 10; i < 16; i++) {
    planetData1[i + 2] = parseFloat(arrData1[i]);
    planetData2[i + 2] = parseFloat(arrData2[i]);
  }
  for(var i = 16; i < 22; i++){
    palaceData[i - 16] = parseFloat(arrData1[i]);
    palaceData[i - 16 + 6] = parseFloat(arrData1[i]) + 180;
  }
  //处理得到画图可用的数据
  for(var i = 0; i < 18; i++){//度数、理论偏转、星座、度、分[、实际偏转、宫位]
    planetUse1[i] = calAngle(planetData1[i], planetData1[10]);
    planetUse1[i][5] = planetUse1[i][1];
    planetUse2[i] = calAngle(planetData2[i], planetData2[10]);
    planetUse2[i][5] = planetUse2[i][1];
    if(i < 12){palaceUse[i] = calAngle(palaceData[i], palaceData[0]);}
  }
  var planetPalace1 = calPlanetPalace(planetUse1, palaceUse);
  for(var i = 0; i < 18; i++){
    planetUse1[i][6] = planetPalace1[i];
  }
  var planetPalace2 = calPlanetPalace(planetUse2, palaceUse);
  for(var i = 0; i < 18; i++){
    planetUse2[i][6] = planetPalace2[i];
  }
  for(var i = 0; i < phaseData.length; i++){
    var arrPhase = calLineWeight(phaseData[i]);
    if(arrPhase.length > 1){
      phaseUse.push(arrPhase);
    }
  }
  var objDouble = {planet1:planetUse1, planet2:planetUse2, palace:palaceUse, phase:phaseUse};
  return objDouble;
}
/**a.1>根据度数计算对应星座和角度
  入口参数：相对星座度数、第一宫位相对星座度数
  出口参数：[相对星座度数,理论偏转角度,对应星座,度,分,实际偏转角度]
**/
function calAngle(dushu, ascDu){
  var angle = [
    dushu,//原始度数
    (((dushu - ascDu) / 360) + 1) % 1,//理论偏转角度
    0 | ((dushu/30) % 12),//取整——对应星座
    0 | (dushu%30),//取整——度
    0 | (dushu%1 * 60) + 0.5,//四舍五入——分
    (((dushu - ascDu) / 360) + 1) % 1,//实际偏转角度
  ];
  return angle;
}
/**a.2>计算对应宫位
  入口参数：处理后的星体数据arrPlanet、处理后的宫位数据arrPalace
  出口参数：星体对应的宫位[]
**/
const calPlanetPalace = function(arrPlanet, arrPalace){
  var planetPalace = [];
  for(var i = 0; i < arrPlanet.length; i++){
    for(var j = 0; j < 12; j++){
      if(arrPlanet[i][1] < arrPalace[j][1]){
        planetPalace[i] = j;
        break;
      }
      if(j === 11){
        planetPalace[i] = 12;
      }
    }
  }
  return planetPalace;
}
/**b>根据相位字符串解析相位线权重
  入口参数：相位数据
  出口参数：相位数组[星体1,星体2,颜色,误差,相位类型,线型]
**/
function calLineWeight(xiangwei){
  var arrPhase = xiangwei.split("_");
  if(arrPhase.length > 2){
    if(arrPhase[0] < 12 && arrPhase[1] < 12){
      var grade = 0, space = arrPhase[3];
      if(space <= 0.1){
        grade = 1;
      }else if(space <= 0.25){
        grade = 2;
      }else if(space <= 0.45){
        grade = 3;
      }else if(space <= 0.7){
        grade = 4;
      }else{grade = 5;}
      arrPhase[5] = grade;
      return arrPhase;
    }
  }
  return [];
}
/**c>根据绘盘原始数据计算得到相位字符串
  入口参数：数据源(arr1, arr2)
  出口参数：相位字符串“planet1_planet2_color_误差_相位类型|next|……”
**/
function getPhaseDoubleStr(arr1, arr2) {
  var starTemp1 = [];
  var starTemp2 = [];
  for(var i = 0; i < 10; i++){
    starTemp1[i] = arr1[i];
    starTemp2[i] = arr2[i];
  }
  starTemp1[10] = arr1[16];
  starTemp1[11] = arr1[25];
  starTemp2[10] = arr2[16];
  starTemp2[11] = arr2[25];
  var phaseDoubleStr = "";
  for (var i = 0; i < starTemp1.length; i++) {
    for (var j = 0; j < starTemp2.length; j++) {
      if ((arr1 == arr2) && (i == j)){ continue; }
      var sname = getPhaseType(i, j, starTemp1[i] - starTemp2[j]);
      var cha = 0;
      var colors = "";
      var phaseName = "0";
      if (sname != 0) {
        switch (sname) {
          case 1:
            colors = "FFFF00";
            cha = getPhaseDeviate(1, starTemp1[i] - starTemp2[j]);
            // phaseName = "he";
            phaseName = "合";
            break;
          case 2:
            colors = "0000FF";
            cha = getPhaseDeviate(2, starTemp1[i] - starTemp2[j]);
            // phaseName = "dui";
            phaseName = "冲";
            break;
          case 4:
            colors = "ff1111";
            cha = getPhaseDeviate(4, starTemp1[i] - starTemp2[j]);
            // phaseName = "si";
            phaseName = "刑";
            break;
          case 3:
            colors = "09ff11";
            cha = getPhaseDeviate(3, starTemp1[i] - starTemp2[j]);
            // phaseName = "san";
            phaseName = "拱";
            break;
          case 6:
            colors = "03F5FE";
            cha = getPhaseDeviate(6, starTemp1[i] - starTemp2[j]);
            // phaseName = "liu";
            phaseName = "六";
            break;
          case 5:
            colors = "ff0fff";
            cha = getPhaseDeviate(5, starTemp1[i] - starTemp2[j]);
            // phaseName = "mei";
            phaseName = "梅";
            break;
        }
        var dtemp = cha / getRongXu(i, j);
        phaseDoubleStr = phaseDoubleStr + i + "_" + j + "_" + colors + "_" + dtemp.toFixed(4) + "_" + phaseName + "|";
      }
    }
  }
  return phaseDoubleStr;
}
/**c.1>获取相位类型
  根据星体编号和星体角度差，获取相位类型
  入口参数：星体1编号p1、星体2编号p2、星体度数差f(能保证是正数吗？)
  出口参数：返回符合的相位类型(f与相位标准度数的差值 要小于 星体容许度/(8-该相位容许度/2))
  引用外部参数：相位容许度数组aspectOrb
**/
function getPhaseType(p1, p2, f) {
  var orb = getRongXu(p1, p2);
  var tempar = aspectOrb.slice(0);
  var xingname = 0;
  if (Math.abs(f - 0) < orb / (8 - Math.floor(tempar[0] / 2)) || (Math.abs(f - 360) < orb / (8 - Math.floor(tempar[0] / 2)))) {             //合
    xingname = 1;
  }
  if (Math.abs(f - 180) < orb / (8 - Math.floor(tempar[1] / 2))) {                                                              //冲
    xingname = 2;
  }
  if (Math.abs(f - 60) < (orb / (8 - Math.floor(tempar[2] / 2))) || (Math.abs(f - 300) < (orb / (8 - Math.floor(tempar[2] / 2))))) {        //六合
    xingname = 6;
  }
  if (Math.abs(f - 150) < (orb / (8 - Math.floor(tempar[3] / 2))) || (Math.abs(f - 210) < (orb / (8 - Math.floor(tempar[3] / 2))))) {       //梅花
    xingname = 5;
  }
  if (Math.abs(f - 90) < (orb / (8 - Math.floor(tempar[4] / 2))) || (Math.abs(f - 270) < (orb / (8 - Math.floor(tempar[4] / 2))))) {        //四分
    xingname = 4;
  }
  if (Math.abs(f - 120) < (orb / (8 - Math.floor(tempar[5] / 2))) || (Math.abs(f - 240) < (orb / (8 - Math.floor(tempar[5] / 2))))) {       //三分
    xingname = 3;
  }
  return xingname;
}
//相位容许度
let aspectOrb = ['15','15','8','1','12','12'];
//星体的容许度
let nBei = 1;
let Liuorb = [15 / nBei, 12 / nBei, 7 / nBei, 7 / nBei, 7 / nBei, 9 / nBei, 9 / nBei, 6 / nBei, 6 / nBei, 6 / nBei, 3 / nBei, 3 / nBei, 3 / nBei, 3 / nBei, 3 / nBei, 1 / nBei];
//根据星体编号，返回星体间容许度
function getRongXu(p1, p2) {
  var aaa = Liuorb[p1] / 2;
  var bbb = Liuorb[p2] / 2;
  var orb = aaa + bbb;
  return orb;
}
/**c.2>获取相位误差
  入口参数：a 相位类型；f星体度数差值(能保证是正数吗？)
  出口参数：星体度数差值相对于相位指定度数的差值
**/
function getPhaseDeviate(a, f) {
  var aaa = 0;
  switch (a) {
    case 0:
      aaa = 0;
      break;
    case 1:
      aaa = Math.abs(f - 0);
      break;
    case 2:
      aaa = Math.abs(f - 180);
      break;
    case 3:
      if (f > 180) {
        aaa = Math.abs(f - 240);
      }
      else {
        aaa = Math.abs(f - 120);
      }
      break;
    case 4:
      if (f > 180) {
        aaa = Math.abs(f - 270);
      }
      else {
        aaa = Math.abs(f - 90);
      }
      break;
    case 5:
      if (f > 180) {
        aaa = Math.abs(f - 210);
      }
      else {
        aaa = Math.abs(f - 150);
      }
      aaa = 0;
      break;
    case 6:
      if (f > 180) {
        aaa = Math.abs(f - 300);
      }
      else {
        aaa = Math.abs(f - 60);
      }
      break;
  }
  return aaa;
}

// export {dealSingleData, dealDoubleData, dealSinglePlanet, dealDoublePlanet, getSignName, getMainSign, parseDateString, getSunSign}