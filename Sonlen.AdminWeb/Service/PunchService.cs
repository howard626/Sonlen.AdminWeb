using Blazored.LocalStorage;
using Newtonsoft.Json;
using Sonlen.WebAdmin.Model;
using Sonlen.WebAdmin.Model.Utility;
using System.Text;

namespace Sonlen.AdminWeb.Service
{
    public class PunchService : IPunchService
    {
        private readonly HttpClient httpClient;
        private readonly string API_ADDRESS = $"{Setting.API_ADDRESS}Punch/";
        private readonly ILocalStorageService localStorageService;
        public PunchService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            this.httpClient = httpClient;
            this.localStorageService = localStorageService;
        }

        /** 上班打卡*/
        public async Task<string> PunchInAsync()
        {
            var tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            var claim = JwtParser.ParseClaimsFromJwt(tokenInLocalStorage);
            Employee employee = new Employee()
            {
                Email = claim.FirstOrDefault(c => c.Type == "Email")?.Value ?? String.Empty,
                EmployeeID = claim.FirstOrDefault(c => c.Type == "EmployeeID")?.Value ?? String.Empty
            };
            var json = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}In", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }

        /** 下班打卡*/
        public async Task<string> PunchOutAsync()
        {
            var tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            var claim = JwtParser.ParseClaimsFromJwt(tokenInLocalStorage);
            Employee employee = new Employee()
            {
                Email = claim.FirstOrDefault(c => c.Type == "Email")?.Value ?? String.Empty,
                EmployeeID = claim.FirstOrDefault(c => c.Type == "EmployeeID")?.Value ?? String.Empty
            };
            var json = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}Out", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            return resContent;
        }
    }
}
