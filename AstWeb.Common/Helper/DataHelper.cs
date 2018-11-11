using System.Collections.Generic;

namespace AstWeb.Common.Helper
{
    public class DataHelper
    {
        public static Dictionary<string, object> GetResult(bool success, string message, object data = null)
        {
            var d = new Dictionary<string, object>
            {
                { "success",success },
                { "message",message}
            };
            if (data != null)
                d.Add("data", data);

            return d;
        }

        /// <summary>
        /// 返回适合Fly模板的格式
        /// </summary>
        /// <param name="status">状态 1为正常</param>
        /// <param name="msg">提示信息</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetResult(int status, string msg)
        {
            return new Dictionary<string, object>
            {
                { "status",status },
                { "msg",msg}
            };
        }
        /// <summary>
        /// 返回适合Fly模板的格式
        /// </summary>
        /// <param name="status">状态 0为正常</param>
        /// <param name="msg">提示信息</param>
        /// <param name="dict">其他数据的集合</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetResult(int status, string msg,Dictionary<string,object> dict)
        {
            dict.Add("status", status);
            dict.Add("msg", msg);
            return dict;
        }
        /// <summary>
        /// 返回适合Fly模板的格式
        /// </summary>
        /// <param name="status">状态 0为正常</param>
        /// <param name="msg">提示信息</param>
        /// <param name="action">跳转地址</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetResult(int status, string msg,string action)
        {
            return new Dictionary<string, object>
            {
                { "status",status },
                { "msg",msg},
                { "action",action}
            };
        }
        /// <summary>
        /// 返回适合Fly模板的格式
        /// </summary>
        /// <param name="status">状态 0为正常</param>
        /// <param name="msg">提示信息</param>
        /// <param name="code">数据</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetResult(int status, string msg, object rows)
        {
            return new Dictionary<string, object>
            {
                { "status",status },
                { "msg",msg},
                { "rows",rows}
            };
        }
        /// <summary>
        /// 返回适合Fly模板的格式
        /// </summary>
        /// <param name="status">状态 0为正常</param>
        /// <param name="msg">提示信息</param>
        /// <param name="rows">数据</param>
        /// <param name="code">代码</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetResult(int status, string msg, object rows, string code)
        {
            return new Dictionary<string, object>
            {
                { "status",status },
                { "msg",msg},
                { "rows",rows},
                { "code",code}
            };
        }

    }
}
