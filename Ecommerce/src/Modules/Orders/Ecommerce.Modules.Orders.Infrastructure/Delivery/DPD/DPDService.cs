using Ecommerce.Modules.Orders.Application.Orders.Services;
using Ecommerce.Modules.Orders.Domain.Orders.Entities;
using Ecommerce.Modules.Orders.Infrastructure.Delivery.DPD;
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
        private readonly IHttpClientFactory _httpClientFactory;

        //private readonly HttpClient _httpClient;
        private readonly DPDOptions _dpdOptions;
        private readonly OrganizationDetails _organizationDetails;
        private const string _pdfMimeType = "application/pdf";

        public DPDService(/*HttpClient httpClient*/ IHttpClientFactory httpClientFactory, DPDOptions dpdOptions, OrganizationDetails organizationDetails)
        {
            _httpClientFactory = httpClientFactory;
            //_httpClient = httpClient;
            _dpdOptions = dpdOptions;
            _organizationDetails = organizationDetails;
        }
        public async Task<(int Id, string TrackingNumber)> CreateShipmentAsync(Shipment shipment, CancellationToken cancellationToken = default)
        {
            var _httpClient = _httpClientFactory.CreateClient("DPD");
            var serializedShipment = GetDpdJson(shipment);
            using var jsonContent = new StringContent(serializedShipment, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync("public/shipment/v1/generatePackagesNumbers", jsonContent, cancellationToken);
            var json = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
            {
                //using var errorJsonDocument = JsonDocument.Parse(json);
                //var errorMessage = errorJsonDocument.RootElement.GetProperty(_inPostErrorPropertyName).GetString();
                //throw new HttpRequestException(/*$"HTTP request failed: {errorMessage!}"*/);

                //to change
                throw new HttpRequestException("HTTP request failed");
            }
            using var jsonDocument = JsonDocument.Parse(json);
            var id = jsonDocument.RootElement.GetProperty("sessionId").GetInt32();
            var trackingNumber = jsonDocument.RootElement.GetProperty("packages").EnumerateArray().First()
                .GetProperty("parcels").EnumerateArray().First().GetProperty("waybill").GetString()!;
            return (id, trackingNumber);
        }

        public async Task<(Stream FileStream, string MimeType, string FileName)> GetLabelAsync(Shipment shipment, CancellationToken cancellationToken = default)
        {
            var _httpClient = _httpClientFactory.CreateClient("DPD");
            if (shipment.LabelId is null)
            {
                throw new InvalidOperationException("Shipment does not have a label ID.");
            }
            if(!int.TryParse(shipment.LabelId, out var sessionId))
            {
                throw new InvalidOperationException("Invalid label ID format.");
            }
            var getDpdLabelRequest = new GetDPDLabelRequest(sessionId);
            var serializedRequest = JsonSerializer.Serialize(getDpdLabelRequest, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            using var jsonContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync("/public/shipment/v1/generateSpedLabels", jsonContent, cancellationToken);
            if (!httpResponse.IsSuccessStatusCode)
            {
                //using var errorJsonDocument = JsonDocument.Parse(json);
                //var errorMessage = errorJsonDocument.RootElement.GetProperty(_inPostErrorPropertyName).GetString();

                //to change
                throw new HttpRequestException("HTTP request failed");
            }
            using var jsonDocument = JsonDocument.Parse(await httpResponse.Content.ReadAsStringAsync(cancellationToken));
            var base64Label = jsonDocument.RootElement.GetProperty("documentData").GetString() ?? throw new NullReferenceException();
            byte[] bytesLabel = Convert.FromBase64String(base64Label);
            return (new MemoryStream(bytesLabel), _pdfMimeType, $"{shipment.TrackingNumber}-dpd-label.pdf");
        }

        private string GetDpdJson(Shipment shipment)
        {
            var receiver = shipment.Receiver;

            var request = new CreateDPDShipmentRequest
            {
                GenerationPolicy = _dpdOptions.GenerationPolicy,
                Packages =
                [
                    new DPDPackage
                    {
                        Receiver = new DPDReceiver
                        {
                            Name = receiver.FirstName + " " + receiver.LastName,
                            Address = receiver.Address.Street + " " + receiver.Address.BuildingNumber,
                            City = receiver.Address.City,
                            CountryCode = receiver.Address.CountryCode,
                            PostalCode = receiver.Address.PostCode.Replace("-", ""),
                            Email = receiver.Email,
                            Phone = receiver.Phone,
                        },
                        Sender = new DPDSender
                        {
                            Company = _organizationDetails.Company,
                            Name = _organizationDetails.Name,
                            Address = _organizationDetails.Address,
                            City = _organizationDetails.City,
                            CountryCode = _organizationDetails.CountryCode,
                            PostalCode = _organizationDetails.PostalCode.Replace("-", ""),
                            Phone = _organizationDetails.Phone,
                            Email = _organizationDetails.Email
                        },
                        PayerFID = _dpdOptions.OrganizationFID,
                        Parcels = [.. shipment.Parcels!.Select(p => new DPDParcel
                        {
                            SizeZ = decimal.Parse(p.Dimensions.Length),
                            SizeX = decimal.Parse(p.Dimensions.Width),
                            SizeY = decimal.Parse(p.Dimensions.Height),
                            Weight = decimal.Parse(p.Weight.Amount),
                            Content = _dpdOptions.Content
                        })]
                    }
                ],
                LangCode = _dpdOptions.LanguageCode,
                OutputDocFormat = _dpdOptions.OutputDocFormat,
                Format = _dpdOptions.Format,
                OutputType = _dpdOptions.OutputType,
                Variant = _dpdOptions.Variant
            };

            return JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        }
    }
}
