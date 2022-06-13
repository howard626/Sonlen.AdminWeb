using Newtonsoft.Json;
using Sonlen.WebAdmin.Model;
using System.Text;

namespace Sonlen.AdminWeb.Service
{
    public class SalaryService : ISalaryService
    {
        private readonly HttpClient httpClient;
        private readonly string API_ADDRESS = $"{Setting.API_ADDRESS}Salary/";

        public SalaryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /** 取得上個月的薪資資料*/
        public async Task<SalaryViewModel?> GetDataByModelAsync(SalaryViewModel model)
        {
            string paymon;
            if (model.Month == 1)
            {
                paymon = $"{(model.Year - 1).ToString().PadLeft(3, '0')}12";
            }
            else 
            {
                paymon = $"{model.Year.ToString().PadLeft(3, '0')}{(model.Month - 1).ToString().PadLeft(2, '0')}";
            }

            SalaryViewModel salary = new SalaryViewModel()
            {
                EmployeeID = model.EmployeeID,
                PayMon = paymon
            };
            var json = JsonConvert.SerializeObject(salary);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{API_ADDRESS}GetSalary", httpContent);
            var resContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                salary = JsonConvert.DeserializeObject<SalaryViewModel>(resContent) ?? new SalaryViewModel();
                if (string.IsNullOrEmpty(salary.EmployeeID))
                {
                    return null;
                }
                return salary;
            }

            return null;
        }

        /** 列印薪資*/
        public async Task<UploadFile> PrintAsync(SalaryViewModel model)
        {
            UploadFile? uploadFile = null;
            model.PayMon = $"{model.Year.ToString().PadLeft(3, '0')}{model.Month.ToString().PadLeft(2, '0')}";

            var json = JsonConvert.SerializeObject(model);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{API_ADDRESS}Print", httpContent);

            var resContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                uploadFile = JsonConvert.DeserializeObject<UploadFile>(resContent);
            }

            return uploadFile ?? new UploadFile();
        }
    }
}
