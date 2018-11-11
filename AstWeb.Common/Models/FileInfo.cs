using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstWeb.Common.Models
{
    public class FileInfo
    {
        public FileInfo()
        {
            code = 0;
            msg = "上传成功";
            data = new Data
            {
                src = "",
                title = ""
            };
        }

        public int code { get; set; }  // 0 -成功  1-失败
        public string msg { get; set; } // 消息
        public Data data { get; set; } // 信息
    }
    public struct Data
    {
        public string src { get; set; } //图片路径
        public string title { get; set; } //图片名称
    }
}
