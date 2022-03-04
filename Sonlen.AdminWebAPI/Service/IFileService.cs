namespace Sonlen.AdminWebAPI.Service
{
    public interface IFileService
    {
        void SaveToFtp(byte[] buffer, string fileName, string ext);

        void UploadFile(byte[] buffer, string fileName, string ext);
    }
}