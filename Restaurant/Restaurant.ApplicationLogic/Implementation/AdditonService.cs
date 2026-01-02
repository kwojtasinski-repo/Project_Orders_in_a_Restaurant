using Restaurant.ApplicationLogic.DTO;
using Restaurant.ApplicationLogic.Interfaces;
using Restaurant.ApplicationLogic.Mappings;
using Restaurant.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.ApplicationLogic.Implementation
{
    internal class AdditonService : IAdditonService
    {
        private readonly IAdditonRepository _additonRepository;

        public AdditonService(IAdditonRepository additionRepository)
        {
            _additonRepository = additionRepository;
        }

        public AdditionDto Get(Guid id)
        {
            var addition = _additonRepository.Get(id);
            return addition?.AsDto();
        }

        public IEnumerable<AdditionDto> GetAll()
        {
            var additions = _additonRepository.GetAll();
            return additions.Select(a => a.AsDto());
        }

        public async Task<IEnumerable<AdditionDto>> GetAllAsync()
        {
            var additions = await _additonRepository.GetAllAsync();
            return additions.Select(a => a.AsDto());
        }
    }
}
