using StoreApi.ModelsDTO.Customer;
using StoreApi.ModelsDTO.Item;
namespace StoreApi.Interface.Item
{
    public interface IPriceHistoryService
    {
        // ===============================
            // CURRENT VALUES
            // ===============================
            Task<decimal?> GetCurrentSalePriceAsync(int itemVariantId);
            Task<decimal?> GetCurrentCostAsync(int itemVariantId);

            // ===============================
            // BY VARIANT
            // ===============================
            Task<List<PriceHistoryDTO>> GetPriceHistoryByVariantAsync(
                int itemVariantId,
                DateTime? from = null,
                DateTime? to = null
            );

            // ===============================
            // DASHBOARD LISTS
            // ===============================
            Task<List<ListPriceHistoryDTO>> ListCurrentPricesAsync(string? search);
            Task<List<ListCostHistoryDTO>> ListCurrentCostsAsync(string? search);

            // ===============================
            // ADMIN / AUDIT
            // ===============================
            Task<List<ListAllPriceHistoryDTO>> ListAllPriceHistoryAsync(
                string? search,
                int page,
                int limit
            );

            Task<List<ListCostByUserDTO>> ListCostHistoryByUserAsync(int userAccountId);
            Task<List<ListPriceByUserDTO>> ListPriceHistoryByUserAsync(int userAccountId);

            // ===============================
            // WRITE
            // ===============================
            Task<int> CreatePriceHistoryAsync(PriceHistoryCreateDTO dto);
            Task<bool> UpdatePriceHistoryAsync(int priceHistoryId, PriceHistoryUpdateDTO dto);
            Task<bool> DeletePriceHistoryAsync(int priceHistoryId);
        }

    
}




