using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace AstWeb.Common.Util
{
    /// <summary>
    /// [辅助类]文件操作辅助类
    /// </summary>
    public sealed class FileUtil
    {
        #region 构造实例

        /// <summary>
        /// 文件操作辅助类
        /// </summary>
        private FileUtil()
        {
        }

        /// <summary>
        /// 构建类的实例对象
        /// </summary>
        public static FileUtil Instance
        {
            get { return new FileUtil(); }
        }

        #endregion 构造实例

        #region 公共方法

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="fileOrPath">文件或目录</param>
        public void CreateDirectory(string fileOrPath)
        {
            if (fileOrPath == null)
                return;

            var path = ".".Contains(fileOrPath) ? Path.GetDirectoryName(fileOrPath) : fileOrPath;

            if (path == null)
                return;

            if (Directory.Exists(path))
                return;

            Directory.CreateDirectory(path);
        }

        /// <summary>
        ///取得文件命名的安全路径（Windows文件命名规则：不能以空格为开头和结束,不能用//:*?""|作为文件名称,文件名称为1-255位）
        /// </summary>
        /// <param name="name">路径名称</param>
        /// <returns>安全路径名称</returns>
        public string GetSafeFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            name = new[] { "//", ":", "*", "?", "<", ",", ">", "|", "'", "\"" }.Aggregate(name,
                                                                                        (current, badChar) =>
                                                                                        current.Replace(badChar, ""));
            name = name.Length > 255 ? name.Substring(0, 250) : name;
            return name.Trim();
        }

        /// <summary>
        /// 获取文件的扩展名
        /// </summary>
        /// <param name="fileName">文件</param>
        public string GetExtension(string fileName)
        {
            var fileExt = string.Empty;

            if (String.IsNullOrEmpty(fileName)) return fileExt;
            var extension = Path.GetExtension(fileName);
            fileExt = extension.Remove(0, 1).ToLower();

            return fileExt;
        }

        /// <summary>
        /// 备份文件，备份后的文件名为原文件名+.bak，如目标文件存在时覆盖
        /// </summary>
        /// <param name="sourceFileName">源文件名</param> 
        /// <returns>true操作成功，false操作失败</returns>
        public bool BackupFile(string sourceFileName)
        {
            return BackupFile(sourceFileName, sourceFileName + ".bak", true);
        }

        /// <summary>
        /// 备份文件,当目标文件存在时覆盖
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <returns>true操作成功，false操作失败</returns>
        public bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }

        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns>true操作成功，false操作失败</returns>
        public bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }
            if (!overwrite && File.Exists(destFileName))
            {
                return false;
            }
            try
            {
                File.Copy(sourceFileName, destFileName, true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 下载文件，分流标准，默认：1024000
        /// </summary>
        /// <param name="fileName">目的文件名称，包含后辍名</param>
        /// <param name="fullPath">源文件路径</param>
        /// <returns>true操作成功，false操作失败</returns>
        public bool DownFile(string fileName, string fullPath)
        {
            return DownFile(fileName, fullPath, 1024000);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName">目的文件名称，包含后辍名</param>
        /// <param name="fullPath">源文件路径</param>
        /// <param name="speed">分流标准，默认：1024000</param>
        /// <returns>true操作成功，false操作失败</returns>
        public bool DownFile(string fileName, string fullPath, long speed)
        {
            try
            {
                var myFile = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var br = new BinaryReader(myFile);

                var request = HttpContext.Current.Request;
                var response = HttpContext.Current.Response;

                try
                {
                    response.AddHeader("Accept-Ranges", "bytes");
                    response.Buffer = false;
                    var fileLength = myFile.Length;
                    long startBytes = 0;

                    const double pack = 10240; //10K bytes
                    var sleep = (int)Math.Floor(1000 * pack / speed) + 1;
                    if (request.Headers["Range"] != null)
                    {
                        response.StatusCode = 206;
                        var range = request.Headers["Range"].Split(new[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        //Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength-1, fileLength));
                    }
                    response.AddHeader("Connection", "Keep-Alive");
                    response.ContentType = "application/octet-stream";
                    response.AddHeader("Content-Disposition",
                                        "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    var maxCount = (int)Math.Floor((fileLength - startBytes) / pack) + 1;

                    for (var i = 0; i < maxCount; i++)
                    {
                        if (response.IsClientConnected)
                        {
                            response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion 公共方法

        #region 读取文件

        /// <summary>
        /// 读取文件，默认读取编码为UTF-8的编码，开启缓存
        /// </summary>
        /// <param name="fileName">文件文称，要求绝对路径</param>
        /// <returns>文件内容</returns>
        public string ReadFile(string fileName)
        {
            return ReadFile(fileName, Encoding.UTF8, true);
        }

        /// <summary>
        /// 读取文件，默认读取编码为UTF-8的编码
        /// </summary>
        /// <param name="fileName">文件文称，要求绝对路径</param>
        /// <param name="isCache">是否启用缓存</param>
        /// <returns>文件内容</returns>
        public string ReadFile(string fileName, bool isCache)
        {
            return ReadFile(fileName, Encoding.UTF8, isCache);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="fileName">文件文称，要求绝对路径</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="isCache">是否启用缓存</param>    
        /// <returns>文件内容</returns>
        public string ReadFile(string fileName, Encoding encoding, bool isCache)
        {
            string result; //返回结果
            if (isCache)
            {
                result = (string)HttpContext.Current.Cache[fileName];
                if (result == null)
                {
                    result = ReadFile(fileName, encoding, false);
                    HttpContext.Current.Cache.Add(fileName, result, new CacheDependency(fileName), DateTime.MaxValue,
                                                  TimeSpan.Zero, CacheItemPriority.High, null);
                }
            }
            else
            {
                using (var sr = new StreamReader(fileName, encoding))
                {
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }


        /// <summary>
        /// 获取指定目录下所有的文件夹及文件
        /// </summary>
        /// <param name="dir">要检索的文件夹绝对路径</param>
        /// <param name="folderName">指定的目录绝对路径,若为空则表示获取dir下所有文件</param>
        /// <param name="folderPaths">保存文件夹绝对路径的集合</param>
        /// <param name="filePaths">保存文件绝对路径的集合</param>
        public void GetAllFolderAndFile(string dir, string folderName, List<string> folderPaths, List<string> filePaths)
        {
            var files = Directory.GetFileSystemEntries(dir);

            foreach (var item in files)
            {
                if (Directory.Exists(item))
                {
                    var direct = new DirectoryInfo(item);
                    if ((direct.FullName + "\\").ToLower() != folderName.ToLower())
                        folderPaths.Add(direct.FullName + "\\");
                    GetAllFolderAndFile(item, folderName, folderPaths, filePaths);
                }
                else
                {
                    var file = new FileInfo(item);
                    if (file.Directory != null && (!string.IsNullOrEmpty(folderName) || file.Directory.FullName.ToLower().Contains(folderName.ToLower())))
                        filePaths.Add(item);
                }
            }
        }


        /// <summary>
        /// 获取指定目录下的所有文件,包含子目录
        /// </summary>
        /// <param name="dir">绝对路径</param>
        /// <param name="folderName">文件夹名称</param>
        /// <param name="fileList">文件列表</param>
        /// <returns>文件列表</returns>
        public List<string> GetAllFolderAndFile(string dir, string folderName, List<string> fileList)
        {
            var dirs = Directory.GetFileSystemEntries(dir);

            foreach (var d in dirs)
            {
                var names = d.Split('\\');
                var name = names[names.Length - 1]; //获得名称

                if (!name.Contains(".html")) //非文件的情况下创建文件夹
                {
                    //添加文件夹
                    var folderPath = HttpContext.Current.Server.MapPath("~/") + "\\" +
                                        dir.Substring(dir.IndexOf(folderName, StringComparison.Ordinal)).Replace(folderName + "\\", "");
                    if (!folderPath.EndsWith("\\"))
                    {
                        folderPath = folderPath + "\\";
                    }
                    Directory.CreateDirectory(folderPath + name);

                    //调用路径再次解析
                    dir = dir.EndsWith("\\") ? dir : dir + "\\";
                    fileList = GetAllFolderAndFile(dir + name, folderName, fileList);
                }
                else
                {
                    var filePath = d.Substring(d.IndexOf(folderName, StringComparison.Ordinal));
                    var f = filePath.Replace(folderName + "\\", "").Split('.')[0];

                    fileList.Add(f);
                }
            }

            return fileList;
        }

        #endregion 读取文件

        #region 写文件

        /// <summary>
        /// 写入文件，默认字符编码为 UTF-8
        /// </summary>
        /// <param name="fileName">文件文称，要求绝对路径</param>
        /// <param name="context">需要写入的内容</param>
        public void WriteFile(string fileName, string context)
        {
            WriteFile(fileName, context, Encoding.UTF8);
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="fileName">文件文称，要求绝对路径</param>
        /// <param name="context">需要写入的内容</param>
        /// <param name="encoding">字符编码</param>
        public void WriteFile(string fileName, string context, Encoding encoding)
        {
            //创建目录
            CreateDirectory(fileName);

            //输出生成文件
            var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

            using (var sw = new StreamWriter(fs, encoding))
            {
                sw.Write(context);
            }

        }

        #endregion 写文件

        #region 文件/文件夹拷贝
        /// <summary>
        /// 将指定文件夹下面的所有内容copy到目标文件夹下面
        /// 如果目标文件夹为只读属性就会报错。
        /// </summary>
        /// <param name="srcPath">源文件夹</param>
        /// <param name="aimPath">目标文件夹</param>
        /// <param name="overWFile">是否覆盖同名文件</param>
        public void CopyDir(string srcPath, string aimPath, bool overWFile = true)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath)) Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles(srcPath);
                var fileList = Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach (var file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file), overWFile);
                    // 否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), overWFile);
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        #endregion
    }
}
