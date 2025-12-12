using StoreApi.ModelsDTO.Item;
using StoreApi.ModelsDTO.Purshase;
using StoreApi.ModelsDTO.Supplier;
namespace StoreApi.Interface.Item
{
    public interface IItemImageService
    {
 

        // Creates a new  image and returns the ID of the newly created record.
        Task<int> CreateAsync(ItemImageDTO dto);

        // Delete Image 
        Task<bool> DeleteAsync(int id);

        // selected if is a primary
        // IMPORTANT: This should set ALL other images of the same item to IsPrimary = false.
        Task<bool> SetAsPrimaryAsync(int imageId);

        
    }
}
