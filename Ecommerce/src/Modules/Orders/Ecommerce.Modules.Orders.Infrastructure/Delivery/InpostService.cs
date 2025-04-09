using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Infrastructure.InPost;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery
{
    internal class InpostService : IDeliveryService
    {
        private readonly HttpClient _httpClient;
        private readonly InPostOptions _inPostOptions;
        private readonly ILogger<InpostService> _logger;
        private const string _inPostErrorPropertyName = "error";
        private const string _inPostItemsPropertyName = "items";
        private const string _inPostTrackingNumberPropertyName = "tracking_number";
        private const string _pdfMimeType = "application/pdf";
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        public InpostService(HttpClient httpClient, InPostOptions inPostOptions, ILogger<InpostService> logger)
        {
            _httpClient = httpClient;
            _inPostOptions = inPostOptions;
            _logger = logger;
        }
        public async Task<(int Id, string TrackingNumber)> CreateShipmentAsync(Shipment shipment, CancellationToken cancellationToken = default)
        {
            var id = await PostCreateShipmentRequestAsync(shipment, cancellationToken);
            await Task.Delay(6000);
            var trackingNumber = await GetTrackingNumberFromCreatedShipmentAsync(id, cancellationToken);
            _logger.LogDebug("Shipment was created on InPost.");
            return (id, trackingNumber);
        }
        public async Task<(Stream FileStream, string MimeType, string FileName)> GetLabelAsync(Shipment shipment, CancellationToken cancellationToken = default)
        {
            var httpResponse = await _httpClient.GetAsync($"v1/shipments/{shipment.LabelId}/label", cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                using var errorJsonDocument = JsonDocument.Parse(json);
                var errorMessage = errorJsonDocument.RootElement.GetProperty(_inPostErrorPropertyName).GetString();
                throw new HttpRequestException($"HTTP request failed: {errorMessage!}");
            }
            var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);
            _logger.LogDebug("Label was downloaded for shipment: {shipmentId}.", shipment.Id);
            return (stream, _pdfMimeType, $"{shipment.TrackingNumber}-inpost-label");
        }
        private async Task<int> PostCreateShipmentRequestAsync(Shipment shipment, CancellationToken cancellationToken)
        {
            var serializedShipment = JsonSerializer.Serialize(shipment, _serializerOptions);
            using var jsonContent = new StringContent(serializedShipment, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync($"v1/organizations/{_inPostOptions.OrganizationId}/shipments", jsonContent);
            var json = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                using var errorJsonDocument = JsonDocument.Parse(json);
                var errorMessage = errorJsonDocument.RootElement.GetProperty(_inPostErrorPropertyName).GetString();
                throw new HttpRequestException($"HTTP request failed: {errorMessage!}");
            }
            using var jsonDocument = JsonDocument.Parse(json);
            var id = jsonDocument.RootElement.GetProperty("id").GetInt32();
            return id;
        }
        private async Task<string> GetTrackingNumberFromCreatedShipmentAsync(int id, CancellationToken cancellationToken = default)
        {
            var httpResponse = await _httpClient.GetAsync($"v1/organizations/{_inPostOptions.OrganizationId}/shipments?id={id}");
            var json = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                using var errorJsonDocument = JsonDocument.Parse(json);
                var errorMessage = errorJsonDocument.RootElement.GetProperty(_inPostErrorPropertyName).GetString();
                throw new HttpRequestException($"HTTP request failed: {errorMessage!}");
            }
            using var jsonDocument = JsonDocument.Parse(json);
            var trackingNumber = jsonDocument.RootElement.GetProperty(_inPostItemsPropertyName)[0].GetProperty(_inPostTrackingNumberPropertyName).GetString();
            return trackingNumber!;
        }
    }
}
