

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Sonlen.AdminWeb.Auth;
using Sonlen.WebAdmin.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sonlen.AdminWeb.Service
{
    public class LoginService : ILoginService
    {
        private readonly ILocalStorageService localStorageService;
        private readonly HttpClient httpClient;
        private readonly AuthenticationStateProvider authenticationStateProvider;
        private readonly string APIADDRESS = "http://localhost:5149/api/Login/";

        public LoginService(ILocalStorageService localStorageService, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            this.localStorageService = localStorageService;
            this.httpClient = httpClient;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        /** 登入*/
        public async Task<string> LoginAsync(LoginModel userInfo)
        {
            string result = string.Empty;
            var json = JsonConvert.SerializeObject(userInfo);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await httpClient.PostAsync($"{APIADDRESS}Login", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                UserToken? userToken = JsonConvert.DeserializeObject<UserToken>(resContent);
                if (userToken != null)
                {
                    await localStorageService.SetItemAsync("authToken", userToken.token);
                    ((CustomAuthStateProvider)authenticationStateProvider).NotifyUserAuthentication(userToken.token);
                }
            }
            else 
            {
                result = resContent;
            }

            return result;
        }

        /** 登出*/
        public async Task LogoutAsync()
        {
            await localStorageService.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)authenticationStateProvider).NotifyUserLogOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }

        /** 註冊*/
        public async Task<string> RegisterAsync(RegisterModel userInfo)
        {
            string result = string.Empty;
            var json = JsonConvert.SerializeObject(userInfo);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{APIADDRESS}Register", httpContent);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        public async Task<Employee?> GetEmployeeAsync(string id)
        {
            Employee? employee = null;
            var json = JsonConvert.SerializeObject(id);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{APIADDRESS}GetEmployee", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                employee = JsonConvert.DeserializeObject<Employee>(resContent);
            }

            return employee;
        }

        public async Task<List<Employee>> GetAllEmployeeAsync()
        {
            List<Employee>? employees = null;
            var json = JsonConvert.SerializeObject("");
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{APIADDRESS}GetAllEmployees", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                employees = JsonConvert.DeserializeObject<List<Employee>>(resContent);
            }

            return employees ?? new List<Employee>();
        }

    }
}
