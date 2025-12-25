using StoreApi.ModelsDTO.ItemVariant;

namespace StoreApi.Interface.ItemVariant
{
    public interface IItemVariantService
    {
        Task<(bool Success, string Message, int? NewId)> CreateAsync(ItemVariantCreateDTO dto);
        Task<(bool Success, string Message, ItemVariantDTO? Data)> GetByIdAsync(int itemVariantId);

        Task<(bool Success, string Message)> UpdateAsync(int itemVariantId, ItemVariantUpdateDTO dto);
        Task<(bool Success, string Message)> DeleteAsync(int itemVariantId);

        Task<List<ListVariantDTO>> GetAllActiveAsync(string? search = null);
        Task<List<ListVariantDTO>> GetAllInactiveAsync(string? search = null);

        Task<List<ListCompleteVariantDTO>> GetCompleteListAsync(int itemId);
        Task<List<ListPriceCost>> GetPriceCostListAsync(int itemId);

        Task<(bool Success, string Message)> ChangeStatusAsync(int itemVariantId, bool isActive);

        Task<List<ListItemVariantByItemIdDTO>> GetByItemIdAsync(int itemId);
        Task<(bool Success, string Message, ItemStatsDTO? Data)> GetItemStatsAsync();



        // NUEVO
        Task<(bool Success, string Message)> UpdatePriceAsync(int itemVariantId, UpdateVariantPriceDTO dto);
        Task<(bool Success, string Message)> UpdateCostAsync(int itemVariantId, UpdateVariantCostDTO dto);
        Task<(bool Success, string Message, List<PriceHistoryDTO>? Data)> GetPriceHistoryAsync(int itemVariantId);
        Task<(bool Success, string Message, List<ItemCostHistoryDTO>? Data)> GetCostHistoryAsync(int itemVariantId);
    }
}
