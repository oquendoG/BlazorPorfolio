using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Server.Feats.Blog.Categories.Controllers;
using Server.Feats.Blog.Images.Commands;
using Server.Feats.Blog.Images.Controllers;
using Shared.Models.Blog;
using System.Drawing;
using System.Net;

namespace Tests.ServerTests.Blog.Images;
public class ImageUploadControllerTests
{
    private readonly ImageUploadController imageUploadController;
    private readonly Mock<IMediator> mediatorMock;
    private readonly Fixture fixture;
    public ImageUploadControllerTests()
    {
        mediatorMock = new();
        imageUploadController = new(mediatorMock.Object);
        fixture = new();
    }

    [Fact]
    public async void ShouldReturnBadRequest_WhenModelIsInvalid()
    {
        UploadedImage uploadedImage = new();

        imageUploadController.ModelState.AddModelError("NewImageFileExtension", "Campo requerido");
        imageUploadController.ModelState.AddModelError("NewImageBase64Content", "Campo requerido");
        imageUploadController.ModelState.AddModelError("OldImagePath", "Campo requerido");

        IActionResult result =  await imageUploadController.Post(uploadedImage);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async void ShoudReturn500Error_WhenExceptionIsFound()
    {
        UploadedImage uploadedImage = fixture.Create<UploadedImage>();

        mediatorMock.Setup(mediator => mediator.Send(It.IsAny<CreateImageCommandRequest>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(string.Empty);

        IActionResult actionResult = await imageUploadController.Post(uploadedImage);
        ObjectResult? objectResult = (actionResult as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async void ShoudReturnCreated_WhenModelIsValid()
    {
        UploadedImage uploadedImage = new()
        {
            NewImageBase64Content = Convert.ToBase64String(CreateTestImageBytes(500, 500)),
            NewImageFileExtension = ".jpeg",
            OldImagePath = "uploads/placeholder.jpg",
        };

        imageUploadController.StatusCode(201);

        IActionResult result = await imageUploadController.Post(uploadedImage);
        ObjectResult? objectResult = (result as ObjectResult)!;
        objectResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
    }

    private static byte[] CreateTestImageBytes(int width, int height)
    {
        using var bitmap = new Bitmap(width, height);
        using var ms = new MemoryStream();
        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        return ms.ToArray();
    }
}
