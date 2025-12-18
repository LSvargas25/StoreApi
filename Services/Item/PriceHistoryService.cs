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

        public Task<bool> UpdatePriceHistoryAsync(int id, PriceHistoryUpdateDTO dto)
            => _repository.UpdateAsync(id, dto);

        public Task<bool> DeletePriceHistoryAsync(int id)
            => _repository.DeleteAsync(id);

        public Task<decimal?> GetCurrentSalePriceAsync(int itemVariantId)
            => _repository.GetCurrentSalePriceAsync(itemVariantId);

        public Task<decimal?> GetCurrentCostAsync(int itemVariantId)
            => _repository.GetCurrentCostAsync(itemVariantId);

        public Task<List<PriceHistoryDTO>> GetPriceHistoryByVariantAsync(
            int itemVariantId,
            DateTime? from,
            DateTime? to)
            => _repository.GetByVariantAsync(itemVariantId, from, to);

        public Task<List<ListPriceHistoryDTO>> ListCurrentPricesAsync(string? search)
            => _repository.ListCurrentPricesAsync(search);

        public Task<List<ListCostHistoryDTO>> ListCurrentCostsAsync(string? search)
            => _repository.ListCurrentCostsAsync(search);

        public Task<List<ListCostByUserDTO>> ListCostHistoryByUserAsync(int userId)
            => _repository.ListCostByUserAsync(userId);

        public Task<List<ListPriceByUserDTO>> ListPriceHistoryByUserAsync(int userId)
            => _repository.ListPriceByUserAsync(userId);

        public Task<List<ListAllPriceHistoryDTO>> ListAllPriceHistoryAsync(
            string? search,
            int page,
            int limit)
            => _repository.ListAllAsync(search, page, limit);
    }
}
