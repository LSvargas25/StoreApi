using StoreApi.ModelsDTO.Item;

namespace StoreApi.Interface.Item
{
    public interface IItemAttributeService
    {
        Task<List<AttributeDTO>> GetAllAttributesAsync(string? search);
        Task<int> CreateAttributeAsync(AttributeCreateDTO attribute);

        Task<bool> UpdateAttributeAsync(int id, AttributeCreateDTO attribute);
        Task<bool> DeleteAttributeAsync(int id);


    }
}
