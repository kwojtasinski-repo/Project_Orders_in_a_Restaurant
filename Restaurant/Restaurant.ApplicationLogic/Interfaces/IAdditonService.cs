using Restaurant.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurant.ApplicationLogic.Interfaces
{
    public interface IAdditonService : IService
    {
        AdditionDto Get(Guid id);
        IEnumerable<AdditionDto> GetAll();
        Task<IEnumerable<AdditionDto>> GetAllAsync();
    }
}
