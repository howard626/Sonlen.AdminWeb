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
                EmployeeID = claim.FirstOrDefault(c => c.Type == "EmployeeID")?.Value ?? String.Empty,
                EmployeeName = claim.FirstOrDefault(c => c.Type == "Name")?.Value ?? String.Empty
            };
            model.Employee = employee;

            var json = JsonConvert.SerializeObject(model);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}DayOff", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 取消請假*/
        public async Task<string> LeaveOffAsync(LeaveRecord leave)
        {
            var json = JsonConvert.SerializeObject(leave);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}CancelDayOff", httpContent);
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

        /** 取得全部請假紀錄*/
        public async Task<List<LeaveRecordViewModel>> GetAllLeaveRecordAsync()
        {
            List<LeaveRecordViewModel>? leaveRecords = null;

            var json = JsonConvert.SerializeObject("");
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}GetAllLeaveRecord", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                leaveRecords = JsonConvert.DeserializeObject<List<LeaveRecordViewModel>>(resContent);
            }

            return leaveRecords ?? new List<LeaveRecordViewModel>();
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

        /** 取得請假證明*/
        public async Task<UploadFile> GetLeaveProveAsync(string fileName)
        {
            UploadFile? uploadFile = null;

            var tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            var claim = JwtParser.ParseClaimsFromJwt(tokenInLocalStorage);
            UploadFile file = new UploadFile()
            {
                FileName = fileName
            };

            var json = JsonConvert.SerializeObject(file);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}GetLeaveProve", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                uploadFile = JsonConvert.DeserializeObject<UploadFile>(resContent);
            }

            return uploadFile ?? new UploadFile();
        }

        /** 同意請假*/
        public async Task<string> AgreeLeaveRecordAsync(LeaveRecordViewModel leave)
        {
            var json = JsonConvert.SerializeObject(leave);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}AgreeLeaveRecord", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 駁回請假*/
        public async Task<string> DisagreeLeaveRecordAsync(LeaveRecordViewModel leave)
        {
            var json = JsonConvert.SerializeObject(leave);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}DisagreeLeaveRecord", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }
    }
}
