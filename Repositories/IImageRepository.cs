using NZ_Walk.Models.Domain;

namespace NZ_Walk.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
