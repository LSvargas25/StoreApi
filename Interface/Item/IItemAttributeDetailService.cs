using StoreApi.ModelsDTO.Item;

namespace StoreApi.Interface.Item
{
    public interface IItemAttributeDetailService
    {
        Task<List<AttributeDetailDTO>> GetAllAttributeDetailsByItemIdAsync(int itemId);

        Task<bool> CreateAttributeDetailAsync(
            int itemId,
            AttributeCreateDetailDTO attributeDetail);

        Task<bool> UpdateAttributeDetailAsync(
            int itemId,
            int attributeId,
            AttributeCreateDetailDTO attributeDetail);

        Task<bool> DeleteAttributeDetailAsync(
            int itemId,
            int attributeId);

        Task<bool> ChangeFavoriteStatusAsync(
            int itemId,
            ChangeFavoriteStatusDTO statusDto);

        Task<List<ResultDTO>> GetResultAsync(string? search);
    }
}
