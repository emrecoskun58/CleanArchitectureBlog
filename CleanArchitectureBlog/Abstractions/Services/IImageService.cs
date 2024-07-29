namespace CleanArchitectureBlog.Abstractions.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string blogId);
    }
}
