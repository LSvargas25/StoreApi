using StoreApi.ModelsDTO.Supplier;
using StoreApi.ModelsDTO.Unit;

namespace StoreApi.Interface.Unit
{
    public interface IUnitRelationService
    {
        //create a new UnitRelation
        Task<UnitRelationCreateDTO> CreateUnitRelationAsync(UnitRelationCreateDTO dto);

        // Get all Units with optional search
        Task<List<UnitRelationDTO>> GetAllUnitRelationsAsync(string? search);
        // update the Unit with the id
        Task<bool> UpdateUnitRelationAsync(int id, UnitRelationCreateDTO dto);
        // Delete the Unit with the id
        Task<bool> DeleteUnitRelationAsync(int id);

        // Change the status for Unit with the id
        Task<bool> ChangeStatus(int id, UnitRelationStatus dto);
    }
}
