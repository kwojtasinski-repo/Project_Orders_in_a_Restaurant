using Restaurant.Shared.DTO;

namespace Restaurant.UI.Services
{
    public interface ISettingsService
    {
        Task<ApiResult<SettingsDto>> GetSettings();
        Task<ApiResult> SaveSettings(SettingsDto dto);
    }
}
