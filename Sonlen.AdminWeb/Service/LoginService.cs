

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

        public LoginService(ILocalStorageService localStorageService, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            this.localStorageService = localStorageService;
            this.httpClient = httpClient;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> LoginAsync(LoginModel userInfo)
        {
            bool result = false;
            //var json = JsonConvert.SerializeObject(userInfo);
            //HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            //var response = await httpClient.PostAsync("/api/Auth/Login", httpContent);

            //if (response.IsSuccessStatusCode)
            //{

            //    var resContent = await response.Content.ReadAsStringAsync();
            //    UserToken? userToken = JsonConvert.DeserializeObject<UserToken>(resContent);
            //    if (userToken != null)
            //    {
            //        await localStorageService.SetItemAsync<string>("authToken", userToken.token);
            //        ((CustomAuthStateProvider)authenticationStateProvider).NotifyUserAuthentication(userToken.token);

            //        result = true;
            //    }
            //}

            /**假登入*/
            Employee employee = new Employee()
            {
                EmployeeName = "Test",
                Sex = 1,
                EmployeeID = "1",
                Email = "test@gmail.com"
            };

            var claims = LoginInfo(employee);
            if (claims.Count > 0)
            {
                await localStorageService.SetItemAsync("authToken", employee.EmployeeName);
                ((CustomAuthStateProvider)authenticationStateProvider).FakeUserAuthentication(claims);
                result = true;
            }
            return result;
        }

        public async Task LogoutAsync()
        {
            await localStorageService.RemoveItemAsync("authToken");
            ((CustomAuthStateProvider)authenticationStateProvider).NotifyUserLogOut();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }

        /// <summary>
        ///假登入
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private List<Claim> LoginInfo(Employee employee)
        {
            var claims = new List<Claim>()
            {
                new Claim("Name", employee.EmployeeName ?? string.Empty),
                new Claim("role", "employee")
            };

            return claims;
        }
    }
}
