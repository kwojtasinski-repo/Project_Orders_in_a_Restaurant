using Restaurant.Shared.DTO;
using System.Threading.Tasks;

namespace Restaurant.UI.Services
{
    public interface ISettingsService
    {
        Task<ApiResult<SettingsDto>> GetSettings();
        Task<ApiResult> SaveSettings(SettingsDto dto);
    }
}
