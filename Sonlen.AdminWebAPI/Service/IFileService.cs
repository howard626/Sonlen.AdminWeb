using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Service
{
    public interface IFileService
    {
        /// <summary>
        /// 上傳檔案至 FTP
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="fileName"></param>
        /// <param name="ext"></param>
        void SaveToFtp(byte[] buffer, string fileName, string ext);

        /// <summary>
        /// 上傳檔案至 SERVER
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="fileName"></param>
        void UploadFile(byte[] buffer, string fileName);

        /// <summary>
        /// 下載檔案
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        UploadFile DownloadFile(string fileName);
    }
}