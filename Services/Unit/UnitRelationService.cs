using StoreApi.Interface.Unit;
using StoreApi.ModelsDTO.Unit;
using StoreApi.Repositorys.Unit;

namespace StoreApi.Services.Unit
{
    public class UnitRelationService : IUnitRelationService
    {
        private readonly IUnitRelationRepository _repo;

        public UnitRelationService(IUnitRelationRepository repo)
        {
            _repo = repo;
        }

        public async Task<UnitRelationCreateDTO> CreateUnitRelationAsync(UnitRelationCreateDTO dto)
        {
            var newId = await _repo.CreateAsync(dto);

            if (newId <= 0)
                throw new Exception("Unit relation was not created successfully.");

            return dto;
        }

        public Task<List<UnitRelationDTO>> GetAllUnitRelationsAsync(string? search)
            => _repo.GetAllForViewAsync(search);

        public Task<bool> UpdateUnitRelationAsync(int id, UnitRelationCreateDTO dto)
            => _repo.UpdateAsync(id, dto);

        public Task<bool> DeleteUnitRelationAsync(int id)
            => _repo.DeleteAsync(id);

        public Task<bool> ChangeStatus(int id, UnitRelationStatus dto)
            => _repo.ChangeStatusAsync(id, dto);
    }
}
