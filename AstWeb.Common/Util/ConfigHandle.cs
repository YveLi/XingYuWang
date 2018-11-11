using AstWeb.Common.Helper;
using AstWeb.Common.Models;
using System.Configuration;

namespace AstWeb.Common.Util
{
    public class ConfigHandle
    {
        public static UploadConfigModel GetUploadConfig()
        {
            UploadConfigModel model = new UploadConfigModel
            {
                UploadWebPath = ConfigurationManager.AppSettings["UploadWebPath"],
                AttachPath = ConfigurationManager.AppSettings["AttachPath"],
                AttachExtension = ConfigurationManager.AppSettings["AttachExtension"],
                AttachFileSize = DataConversion.StrToInt(ConfigurationManager.AppSettings["AttachFileSize"]),
                AttachImgSize = DataConversion.StrToInt(ConfigurationManager.AppSettings["AttachImgSize"]),
                AttachImgMaxHeight = DataConversion.StrToInt(ConfigurationManager.AppSettings["AttachImgMaxHeight"]),
                AttachImgMaxWidth = DataConversion.StrToInt(ConfigurationManager.AppSettings["AttachImgMaxWidth"]),
                ThumbnailHeight = DataConversion.StrToInt(ConfigurationManager.AppSettings["ThumbnailHeight"]),
                ThumbnailWidth = DataConversion.StrToInt(ConfigurationManager.AppSettings["ThumbnailWidth"]),
                WatermarkPosition = DataConversion.StrToInt(ConfigurationManager.AppSettings["WatermarkPosition"]),
                WatermarkImgQuality = DataConversion.StrToInt(ConfigurationManager.AppSettings["WatermarkImgQuality"]),
                //水印暂无
                WatermarkType = 0,
                WatermarkPic = "",
                WatermarkTransparency = 0,
                WatermarkText = "",
                WatermarkFont = "",
                WatermarkFontSize = 0,
            };

            return model;
        }
    }
}
