using StoreApi.ModelsDTO.Item;

namespace StoreApi.Repositorys.Item
{
    public interface IPriceHistoryRepository
    {
       
        // CREATE 
       
        Task<int> CreateAsync(PriceHistoryCreateDTO dto);
       


   
        // HISTORY
       
        Task<List<PriceHistoryDTO>> GetByVariantAsync(
            int itemVariantId,
            DateTime? from,
            DateTime? to
        );

      
        // AUDIT
      
        Task<List<ListPriceByUserDTO>> ListPriceByUserAsync(int userAccountId);

        // ADMIN
       
        Task<List<ListAllPriceHistoryDTO>> ListAllAsync(
            string? search,
            int page,
            int limit
        );
    }
}
