using Newtonsoft.Json;
using Sonlen.WebAdmin.Model;
using System.Text;

namespace Sonlen.AdminWeb.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly HttpClient httpClient;
        private readonly string API_ADDRESS = $"{Setting.API_ADDRESS}Employee/";

        public EmployeeService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /** 以身分證字號取得單一員工資料*/
        public async Task<Employee?> GetEmployeeAsync(string id)
        {
            Employee employee = new Employee()
            {
                EmployeeID = id
            };
            var json = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}GetEmployee", httpContent);
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

            var json = JsonConvert.SerializeObject("");
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}GetAllEmployees", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                employees = JsonConvert.DeserializeObject<List<Employee>>(resContent);
            }

            return employees ?? new List<Employee>();
        }

        /** 新增員工*/
        public async Task<string> AddEmployeeAsync(Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}Add", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        /** 更新員工*/
        public async Task<string> UpdateEmployeeAsync(Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}Update", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }

        /** 刪除員工*/
        public async Task<string> DeleteEmployeeAsync(Employee employee)
        {
            var json = JsonConvert.SerializeObject(employee);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}Delete", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
