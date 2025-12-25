using Microsoft.EntityFrameworkCore;
using StoreApi.Models;
using StoreApi.ModelsDTO.Item;

namespace StoreApi.Repositorys.Item
{
    public class ItemVersionRepository : IItemVersionRepository
    {
        private readonly StoreContext _context;

        public ItemVersionRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ItemVersionCreateDTO dto)
        {
            var entity = new ItemVersion
            {
                ItemId = dto.ItemId,
                VersionNumber = dto.VersionNumber,
                DataSnapshot = dto.DataSnapshot,
                CreatedAt = dto.CreatedAt,
                CreatedByUserId = dto.CreatedByUserId
            };

            _context.ItemVersions.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ItemVersionDTO>> GetByItemIdAsync(int itemId)
        {
            return await _context.ItemVersions
                .Where(x => x.ItemId == itemId)
                .OrderByDescending(x => x.VersionNumber)
                .Select(x => new ItemVersionDTO
                {
                    ItemVersionId = x.ItemVersionId,
                    ItemId = x.ItemId,
                    VersionNumber = x.VersionNumber,
                    DataSnapshot = x.DataSnapshot,
                    CreatedAt = x.CreatedAt,
                    CreatedByUserId = x.CreatedByUserId
                })
                .ToListAsync();
        }

        public async Task<int> GetLastVersionNumberAsync(int itemId)
        {
            return await _context.ItemVersions
                .Where(x => x.ItemId == itemId)
                .Select(x => (int?)x.VersionNumber)
                .MaxAsync() ?? 0;
        }
    }
}
