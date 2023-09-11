using MediatR;
using Shared.Models.Blog;

namespace Server.Feats.Blog.Images.Commands;

public record CreateImageCommandRequest(UploadedImage UploadedImage) : IRequest<string>;
public class CreateImageCommandHandler : IRequestHandler<CreateImageCommandRequest, string>
{
    private readonly ILogger<CreateImageCommandHandler> logger;
    private readonly IWebHostEnvironment hostEnvironment;

    public CreateImageCommandHandler(
        ILogger<CreateImageCommandHandler> logger, IWebHostEnvironment hostEnvironment)
    {
        this.logger = logger;
        this.hostEnvironment = hostEnvironment;
    }

    public async Task<string> Handle(CreateImageCommandRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.UploadedImage.OldImagePath != string.Empty && request.UploadedImage.OldImagePath != "uploads/placeholder.jpg")
            {
                string oldUploadedImageFileName = request.UploadedImage.OldImagePath.Split('/').Last();
                File.Delete(Path.Combine(hostEnvironment.ContentRootPath, "wwwroot", "uploads", oldUploadedImageFileName));
            }

            string guid = Guid.NewGuid().ToString();
            string imageFileName = guid + request.UploadedImage.NewImageFileExtension;
            string fullImageImageFileSystemPath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot", "uploads", imageFileName);

            using FileStream filestream = File.Create(fullImageImageFileSystemPath);
            byte[] imageContent = Convert.FromBase64String(request.UploadedImage.NewImageBase64Content);
            await filestream.WriteAsync(imageContent, cancellationToken);
            filestream.Close();

            string relativePathWithoutTrailingSlashes = Path.Combine("uploads", imageFileName);

            return relativePathWithoutTrailingSlashes;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Se ha producido una excepción al guardar la imagen");
            return string.Empty;
        }
    }
}