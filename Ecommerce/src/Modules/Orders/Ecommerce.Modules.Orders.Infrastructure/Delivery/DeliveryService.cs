﻿using Ecommerce.Modules.Orders.Application.Delivery;
using Ecommerce.Modules.Orders.Domain.Shipping.Entities;
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
        private static readonly JsonSerializerOptions SerializerOptions = new()
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
            var client = _factory.CreateClient(_inPost);
            var id = await PostCreateShipmentRequestAsync(shipment, client);
            var trackingNumber = await GetTrackingNumberFromCreatedShipmentAsync(id, client);
            client.Dispose();
            return (id, trackingNumber);
        }
        public async Task<(Stream FileStream, string MimeType, string FileName)> GetLabelAsync(Shipment shipment)
        {
            var client = _factory.CreateClient(_inPost);
            var response = await client.GetAsync($"v1/shipments/{shipment.LabelId}/label");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpClientRequestFailedException();
            }
            var stream = await response.Content.ReadAsStreamAsync();
            return (stream, "application/pdf", $"{shipment.TrackingNumber}-inpost-label");
        }
        private async Task<int> PostCreateShipmentRequestAsync(Shipment shipment, HttpClient client)
        {
            var serializedShipment = JsonSerializer.Serialize(shipment, SerializerOptions);
            using var jsonContent = new StringContent(serializedShipment, Encoding.UTF8, "application/json");
            var httpResponse = await client.PostAsync($"v1/organizations/{_inPostOptions.OrganizationId}/shipments", jsonContent);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new HttpClientRequestFailedException();
            }
            var json = await httpResponse.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(json);
            var id = jsonDocument.RootElement.GetProperty("id").GetInt32();
            return id;
        }
        private async Task<string> GetTrackingNumberFromCreatedShipmentAsync(int id, HttpClient client)
        {
            var httpResponse = await client.GetAsync($"v1/organizations/{_inPostOptions.OrganizationId}/shipments?id={id}");
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new HttpClientRequestFailedException();
            }
            var json = await httpResponse.Content.ReadAsStringAsync();
            using var jsonDocument = JsonDocument.Parse(json);
            var trackingNumber = jsonDocument.RootElement.GetProperty("items")[0].GetProperty("tracking_number").GetString();
            return trackingNumber!;
        }
    }
}
