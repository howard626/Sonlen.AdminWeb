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
        public async Task<string> AddWorkOvertimeAsync(WorkOvertime overtime)
        {
            var tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            var claim = JwtParser.ParseClaimsFromJwt(tokenInLocalStorage);
            WorkOvertimeViewModel model = new WorkOvertimeViewModel(overtime)
            {
                EmployeeID = claim.FirstOrDefault(c => c.Type == "EmployeeID")?.Value ?? String.Empty,
                EmployeeName = claim.FirstOrDefault(c => c.Type == "Name")?.Value ?? String.Empty
            };


            var json = JsonConvert.SerializeObject(model);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}AddWorkOvertime", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 取消加班*/
        public async Task<string> CancelWorkOvertimeAsync(WorkOvertimeViewModel overtime)
        {
            var json = JsonConvert.SerializeObject(overtime);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}CancelWorkOvertime", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 取得全部加班紀錄*/
        public async Task<List<WorkOvertimeViewModel>> GetAllWorkOvertimeAsync()
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
        public async Task<List<WorkOvertimeViewModel>> GetOvertimeRecordByEIDAsync()
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
        public async Task<string> AgreeWorkOvertimeAsync(WorkOvertimeViewModel overtime)
        {
            var json = JsonConvert.SerializeObject(overtime);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}AgreeWorkOvertime", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 駁回加班*/
        public async Task<string> DisagreeWorkOvertimeAsync(WorkOvertimeViewModel overtime)
        {
            var json = JsonConvert.SerializeObject(overtime);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}DisagreeWorkOvertime", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

    }
}
