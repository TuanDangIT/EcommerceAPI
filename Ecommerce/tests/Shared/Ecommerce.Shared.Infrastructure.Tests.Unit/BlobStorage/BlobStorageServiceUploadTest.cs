using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using Ecommerce.Shared.Abstractions.BloblStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;
using Ecommerce.Shared.Infrastructure.Storage;
using Azure.Storage;
using System.Linq.Expressions;

namespace Ecommerce.Shared.Infrastructure.Tests.Unit.BlobStorage
{
    public class BlobStorageServiceUploadTest
    {
        private readonly string _baseUri = "https://testaccount.blob.core.windows.net";

        [Theory]
        [InlineData("test-image.jpg", "images", "image/jpeg")]
        [InlineData("document.pdf", "documents", "application/pdf")]
        [InlineData("data.json", "data", "application/json")]
        public async Task UploadAsync_WithValidFile_ShouldSucceed(string fileName, string containerName, string contentType)
        {
            // Arrange
            var expectedPath = $"/{containerName}/{fileName}";
            var uri = $"{_baseUri}/{containerName}/{fileName}";

            var (service, _, mockContainerClient, mockBlobClient) = SetupMocks();
            var mockFormFile = SetupMockFormFile(contentType);

            var mockResponse = new Mock<Response<BlobContentInfo>>();
            var mockRawResponse = new Mock<Response>();
            mockRawResponse.Setup(r => r.IsError).Returns(false);
            mockResponse.Setup(r => r.GetRawResponse()).Returns(mockRawResponse.Object);

            var uploadExpression = GetUploadExpression(contentType);
            mockBlobClient.Setup(uploadExpression).ReturnsAsync(mockResponse.Object);
            mockBlobClient.Setup(b => b.Uri).Returns(new Uri(uri));

            // Act
            var result = await service.UploadAsync(mockFormFile.Object, fileName, containerName);

            // Assert
            result.Should().Be(expectedPath);

            mockContainerClient.Verify(c => c.CreateIfNotExistsAsync(
                PublicAccessType.Blob,
                It.IsAny<IDictionary<string, string>>(),
                It.IsAny<BlobContainerEncryptionScopeOptions>(),
                It.IsAny<CancellationToken>()
            ), Times.Once);

            mockBlobClient.Verify(uploadExpression, Times.Once);
        }

        [Fact]
        public async Task UploadAsync_WhenUploadFails_ShouldThrowRequestFailedException()
        {
            // Arrange
            var contentType = "image/jpeg";
            var fileName = "test-image.jpg";
            var containerName = "images";

            var (service, _, _, mockBlobClient) = SetupMocks();
            var mockFormFile = SetupMockFormFile(contentType);

            var mockResponse = new Mock<Response<BlobContentInfo>>();
            var mockRawResponse = new Mock<Response>();
            mockRawResponse.Setup(r => r.IsError).Returns(true);
            mockResponse.Setup(r => r.GetRawResponse()).Returns(mockRawResponse.Object);

            var uploadExpression = GetUploadExpression(contentType);
            mockBlobClient.Setup(uploadExpression).ReturnsAsync(mockResponse.Object);

            // Act
            Func<Task> act = async () => await service.UploadAsync(mockFormFile.Object, fileName, containerName);

            //Assert
            await act.Should().ThrowAsync<RequestFailedException>();
        }

        private (BlobStorageService service, Mock<BlobServiceClient> mockBlobServiceClient,
                Mock<BlobContainerClient> mockContainerClient, Mock<BlobClient> mockBlobClient) SetupMocks()
        {
            var mockFactory = new Mock<IAzureClientFactory<BlobServiceClient>>();
            var mockBlobServiceClient = new Mock<BlobServiceClient>();
            var mockContainerClient = new Mock<BlobContainerClient>();
            var mockBlobClient = new Mock<BlobClient>();

            mockFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockBlobServiceClient.Object);
            mockBlobServiceClient.Setup(b => b.GetBlobContainerClient(It.IsAny<string>())).Returns(mockContainerClient.Object);
            mockContainerClient.Setup(c => c.GetBlobClient(It.IsAny<string>())).Returns(mockBlobClient.Object);
            mockContainerClient.Setup(c => c.CreateIfNotExistsAsync(
                It.IsAny<PublicAccessType>(),
                It.IsAny<IDictionary<string, string>>(),
                It.IsAny<BlobContainerEncryptionScopeOptions>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Mock<Response<BlobContainerInfo>>().Object);

            var service = new BlobStorageService(mockFactory.Object);

            return (service, mockBlobServiceClient, mockContainerClient, mockBlobClient);
        }

        private Mock<IFormFile> SetupMockFormFile(string contentType)
        {
            var mockFormFile = new Mock<IFormFile>();
            var content = "Mock file content";
            var fileName = "test.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            mockFormFile.Setup(f => f.OpenReadStream()).Returns(ms);
            mockFormFile.Setup(f => f.FileName).Returns(fileName);
            mockFormFile.Setup(f => f.Length).Returns(ms.Length);
            mockFormFile.Setup(f => f.ContentType).Returns(contentType);

            return mockFormFile;
        }

        private Expression<Func<BlobClient, Task<Response<BlobContentInfo>>>> GetUploadExpression(string contentType)
        {
            return b => b.UploadAsync(
                It.IsAny<Stream>(),
                It.Is<BlobHttpHeaders>(h => h.ContentType == contentType),
                It.IsAny<IDictionary<string, string>>(),
                It.IsAny<BlobRequestConditions>(),
                It.IsAny<IProgress<long>>(),
                It.IsAny<AccessTier?>(),
                It.IsAny<StorageTransferOptions>(),
                It.IsAny<CancellationToken>());
        }

        //[Fact]
        //public async Task UploadAsync_WithNullFormFile_ShouldThrowArgumentNullException()
        //{
        //    // Arrange
        //    var (service, _, _, _) = SetupMocks();
        //    IFormFile formFile = null;
        //    var fileName = "test.jpg";
        //    var containerName = "images";

        //    // Act
        //    Func<Task> act = async () => await service.UploadAsync(formFile, fileName, containerName);

        //    // Assert
        //    await act.Should().ThrowAsync<ArgumentNullException>();
        //}

        //[Fact]
        //public async Task UploadAsync_WithNullFileName_ShouldThrowArgumentNullException()
        //{
        //    // Arrange
        //    var (service, _, _, _) = SetupMocks();
        //    var mockFile = new Mock<IFormFile>().Object;
        //    string fileName = null;
        //    var containerName = "images";

        //    // Act
        //    Func<Task> act = async () => await service.UploadAsync(mockFile, fileName, containerName);

        //    // Assert
        //    await act.Should().ThrowAsync<ArgumentNullException>();
        //}

        //[Fact]
        //public async Task UploadAsync_WithEmptyFileName_ShouldThrowArgumentException()
        //{
        //    // Arrange
        //    var (service, _, _, _) = SetupMocks();
        //    var mockFile = new Mock<IFormFile>().Object;
        //    var fileName = "";
        //    var containerName = "images";

        //    // Act
        //    Func<Task> act = async () => await service.UploadAsync(mockFile, fileName, containerName);

        //    // Assert
        //    await act.Should().ThrowAsync<ArgumentException>();
        //}

        //[Fact]
        //public async Task UploadAsync_WithNullContainerName_ShouldThrowArgumentNullException()
        //{
        //    // Arrange
        //    var (service, _, _, _) = SetupMocks();
        //    var mockFile = new Mock<IFormFile>().Object;
        //    var fileName = "test.jpg";
        //    string containerName = null;

        //    // Act
        //    Func<Task> act = async () => await service.UploadAsync(mockFile, fileName, containerName);

        //    // Assert
        //    await act.Should().ThrowAsync<ArgumentNullException>();
        //}

        //[Fact]
        //public async Task UploadAsync_WithEmptyContainerName_ShouldThrowArgumentException()
        //{
        //    // Arrange
        //    var (service, _, _, _) = SetupMocks();
        //    var mockFile = new Mock<IFormFile>().Object;
        //    var fileName = "test.jpg";
        //    var containerName = "";

        //    // Act
        //    Func<Task> act = async () => await service.UploadAsync(mockFile, fileName, containerName);

        //    // Assert
        //    await act.Should().ThrowAsync<ArgumentException>();
        //}
    }
}
