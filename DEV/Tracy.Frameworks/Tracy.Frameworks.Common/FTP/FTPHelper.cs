using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/6/11 18:05:24
 * 描述说明：FTP操作辅助类
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.FTP
{
    public class FTPHelper
    {
        #region 字段
        /// <summary>
        /// ftp地址，带ftp协议
        /// </summary>
        private string strFtpURI;
        /// <summary>
        /// ftp用户名
        /// </summary>
        private string strFtpUserID;
        /// <summary>
        /// ftp的ip地址
        /// </summary>
        private string strFtpServerIP;
        /// <summary>
        /// ftp用户登录密码
        /// </summary>
        private string strFtpPassword;
        /// <summary>
        /// ftp目录路径
        /// </summary>
        private string strFtpRemotePath;
        #endregion

        /// <summary>  
        /// 连接FTP服务器
        /// </summary>  
        /// <param name="strFtpServerIP">FTP连接地址</param>  
        /// <param name="strFtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>  
        /// <param name="strFtpUserID">用户名</param>  
        /// <param name="strFtpPassword">密码</param>  
        public FTPHelper(string strFtpServerIP, string strFtpRemotePath, string strFtpUserID, string strFtpPassword)
        {
            this.strFtpServerIP = strFtpServerIP;
            this.strFtpRemotePath = strFtpRemotePath;
            this.strFtpUserID = strFtpUserID;
            this.strFtpPassword = strFtpPassword;
            this.strFtpURI = "ftp://" + strFtpServerIP + strFtpRemotePath;
        }

        /// <summary>
        /// 上载
        /// </summary>
        /// <param name="strFilename">本地文件路径</param>
        /// <param name="strSavePath">ftp服务器文件保存路径</param>
        public void Upload(string strFilename, string strSavePath)
        {
            FileInfo fileInf = new FileInfo(strFilename);
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFtpURI + strSavePath + fileInf.Name));
            reqFTP.Credentials = new NetworkCredential(strFtpUserID, strFtpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.KeepAlive = false;
            reqFTP.UseBinary = true;
            reqFTP.Proxy = null;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 上载
        /// </summary>
        /// <param name="strFilename">本地文件路径</param>
        /// <param name="strSavePath">ftp服务器文件保存路径</param>
        /// <param name="strStrOldName">ftp服务器文件保存的名字</param>
        public void Upload(string strFilename, string strSavePath, string strStrOldName)
        {
            FileInfo fileInf = new FileInfo(strFilename);
            FtpWebRequest reqFTP;
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFtpURI + strSavePath + strStrOldName));
            reqFTP.Credentials = new NetworkCredential(strFtpUserID, strFtpPassword);
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.KeepAlive = false;
            reqFTP.UseBinary = true;
            reqFTP.Proxy = null;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="strFilePath">本地保存路径</param>
        /// <param name="strFileName">文件名</param> 
        /// <param name="strFileName">本地临时名称</param>
        public void Download(string strFilePath, string strFileName, string strLocalName)
        {
            try
            {
                FileStream outputStream = new FileStream(strFilePath + strLocalName, FileMode.Create);
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFtpURI + strFileName));
                reqFTP.Credentials = new NetworkCredential(strFtpUserID, strFtpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = true;
                reqFTP.Proxy = null;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                //记录日志
                //Common.LogHelper.WriteLog("文件下载异常：" + ex.Message);
            }
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="strFilePath">本地保存路径</param>
        /// <param name="strFileName">文件名</param>
        public void Download(string strFilePath, string strFileName)
        {
            try
            {
                FileStream outputStream = new FileStream(strFilePath + strFileName, FileMode.Create);
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFtpURI + strFileName));
                reqFTP.Credentials = new NetworkCredential(strFtpUserID, strFtpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.UsePassive = true;
                reqFTP.Proxy = null;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                //记录日志
                //Common.LogHelper.WriteLog("文件下载异常：" + ex.Message);
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="strFileName">文件名</param>
        public void Delete(string strFileName)
        {
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFtpURI + strFileName));
                reqFTP.Credentials = new NetworkCredential(strFtpUserID, strFtpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                reqFTP.KeepAlive = false;
                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)  
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList()
        {
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFtpURI));
                ftp.Credentials = new NetworkCredential(strFtpUserID, strFtpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                line = reader.ReadLine();
                line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  获取FTP文件列表(包括文件夹)
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        private string[] GetAllList(string strUrl)
        {
            List<string> list = new List<string>();
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(new Uri(strUrl));
            req.Credentials = new NetworkCredential(strFtpPassword, strFtpPassword);
            req.Method = WebRequestMethods.Ftp.ListDirectory;
            req.UseBinary = true;
            req.UsePassive = true;
            try
            {
                using (FtpWebResponse res = (FtpWebResponse)req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            list.Add(s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return list.ToArray();
        }

        /// <summary>  
        /// 获取当前目录下文件列表(不包括文件夹)  
        /// </summary>  
        public string[] GetFileList(string strUrl)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strUrl));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(strFtpPassword, strFtpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {

                    if (line.IndexOf("<DIR>") == -1)
                    {
                        result.Append(Regex.Match(line, @"[\S]+ [\S]+", RegexOptions.IgnoreCase).Value.Split(' ')[1]);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result.ToString().Split('\n');
        }

        /// <summary>  
        /// 判断当前目录下指定的文件是否存在  
        /// </summary>  
        /// <param name="strRemoteFileName">远程文件名</param>  
        public bool FileExist(string strRemoteFileName)
        {
            string[] fileList = GetFileList("*.*");
            foreach (string str in fileList)
            {
                if (str.Trim() == strRemoteFileName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建文件夹  
        /// </summary>
        /// <param name="strDirName">目录名</param>
        public void MakeDir(string strDirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFtpURI + strDirName));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(strFtpUserID, strFtpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            { 
                //Common.LogHelper.WriteLog("ftp服务器创建目录异常：" + ex.Message); 
            }
        }

        /// <summary>  
        /// 获取指定文件大小  
        /// </summary>  
        public long GetFileSize(string strFilename)
        {
            FtpWebRequest reqFTP;
            long fileSize = 0;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFtpURI + strFilename));
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(strFtpUserID, strFtpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                fileSize = response.ContentLength;
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            { throw ex; }
            return fileSize;
        }

        /// <summary>  
        /// 更改文件名  
        /// </summary> 
        public void ReName(string strCurrentFilename, string strNewFilename)
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFtpURI + strCurrentFilename));
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = strNewFilename;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(strFtpUserID, strFtpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>  
        /// 移动文件  
        /// </summary>  
        public void MovieFile(string strCurrentFilename, string strNewDirectory)
        {
            ReName(strCurrentFilename, strNewDirectory);
        }

        /// <summary>  
        /// 切换当前目录  
        /// </summary>  
        /// <param name="bIsRoot">true:绝对路径 false:相对路径</param>   
        public void GotoDirectory(string strDirectoryName, bool bIsRoot)
        {
            if (bIsRoot)
            {
                strFtpRemotePath = strDirectoryName;
            }
            else
            {
                strFtpRemotePath += strDirectoryName + "/";
            }
            strFtpURI = "ftp://" + strFtpServerIP + "/" + strFtpRemotePath + "/";
        }
    }
}
