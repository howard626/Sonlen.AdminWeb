using Newtonsoft.Json;
using Sonlen.WebAdmin.Model;
using System.Text;

namespace Sonlen.AdminWeb.Service
{
    public class LeaveTypeService : ILeaveTypeService
    {

        private readonly HttpClient httpClient;
        private readonly string API_ADDRESS = $"{Setting.API_ADDRESS}Leave/";
        public LeaveTypeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /** 取得請假類別*/
        public async Task<List<LeaveType>> GetAllDataAsync()
        {
            List<LeaveType>? leaveTypes = null;

            HttpContent httpContent = new StringContent("", Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}GetLeaveTypes", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                leaveTypes = JsonConvert.DeserializeObject<List<LeaveType>>(resContent);
            }

            return leaveTypes ?? new List<LeaveType>();
        }

        #region 未實作項目

        public Task<string> AddDataAsync(LeaveType item)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteDataAsync(LeaveType item)
        {
            throw new NotImplementedException();
        }

        public Task<LeaveType?> GetDataByIDAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateDataAsync(LeaveType item)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
