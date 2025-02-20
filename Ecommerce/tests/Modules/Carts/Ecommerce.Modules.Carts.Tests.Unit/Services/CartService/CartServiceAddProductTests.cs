using Castle.Core.Logging;
using Ecommerce.Modules.Carts.Core.DAL;
using Ecommerce.Modules.Carts.Core.Entities;
using Ecommerce.Modules.Carts.Core.Events;
using Ecommerce.Modules.Carts.Core.Exceptions;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Abstractions.Contexts;
using Ecommerce.Shared.Abstractions.Messaging;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Carts.Tests.Unit.Services.CartService
{
    public class CartServiceAddProductTests : IAsyncDisposable
    {
        private readonly Mock<IContextService> _contextServiceMock = new Mock<IContextService>();
        private readonly Mock<IMessageBroker> _messageBrokerMock = new Mock<IMessageBroker>();
        private readonly ILogger<Core.Services.CartService> _logger = new NullLogger<Core.Services.CartService>();
        private readonly CartsDbContext _dbContext;
        private readonly FakeTimeProvider _fakeTimeProvider;
        public CartServiceAddProductTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CartsDbContext>();
            optionsBuilder.UseInMemoryDatabase("database")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            _fakeTimeProvider = new FakeTimeProvider();
            _fakeTimeProvider.SetUtcNow(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
            _dbContext = new CartsDbContext(optionsBuilder.Options, _fakeTimeProvider);
        }
        [Fact]
        public async Task AddProduct_WithCorrectProduct_ShouldSucceed()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            var cartEntry = await _dbContext.Carts.AddAsync(new Cart());
            var productEntry = await _dbContext.Products.AddAsync(new Product("sku", "product", 11, 10, "image"));
            await _dbContext.SaveChangesAsync();
            var cartService = new Core.Services.CartService(_dbContext, _contextServiceMock.Object, _messageBrokerMock.Object, _logger);
            await cartService.AddProductAsync(cartEntry.Entity.Id, productEntry.Entity.Id, 5);
            _messageBrokerMock.Verify(m => m.PublishAsync(It.IsAny<ProductReserved>()), Times.Once);
        }
        [Fact]
        public async Task AddProduct_WhenCartDoesNotExist_ShouldFail()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            var cartService = new Core.Services.CartService(_dbContext, _contextServiceMock.Object, _messageBrokerMock.Object, _logger);
            var exception = await Record.ExceptionAsync(() => cartService.AddProductAsync(Guid.NewGuid(), Guid.NewGuid(), 5));
            exception.Should().BeOfType<CartNotFoundException>();
            _messageBrokerMock.Verify(m => m.PublishAsync(It.IsAny<ProductReserved>()), Times.Never);
        }
        [Fact]
        public async Task AddProduct_WhenProductDoesNotExist_ShouldThrowException()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            var cartEntry = await _dbContext.Carts.AddAsync(new Cart());
            await _dbContext.SaveChangesAsync();
            var cartService = new Core.Services.CartService(_dbContext, _contextServiceMock.Object, _messageBrokerMock.Object, _logger);
            var exception = await Record.ExceptionAsync(() => cartService.AddProductAsync(cartEntry.Entity.Id, Guid.NewGuid(), 5));
            exception.Should().BeOfType<ProductNotFoundException>();
            _messageBrokerMock.Verify(m => m.PublishAsync(It.IsAny<ProductReserved>()), Times.Never);
        }
        public async ValueTask DisposeAsync() => await _dbContext.Database.EnsureDeletedAsync();
    }
}
