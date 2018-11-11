using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AstWeb.Models
{
    public class UploadConfigModel
    {
        /// <summary>
        /// 上传站点目录
        /// </summary>
        public string UploadWebPath { get; set; }

        /// <summary>
        /// 附件上传目录
        /// </summary>
        public string AttachPath { get; set; }

        /// <summary>
        /// 附件上传类型
        /// </summary>
        public string AttachExtension { get; set; }

        /// <summary>
        /// 附件保存方式(1:按"年月日"每天一个文件夹,其他：按"年月/日"存入不同的文件夹)
        /// </summary>
        public int AttachSave { get; set; }

        /// <summary>
        /// 文件上传大小
        /// </summary>
        public int AttachFileSize { get; set; }

        /// <summary>
        /// 图片上传大小
        /// </summary>
        public int AttachImgSize { get; set; }

        /// <summary>
        /// 图片最大高度(像素)
        /// </summary>
        public int AttachImgMaxHeight { get; set; }

        /// <summary>
        /// 图片最大宽度(像素)
        /// </summary>
        public int AttachImgMaxWidth { get; set; }

        /// <summary>
        /// 生成缩略图高度(像素)
        /// </summary>
        public int ThumbnailHeight { get; set; }

        /// <summary>
        /// 生成缩略图宽度(像素)
        /// </summary>
        public int ThumbnailWidth { get; set; }

        /// <summary>
        /// 图片水印类型(1：文字，2：图片)
        /// </summary>
        public int WatermarkType { get; set; }

        /// <summary>
        /// 水印位置
        /// </summary>
        public int WatermarkPosition { get; set; }

        /// <summary>
        /// 图片生成质量
        /// </summary>
        public int WatermarkImgQuality { get; set; }

        /// <summary>
        /// 图片水印文件
        /// </summary>
        public string WatermarkPic { get; set; }

        /// <summary>
        /// 水印透明度
        /// </summary>
        public int WatermarkTransparency { get; set; }

        /// <summary>
        /// 水印文字
        /// </summary>
        public string WatermarkText { get; set; }

        /// <summary>
        /// 文字字体
        /// </summary>
        public string WatermarkFont { get; set; }

        /// <summary>
        /// 文字大小(像素)
        /// </summary>
        public int WatermarkFontSize { get; set; }
    }
}