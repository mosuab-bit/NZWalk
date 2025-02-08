using NZ_Walk.Models.Domain;

namespace NZ_Walk.Repositories
{
    public interface IRegionRepository
    {
        // you cannot make List<T> nullable in this way, because List<T> is a reference type,
        // and reference types in C# are already nullable by default.
        Task<List<Region>> GetAllAsync();

        //beacuse it possible to be null we put ? put the previous method because it return list
        //if the nothing to return by default return null 
        //we use nullable (?) to avoid exception error.
        Task<Region?> GetIdAsync(Guid id);

        Task<Region> CreateAsync(Region region);

        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);

        
    }
}
