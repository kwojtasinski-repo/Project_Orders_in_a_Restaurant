using System.Threading.Tasks;
using Restaurant.UI.DTO;

namespace Restaurant.UI.Services
{
    public interface IMenuService
    {
        Task<ApiResult<MenuDto>> GetMenuAsync();
    }
}
