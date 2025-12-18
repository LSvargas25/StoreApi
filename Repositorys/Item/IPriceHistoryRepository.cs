using StoreApi.ModelsDTO.Item;

namespace StoreApi.Repositorys.Item
{
    public interface IPriceHistoryRepository
    {
        // ===============================
        // CREATE / UPDATE / DELETE
        // ===============================
        Task<int> CreateAsync(PriceHistoryCreateDTO dto);
        Task<bool> UpdateAsync(int priceHistoryId, PriceHistoryUpdateDTO dto);
        Task<bool> DeleteAsync(int priceHistoryId);

        // ===============================
        // CURRENT VALUES
        // ===============================
        Task<decimal?> GetCurrentSalePriceAsync(int itemVariantId);
        Task<decimal?> GetCurrentCostAsync(int itemVariantId);

        // ===============================
        // HISTORY
        // ===============================
        Task<List<PriceHistoryDTO>> GetByVariantAsync(
            int itemVariantId,
            DateTime? from,
            DateTime? to
        );

        // ===============================
        // DASHBOARD
        // ===============================
        Task<List<ListPriceHistoryDTO>> ListCurrentPricesAsync(string? search);
        Task<List<ListCostHistoryDTO>> ListCurrentCostsAsync(string? search);

        // ===============================
        // AUDIT
        // ===============================
        Task<List<ListCostByUserDTO>> ListCostByUserAsync(int userAccountId);
        Task<List<ListPriceByUserDTO>> ListPriceByUserAsync(int userAccountId);

        // ===============================
        // ADMIN
        // ===============================
        Task<List<ListAllPriceHistoryDTO>> ListAllAsync(
            string? search,
            int page,
            int limit
        );
    }
}
