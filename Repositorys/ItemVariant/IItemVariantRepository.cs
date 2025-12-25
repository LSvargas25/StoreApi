using StoreApi.ModelsDTO.ItemVariant;

namespace StoreApi.Repositorys.ItemVariant
{
    public interface IItemVariantRepository
    {
        Task<ItemStatsDTO> GetItemStatsAsync();
        Task<int> CreateAsync(ItemVariantCreateDTO dto);
        Task<ItemVariantDTO?> GetByIdAsync(int itemVariantId);

        Task<bool> UpdateAsync(int itemVariantId, ItemVariantUpdateDTO dto);
        Task<bool> DeleteAsync(int itemVariantId);

        Task<bool> ChangeStatusAsync(int itemVariantId, bool isActive);

        Task<List<ListCompleteVariantDTO>> GetCompleteListAsync(int itemId);
        Task<List<ListPriceCost>> GetPriceCostListAsync(int itemId);

        // NUEVO: historial y cambios con auditoría
        Task<bool> UpdatePriceAsync(int itemVariantId, UpdateVariantPriceDTO dto);
        Task<bool> UpdateCostAsync(int itemVariantId, UpdateVariantCostDTO dto);
        Task<List<PriceHistoryDTO>> GetPriceHistoryAsync(int itemVariantId);
        Task<List<ItemCostHistoryDTO>> GetCostHistoryAsync(int itemVariantId);
    }
}
