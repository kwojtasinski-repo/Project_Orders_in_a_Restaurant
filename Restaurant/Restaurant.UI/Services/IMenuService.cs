using System.Threading.Tasks;
using Restaurant.Shared.DTO;

namespace Restaurant.UI.Services
{
    public interface IMenuService
    {
        Task<ApiResult<MenuDto>> GetMenuAsync();
    }
}
