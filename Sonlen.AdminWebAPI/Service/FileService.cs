using Sonlen.WebAdmin.Model;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Sonlen.AdminWebAPI.Service
{
    /// <summary>
    /// 檔案處理服務
    /// </summary>
    public class FileService : IFileService
    {
        private string ftpHost = string.Empty;

        private string ftpUser = string.Empty;

        private string ftpPassword = string.Empty;

        /// <summary>
        /// 檔案上傳根目錄
        /// </summary>
        public string Root;

        public FileService(IConfiguration configuration)
        {
            ftpHost = configuration["Ftp:Host"];
            ftpUser = configuration["Ftp:User"];
            ftpPassword = configuration["Ftp:Password"];
            Root = configuration["Ftp:Root"];
        }

        /// <summary>
        /// 將檔案上傳到FTP Server
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="fileName"></param>
        /// <param name="ext"></param>
        public void SaveToFtp(byte[] buffer, string fileName, string ext)
        {
            FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create($"{ftpHost}/{fileName}.{ext}");
            request.UsePassive = false;
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
            // enter FTP Server credentials

            request.UsePassive = true;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Flush();
                requestStream.Close();
            }
        }

        /// <summary>
        /// 將檔案上傳到 Server
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="fileName"></param>
        public void UploadFile(byte[] buffer, string fileName)
        {
            var path = $"{Root}\\{fileName}";
            var fs = File.Create(path);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
        }

        /// <summary>
        /// 下載檔案
        /// </summary>
        /// <param name="fileName"></param>
        public UploadFile DownloadFile(string fileName)
        {
            UploadFile uploadFile = new UploadFile();
            var path = $"{Root}\\{fileName}";
            byte[] byteArray = File.ReadAllBytes(path);

            uploadFile.FileContent = byteArray;
            uploadFile.FileName = fileName;

            return uploadFile;
        }
    }
}
