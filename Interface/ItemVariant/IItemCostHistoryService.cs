using StoreApi.ModelsDTO.Item;

namespace StoreApi.Interface.ItemVariant
{
    public interface IItemCostHistoryService
    {
        Task<List<ListCostByUserDTO>> ListCostHistoryByUserAsync(int userAccountId);

    }
}
