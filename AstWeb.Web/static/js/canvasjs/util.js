/*-------------------------尚未分类-------------------------*/
function getAssert(prefix){
  return function(condition, warning){
    if(!condition){
      throw new Error(`[${prefix} error]: ${warning}` )
    }
  }
}
function getType(arg){
  return Object.prototype.toString.call(arg).slice(8,-1)
}

/*---------------------------数组相关--------------------------*/
//删除数组指定位置（index从0开始）的项目
function delArrayItem(arr, index) {
	return arr.slice(0, index).concat(arr.slice(index + 1));
}