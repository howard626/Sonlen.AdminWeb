using Newtonsoft.Json;
using Sonlen.WebAdmin.Model;
using System.Text;

namespace Sonlen.AdminWeb.Service
{
    public class AttendanceService
    {
        private readonly HttpClient httpClient;
        private readonly string API_ADDRESS = $"{Setting.API_ADDRESS}Attendance/";

        public AttendanceService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /** 列印考勤*/
        public async Task<string> UpdateEmployeeAsync(Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}Update", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
