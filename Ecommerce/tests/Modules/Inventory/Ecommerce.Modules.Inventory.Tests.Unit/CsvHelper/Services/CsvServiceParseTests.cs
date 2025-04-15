using Ecommerce.Modules.Inventory.Application.Shared.Abstractions;
using Ecommerce.Modules.Inventory.Infrastructure.CsvHelper;
using Ecommerce.Modules.Inventory.Infrastructure.CsvHelper.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Modules.Inventory.Tests.Unit.CsvHelper.Services
{
    public class CsvServiceParseTests
    {
        private readonly CsvService _csvService;
        private readonly Mock<IFormFile> _mockFile;

        public CsvServiceParseTests()
        {
            _csvService = new CsvService();
            _mockFile = new Mock<IFormFile>();
        }

        [Theory]
        [InlineData(',')]
        [InlineData(';')]
        public void ParseCsvFile_WithValidFile_ReturnsExpectedRecords(char delimiter)
        {
            // Arrange
            var mockFile = CreateMockCsvFile(GetValidCsvContent(delimiter), "products.csv");

            // Act
            var result = _csvService.ParseCsvFile(mockFile.Object, delimiter);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            var products = result.ToList();

            // First product validation
            products[0].SKU.Should().Be("1000001123");
            products[0].EAN.Should().Be("1234567890123");
            products[0].Name.Should().Be("Product 1");
            products[0].Price.Should().Be(10.99m);
            products[0].VAT.Should().Be(23);
            products[0].Quantity.Should().Be(100);
            products[0].Location.Should().Be("Warehouse A");
            products[0].Description.Should().Be("Description 1");
            products[0].AdditionalDescription.Should().Be("Additional info 1");
            products[0].Manufacturer.Should().Be("Manufacturer 1");
            products[0].Category.Should().Be("Category 1");

            products[0].Parameters.Should().ContainKey("Color");
            products[0].Parameters["Color"].Should().Be("Red");
            products[0].Parameters.Should().ContainKey("Size");
            products[0].Parameters["Size"].Should().Be("M");

            products[0].Images.Should().HaveCount(2);
            products[0].Images.Should().Contain("image1_1.jpg");
            products[0].Images.Should().Contain("image1_2.jpg");

            //second product validation
            products[1].SKU.Should().Be("3000001123");
            products[1].EAN.Should().BeNull();
            products[1].Name.Should().Be("Product 2");
            products[1].Price.Should().Be(20.99m);
            products[1].VAT.Should().Be(8);
            products[1].Quantity.Should().BeNull();
            products[1].Location.Should().Be("Warehouse B");
            products[1].Description.Should().Be("Description 2");
            products[1].AdditionalDescription.Should().BeNull();
            products[1].Manufacturer.Should().Be("Manufacturer 2");
            products[1].Category.Should().Be("Category 2");

            products[1].Parameters.Should().ContainKey("Material");
            products[1].Parameters["Material"].Should().Be("Cotton");

            products[1].Images.Should().HaveCount(1);
            products[1].Images.Should().Contain("image2_1.jpg");
        }

        [Fact]
        public void ParseCsvFile_WithEmptyRequiredField_ThrowsCsvHelperBadDataException()
        {
            // Arrange
            var delimiter = ',';
            var csvContent = "SKU,EAN,Name,Price,VAT,Quantity,Location,Description,AdditionalDescription,Manufacturer,Category,Images\n" +
                             ",1234567890123,Product 1,10.99,23,100,Warehouse A,Description 1,Additional info 1,Manufacturer 1,Category 1,image1_1.jpg";
            var mockFile = CreateMockCsvFile(csvContent, "products.csv");

            // Act
            Action action = () => _csvService.ParseCsvFile(mockFile.Object, delimiter);

            //Assert
            action.Should().Throw<CsvHelperBadDataException>();
        }

        [Fact]
        public void ParseCsvFile_WithInvalidPrice_ThrowsCsvHelperBadDataException()
        {
            // Arrange
            var delimiter = ',';
            var csvContent = "SKU,EAN,Name,Price,VAT,Quantity,Location,Description,AdditionalDescription,Manufacturer,Category,Images\n" +
                             "SKU001,1234567890123,Product 1,invalid,23,100,Warehouse A,Description 1,Additional info 1,Manufacturer 1,Category 1,image1_1.jpg";
            var mockFile = CreateMockCsvFile(csvContent, "products.csv");

            // Act
            Action action = () => _csvService.ParseCsvFile(mockFile.Object, delimiter);

            //Assert
            action.Should().Throw<CsvHelperBadDataException>();
        }

        [Fact]
        public void ParseCsvFile_WithCustomParameters_ParsesParametersCorrectly()
        {
            // Arrange
            var delimiter = ',';
            var csvContent = "SKU,EAN,Name,Price,VAT,Quantity,Location,Description,AdditionalDescription,Manufacturer,Category,Images,Color,Size,Weight\n" +
                             "SKU0000001,1234567890123,Product 1,\"10,99\",23,100,Warehouse A,Description 1,Additional info 1,Manufacturer 1,Category 1,image1_1.jpg,Red,M,500g";
            var mockFile = CreateMockCsvFile(csvContent, "products.csv");

            // Act
            var result = _csvService.ParseCsvFile(mockFile.Object, delimiter);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            var product = result.First();
            product.Parameters.Should().ContainKey("Color");
            product.Parameters["Color"].Should().Be("Red");
            product.Parameters.Should().ContainKey("Size");
            product.Parameters["Size"].Should().Be("M");
            product.Parameters.Should().ContainKey("Weight");
            product.Parameters["Weight"].Should().Be("500g");
        }

        [Fact]
        public void ParseCsvFile_WithEmptyOptionalField_ParsesSuccessfully()
        {
            // Arrange
            var delimiter = ',';
            var csvContent = "SKU,EAN,Name,Price,VAT,Quantity,Location,Description,AdditionalDescription,Manufacturer,Category,Images,Size\n" +
                             "PRODUCT0001,,Product One,\"10,99\",23,,,This is a detailed description,,,,image1_1.jpg,43";
            var mockFile = CreateMockCsvFile(csvContent, "products.csv");

            // Act
            var result = _csvService.ParseCsvFile(mockFile.Object, delimiter);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);

            var product = result.First();
            product.EAN.Should().BeNull();
            product.Quantity.Should().BeNull();
            product.AdditionalDescription.Should().BeNull();
        }

        private Mock<IFormFile> CreateMockCsvFile(string csvContent, string fileName)
        {
            var bytes = Encoding.UTF8.GetBytes(csvContent);
            var stream = new MemoryStream(bytes);

            _mockFile.Setup(f => f.OpenReadStream()).Returns(stream);
            _mockFile.Setup(f => f.FileName).Returns(fileName);
            _mockFile.Setup(f => f.Length).Returns(bytes.Length);

            return _mockFile;
        }

        private string GetValidCsvContent(char delimiter)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"SKU{delimiter}EAN{delimiter}Name{delimiter}Price{delimiter}VAT{delimiter}Quantity{delimiter}Location{delimiter}Description{delimiter}AdditionalDescription{delimiter}Manufacturer{delimiter}Category{delimiter}Images{delimiter}Color{delimiter}Size{delimiter}Material");

            stringBuilder.AppendLine($"1000001123{delimiter}1234567890123{delimiter}Product 1{delimiter}\"10,99\"{delimiter}23{delimiter}100{delimiter}Warehouse A{delimiter}Description 1{delimiter}Additional info 1{delimiter}Manufacturer 1{delimiter}Category 1{delimiter}\"image1_1.jpg,image1_2.jpg\"{delimiter}Red{delimiter}M{delimiter}");

            stringBuilder.AppendLine($"3000001123{delimiter}{delimiter}Product 2{delimiter}\"20,99\"{delimiter}8{delimiter}{delimiter}Warehouse B{delimiter}Description 2{delimiter}{delimiter}Manufacturer 2{delimiter}Category 2{delimiter}image2_1.jpg{delimiter}{delimiter}{delimiter}Cotton");

            return stringBuilder.ToString();
        }
    }
}
