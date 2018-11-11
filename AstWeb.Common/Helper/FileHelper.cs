using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Web;
using AstWeb.Common.Models;
using AstWeb.Common.Util;

namespace AstWeb.Common.Helper
{
    public class FileHelper
    {
        private UploadConfigModel siteConfig;
        public FileHelper(UploadConfigModel model)
        {
            siteConfig = model;
        }

        /// <summary>
        /// 裁剪图片并保存
        /// </summary>
        public bool cropSaveAs(string fileName, string newFileName, int maxWidth, int maxHeight, int cropWidth, int cropHeight, int X, int Y)
        {
            string fileExt = Utils.GetFileExt(fileName); //文件扩展名，不含“.”
            if (!IsImage(fileExt))
            {
                return false;
            }
            string newFileDir = Utils.GetMapPath(newFileName.Substring(0, newFileName.LastIndexOf(@"/") + 1));

            //检查是否有该路径，没有则创建
            if (!Directory.Exists(newFileDir))
            {
                Directory.CreateDirectory(newFileDir);
            }

            try
            {
                string fileFullPath = Utils.GetMapPath(fileName);
                string toFileFullPath = Utils.GetMapPath(newFileName);
                return Thumbnail.MakeThumbnailImage(fileFullPath, toFileFullPath, 180, 180, cropWidth, cropHeight, X, Y);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 文件上传方法A
        /// </summary>
        /// <param name="postedFile">文件流</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isWater">是否打水印</param>
        /// <returns>服务器文件路径</returns>
        public string fileSaveAs(HttpPostedFileBase postedFile, bool isThumbnail, bool isWater)
        {
            return fileSaveAs(postedFile, isThumbnail, isWater, false, false);
        }

        /// <summary>
        /// 文件上传方法B
        /// </summary>
        /// <param name="postedFile">文件流</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isWater">是否打水印</param>
        /// <param name="isImage">是否必须上传图片</param>
        /// <returns>服务器文件路径</returns>
        public string fileSaveAs(HttpPostedFileBase postedFile, bool isThumbnail, bool isWater, bool _isImage)
        {
            return fileSaveAs(postedFile, isThumbnail, isWater, _isImage, false);
        }

        /// <summary>
        /// 文件上传方法C
        /// </summary>
        /// <param name="postedFile">文件流</param>
        /// <param name="isThumbnail">是否生成缩略图</param>
        /// <param name="isWater">是否打水印</param>
        /// <param name="isReOriginal">是否返回文件原名称</param>
        /// <returns>服务器文件路径</returns>
        public string fileSaveAs(HttpPostedFileBase postedFile, bool isThumbnail, bool isWater, bool _isImage, bool _isReOriginal)
        {
            try
            {
                string fileExt = Utils.GetFileExt(postedFile.FileName); //文件扩展名，不含“.”
                string originalFileName = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf(@"\") + 1); //取得文件原名
                string fileName = Utils.GetRamCode() + "." + fileExt; //随机文件名
                string dirPath = GetUpLoadPath(); //上传目录相对路径

                //检查文件扩展名是否合法
                if (!CheckFileExt(fileExt))
                {
                    return "不允许上传" + fileExt + "类型的文件！";
                }
                //检查是否必须上传图片
                if (_isImage && !IsImage(fileExt))
                {
                    return "对不起，仅允许上传图片文件！";
                }
                //检查文件大小是否合法
                if (!CheckFileSize(fileExt, postedFile.ContentLength))
                {
                    return "文件超过限制的大小啦！";
                }
                //获得要保存的文件路径
                string serverFileName = dirPath + fileName;
                string serverThumbnailFileName = dirPath + "small_" + fileName;
                string returnFileName = serverFileName;
                //物理完整路径                    
                string toFileFullPath = Utils.GetMapPath(dirPath);
                //检查有该路径是否就创建
                if (!Directory.Exists(toFileFullPath))
                {
                    Directory.CreateDirectory(toFileFullPath);
                }
                //保存文件
                postedFile.SaveAs(toFileFullPath + fileName);
                //如果是图片，检查图片尺寸是否超出限制
                if (IsImage(fileExt) && (this.siteConfig.AttachImgMaxHeight > 0 || this.siteConfig.AttachImgMaxWidth > 0))
                {
                    Thumbnail.MakeThumbnailImage(toFileFullPath + fileName, toFileFullPath + fileName, this.siteConfig.AttachImgMaxWidth, this.siteConfig.AttachImgMaxHeight);
                }
                //是否生成缩略图
                if (IsImage(fileExt) && isThumbnail && this.siteConfig.ThumbnailWidth > 0 && this.siteConfig.ThumbnailHeight > 0)
                {
                    Thumbnail.MakeThumbnailImage(toFileFullPath + fileName, toFileFullPath + "small_" + fileName, this.siteConfig.ThumbnailWidth, this.siteConfig.ThumbnailHeight, "Cut");
                    returnFileName += "," + serverThumbnailFileName; //返回缩略图，以逗号分隔开
                }
                //是否打图片水印
                if (IsWaterMark(fileExt) && isWater)
                {
                    switch (this.siteConfig.WatermarkType)
                    {
                        case 1://文字水印
                            WaterMark.AddImageSignText(serverFileName, serverFileName,
                                this.siteConfig.WatermarkText, this.siteConfig.WatermarkPosition,
                                this.siteConfig.WatermarkImgQuality, this.siteConfig.WatermarkFont, this.siteConfig.WatermarkFontSize);
                            break;
                        case 2://图片水印
                            WaterMark.AddImageSignPic(serverFileName, serverFileName,
                                this.siteConfig.WatermarkPic, this.siteConfig.WatermarkPosition,
                                this.siteConfig.WatermarkImgQuality, this.siteConfig.WatermarkTransparency);
                            break;
                    }
                }
                //如果需要返回原文件名
                if (_isReOriginal)
                {
                    return serverFileName + "-" + originalFileName;
                }
                return "success" + returnFileName;
            }
            catch
            {
                return "上传过程中发生意外错误！";
            }
        }

        #region 私有方法=============================================

        /// <summary>
        /// 返回上传目录相对路径
        /// </summary>
        /// <param name="fileName">上传文件名</param>
        private string GetUpLoadPath()
        {
            string path = siteConfig.UploadWebPath + siteConfig.AttachPath + "/"; //站点目录+上传目录
            switch (this.siteConfig.AttachSave)
            {
                case 1: //按"年月日"每天一个文件夹
                    path += DateTime.Now.ToString("yyyyMMdd");
                    break;
                default: //按"年月/日"存入不同的文件夹
                    path += DateTime.Now.ToString("yyyyMM") + "/" + DateTime.Now.ToString("dd");
                    break;
            }
            return path + "/";
        }

        /// <summary>
        /// 是否需要打水印
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsWaterMark(string _fileExt)
        {
            //判断是否开启水印
            if (this.siteConfig.WatermarkType > 0)
            {
                //判断是否可以打水印的图片类型
                ArrayList al = new ArrayList();
                al.Add("bmp");
                al.Add("jpeg");
                al.Add("jpg");
                al.Add("png");

                if (al.Contains(_fileExt.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为图片文件
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        private bool IsImage(string _fileExt)
        {
            ArrayList al = new ArrayList();
            al.Add("bmp");
            al.Add("jpeg");
            al.Add("jpg");
            al.Add("gif");
            al.Add("png");

            if (al.Contains(_fileExt.ToLower()))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是否为合法的上传文件
        /// </summary>
        private bool CheckFileExt(string _fileExt)
        {
            //检查危险文件
            string[] excExt = { "asp", "aspx", "php", "jsp", "js", "htm", "html" };
            for (int i = 0; i < excExt.Length; i++)
            {
                if (excExt[i].ToLower() == _fileExt.ToLower())
                {
                    return false;
                }
            }
            //检查合法文件
            string[] allowExt = this.siteConfig.AttachExtension.Split(',');
            for (int i = 0; i < allowExt.Length; i++)
            {
                if (allowExt[i].ToLower() == _fileExt.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 检查文件大小是否合法
        /// </summary>
        /// <param name="_fileExt">文件扩展名，不含“.”</param>
        /// <param name="_fileSize">文件大小(KB)</param>
        private bool CheckFileSize(string _fileExt, int _fileSize)
        {
            //判断是否为图片文件
            if (IsImage(_fileExt))
            {
                //图片大小
                if (this.siteConfig.AttachImgSize > 0 && _fileSize > this.siteConfig.AttachImgSize * 1024)
                {
                    return false;
                }
            }
            else
            {
                //文件大小
                if (this.siteConfig.AttachFileSize > 0 && _fileSize > this.siteConfig.AttachFileSize * 1024)
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
