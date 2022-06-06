using Newtonsoft.Json;
using Sonlen.WebAdmin.Model;
using System.Text;

namespace Sonlen.AdminWeb.Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly HttpClient httpClient;
        private readonly string API_ADDRESS = $"{Setting.API_ADDRESS}Attendance/";

        public AttendanceService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /** 列印考勤*/
        public async Task<UploadFile> PrintAsync(AttendanceViewModel model)
        {
            UploadFile? uploadFile = null;

            var json = JsonConvert.SerializeObject(model);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{API_ADDRESS}Print", httpContent);

            var resContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                uploadFile = JsonConvert.DeserializeObject<UploadFile>(resContent);
            }

            return uploadFile ?? new UploadFile();
        }
    }
}
