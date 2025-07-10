using Asp.Versioning;
using Ecommerce.Modules.Carts.Core.DTO;
using Ecommerce.Modules.Carts.Core.Services;
using Ecommerce.Shared.Abstractions.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Carts.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class DeliveryServiceController : BaseController
    {
        private readonly IDeliveryServiceService _deliveryServiceService;

        public DeliveryServiceController(IDeliveryServiceService deliveryServiceService)
        {
            _deliveryServiceService = deliveryServiceService;
        }

        [AllowAnonymous]
        [SwaggerOperation("Gets available delivery services")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns available delivery services.", typeof(ApiResponse<IEnumerable<DeliveryServiceDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpGet("available")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DeliveryServiceDto>>>> GetAllAvailableDeliveryServices(CancellationToken cancellationToken)
            => Ok(new ApiResponse<IEnumerable<DeliveryServiceDto>>(System.Net.HttpStatusCode.OK, await _deliveryServiceService.GetAllAsync(true, cancellationToken)));

        [SwaggerOperation("Gets delivery services")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns delivery services.", typeof(ApiResponse<IEnumerable<DeliveryServiceDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<DeliveryServiceDto>>>> GetAllDeliveryServices(CancellationToken cancellationToken)
            => Ok(new ApiResponse<IEnumerable<DeliveryServiceDto>>(System.Net.HttpStatusCode.OK, await _deliveryServiceService.GetAllAsync(null, cancellationToken)));

        [SwaggerOperation("Set delivery service status")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status403Forbidden, "Access is forbidden for this user")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "User not authorized")]
        [HttpPatch("{deliveryServiceId:int}/active/{isActive:bool}")]
        public async Task<ActionResult> SetActiveAsync(int deliveryServiceId, bool isActive, CancellationToken cancellationToken)
        {
            await _deliveryServiceService.SetActiveAsync(deliveryServiceId, isActive, cancellationToken);
            return NoContent();
        }
    }
}
