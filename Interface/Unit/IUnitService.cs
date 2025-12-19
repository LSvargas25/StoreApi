using StoreApi.ModelsDTO.Unit;
namespace StoreApi.Interface.Unit
{
    public interface IUnitService
    {
        //get all units
        Task<List<UnitDTO>> GetAllUnitsAsync(string? search);
        //create a new unit
        Task<int> CreateUnitAsync(UnitCreateDTO dto);
        //update a unit
        Task<bool> UpdateUnitAsync(int id, UnitCreateDTO dto);
        //delete a unit
        Task<bool> DeleteUnitAsync(int id);
    }
}
