using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Shared.Infrastructure.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Infrastructure.Delivery
{
    internal class DPDService : IDeliveryService
    {
        private readonly HttpClient _httpClient;
        private readonly DPDOptions _dpdOptions;
        private readonly OrganizationDetails _organizationDetails;

        public DPDService(HttpClient httpClient, DPDOptions dpdOptions, OrganizationDetails organizationDetails)
        {
            _httpClient = httpClient;
            _dpdOptions = dpdOptions;
            _organizationDetails = organizationDetails;
        }
        public async Task<(int Id, string TrackingNumber)> CreateShipmentAsync(Shipment shipment, CancellationToken cancellationToken = default)
        {
            var serializedShipment = GetDpdJson(shipment);
            using var jsonContent = new StringContent(serializedShipment, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync("public/shipment/v1/generatePackagesNumbers", jsonContent);
            var json = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                //using var errorJsonDocument = JsonDocument.Parse(json);
                //var errorMessage = errorJsonDocument.RootElement.GetProperty(_inPostErrorPropertyName).GetString();
                throw new HttpRequestException(/*$"HTTP request failed: {errorMessage!}"*/);
            }
            using var jsonDocument = JsonDocument.Parse(json);
            var id = jsonDocument.RootElement.GetProperty("sessionId").GetInt32();
            var trackingNumber = jsonDocument.RootElement.GetProperty("packages").EnumerateArray().First().GetProperty("waybill").GetString()!;
            return (id, trackingNumber);
        }

        public Task<(Stream FileStream, string MimeType, string FileName)> GetLabelAsync(Shipment shipment, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private string GetDpdJson(Shipment shipment)
        {
            var receiver = shipment.Receiver;   
            var jsonObject = new
            {
                _dpdOptions.GenerationPolicy,
                Packages = new[]
                {
                    new
                    {
                        Receiver = new
                        {
                            Name = receiver.FirstName + " " + receiver.LastName,
                            Address = receiver.Address.Street + " " + receiver.Address.BuildingNumber,
                            City = receiver.Address.City,
                            CountryCode = receiver.Address.CountryCode,
                            PostalCode = receiver.Address.PostCode,
                            Email = receiver.Email,
                            Phone = receiver.Phone,
                        },
                        Sender = new
                        {
                            _organizationDetails.Company,
                            _organizationDetails.Name,
                            _organizationDetails.Address,
                            _organizationDetails.City,
                            _organizationDetails.CountryCode,
                            _organizationDetails.PostalCode,
                            _organizationDetails.Phone,
                            _organizationDetails.Email
                        },
                        PayerFid = _dpdOptions.OrganizationFID,
                        Parcels = shipment.Parcels!.Select(p => new
                        {
                            SizeZ = p.Dimensions.Length,
                            SizeX = p.Dimensions.Width,
                            SizeY = p.Dimensions.Height,
                            Weight = p.Weight,
                            Content = _dpdOptions.Content
                        }).ToArray(),
                    }
                },
                LangCode = _dpdOptions.LanguageCode,
                _dpdOptions.OutputDocFormat,
                _dpdOptions.Format,
                _dpdOptions.OutputType,
                _dpdOptions.Variant,
            };

            return JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        }
    }
}
