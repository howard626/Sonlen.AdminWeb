using Blazored.LocalStorage;
using Newtonsoft.Json;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Text;

namespace Sonlen.AdminWeb.Service
{
    public class LeaveService : ILeaveService
    {
        private readonly HttpClient httpClient;
        private readonly string API_ADDRESS = $"{Setting.API_ADDRESS}Leave/";
        private readonly ILocalStorageService localStorageService;
        public LeaveService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
        }

        /** 請假*/
        public async Task<string> LeaveOnAsync(LeaveViewModel model)
        {
            var tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            var claim = JwtParser.ParseClaimsFromJwt(tokenInLocalStorage);
            Employee employee = new Employee()
            {
                Email = claim.FirstOrDefault(c => c.Type == "Email")?.Value ?? String.Empty,
                EmployeeID = claim.FirstOrDefault(c => c.Type == "EmployeeID")?.Value ?? String.Empty
            };
            model.Employee = employee;
            
            var json = JsonConvert.SerializeObject(model);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}DayOff", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 取消請假*/
        public async Task<string> LeaveOffAsync(DateTime leaveDate)
        {
            var tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            var claim = JwtParser.ParseClaimsFromJwt(tokenInLocalStorage);
            Employee employee = new Employee()
            {
                Email = claim.FirstOrDefault(c => c.Type == "Email")?.Value ?? String.Empty,
                EmployeeID = claim.FirstOrDefault(c => c.Type == "EmployeeID")?.Value ?? String.Empty
            };

            LeaveViewModel leave = new LeaveViewModel()
            {
                LeaveDate = leaveDate,
                Employee = employee
            };

            var json = JsonConvert.SerializeObject(leave);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}Cancel", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 取得請假類別*/
        public async Task<List<LeaveType>> GetLeaveTypesAsync()
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

        /** 從 EmployeeID 取得請假紀錄*/
        public async Task<List<LeaveRecordViewModel>> GetLeaveRecordByEIDAsync()
        {
            List<LeaveRecordViewModel>? leaveRecords = null;

            var tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            var claim = JwtParser.ParseClaimsFromJwt(tokenInLocalStorage);
            Employee employee = new Employee()
            {
                Email = claim.FirstOrDefault(c => c.Type == "Email")?.Value ?? String.Empty,
                EmployeeID = claim.FirstOrDefault(c => c.Type == "EmployeeID")?.Value ?? String.Empty
            };
            
            var json = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}GetLeaveRecordByEID", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                leaveRecords = JsonConvert.DeserializeObject<List<LeaveRecordViewModel>>(resContent);
            }

            return leaveRecords ?? new List<LeaveRecordViewModel>();
        }

    }
}
