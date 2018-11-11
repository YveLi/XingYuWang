using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Astrology.Common
{
    public class SysConfig
    {
        /// <summary>
        /// 是否是正式线
        /// </summary>
        public const bool IsOnLine = false;

        /// <summary>
        /// FTP 一号服务器 
        /// </summary>
        public class FtpS1
        {
            /// <summary>
            /// ftp 连接地址
            /// </summary>
            public const string FtpUrl = "101.132.160.116:21";

            /// <summary>
            /// ftp 登录帐号
            /// </summary>
            public const string FtpLoginId = "Administrator";

            /// <summary>
            /// ftp 登录密码
            /// </summary>
            public const string FtpLoginPwd = "Astro666";

            /// <summary>
            /// ftp Astrology存储图片目录名称
            /// </summary>
            public const string FtpGyImgFileName = "Astro_img";

            /// <summary>
            /// ftp Astrology存储影音视频目录名称
            /// </summary>
            public const string FtpGyImgMediaFileName = "Astro_media";

            /// <summary>
            /// ftp远程上传身份token校验
            /// </summary>
            public const string FileUploadToken = "P0inSoftW11sONxdX";
        }
        /// <summary>
        /// 空间相册站点
        /// </summary>
        public class FileSpaceServer
        {
            public const string Url = "http://img1.xingzuoxingyu.com/";
            public const string ImgFileName = "Astro_img/";
            public const string ImgMediaFileName = "Astro_media/";
        }
        public class SMS
        {
            //容联云 短信入口 手机号码：13910080990， 密码：kaixin2006
            //public const string AccountSid = "8aaf070858a8910b0158a98756d202f6";
            //public const string Authtoken = "437e73853b6f4c838f5918f3ce75338a";
            //public const string AppId = "8aaf070858a8910b0158a987578f02fb";
            //public const string TempleId = "147259";

            //星座心语
            public const string AccountSid = "8a48b5514b6f8ca0014b710471db0012";
            public const string Authtoken = "200947ba793c4cf28545ce86d5bb4eb8";
            public const string AppId = "8aaf07085f9eb021015fbd3d8ac709d8";
            public const string TempleId0 = "217673";//注册短信验证码
            public const string TempleId1 = "217674";//修改密码短信验证码
            //test
            //public const string AccountSid = "8aaf07085def8a38015df0dd787500b8";
            //public const string Authtoken = "d46419efabd94b48a34b93a404c35cd8";
            //public const string AppId = "8aaf07085def8a38015df0dd7bb200be";
            //public const string TempleId = "147259";


            public const string RestUrl = "app.cloopen.com";
            public const string RestPort = "8883";

            public const string SoftVer = "2013-12-26";
            public const string Type = "json";

        }
    }
}
