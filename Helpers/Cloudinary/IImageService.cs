namespace WriteTogether.Helpers.Cloudinary
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
