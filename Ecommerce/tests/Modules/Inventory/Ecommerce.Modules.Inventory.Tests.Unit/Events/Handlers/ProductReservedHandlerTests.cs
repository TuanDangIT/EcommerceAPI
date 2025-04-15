using Ecommerce.Modules.Inventory.Application.Auctions.Exceptions;
using Ecommerce.Modules.Inventory.Application.Inventory.Events.Externals;
using Ecommerce.Modules.Inventory.Application.Inventory.Events.Externals.Handlers;
using Ecommerce.Modules.Inventory.Application.Inventory.Exceptions;
using Ecommerce.Modules.Inventory.Domain.Auctions.Entities;
using Ecommerce.Modules.Inventory.Domain.Auctions.Repositories;
using Ecommerce.Modules.Inventory.Domain.Inventory.Entities;
using Ecommerce.Modules.Inventory.Domain.Inventory.Repositories;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Inventory.Tests.Unit.Events.Handlers
{
    public class ProductReservedHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IAuctionRepository> _auctionRepositoryMock;
        private readonly ProductReservedHandler _handler;

        public ProductReservedHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _auctionRepositoryMock = new Mock<IAuctionRepository>();
            _handler = new ProductReservedHandler(_productRepositoryMock.Object, _auctionRepositoryMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WithValidData_ShouldReserveProductAndDecreaseAuctionQuantity()
        {
            // Arrange
            var quantity = It.Is<int>(x => x > 0);
            var productEvent = new ProductReserved(It.IsAny<Guid>(), quantity);

            var mockProduct = new Mock<Product>();
            mockProduct.Setup(p => p.HasQuantity).Returns(true);
            mockProduct.Setup(p => p.Reserve(quantity));

            var mockAuction = new Mock<Auction>();
            mockAuction.Setup(a => a.HasQuantity).Returns(true);
            mockAuction.Setup(a => a.DecreaseQuantity(quantity));

            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockProduct.Object);
            _auctionRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockAuction.Object);
            _productRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<CancellationToken>()));

            // Act
            await _handler.HandleAsync(productEvent);

            // Assert
            mockProduct.Verify(p => p.Reserve(quantity), Times.Once);
            mockAuction.Verify(a => a.DecreaseQuantity(quantity), Times.Once);
            _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_WithNonExistingProduct_ShouldThrowProductNotFoundException()
        {
            // Arrange
            var productEvent = new ProductReserved(It.IsAny<Guid>(), It.IsAny<int>());

            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null!);

            // Act
            Func<Task> act = async () => await _handler.HandleAsync(productEvent);

            // Assert
            await act.Should().ThrowAsync<ProductNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithNonExistingAuction_ShouldThrowAuctionNotFoundException()
        {
            // Arrange
            var productEvent = new ProductReserved(It.IsAny<Guid>(), It.IsAny<int>());

            var mockProduct = new Mock<Product>();
            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockProduct.Object);
            _auctionRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Auction)null!);

            // Act
            Func<Task> act = async () => await _handler.HandleAsync(productEvent);

            // Assert
            await act.Should().ThrowAsync<AuctionNotFoundException>();
        }

        [Fact]
        public async Task HandleAsync_WithProductWithoutQuantity_ShouldNotExecuteReserveOrDecrease()
        {
            // Arrange
            var quantity = It.Is<int>(x => x > 0);
            var productEvent = new ProductReserved(It.IsAny<Guid>(), quantity);

            var mockProduct = new Mock<Product>();
            mockProduct.Setup(p => p.HasQuantity).Returns(false);

            var mockAuction = new Mock<Auction>();
            mockAuction.Setup(a => a.HasQuantity).Returns(true);

            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockProduct.Object);
            _auctionRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockAuction.Object);

            // Act
            await _handler.HandleAsync(productEvent);

            // Assert
            mockProduct.Verify(p => p.Reserve(It.IsAny<int>()), Times.Never);
            mockAuction.Verify(a => a.DecreaseQuantity(It.IsAny<int>()), Times.Never);
            _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_WithAuctionWithoutQuantity_ShouldNotExecuteReserveOrDecrease()
        {
            // Arrange
            var quantity = It.Is<int>(x => x > 0);
            var productEvent = new ProductReserved(It.IsAny<Guid>(), quantity);

            var mockProduct = new Mock<Product>();
            mockProduct.Setup(p => p.HasQuantity).Returns(true);

            var mockAuction = new Mock<Auction>();
            mockAuction.Setup(a => a.HasQuantity).Returns(false);

            _productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockProduct.Object);
            _auctionRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockAuction.Object);

            // Act
            await _handler.HandleAsync(productEvent);

            // Assert
            mockProduct.Verify(p => p.Reserve(It.IsAny<int>()), Times.Never);
            mockAuction.Verify(a => a.DecreaseQuantity(It.IsAny<int>()), Times.Never);
            _productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}


