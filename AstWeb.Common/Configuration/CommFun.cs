using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using System.Xml;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using Astrology.Common;

namespace Astrology.Common
{

    public static class CommFun
    {
        static char[] character = { '1', '2', '3', '4', '5', '6', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'z' };
        static Random rnd = new Random();
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5(string str)
        {
            var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            return hashPasswordForStoringInConfigFile != null ? hashPasswordForStoringInConfigFile.ToUpper() : "";
        }

        /// <summary>
        /// 获取session的用户信息 By Xuwx Add 201702262145
        /// </summary>
        /// <returns></returns>
        public static T GetSession<T>(string key) where T : new()
        {
            T t = new T();
            if (HttpContext.Current.Session[key] != null)
                t = (T)HttpContext.Current.Session[key];
            return t;
        }

        /// <summary>
        /// 转换成 json 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetRandomFileName(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                System.Threading.Thread.Sleep(100);
                string fileType = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                Random rand = new Random();
                DateTime now = DateTime.Now;
                string str = string.Empty;
                str += now.Year.ToString(CultureInfo.InvariantCulture);
                str += now.Month.ToString(CultureInfo.InvariantCulture);
                str += now.Day.ToString(CultureInfo.InvariantCulture);
                str += now.Hour.ToString(CultureInfo.InvariantCulture);
                str += now.Minute.ToString(CultureInfo.InvariantCulture);
                str += now.Second.ToString(CultureInfo.InvariantCulture);
                str += rand.Next(0, 1000);
                return str + fileType;
            }
            else
            {
                return "";
            }
        }


        public static string GetRandomCode(int len)
        {
            string chkCode = string.Empty;
            if (len <= 0)
                return chkCode;
            for (int i = 0; i < len; i++)
            {
                chkCode += character[rnd.Next(character.Length)];
            }
            return chkCode;
        }
        /// <summary>
        /// 定义一个静态的int型变量，保存在服务器内存中，作为Random的种子
        /// </summary>
        public static int staticRndCount = int.MaxValue;
        //随机数字
        public static string GetRndNum(int Length)
        {
            char[] constant = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);

            staticRndCount--;//要减，否则会出错，以为已经定义int.MaxValue
            if (staticRndCount < 0) staticRndCount = int.MaxValue;

            Random rd = new Random(staticRndCount);
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(10)]);
            }
            return newRandom.ToString();
        }
        #region "过滤sql语句"
        /// <summary>
        /// 过滤sql语句
        /// </summary>
        /// <param name="str">传入的字符串</param>
        /// <returns></returns>
        public static string FilterSql(object str)
        {
            if (str == null)
            {
                return "";
            }
            else
            {
                string id = str.ToString().Trim();
                id = id.Replace("'", "");
                id = id.Replace(" and ", "");
                id = id.Replace("select ", "");
                id = id.Replace("update ", "");
                id = id.Replace(" chr ", "");
                id = id.Replace(" delete ", "");
                id = id.Replace("%20from", "");
                id = id.Replace(";", "");
                id = id.Replace("insert ", "");
                id = id.Replace(" mid ", "");
                id = id.Replace("set", "");
                id = id.Replace("chr(37)", "");
                id = id.Replace("=", "");
                id = id.Replace("(", "");
                id = id.Replace("exec%20master.dbo.xp_cmdshell", "");
                id = id.Replace("xp_cmdshell", "");
                id = id.Replace("net localgroup administrators", "");
                return id;
            }
        }
        #endregion
        #region 高性能字符串截取方法
        /// <summary>
        /// 高性能字符串截取方法
        /// </summary>
        /// <param name="str">被截取字符串</param>
        /// <param name="length">截取的长度</param>
        /// <returns></returns>
        public static string GetSubString(string str, int length)
        {
            string temp = "";
            if (str != null)
            {
                temp = str;
                int j = 0;
                int k = 0;
                for (int i = 0; i < temp.Length; i++)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(temp.Substring(i, 1), @"[\u4e00-\u9fa5]+"))
                    {
                        j += 2;
                    }
                    else
                    {
                        j += 1;
                    }
                    if (j <= length)
                    {
                        k += 1;
                    }
                    if (j >= length)
                    {
                        return temp.Substring(0, k);
                    }
                }
            }
            return temp;
        }
        #endregion

        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="str">提示语言</param> 
        /// <param name="url">跳转的地址("" 返回，none 不跳转)</param>
        /// <returns></returns>
        public static string Show_Message(string str, string url)
        {
            string returnMsg = "";
            if (url == "")
            {
                returnMsg = "<script type='text/javascript'>alert('" + str + "');history.go(-1);</script>";
            }
            else if (url == "0")
            {
                returnMsg = "<script type='text/javascript'>alert('" + str + "');window.close(0);</script>";
            }
            else if (url == "none")
            {
                returnMsg = "<script type='text/javascript'>alert('" + str + "');</script>";
            }
            else
            {
                if (str == "")
                {
                    returnMsg = "<script type='text/javascript'>window.location=('" + url + "');</script>";
                }
                else
                {
                    returnMsg = "<script type='text/javascript'>alert('" + str + "');window.location=('" + url + "');</script>";
                }
            }
            HttpContext.Current.Response.Write(returnMsg);
            if (url != "none") HttpContext.Current.Response.End();
            return "";
        }





        /// <summary>
        /// 无条件跳转至相应页面
        /// </summary>
        /// <param name="strUrl"></param>
        public static void ResponseToUrl(string strUrl)
        {
            try
            {
                HttpContext.Current.Response.Redirect(strUrl);
            }
            catch
            {
            }
        }

        //是否是整数型
        public static bool CheckInt(string str)
        {
            if (str == null)
            {
                return false;
            }
            else
            {
                long re = 0;
                return Int64.TryParse(str, out re);
            }
        }

        //是否是数字
        public static bool CheckNum(string str)
        {
            if (str == "" || str == null)
            {
                return false;
            }
            else
            {
                str = str.Replace(".", "");
                return CheckInt(str);
            }
        }


        ///   <summary> 
        ///   移除HTML标签 
        ///   </summary> 
        ///   <param name="HTMLStr">HTMLStr</param> 
        public static string RemoveHTMLTags(string HTMLStr)
        {
            return System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
        }

        /// <summary>
        /// MD5加密(对接外部系统) 32位大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        //public static string Md5(string str)
        //{
        //    var returnStr = FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToUpper();
        //    return returnStr;
        //}

        /// <summary>
        /// Token加密
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string UrlTokenEncode(string strValue)
        {
            if (strValue != "")
            {
                strValue = "/w\\@#!" + strValue;
                byte[] by = System.Text.Encoding.UTF8.GetBytes(strValue.ToCharArray());
                strValue = HttpServerUtility.UrlTokenEncode(by);
            }
            return strValue;
        }

        /// <summary>
        /// Token解密
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string UrlTokenDecode(string strValue)
        {
            if (strValue != "")
            {
                byte[] byteHidDcsql = HttpServerUtility.UrlTokenDecode(strValue);
                strValue = System.Text.Encoding.UTF8.GetString(byteHidDcsql);
                strValue = strValue.Replace("/w\\@#!", "");
            }
            return strValue;
        }

        //循环整除时，获取字符串
        public static string GetStrByZero(string Num1, string Num2, string Str)
        {
            if (CheckInt(Num1))
            {
                if (int.Parse(Num1) % int.Parse(Num2) == 0)
                {
                    return Str;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        //截取字符串(长度包括endStr的长度)
        public static string StrLeft(string s, int l, string endStr)
        {
            string temp = s.Substring(0, (s.Length < l) ? s.Length : l);

            if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l)
            {
                return temp;
            }
            for (int i = temp.Length; i >= 0; i--)
            {
                temp = temp.Substring(0, i);
                if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l - endStr.Length)
                {
                    return temp + endStr;
                }
            }
            return endStr;
        }

        //返回当前时间+随机字符串
        public static string GetDateTimeRndStr()
        {
            string returnStr = DateTime.Now.ToString();
            returnStr = returnStr.Replace(" ", "");
            returnStr = returnStr.Replace("-", "");
            returnStr = returnStr.Replace(":", "");
            returnStr += GetRndNum(3);
            return returnStr;
        }


        /// <summary>
        /// 返回带@参数的字符串(1 插入，2 更新)
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static string GetStrPara(string Str, int Type)
        {
            string[] sArray = Str.Split(',');
            string returnStr = "";
            foreach (string i in sArray)
            {
                if (Type == 1)
                {
                    returnStr += "@" + i.ToString().Trim() + ", ";
                }
                else
                {
                    returnStr += i.ToString().Trim() + " = @" + i.ToString().Trim() + ", ";
                }
            }
            returnStr = returnStr.Trim();
            return returnStr.TrimEnd(',');
        }

        //设置COOKIES(无KEY子键的)
        public static object Set_Cookies(string strCookieName, string strCookieValue, int ExpiresYears)
        {
            if (ExpiresYears < 0)
            {
                HttpContext.Current.Response.Cookies[strCookieName].Expires = DateTime.Now.AddYears(ExpiresYears * 100);
            }
            else if (ExpiresYears > 0)
            {
                HttpContext.Current.Response.Cookies[strCookieName].Expires = DateTime.Now.AddYears(ExpiresYears);
            }
            HttpContext.Current.Response.Cookies[strCookieName].Value = HttpContext.Current.Server.UrlEncode(strCookieValue);
            return null;
        }

        //读取COOKIES(无KEY子键的)
        public static string Get_Cookies(string strCookieName)
        {
            string strCookieValue = "";
            try
            {
                if (HttpContext.Current.Request.Cookies[strCookieName] == null)
                {
                    strCookieValue = "";
                }
                else
                {
                    strCookieValue = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[strCookieName].Value);
                }
            }
            catch
            { }
            return strCookieValue;
        }


        //返回http-URL格式
        public static string Get_Http_Url(string strUrl)
        {
            strUrl = Regex.Replace(strUrl, "http://", "", RegexOptions.IgnoreCase);
            return "http://" + strUrl;
        }

        //SQL语句IN
        public static string Get_Str_Sql_In(string Str)
        {
            string strSql;
            if (string.IsNullOrEmpty(Str))
            {
                strSql = "0";
            }
            else
            {
                Str = Str.TrimEnd(',');
                //D1003, D1004
                Str = Str.Replace(", ", ",");
                strSql = Str.Replace(",", "','");
            }
            return strSql;
        }

        /// <summary> 
        /// 得到一个月的第一天 
        /// 并且时间为00：00：00
        /// </summary> 
        /// <param name="someday">这个月的随便一天</param> 
        /// <returns>DateTime</returns> 
        public static DateTime GetFirstDayOfMonth(DateTime someDay)
        {
            DateTime result;
            int Ts = 1 - someDay.Day;
            result = someDay.AddDays(Ts).Date;
            return result;
        }

        /// <summary> 
        /// 得到一个月的最后一天 
        /// 并且时间为23：59：59
        /// </summary> 
        /// <param name="someday">这个月的随便一天</param> 
        /// <returns>DateTime</returns> 
        public static DateTime GetLastDayOfMonth(DateTime someDay)
        {
            int totalDays = DateTime.DaysInMonth(someDay.Year, someDay.Month);
            DateTime result;
            int Ts = totalDays - someDay.Day;
            result = someDay.AddDays(Ts);
            return result.Date.AddDays(1).AddSeconds(-1);
        }

        //远程页面源代码
        public static string Get_Remote_Page(string Url, string UrlEncoding)
        {
            HttpWebRequest webRequest = WebRequest.Create(Url) as HttpWebRequest;
            webRequest.AllowAutoRedirect = false;//请求是否应跟随重定向响应，不要设置为true，否则如果目标url不存在，或者错误时，会运行很久
            HttpWebResponse webResponse = webRequest.GetResponse() as HttpWebResponse;
            StreamReader Sr = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding(UrlEncoding));
            return Sr.ReadToEnd();
        }


        /// <summary>
        /// 字符串剪切(取start_Str 和 end_Str之间的字符串)
        /// </summary>
        /// <param name="Str">被剪切的字符串</param>
        /// <param name="start_Str">开始字符串</param>
        /// <param name="end_Str">结束字符串</param>
        /// <returns></returns>
        public static string Get_Cut_From_Str(string Str, string start_Str, string end_Str)
        {
            Str = Str.Trim();
            int x = 0, y = 0, z = 0;
            x = Str.IndexOf(start_Str);
            if (x >= 0)
            {
                z = start_Str.Length;
                string Str_X = Str.Substring(x + z, Str.Length - x - z).Trim();
                y = Str_X.IndexOf(end_Str);
                if (y > 0)
                {
                    return Str_X.Substring(0, y).Trim();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取域名(结尾不带/)
        /// </summary>
        /// <param name="AppPath">是否返回虚拟目录名称</param>
        /// <returns></returns>
        public static string Get_Url_Host(bool AppPath)
        {
            if (AppPath == false)
            {
                return HttpContext.Current.Request.Url.Host.TrimEnd('/');
            }
            else
            {
                return (HttpContext.Current.Request.Url.Host + HttpContext.Current.Request.ApplicationPath).TrimEnd('/');
            }
        }

        /// <summary>
        /// 输出格式不能有换行和引号的JS
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Get_Safe_Js_Str(string str)
        {
            str = str.Replace("\"", "“");
            str = str.Replace("\n", " "); //换行
            str = str.Replace("\r", " "); //回车
            str = str.Replace("\t", " "); //制表定位符
            str = str.Replace("\r\n", " ");
            return str;
        }

        /// <summary>
        /// 判断链接是否来自外部
        /// </summary>
        /// <returns></returns>
        public static bool IsOutsidePost()
        {
            string str1 = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            string str2 = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            return ((str1 == null) || (str1.IndexOf(str2) != 7));
        }

        /// <summary>
        /// <summary>
        /// 自定义的替换字符串函数， 2011-09-07 修改升级
        /// 修改要点：递归替换字符串，例如：SourceString="aaa;;bbb;;;ccc"，要求替换后 SourceString = "aaa;bbb;ccc"
        /// </summary>
        /// </summary>
        /// <param name="SourceString"></param>
        /// <param name="SearchString"></param>
        /// <param name="ReplaceToString"></param>
        /// <param name="IsCaseInsensetive">true 为指定不区分大小写的匹配。</param>
        /// <returns></returns>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceToString, bool IsCaseInsensetive)
        {
            string result = SourceString;
            //条件判断一下，防止递归死循环
            if (string.IsNullOrEmpty(SourceString) || string.IsNullOrEmpty(SearchString) || SearchString == ReplaceToString) return result;
            result = Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceToString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
            if (result.Contains(SearchString))
            {
                //注意这时的SourceString赋值为result，否则死循环
                result = ReplaceString(result, SearchString, ReplaceToString, IsCaseInsensetive);
            }
            return result;
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// Code-39条形码
        /// </summary>
        /// <param name="str">输入的字符串</param>
        /// <param name="ch">要显示条形码的高度</param>
        /// <param name="cw">要显示条形码的宽度</param>
        /// <param name="unit">长度单位：(px,pt)</param>
        /// <returns></returns>
        public static string Get_BarCode39(object str, int ch, int cw, string unit)
        {
            //str:输入的字符串;ch:要显示条形码的高度;cw:要显示条形码的宽度
            str = "*" + str + "*"; //头尾需要加*
            string strTmp = str.ToString();
            strTmp = strTmp.ToUpper();
            int height = ch;
            int width = cw;

            //将传入的参数进行转化为39码(Code-39)
            strTmp = strTmp.Replace("0", "_|_|__||_||_|");
            strTmp = strTmp.Replace("1", "_||_|__|_|_||");
            strTmp = strTmp.Replace("2", "_|_||__|_|_||");
            strTmp = strTmp.Replace("3", "_||_||__|_|_|");
            strTmp = strTmp.Replace("4", "_|_|__||_|_||");
            strTmp = strTmp.Replace("5", "_||_|__||_|_|");
            strTmp = strTmp.Replace("7", "_|_|__|_||_||");
            strTmp = strTmp.Replace("6", "_|_||__||_|_|");
            strTmp = strTmp.Replace("8", "_||_|__|_||_|");
            strTmp = strTmp.Replace("9", "_|_||__|_||_|");
            strTmp = strTmp.Replace("A", "_||_|_|__|_||");
            strTmp = strTmp.Replace("B", "_|_||_|__|_||");
            strTmp = strTmp.Replace("C", "_||_||_|__|_|");
            strTmp = strTmp.Replace("D", "_|_|_||__|_||");
            strTmp = strTmp.Replace("E", "_||_|_||__|_|");
            strTmp = strTmp.Replace("F", "_|_||_||__|_|");
            strTmp = strTmp.Replace("G", "_|_|_|__||_||");
            strTmp = strTmp.Replace("H", "_||_|_|__||_|");
            strTmp = strTmp.Replace("I", "_|_||_|__||_|");
            strTmp = strTmp.Replace("J", "_|_|_||__||_|");
            strTmp = strTmp.Replace("K", "_||_|_|_|__||");
            strTmp = strTmp.Replace("L", "_|_||_|_|__||");
            strTmp = strTmp.Replace("M", "_||_||_|_|__|");
            strTmp = strTmp.Replace("N", "_|_|_||_|__||");
            strTmp = strTmp.Replace("O", "_||_|_||_|__|");
            strTmp = strTmp.Replace("P", "_|_||_||_|__|");
            strTmp = strTmp.Replace("R", "_||_|_|_||__|");
            strTmp = strTmp.Replace("Q", "_|_|_|_||__||");
            strTmp = strTmp.Replace("S", "_|_||_|_||__|");
            strTmp = strTmp.Replace("T", "_|_|_||_||__|");
            strTmp = strTmp.Replace("U", "_||__|_|_|_||");
            strTmp = strTmp.Replace("V", "_|__||_|_|_||");
            strTmp = strTmp.Replace("W", "_||__||_|_|_|");
            strTmp = strTmp.Replace("X", "_|__|_||_|_||");
            strTmp = strTmp.Replace("Y", "_||__|_||_|_|");
            strTmp = strTmp.Replace("Z", "_|__||_||_|_|");
            strTmp = strTmp.Replace("-", "_|__|_|_||_||");
            strTmp = strTmp.Replace("*", "_|__|_||_||_|");
            strTmp = strTmp.Replace("/", "_|__|__|_|__|");
            strTmp = strTmp.Replace("%", "_|_|__|__|__|");
            strTmp = strTmp.Replace("+", "_|__|_|__|__|");
            strTmp = strTmp.Replace(".", "_||__|_|_||_|");
            //使用背景，打印时 要设置 Internet选项=>高级=>选定“打印背景颜色和图像
            //strTmp = strTmp.Replace("_", "<span style='height:" + height + unit + "pt;width:" + width + unit + "pt;background:#FFFFFF;'></span>");
            //strTmp = strTmp.Replace("|", "<span style='height:" + height + unit + "pt;width:" + width + unit + "pt;background:#000000;'></span>");
            //使用图片，可以直接打印
            strTmp = strTmp.Replace("_", "<img src='../Images/barCodeW.gif' style='width:" + width + unit + "; height:" + height + unit + ";'>");
            strTmp = strTmp.Replace("|", "<img src='../Images/barCodeB.gif' style='width:" + width + unit + "; height:" + height + unit + ";'>");

            return strTmp;
        }

 

        #region 生成缩略图
        /// <summary>    
        /// 生成缩略图    
        /// </summary>    
        /// <param name="originalImagePath">源图路径（物理路径）</param>    
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>    
        /// <param name="width">缩略图宽度</param>    
        /// <param name="height">缩略图高度</param>    
        /// <param name="fileExtension">图片类型</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int int_Width, int int_Height)
        {

            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            //获取图片类型  
            string fileExtension = System.IO.Path.GetExtension(originalImagePath).ToLower();
            float New_Width; // 新的宽度    
            float New_Height; // 新的高度    
            float Old_Width, Old_Height; //原始高宽 
            int flat = 0;//标记图片是不是等比    
            int xPoint = 0;//若果要补白边的话，原图像所在的x坐标。    
            int yPoint = 0;//y坐标
            //判断图片    

            Old_Width = (float)originalImage.Width;
            Old_Height = (float)originalImage.Height;

            if ((Old_Width / Old_Height) > ((float)int_Width / (float)int_Height)) //当图片太宽的时候    
            {
                New_Height = Old_Height * ((float)int_Width / (float)Old_Width);
                New_Width = (float)int_Width;
                //此时x坐标不用修改    
                yPoint = (int)(((float)int_Height - New_Height) / 2);
                flat = 1;
            }
            else if ((Old_Width / Old_Height) == ((float)int_Width / (float)int_Height))
            {
                New_Width = int_Width;
                New_Height = int_Height;
            }
            else
            {
                New_Width = (int)Old_Width * ((float)int_Height / (float)Old_Height);  //太高的时候   
                New_Height = int_Height;
                //此时y坐标不用修改    
                xPoint = (int)(((float)int_Width - New_Width) / 2);
                flat = 1;
            }
            //新建一个bmp图片    
            System.Drawing.Image bitmap = null;
            if (flat != 1)
            {
                bitmap = new System.Drawing.Bitmap(Convert.ToInt32(New_Width), Convert.ToInt32(New_Height));
            }
            else
            {
                bitmap = new System.Drawing.Bitmap(int_Width, int_Height);
            }
            //新建一个画板    
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法    
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度    
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            if (flat != 1)
            {
                //清空画布并以透明背景色填充    
                g.Clear(Color.Transparent);
                //在指定位置并且按指定大小绘制原图片的指定部分    
                g.DrawImage(originalImage, 0, 0, Convert.ToInt32(New_Width), Convert.ToInt32(New_Height));
            }
            else
            {
                SolidBrush tbBg = new SolidBrush(Color.White);
                g.FillRectangle(tbBg, 0, 0, int_Width, int_Height); //填充为白色 
                //在指定位置并且按指定大小绘制原图片的指定部分    
                g.DrawImage(originalImage, xPoint, yPoint, Convert.ToInt32(New_Width), Convert.ToInt32(New_Height));
            }
            try
            {
                //按原图片类型保存缩略图片,不按原格式图片会出现模糊,锯齿等问题.  
                switch (fileExtension)
                {
                    case ".gif": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif); break;
                    case ".jpg": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                    case ".bmp": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp); break;
                    case ".png": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png); break;
                }
            }
            catch (System.Exception)
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        #endregion

        #region 截取淘宝远程包裹码图片，并改变颜色

        ///<summary>    
        ///截取淘宝远程包裹码图片，并改变颜色    
        ///</summary>    
        ///<param name="originalImagePath">远程图片路径</param>    
        ///<param name="thumbnailPath">新图片路径（物理路径）</param>    
        ///<param name="int_Width">要截取的宽度</param>
        ///<param name="int_Height">要截取的高度</param>
        ///<param name="xPoint">横坐标</param>
        ///<param name="yPoint">纵坐标</param>
        public static void SubImg(string originalImagePath, string thumbnailPath, int int_Width, int int_Height, int xPoint, int yPoint)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(originalImagePath);
            request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
            request.Timeout = 3000;
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                System.Drawing.Image originalImage = System.Drawing.Image.FromStream(stream);
                //获取图片类型  
                string fileExtension = System.IO.Path.GetExtension(originalImagePath).ToLower();
                int x = 0;   //截取X坐标   
                int y = 0;   //截取Y坐标       
                //原图宽与生成图片宽之差，小于0(即原图宽小于要生成的图)时，新图宽度为较小者即原图宽度   X坐标则为0 
                //大于0(即原图宽大于要生成的图)时，新图宽度为设置值即int_Width    X坐标则为   sX与xPoint之间较小者
                int sX = originalImage.Width - int_Width;
                int sY = originalImage.Height - int_Height;
                if (sX > 0)
                {
                    x = sX > xPoint ? xPoint : sX;
                }
                else
                {
                    int_Width = originalImage.Width;
                }
                //Y方向同理
                if (sY > 0)
                {
                    y = sY > yPoint ? yPoint : sY;
                }
                else
                {
                    int_Height = originalImage.Height;
                }

                //新建一个bmp图片    
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(int_Width, int_Height);
                //新建一个画板    
                Graphics g = System.Drawing.Graphics.FromImage(bitmap);
                //设置高质量插值法    
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //设置高质量,低速度呈现平滑程度    
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                //在指定位置并且按指定大小绘制原图片的指定部分    
                g.DrawImage(originalImage, 0, 0, new Rectangle(x, y, int_Width, int_Height), GraphicsUnit.Pixel);
                string fileName = System.IO.Path.GetFileName(originalImagePath).ToLower();

                for (int i = 0; i < int_Width; i++)
                {
                    for (int k = 0; k < int_Height; k++)
                    {
                        string Color = bitmap.GetPixel(i, k).Name.ToString();
                        if (Color == "ffef4f2b")
                        {
                            bitmap.SetPixel(i, k, System.Drawing.Color.Black);
                        }
                    }
                }

                try
                {
                    //按原图片类型保存缩略图片,不按原格式图片会出现模糊,锯齿等问题.  
                    switch (fileExtension)
                    {
                        case ".gif": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif); break;
                        case ".jpg": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                        case ".bmp": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp); break;
                        case ".png": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png); break;
                    }
                }
                catch (System.Exception)
                {
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }
                finally
                {
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }
            }
        }

        #endregion


        ///// <summary>
        ///// 简繁体转化
        ///// </summary>
        ///// <param name="str">字符串</param>
        ///// <param name="type">转化类型，0：转化成繁体，1：转化成简体</param>
        ///// <returns></returns>
        //public static string ChineseCovert(string str,int type)
        //{
        //    if (string.IsNullOrEmpty(str))
        //        return "";
        //    else
        //        if (type == 0)
        //        {
        //            return Strings.StrConv(str, VbStrConv.TraditionalChinese, CultureInfo.CurrentCulture.LCID);
        //        }
        //        else
        //        {
        //            return Strings.StrConv(str, VbStrConv.SimplifiedChinese, CultureInfo.CurrentCulture.LCID);
        //        }
        //}

        /// <summary>
        /// 获取XML结点值
        /// </summary>
        /// <param name="Xml">Xml文档</param>
        /// <param name="xPath">结点路径，如："//CreateTime"</param>
        /// <returns></returns>
        public static string Get_Xml_Nodes(XmlDocument Xml, string xPath)
        {
            XmlNodeList Xml_Nodes = Xml.SelectNodes(xPath);
            try
            {
                return Xml_Nodes[0].InnerText.Replace("'", "`");
            }
            catch
            {
                return "";
            }

        }
        /// <summary>
        /// 获取XML结点值
        /// </summary>
        /// <param name="Str">xml，如：XmlNodeList[0].InnerXml</param>
        /// <param name="xPath">结点，如：time这个结点</param>
        /// <returns>值</returns>
        public static string Get_Str_Nodes(string Str, string xPath)
        {
            int x = 0, y = 0, z = 0;
            x = Str.IndexOf("<" + xPath + ">");
            y = Str.IndexOf("</" + xPath + ">");
            z = xPath.Length + 2;
            if (y > x)
            {
                return Str.Substring(x + z, y - x - z).Trim().Replace("'", "`");
            }
            else
            {
                return "";
            }
        }


        #region 检测并删除N天前产生的垃圾文件
        /// <summary>
        /// 检测并删除N天前产生的垃圾文件 www
        /// </summary>
        /// <param name="strPath">文件路径</param>
        /// <param name="intDayNum">设定删除第几天前的文件</param>
        /// <param name="strExtName">指定后缀名</param> 
        /// <returns>删除状态</returns>
        public static Boolean DeleteTempFiles(string strPath, int intDayNum, string strExtName)
        {

            System.IO.DirectoryInfo dif = new System.IO.DirectoryInfo(strPath);
            DateTime DelDate = DateTime.Now.AddDays(-intDayNum);

            if (dif.Exists && dif.GetFiles().Length > 0)
            {

                for (int i = 0; i < dif.GetFiles().Length; i++)
                {
                    FileInfo file = dif.GetFiles()[i];
                    if (file.LastWriteTime.Date <= DelDate)
                    {
                        try
                        {
                            if (strExtName != "*")
                            {
                                if (file.Extension == "." + strExtName)
                                {
                                    file.Delete();
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                file.Delete();
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                return true;
            }
            else
            {
                //表示已经已无可删除的文件
                return false;
            }

        }
        #endregion

        #region 时间控件相关  2011-09-13

        /// <summary>
        /// 绑定小时控件
        /// </summary>
        /// <param name="ddl">小时控件</param>
        public static void DropDownList_TimeHourBind(DropDownList ddl)
        {
            DropDownList_TimeHourBind(ddl, false, "");
        }

        /// <summary>
        /// 绑定小时控件
        /// </summary>
        /// <param name="ddl">小时控件</param>
        /// <param name="selectedValue">选定值</param>
        public static void DropDownList_TimeHourBind(DropDownList ddl, string selectedValue)
        {
            DropDownList_TimeHourBind(ddl, true, selectedValue);
        }

        /// <summary>
        /// 绑定小时控件
        /// </summary>
        /// <param name="ddl">小时控件</param>
        /// <param name="hasSelectedValue">是否有选定值</param>
        /// <param name="selectedValue">选定值</param>
        public static void DropDownList_TimeHourBind(DropDownList ddl, bool hasSelectedValue, string selectedValue)
        {
            for (int i = 0; i < 24; i++)
            {
                ddl.Items.Add(new ListItem(i.ToString() + " 时", i.ToString()));
            }

            if (hasSelectedValue)
            {
                try
                {
                    ddl.Items.FindByValue(selectedValue).Selected = true;
                }
                catch { }
            }
        }

        /// <summary>
        /// 绑定分钟控件
        /// </summary>
        /// <param name="ddl">分钟控件</param>
        public static void DropDownList_TimeMinuteBind(DropDownList ddl)
        {
            DropDownList_TimeMinuteBind(ddl, false, string.Empty);
        }
        /// <summary>
        /// 绑定分钟控件
        /// </summary>
        /// <param name="ddl">分钟控件</param>
        /// <param name="selectedValue">选定值</param>
        public static void DropDownList_TimeMinuteBind(DropDownList ddl, string selectedValue)
        {
            DropDownList_TimeMinuteBind(ddl, true, string.Empty);
        }

        /// <summary>
        /// 绑定分钟控件
        /// </summary>
        /// <param name="ddl">分钟控件</param>
        /// <param name="hasSelectedValue">是否有选定值</param>
        /// <param name="selectedValue">选定值</param>
        public static void DropDownList_TimeMinuteBind(DropDownList ddl, bool hasSelectedValue, string selectedValue)
        {
            for (int i = 0; i < 60; i++)
            {
                ddl.Items.Add(new ListItem(i.ToString() + " 分", i.ToString()));
            }

            if (hasSelectedValue)
            {
                try
                {
                    ddl.Items.FindByValue(selectedValue).Selected = true;
                }
                catch { }
            }

        }

        /// <summary>
        /// 绑定时间
        /// </summary>
        /// <param name="ddlHour">小时控件</param>
        /// <param name="ddlHourHasSelectedValue">小时控件是否有选定值</param>
        /// <param name="ddlHourSelectedValue">小时控件选定值</param>
        /// <param name="ddlMinute">分钟控件</param>
        /// <param name="ddlMinuteHasSelectedValue">分钟控件是否有选定值</param>
        /// <param name="ddlMinuteSelectedValue">分钟控件选定值</param>
        public static void DropDownList_TimeBind(DropDownList ddlHour, DropDownList ddlMinute)
        {
            DropDownList_TimeBind(ddlHour, false, string.Empty, ddlMinute, false, string.Empty);
        }

        /// <summary>
        /// 绑定时间
        /// </summary>
        /// <param name="ddlHour">小时控件</param>
        /// <param name="ddlHourSelectedValue">小时控件选定值</param>
        /// <param name="ddlMinute">分钟控件</param>
        /// <param name="ddlMinuteSelectedValue">分钟控件选定值</param>
        public static void DropDownList_TimeBind(DropDownList ddlHour, string ddlHourSelectedValue, DropDownList ddlMinute, string ddlMinuteSelectedValue)
        {
            DropDownList_TimeHourBind(ddlHour, true, ddlHourSelectedValue);
            DropDownList_TimeMinuteBind(ddlMinute, true, ddlMinuteSelectedValue);
        }

        /// <summary>
        /// 绑定时间
        /// </summary>
        /// <param name="ddlHour">小时控件</param>
        /// <param name="ddlHourHasSelectedValue">小时控件是否有选定值</param>
        /// <param name="ddlHourSelectedValue">小时控件选定值</param>
        /// <param name="ddlMinute">分钟控件</param>
        /// <param name="ddlMinuteHasSelectedValue">分钟控件是否有选定值</param>
        /// <param name="ddlMinuteSelectedValue">分钟控件选定值</param>
        public static void DropDownList_TimeBind(DropDownList ddlHour, bool ddlHourHasSelectedValue, string ddlHourSelectedValue, DropDownList ddlMinute, bool ddlMinuteHasSelectedValue, string ddlMinuteSelectedValue)
        {
            DropDownList_TimeHourBind(ddlHour, ddlHourHasSelectedValue, ddlHourSelectedValue);
            DropDownList_TimeMinuteBind(ddlMinute, ddlMinuteHasSelectedValue, ddlMinuteSelectedValue);
        }


        /// <summary>
        /// 绑定时间控件
        /// </summary>
        /// <param name="txtDate">日期控件</param>
        /// <param name="ddlHour">小时控件</param>
        /// <param name="ddlMinute">分钟控件</param>
        public static void SetTimeToControls(TextBox txtDate, DropDownList ddlHour, DropDownList ddlMinute, DateTime t)
        {
            txtDate.Text = t.ToString("yyyy-MM-dd");
            DropDownList_TimeBind(ddlHour, true, t.Hour.ToString(), ddlMinute, true, t.Minute.ToString());
        }


        #endregion

        /// <summary>
        /// 根据字段值排序
        ///  2012-8-16
        /// 例如希望订单编号按：2012042013497,2012041711672,2012031511946,2012042313065 排序
        /// </summary>
        /// <param name="field">字段名称</param>
        /// <param name="fieldvalue">字段的值</param>
        /// <param name="sort">Asc 或者 Desc</param>
        /// <returns></returns>
        public static string OrderByCharIndex(string field, string fieldvalue, string sort)
        {
            string strOrderBy = "";
            fieldvalue = fieldvalue.Replace(" ", "");
            fieldvalue = fieldvalue.Trim();
            fieldvalue = fieldvalue.Trim(',');

            if (fieldvalue != "")
            {
                strOrderBy = " charIndex(','+cast(" + field + " as varchar)+',', '," + fieldvalue + ",') " + sort;
            }

            return strOrderBy;
        }


        /// <summary>
        /// 执行线程异步回调方法,内部发生异常，iis不受影响 
        /// eg. RunAsyn(test, null);
        /// public void test(object o) { throw new Exception("fdasfdsa"); }
        /// </summary>
        /// <param name="call">异步回调方法</param>
        /// <param name="par">回调方法的传入参数</param>
        public static void RunAsyn(System.Threading.WaitCallback call, object par)
        {
            call.BeginInvoke(par, null, null);
        }

        /// <summary>
        /// 转半角字符串的函数(DBC case) 任意字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(string strText)
        {
            char[] array = strText.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 12288)
                {
                    array[i] = (char)32;
                    continue;
                }

                if (array[i] > 65280 && array[i] < 65375)
                {
                    array[i] = (char)(array[i] - 65248);
                }
            }
            return new string(array);
        }


        /// <summary>
        /// 用Id来获取相应的名称.已获取过的id和name会保存在参数dicts中,这样保证每一个id只连接数据库一次,可减少数据库连接.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dicts">用于临时存储id/name的字典</param>
        /// <param name="getNamebyIdFun">用Id来获取name的方法.参数一为数据库连接,参数二为id,返回结果为该id对应的name.如果dicts的key中不包含参数id,则会调用本方法去获取一次</param>
        /// <returns></returns>
        public static string GetNameById(int id, Dictionary<string, string> dicts, Func<System.Data.SqlClient.SqlConnection, int, string> getNamebyIdFun, Func<System.Data.SqlClient.SqlConnection> getConnFun)
        {
            string name = string.Empty;
            if (dicts.ContainsKey(id.ToString()))
            {
                return dicts[id.ToString()].ToString();
            }
            else
            {
                using (System.Data.SqlClient.SqlConnection conn = getConnFun())
                {
                    name = getNamebyIdFun(conn, id) ?? string.Empty;
                    dicts.Add(id.ToString(), name);
                }
            }

            return name;
        }

        /// <summary>
        /// 手机号码\电话号码最后5位显示星号
        /// </summary>
        /// <param name="mTel"></param>
        /// <returns></returns>
        public static string GetMaskMTel(string mTel)
        {
            if (mTel != "")
            {
                try
                {
                    if (mTel.Length > 6)
                    {
                        mTel = GetSubString(mTel, mTel.Length - 5);
                        mTel = mTel + "*****";
                    }
                }
                catch
                { }
            }
            return mTel;
        }

        /// <summary>
        /// 返回保质期类型对应的字符串
        /// </summary>
        /// <param name="unit">保质期类型</param>
        /// <returns></returns>
        public static string GetGPeriodUnit(string unit)
        {
            string str = "";
            switch (unit)
            {
                case "1":
                    str = "天";
                    break;
                case "2":
                    str = "周";
                    break;
                case "4":
                    str = "月";
                    break;
                case "8":
                    str = "年";
                    break;
            }
            return str;
        }
        /// <summary>
        /// 返回保质期类型对应的字符串
        /// </summary>
        /// <param name="unit">保质期类型</param>
        /// <returns></returns>
        public static string GetGPeriodUnit(int unit)
        {
            string str = "";
            switch (unit)
            {
                case 1:
                    str = "天";
                    break;
                case 2:
                    str = "周";
                    break;
                case 4:
                    str = "月";
                    break;
                case 8:
                    str = "年";
                    break;
            }
            return str;
        }


        /// <summary>
        /// 以两位精度对Datable的列求和
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static decimal SumColumnByTwoScale(DataTable dt, string columnName)
        {
            decimal sum = 0.0m;
            decimal? temp = 0.0m;
            if (dt == null || dt.Rows.Count == 0) return 0.0m;
            if (!dt.Columns.Contains(columnName)) throw new ArgumentException("列" + columnName + "在Datatable中不存在");
            foreach (var row in dt.AsEnumerable())
            {
                temp = row.Field<decimal?>(columnName);
                if (temp.HasValue)
                {
                    sum += Math.Round(temp.Value, 2);
                }
            }
            return Math.Round(sum, 2);
        }


        /// <summary>
        /// 查看当前调用堆栈  
        /// 用法 :
        /// System.Diagnostics.StackFrame[] stacks = new System.Diagnostics.StackTrace().GetFrames(); 
        /// CommFun.StackFrameToString(stacks);
        /// </summary>
        /// <param name="stacks"></param>
        /// <returns></returns>
        public static string StackFrameToString(System.Diagnostics.StackFrame[] stacks)
        {
            string result = string.Empty;
            foreach (System.Diagnostics.StackFrame stack in stacks)
            {
                result += string.Format("{0} {1} {2} {3}\r\n", stack.GetFileName(),
                    stack.GetFileLineNumber(),
                    stack.GetFileColumnNumber(),
                    stack.GetMethod().ToString());
            }
            return result;
        }

        public static void CreateHgFile(string CkName, string CkNo)
        {
            string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", CkName + "_" + CkNo);
            string filepath1 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", CkName + "_" + CkNo, "UPLOAD");
            string filepath2 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", CkName + "_" + CkNo, "UPLOAD_HIS");
            string filepath3 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", CkName + "_" + CkNo, "DOWNLOAD");
            string filepath4 = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", CkName + "_" + CkNo, "DOWNLOAD_HIS");
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            if (!Directory.Exists(filepath1))
            {
                Directory.CreateDirectory(filepath1);
            }
            if (!Directory.Exists(filepath2))
            {
                Directory.CreateDirectory(filepath2);
            }
            if (!Directory.Exists(filepath3))
            {
                Directory.CreateDirectory(filepath3);
            }
            if (!Directory.Exists(filepath4))
            {
                Directory.CreateDirectory(filepath4);
            }
        }


    }
}
