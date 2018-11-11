using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Helper
{
    public class DateHelper
    {
        /// <summary>
        /// 获取两时间差的元组
        /// </summary>
        /// <param name="DateTime1"></param>
        /// <param name="DateTime2"></param>
        /// <returns></returns>
        private static Tuple<int, int, int, int> DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            Tuple<int, int, int, int> tuple = null;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                tuple = new Tuple<int, int, int, int>(ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
            }
            catch
            {

            }
            return tuple;
        }
        /// <summary>
        /// 获取两时间差的文字显示效果 例：刚刚、5分钟前、1小时间、3天前
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static string TimeAgoStr(DateTime dt1, DateTime dt2)
        {
            var t = DateDiff(dt1, dt2);
            if (t == null)
                return "出错了";

            if (t.Item1 > 0 && t.Item1 > 31)
                return dt1.ToString("yyyy-MM-dd");
            else if (t.Item1 > 0)
                return t.Item1 + "天前";
            else if (t.Item2 > 0)
                return t.Item2 + "小时前";
            else if (t.Item3 > 0)
                return t.Item3 + "分钟前";
            else
                return "刚刚";
        }
    }
}
