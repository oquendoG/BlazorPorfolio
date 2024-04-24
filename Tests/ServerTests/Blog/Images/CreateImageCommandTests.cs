using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Server.Feats.Blog.Images.Commands;
using Shared.Models.Blog;
using System.Drawing;

namespace Tests.ServerTests.Blog.Images;
public class CreateImageCommandTests
{
    private readonly Mock<CreateImageCommandRequest> createImageCommandRequestMock;
    private readonly Mock<CreateImageCommandHandler> createImageCommandHandlerMock;
    private readonly Mock<ILogger<CreateImageCommandHandler>> LoggerMock;
    private readonly Mock<IWebHostEnvironment> hostEnvironmentMock;

    public CreateImageCommandTests()
    {
        createImageCommandRequestMock = new();
        createImageCommandHandlerMock = new();
        LoggerMock = new();
        hostEnvironmentMock = new();
    }

    [Fact]
    public async void ShouldCreateImage_WhenModelIsValid()
    {
        UploadedImage uploadedImage = new()
        {
            NewImageBase64Content = Convert.ToBase64String(CreateTestImageBytes(500, 500)),
            NewImageFileExtension = ".jpeg",
            OldImagePath = "uploads/placeholder.jpg",
        };

        hostEnvironmentMock.Setup(h => h.ContentRootPath).Returns(GetImageDirectory());

        CreateImageCommandRequest request = new(uploadedImage);
        CreateImageCommandHandler handler = new(LoggerMock.Object, hostEnvironmentMock.Object);

        string result = await handler.Handle(request, It.IsAny<CancellationToken>());

        result.Should().BeOfType<string>();
        result.Should().NotBeNull();
        result.Should().Contain(@"\");
        result.Should().Contain(".jpeg");
    }

    private static byte[] CreateTestImageBytes(int width, int height)
    {
        using var bitmap = new Bitmap(width, height);
        using var ms = new MemoryStream();
        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        return ms.ToArray();
    }

    private static string GetImageDirectory()
    {
        string fullPath = System.IO.Directory.GetCurrentDirectory();

        string[] dividedPath = fullPath.Split(@"\");
        List<string> neededPathDivided = [];

        neededPathDivided.Add(dividedPath[0]);

        for (int i = 1; i < dividedPath.Length; i++)
        {
            if (dividedPath[i] == "Tests") break;
            neededPathDivided.Add($@"\{dividedPath[i]}");
        }

        string neededPathJoined = neededPathDivided.Aggregate((a, b) => $"{a}{b}");


        string targetPath = Path.Combine(neededPathJoined, "Server");

        return targetPath;
    }

    [Fact]
    public async Task ShouldReturnEmptyString_WhenExceptionIsFound()
    {
        //al crear datos aleatorios incorrectos no se cumplen las reglas de negocio y se produce una excepción
        CreateImageCommandHandler handler = new(LoggerMock.Object, hostEnvironmentMock.Object);
        string result = await handler.Handle(It.IsAny<CreateImageCommandRequest>(), It.IsAny<CancellationToken>());

        result.Should().Be(string.Empty);
    }
}
