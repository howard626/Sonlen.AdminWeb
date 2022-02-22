
namespace Sonlen.AdminWeb.Service
{
    public interface IPunchService
    {
        Task<string> PunchInAsync();
        Task<string> PunchOutAsync();
    }
}