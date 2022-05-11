using Blazored.LocalStorage;
using Newtonsoft.Json;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Text;

namespace Sonlen.AdminWeb.Service
{
    public class WorkOvertimeService : IWorkOvertimeService
    {
        private readonly HttpClient httpClient;
        private readonly string API_ADDRESS = $"{Setting.API_ADDRESS}WorkOvertime/";
        private readonly ILocalStorageService localStorageService;
        public WorkOvertimeService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
        }

        /** 申請加班*/
        public async Task<string> AddDataAsync(WorkOvertimeViewModel overtime)
        {
            var tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            var claim = JwtParser.ParseClaimsFromJwt(tokenInLocalStorage);
            overtime.EmployeeID = claim.FirstOrDefault(c => c.Type == "EmployeeID")?.Value ?? String.Empty;
            overtime.EmployeeName = claim.FirstOrDefault(c => c.Type == "Name")?.Value ?? String.Empty;
            var json = JsonConvert.SerializeObject(overtime);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}AddWorkOvertime", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 取消加班*/
        public async Task<string> DeleteDataAsync(WorkOvertimeViewModel overtime)
        {
            var json = JsonConvert.SerializeObject(overtime);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}CancelWorkOvertime", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 取得全部加班紀錄*/
        public async Task<List<WorkOvertimeViewModel>> GetAllDataAsync()
        {
            List<WorkOvertimeViewModel>? workOvertimes = null;

            var json = JsonConvert.SerializeObject("");
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}GetAllWorkOvertime", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                workOvertimes = JsonConvert.DeserializeObject<List<WorkOvertimeViewModel>>(resContent);
            }

            return workOvertimes ?? new List<WorkOvertimeViewModel>();
        }

        /** 從 EmployeeID 取得加班紀錄*/
        public async Task<List<WorkOvertimeViewModel>> GetDataByEIDAsync()
        {
            List<WorkOvertimeViewModel>? workOvertimes = null;

            var tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            var claim = JwtParser.ParseClaimsFromJwt(tokenInLocalStorage);
            Employee employee = new Employee()
            {
                Email = claim.FirstOrDefault(c => c.Type == "Email")?.Value ?? String.Empty,
                EmployeeID = claim.FirstOrDefault(c => c.Type == "EmployeeID")?.Value ?? String.Empty
            };

            var json = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}GetOvertimeRecordByEID", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                workOvertimes = JsonConvert.DeserializeObject<List<WorkOvertimeViewModel>>(resContent);
            }

            return workOvertimes ?? new List<WorkOvertimeViewModel>();
        }

        /** 同意加班*/
        public async Task<string> AgreeAsync(WorkOvertimeViewModel overtime)
        {
            var json = JsonConvert.SerializeObject(overtime);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}AgreeWorkOvertime", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 駁回加班*/
        public async Task<string> DisagreeAsync(WorkOvertimeViewModel overtime)
        {
            var json = JsonConvert.SerializeObject(overtime);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}DisagreeWorkOvertime", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        #region 未實作項目

        public Task<WorkOvertimeViewModel?> GetDataByIDAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateDataAsync(WorkOvertimeViewModel item)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
