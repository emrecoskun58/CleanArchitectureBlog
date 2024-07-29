using CleanArchitectureBlog.Abstractions.Repositories.BlogRepository;
using CleanArchitectureBlog.Abstractions.Services;

namespace CleanArchitectureBlog.Services
{
    public class ImageService : IImageService
    {
        private readonly IConfiguration _configuration;
        private readonly IBlogReadRepository _blogReadRepository;
        private readonly ILogger<ImageService> _logger;

        public ImageService(IConfiguration configuration, IBlogReadRepository blogReadRepository, ILogger<ImageService> logger)
        {
            _configuration = configuration;
            _blogReadRepository = blogReadRepository;
            _logger = logger;
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string blogId)
        {
            try
            {
                var blog = await _blogReadRepository.GetByIdAsync(blogId);
                if (blog == null)
                    throw new ArgumentException("Blog Bulunamadı !");
                if (imageFile == null || imageFile.Length == 0)
                    throw new ArgumentException("Resim Bulunamadı !");

                string localPath = _configuration["Path:UserBlogPath"];
                string directoryPath = Path.Combine(localPath);
                string fileName = $"{blog.Id}{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                string filePath = Path.Combine(directoryPath, fileName);

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Resimler kaydedilirken bir sorun oluştu.");
                throw;
            }
        }
    }
}
