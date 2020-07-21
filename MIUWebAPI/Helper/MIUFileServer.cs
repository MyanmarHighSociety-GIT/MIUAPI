using System;
using System.Web.Configuration;

namespace MIUWebAPI.Helper
{
    public static class MIUFileServer
    {
        public static string GetFileUrl(string fileDirectory, string FileName)
        {
            try
            {
                string fileUrl = WebConfigurationManager.AppSettings["MIUFileServerGet"] +"/"+ fileDirectory + "/" + FileName;
                return fileUrl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool SaveToFileServer(string fileDirectory,string fileName,byte[] byteContent)
        {
            try
            {
                string serverUrl = WebConfigurationManager.AppSettings["MIUFileServerPost"] + "/" + fileDirectory;
                string fileUrl = serverUrl + "/" + fileName;
                bool existsFolder = System.IO.Directory.Exists(serverUrl);
                bool existsFile = System.IO.File.Exists(fileUrl);

                if (!existsFolder)
                {
                    System.IO.Directory.CreateDirectory(serverUrl);
                }

                if (existsFile)
                {
                    System.IO.File.Delete(fileUrl);
                }
                System.IO.File.WriteAllBytes(fileUrl,byteContent);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool DeleteFromFileServer(string fileDirectory, string fileName)
        {
            try
            {
                string serverUrl = WebConfigurationManager.AppSettings["MIUFileServerPost"] + "/" + fileDirectory;
                string fileUrl = serverUrl + "/" + fileName;
                bool existsFile = System.IO.File.Exists(fileUrl);

                if (existsFile)
                {
                    System.IO.File.Delete(fileUrl);
                }
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string GetServerUrl()
        {
            return WebConfigurationManager.AppSettings["MIUFileServerPost"] + "/";
        }
    }
}