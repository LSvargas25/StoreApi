using StoreApi.ModelsDTO.Customer;
using StoreApi.ModelsDTO.Item;
namespace StoreApi.Interface.Item
{
    public interface IPriceHistoryService
    {
       
            Task<List<PriceHistoryDTO>> GetPriceHistoryByVariantAsync(
                int itemVariantId,
                DateTime? from = null,
                DateTime? to = null
            );
            // ADMIN / AUDIT
        
            Task<List<ListAllPriceHistoryDTO>> ListAllPriceHistoryAsync(
                string? search,
                int page,
                int limit
            );
            Task<List<ListPriceByUserDTO>> ListPriceHistoryByUserAsync(int userAccountId);

       
            Task<int> CreatePriceHistoryAsync(PriceHistoryCreateDTO dto);
      
        }

    
}




