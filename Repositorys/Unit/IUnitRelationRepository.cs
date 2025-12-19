using StoreApi.ModelsDTO.Unit;

namespace StoreApi.Repositorys.Unit
{
    public interface IUnitRelationRepository
    {
        //create a new UnitRelation
        Task<int> CreateAsync(UnitRelationCreateDTO dto);

        // Get all Units with optional search
        Task<List<UnitRelationDTO>> GetAllForViewAsync(string? search);
        // update the Unit with the id
        Task<bool> UpdateAsync(int id, UnitRelationCreateDTO dto);
        // Delete the Unit with the id
        Task<bool> DeleteAsync(int id);

        // Change the status for Unit with the id
        Task<bool> ChangeStatusAsync(int id, UnitRelationStatus dto);


    }
}



 
       
