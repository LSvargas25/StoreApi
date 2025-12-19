using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;
using StoreApi.Repositorys.Item;

namespace StoreApi.Services.Item
{
    public class PriceHistoryService : IPriceHistoryService
    {
        private readonly IPriceHistoryRepository _repository;

        public PriceHistoryService(IPriceHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreatePriceHistoryAsync(PriceHistoryCreateDTO dto)
        {
            if (dto.Cost < 0 || dto.SalePrice < 0)
                throw new ApplicationException("Costos o precios inválidos.");

            return await _repository.CreateAsync(dto);
        }

       

        public Task<List<PriceHistoryDTO>> GetPriceHistoryByVariantAsync(
            int itemVariantId,
            DateTime? from,
            DateTime? to)
            => _repository.GetByVariantAsync(itemVariantId, from, to);

       
        public Task<List<ListPriceByUserDTO>> ListPriceHistoryByUserAsync(int userId)
            => _repository.ListPriceByUserAsync(userId);

        public Task<List<ListAllPriceHistoryDTO>> ListAllPriceHistoryAsync(
            string? search,
            int page,
            int limit)
            => _repository.ListAllAsync(search, page, limit);
    }
}
