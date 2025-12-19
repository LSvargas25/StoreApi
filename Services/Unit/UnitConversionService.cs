using StoreApi.Interface.Unit;
using StoreApi.ModelsDTO.Unit;
using StoreApi.ModelsDTO.Unit.StoreApi.ModelsDTO.Unit;

namespace StoreApi.Services.Unit
{
    public class UnitConversionService : IUnitConversionService
    {
        private readonly IUnitConversionRepository _repo;

        public UnitConversionService(IUnitConversionRepository repo)
        {
            _repo = repo;
        }

        public async Task<UnitConversionCreateDTO> CreateUnitConversionAsync(UnitConversionCreateDTO dto)
        {
            if (dto.FromUnitId == dto.ToUnitId)
                throw new ArgumentException("FromUnit and ToUnit must be different.");

            if (dto.Factor <= 0)
                throw new ArgumentException("Factor must be greater than zero.");

            var id = await _repo.CreateAsync(dto);

            if (id <= 0)
                throw new Exception("Unit conversion was not created successfully.");

            return dto;
        }

        public Task<List<UnitConversionDTO>> GetAllUnitConversionsAsync(string? search)
            => _repo.GetAllAsync(search);

        public Task<bool> UpdateUnitConversionAsync(int id, UnitConversionCreateDTO dto)
            => _repo.UpdateAsync(id, dto);

        public Task<bool> DeleteUnitConversionAsync(int id)
            => _repo.DeleteAsync(id);

        // ===============================
        // CONVERT (DIRECT + INVERSE + CHAIN)
        // ===============================
        public async Task<UnitConversionResultDTO> ConvertAsync(
            int fromUnitId,
            int toUnitId,
            decimal value)
        {
            var fromName = await _repo.GetUnitNameAsync(fromUnitId);
            var toName = await _repo.GetUnitNameAsync(toUnitId);

            if (fromName == null || toName == null)
                throw new Exception("Unit not found.");

            var result = await ConvertValueAsync(fromUnitId, toUnitId, value);

            return new UnitConversionResultDTO
            {
                OriginalValue = value,
                ResultValue = result,
                FromUnitName = Pluralize(fromName, value),
                ToUnitName = Pluralize(toName, result)
            };
        }

        private async Task<decimal> ConvertValueAsync(
            int fromUnitId,
            int toUnitId,
            decimal value)
        {
            if (fromUnitId == toUnitId)
                return value;

            var direct = await _repo.GetFactorAsync(fromUnitId, toUnitId);
            if (direct.HasValue)
                return value * direct.Value;

            var inverse = await _repo.GetFactorAsync(toUnitId, fromUnitId);
            if (inverse.HasValue)
                return value / inverse.Value;

            var all = await _repo.GetAllConversionsAsync();
            var result = ResolveChain(fromUnitId, toUnitId, value, all, new HashSet<int>());

            if (!result.HasValue)
                throw new Exception("Unit conversion path not found.");

            return result.Value;
        }

        private decimal? ResolveChain(
            int current,
            int target,
            decimal value,
            List<UnitConversionDTO> conversions,
            HashSet<int> visited)
        {
            if (!visited.Add(current))
                return null;

            foreach (var c in conversions.Where(x => x.FromUnitId == current))
            {
                var nextValue = value * c.Factor;

                if (c.ToUnitId == target)
                    return nextValue;

                var chained = ResolveChain(
                    c.ToUnitId,
                    target,
                    nextValue,
                    conversions,
                    visited);

                if (chained.HasValue)
                    return chained.Value;
            }

            return null;
        }

        private string Pluralize(string name, decimal value)
            => value == 1 ? name : $"{name}s";
    }
}
