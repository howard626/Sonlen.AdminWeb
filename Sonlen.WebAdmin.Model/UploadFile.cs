using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sonlen.WebAdmin.Model
{
    /// <summary>
    /// 上傳檔案
    /// </summary>
    public class UploadFile
    {
        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 檔案內容
        /// </summary>
        public byte[]? FileContent { get; set; }
    }
}
