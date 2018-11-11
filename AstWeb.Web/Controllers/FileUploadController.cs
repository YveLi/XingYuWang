using AstWeb.Common.Helper;
using System;
using System.Web;
using System.Web.Mvc;
using AstWeb.Common.Models;
using AstWeb.Common.Util;

namespace AstWeb.Controllers
{
    public class FileUploadController : Controller
    {
        //
        // GET: /FileUpload/
        private const int Success = 0;
        private const int Defeat = 1;
        public JsonResult FileRecive()
        {
            FileInfo fileInfo = new FileInfo();
            HttpFileCollectionBase files = Request.Files;
            try
            {
                if (files.Count > 0)
                {
                    foreach (string str in files)
                    {
                        var file = files[str];
                        string msg = new FileHelper(ConfigHandle.GetUploadConfig()).fileSaveAs(file, false, false);
                        if (msg.Contains("success"))
                        {
                            fileInfo = new FileInfo
                            {
                                code = Success,
                                msg = "上传成功",
                                data = new Data
                                {
                                    src = msg.Replace("success", ""),
                                    title = file.FileName
                                }
                            };
                        }
                        else
                        {
                            fileInfo = new FileInfo
                            {
                                code = Defeat,
                                msg = "文件大小不可超过2MB"
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                fileInfo = new FileInfo
                {
                    code = Defeat,
                    msg = ex.Message
                };
            }

            return Json(fileInfo, JsonRequestBehavior.AllowGet);
        }

    }
}