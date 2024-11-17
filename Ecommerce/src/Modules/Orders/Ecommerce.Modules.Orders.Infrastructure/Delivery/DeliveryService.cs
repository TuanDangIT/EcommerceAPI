using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Infrastructure.Exceptions;
using Ecommerce.Shared.Infrastructure.InPost;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery
{
    internal class DeliveryService : IDeliveryService
    {
        private readonly IHttpClientFactory _factory;
        private readonly InPostOptions _inPostOptions;
        private const string _inPost = "inpost";
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
        public DeliveryService(IHttpClientFactory factory, InPostOptions inPostOptions)
        {
            _factory = factory;
            _inPostOptions = inPostOptions;
        }
        public async Task<(int Id, string TrackingNumber)> CreateShipmentAsync(Shipment shipment)
        {
            using var client = _factory.CreateClient(_inPost);
            var id = await PostCreateShipmentRequestAsync(shipment, client);
            await Task.Delay(6000);
            var trackingNumber = await GetTrackingNumberFromCreatedShipmentAsync(id, client);
            return (id, trackingNumber);
        }
        public async Task<(Stream FileStream, string MimeType, string FileName)> GetLabelAsync(Shipment shipment)
        {
            var client = _factory.CreateClient(_inPost);
            var httpResponse = await client.GetAsync($"v1/shipments/{shipment.LabelId}/label");
            if (!httpResponse.IsSuccessStatusCode)
            {
                var json = await httpResponse.Content.ReadAsStringAsync();
                using var errorJsonDocument = JsonDocument.Parse(json);
                var errorMessage = errorJsonDocument.RootElement.GetProperty(_inPostErrorPropertyName).GetString();
                throw new HttpClientRequestFailedException(errorMessage!);
            }
            var stream = await httpResponse.Content.ReadAsStreamAsync();
            return (stream, _pdfMimeType, $"{shipment.TrackingNumber}-inpost-label");
        }
        private async Task<int> PostCreateShipmentRequestAsync(Shipment shipment, HttpClient client)
        {
            var serializedShipment = JsonSerializer.Serialize(shipment, _serializerOptions);
            using var jsonContent = new StringContent(serializedShipment, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync($"v1/organizations/{_inPostOptions.OrganizationId}/shipments", jsonContent);
            var json = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                using var errorJsonDocument = JsonDocument.Parse(json);
                var errorMessage = errorJsonDocument.RootElement.GetProperty(_inPostErrorPropertyName).GetString();
                throw new HttpClientRequestFailedException(errorMessage!);
            }
            using var jsonDocument = JsonDocument.Parse(json);
            var id = jsonDocument.RootElement.GetProperty("id").GetInt32();
            return id;
        }
        private async Task<string> GetTrackingNumberFromCreatedShipmentAsync(int id, HttpClient client)
        {
            var httpResponse = await client.GetAsync($"v1/organizations/{_inPostOptions.OrganizationId}/shipments?id={id}");
            var json = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                using var errorJsonDocument = JsonDocument.Parse(json);
                var errorMessage = errorJsonDocument.RootElement.GetProperty(_inPostErrorPropertyName).GetString();
                throw new HttpClientRequestFailedException(errorMessage!);
            }
            using var jsonDocument = JsonDocument.Parse(json);
            var trackingNumber = jsonDocument.RootElement.GetProperty(_inPostItemsPropertyName)[0].GetProperty(_inPostTrackingNumberPropertyName).GetString();
            return trackingNumber!;
        }
    }
}
