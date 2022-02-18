

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
        private readonly string APIADDRESS = $"{Setting.API_ADDRESS}Login/";

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

        /** 忘記密碼*/
        public async Task<bool> ForgotPasswordAsync(LoginModel userInfo)
        {
            bool result = false;
            var json = JsonConvert.SerializeObject(userInfo);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{APIADDRESS}ForgotPassword", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(resContent))
                {
                    await localStorageService.SetItemAsync("resetPasswordToken", resContent);
                    result = true;
                }
            }

            return result;
        }

        /** 重設密碼*/
        public async Task<bool> ResetPasswordAsync(ResetPasswordModel userInfo)
        {
            bool result = false;
            string token = await localStorageService.GetItemAsStringAsync("resetPasswordToken");
            
            if (!string.IsNullOrEmpty(token) && token.Replace("\"", "").Equals(userInfo.Token))
            {
                var json = JsonConvert.SerializeObject(userInfo);
                HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{APIADDRESS}ResetPassword", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    await localStorageService.RemoveItemAsync("resetPasswordToken");
                    result = true;
                }
            }
            
            return result;
        }

        /** 取得員工資訊*/
        public async Task<Employee?> GetEmployeeAsync(string id)
        {
            Employee employee = new Employee() 
            {
                EmployeeID = id
            };
            var json = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{APIADDRESS}GetEmployee", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                employee = JsonConvert.DeserializeObject<Employee>(resContent) ?? new Employee();
            }

            return employee;
        }

        /** 取得全部員工資訊*/
        public async Task<List<Employee>> GetAllEmployeeAsync()
        {
            List<Employee>? employees = null;
            var tokenInLocalStorage = await localStorageService.GetItemAsStringAsync("authToken");
            AuthModel<string> auth = new AuthModel<string>() 
            {
                Value = string.Empty,
                Token = tokenInLocalStorage
            };
            var json = JsonConvert.SerializeObject(auth);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", tokenInLocalStorage);
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
