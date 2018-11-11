/*****************1、工具函数——canvas画图*****************/
/**********************画图片**********************/
//angle为-x轴逆时针角度
//pix为图片像素
function drawImgAngle(ctx,src,r,angle,size){
  var x = r * Math.cos(Math.PI*2 * (0.5 - angle));
  var y = r * Math.sin(Math.PI*2 * (0.5 - angle));
  drawImg(ctx, src, x - size/2, y - size/2, size, size);
}
//画图片src到{x,y}
function drawImg(ctx,src,x,y,width,height){
  loadImg(src, function(img){
    ctx.drawImage(img, x, y, width, height);
  });
}
//加载图片，cb处理
function loadImg(src,cb){
  var img = new Image();
  img.src = src;
  img.onload = function(){
    cb(img);
  }
}
/**********************画填充圆弧**********************/
//以(rAngle为半径x轴顺时针angle圈的点)为中心，画一个半径为r的圆弧
function drawRingArcAngle(ctx,rAngle,angle,r,color,angleStart,angleEnd){
  var x = rAngle * Math.cos(Math.PI*2 * angle);
  var y = rAngle * Math.sin(Math.PI*2 * angle);
  drawRing(ctx, x, y, r, color, angleStart, angleEnd);
}
//以原点为中心，画一个半径为r的圆弧
function drawRingArcOrigin(ctx,r,color,angleStart,angleEnd){
  drawRing(ctx, 0, 0, r, color, angleStart, angleEnd);
}
//以(rAngle为半径x轴顺时针angle圈的点)为中心，画一个半径为r的圆
function drawRingAngle(ctx,rAngle,angle,r,color){
  var x = rAngle * Math.cos(Math.PI*2 * angle);
  var y = rAngle * Math.sin(Math.PI*2 * angle);
  drawRing(ctx, x, y, r, color, 0, 1);
}
//以原点为中心，画一个半径为r的圆
function drawRingOrigin(ctx,r,color){
  drawRing(ctx, 0, 0, r, color, 0, 1);
}
//以{x,y}为中心，画一个半径为r的圆
function drawRing(ctx,x,y,r,color,angleStart,angleEnd){
  ctx.save();
  ctx.beginPath();
  ctx.fillStyle = color;
  ctx.arc(x, y, r, Math.PI*2 * angleStart, Math.PI*2 * angleEnd, false);
  if((angleEnd - angleStart) % 1 != 0){
    ctx.lineTo(x, y);//如果不能围成一个闭合的圆,就画一个圆弧
  }
  ctx.fill();
  ctx.restore();
}
/**********************画圆圈**********************/
//以(rAngle为半径-x轴逆时针angle圈的点)为中心，画一个半径为r的圆圈
function drawCircleAngle(ctx,rAngle,angle,r,color){
  var x = rAngle * Math.cos(Math.PI*2 * (0.5 - angle));
  var y = rAngle * Math.sin(Math.PI*2 * (0.5 - angle));
  drawCircle(ctx, x, y, r, color);
}
//以原点为中心，画一个半径为r的圆圈
function drawCircleOrigin(ctx,r,color){
  drawCircle(ctx, 0, 0, r, color);
}
//以{x,y}为中心，画一个半径为r的圆圈
function drawCircle(ctx,x,y,r,color){
  ctx.save();
  ctx.beginPath();
  ctx.strokeStyle = color;
  ctx.arc(x, y, r, 0, Math.PI*2, false);
  ctx.stroke();
  ctx.restore();
}
/**********************画文本**********************/ 
function drawIconAngle(ctx,r,angle,text,color,size){
  var x = r * Math.cos(Math.PI*2 * (0.5 - angle));
  var y = r * Math.sin(Math.PI*2 * (0.5 - angle));
  drawText(ctx, x, y, text, color, size, 'iconfont');
}
//以r为半径，-x轴逆时针angle圈位置显示文本text颜色为color
function drawTextAngle(ctx,r,angle,text,color,size){
  var x = r * Math.cos(Math.PI*2 * (0.5 - angle));
  var y = r * Math.sin(Math.PI*2 * (0.5 - angle));
  drawText(ctx, x, y, text, color, size, 'Calibri');
}
//在{x,y}处显示文本text颜色为color
function drawText(ctx,x,y,text,color,size,family){
  ctx.save();
  ctx.fillStyle = color;
  ctx.font = size + "px " + family;
  if(family == 'iconfont'){
    // ctx.font = 'bold ' + ctx.font;
    ctx.strokeStyle = color;
    ctx.strokeText(text, x - ctx.measureText(text).width/2, y + size/3);
  }
  ctx.fillText(text, x - ctx.measureText(text).width/2, y + size/3);
  ctx.restore();
}
/**********************画线**********************/
//画一条半径r的圆上-x轴逆时针角度angle1到angle2的直线,线型为grade
function drawLineConn(ctx,r1,angle1,r2,angle2,color,grade,sizeGap){
  var x1 = r1 * Math.cos(Math.PI*2 * (0.5 - angle1));
  var y1 = r1 * Math.sin(Math.PI*2 * (0.5 - angle1));
  var x2 = r2 * Math.cos(Math.PI*2 * (0.5 - angle2));
  var y2 = r2 * Math.sin(Math.PI*2 * (0.5 - angle2));
  drawLine(ctx, x1, y1, x2, y2, color, grade, sizeGap);
}
//画一条从{x1,0}到{x2,0}的线(默认线型)，逆时针旋转angle圈
function drawLineAngle(ctx,x1,x2,angle,color){
  ctx.save();
  ctx.rotate(Math.PI*2 * (1 - angle));
  drawLine(ctx, x1, 0, x2, 0, color)
  ctx.restore();
}
//画一条{x1,y1}到{x2,y2}的线
function drawLine(ctx,x1,y1,x2,y2,color,grade,sizeGap){
  ctx.save();
  ctx.beginPath();
  ctx.strokeStyle = color;
  switch(grade){
    case 0://默认
    break;
    case 1://第一档，2px&实线
    ctx.lineWidth = 2;
    break;
    case 2://第二档，1px&实线
    ctx.lineWidth = 1;
    break;
    case 3://第三档，1px&4/1分虚线
    ctx.lineWidth = 1;
    ctx.setLineDash([sizeGap * 2, sizeGap]);
    break;
    case 4://第四档，1px&2/1分虚线
    ctx.lineWidth = 1;
    ctx.setLineDash([sizeGap * 1, sizeGap]);
    break;
    case 5://第五档，1px&1/1分虚线
    ctx.lineWidth = 1;
    ctx.setLineDash([sizeGap * 0.5, sizeGap]);
    // ctx.lineDashOffset = 0;
    break;
    default://使用默认值
    break;
  }
  ctx.lineTo(x1, y1);
  ctx.lineTo(x2, y2);
  ctx.stroke();
  ctx.restore();
}

// export {drawCircleAngle, drawCircleOrigin, drawRingOrigin, drawIconAngle, drawTextAngle, drawLineConn, drawLineAngle}