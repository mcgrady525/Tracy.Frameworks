using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracy.Frameworks.Common.Configuration;
using System.Web;
using System.IO;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：6/5/2014 10:27:05 PM
 * 描述说明：上传辅助类
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Upload_Download
{
    public class UploadHelper
    {
        public static string UpLoadPath
        {
            get
            {
                return AppConfigHelper.GetAppSetting("UploadPath");
            }
        }

        #region 使用ASHX方式

        public static bool Upload()
        {
            bool flag = false;

            try
            {
                HttpContext context = HttpContext.Current; //获取当前请求上下文

                //todo:支持多文件上传
                HttpFileCollection fileList = context.Request.Files;
                if (fileList == null || fileList.Count <= 0) return false;

                foreach (string key in fileList.AllKeys)
                {
                    HttpPostedFile file = fileList[key];
                    if (file != null && !String.IsNullOrEmpty(file.FileName))
                    {
                        string extentionName = Path.GetExtension(file.FileName); //如'.txt'
                        string fileName = DateTime.Now.ToString("yyyyMMdd") + new Random().Next(1000, 10000) + extentionName;

                        string uploadFilePath = UpLoadPath;
                        if (String.IsNullOrEmpty(uploadFilePath))
                        {
                            uploadFilePath = "UploadFiles";
                        }
                        string uploadDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uploadFilePath);

                        //todo:增加判断目录是否存在
                        if (!Directory.Exists(uploadDirectory))
                        {
                            Directory.CreateDirectory(uploadDirectory);
                        }

                        string filePath = Path.Combine(uploadDirectory, fileName);
                        file.SaveAs(filePath);
                        flag = File.Exists(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error("upload file failed!", ex);
                flag = false;
            }
            return flag;
        }

        #endregion

        #region 使用Uploadify

        public static bool UploadByUploadify()
        {
            bool flag = false;

            try
            {
                HttpContext context = HttpContext.Current; //获取当前请求上下文
                string guid = context.Request.QueryString["guid"];
                string folder = context.Request["folder"];
                HttpPostedFile file = context.Request.Files["Filedata"];
                if (file != null && !String.IsNullOrEmpty(file.FileName))
                {
                    string extentionName = Path.GetExtension(file.FileName); //如'.txt'
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + new Random().Next(1000, 10000) + extentionName;

                    string uploadFilePath = UpLoadPath;
                    if (String.IsNullOrEmpty(uploadFilePath))
                    {
                        uploadFilePath = "UploadFiles";
                    }
                    if (String.IsNullOrEmpty(folder))
                    {
                        uploadFilePath = Path.Combine(uploadFilePath, folder);
                    }
                    string uploadDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, uploadFilePath);
                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }
                    string filePath = Path.Combine(uploadDirectory, fileName);
                    file.SaveAs(filePath);
                    flag = File.Exists(filePath);
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error("upload file by uploadify failed", ex);
                flag = false;
            }
            return flag;
        }

        #endregion
    }
}
