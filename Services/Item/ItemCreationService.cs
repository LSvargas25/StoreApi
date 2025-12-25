using StoreApi.Interface.Item;
using StoreApi.ModelsDTO.Item;

namespace StoreApi.Services.Item
{
    public class ItemCreationService : IItemCreationService
    {
        private readonly IItemService _itemService;
        private readonly IItemImageService _itemImageService;
        private readonly IItemAttributeDetailService _attributeDetailService;

        public ItemCreationService(
            IItemService itemService,
            IItemImageService itemImageService,
            IItemAttributeDetailService attributeDetailService)
        {
            _itemService = itemService;
            _itemImageService = itemImageService;
            _attributeDetailService = attributeDetailService;
        }

        public async Task<int> CreateFullItemAsync(ItemFullCreateDTO dto)
        {
            // 1️⃣ Crear ITEM (incluye auditoría)
            var item = await _itemService.CreateAsync(new ItemCreateDTO
            {
                Name = dto.Name,
                Description = dto.Description,
                Barcode = dto.Barcode,
                Brand = dto.Brand,

                Weight = dto.Weight,
                Height = dto.Height,
                Width = dto.Width,
                Length = dto.Length,

                ItemCategoryId = dto.ItemCategoryId


            });

            var itemId = item.ItemId;

            // 2️⃣ Crear IMÁGENES
            foreach (var img in dto.Images)
            {
                await _itemImageService.CreateAsync(new ItemImageDTO
                {
                    ItemId = itemId,
                    Url = img.Url,
                    IsPrimary = img.IsPrimary
                });
            }

            // 3️⃣ Crear ATRIBUTOS
            foreach (var attr in dto.Attributes)
            {
                await _attributeDetailService.CreateAttributeDetailAsync(
                    itemId,
                    attr
                );
            }

            return itemId;
        }
    }
}
