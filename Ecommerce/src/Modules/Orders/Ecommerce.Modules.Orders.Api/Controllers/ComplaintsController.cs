using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.ApproveComplaint;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.BrowseComplaints;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.EditDecision;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.GetComplaint;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.RejectComplaint;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.SetNote;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaints.BrowseComplaintsByCustomerId;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaints.DeleteComplaint;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.GetOrder;
using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using Ecommerce.Shared.Infrastructure.Pagination.OffsetPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    [ApiVersion(1)]
    internal class ComplaintsController : BaseController
    {
        public ComplaintsController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Gets cursor paginated complaints")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns cursor paginated result for complaints.", typeof(ApiResponse<CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>>))]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>>>> BrowseComplaints([FromQuery] BrowseComplaints query, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>>(HttpStatusCode.OK, result));
        }

        [Authorize(Roles = "Customer")]
        [SwaggerOperation("Gets offset paginated complaints per customer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns offset paginated result for complaints for specified customer Id.", typeof(ApiResponse<PagedResult<ComplaintBrowseDto>>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpGet("/api/v{v:apiVersion}/" + OrdersModule.BasePath + "/customer/{customerId:guid}" + "/[controller]")]
        public async Task<ActionResult<ApiResponse<PagedResult<ComplaintBrowseDto>>>> BrowseComplaintsByCustomerId([FromRoute] Guid customerId, [FromQuery] BrowseComplaintsByCustomerId query,
            CancellationToken cancellationToken)
        {
            query.CustomerId = customerId;
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<PagedResult<ComplaintBrowseDto>>(HttpStatusCode.OK, result));
        }

        [SwaggerOperation("Get a specific complaint")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a specific complaint by id.", typeof(ApiResponse<ComplaintDetailsDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Complaint was not found")]
        [AllowAnonymous]
        [HttpGet("{complaintId:guid}")]
        public async Task<ActionResult<ApiResponse<ComplaintDetailsDto>>> GetComplaint([FromRoute] Guid complaintId, CancellationToken cancellationToken)
            => OkOrNotFound<ComplaintDetailsDto, Complaint>(await _mediator.Send(new GetComplaint(complaintId), cancellationToken));

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Approves a complaint")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{complaintId:guid}/approve")]
        public async Task<ActionResult> ApproveComplaint([FromRoute] Guid complaintId, [FromForm] ApproveComplaint command, 
            CancellationToken cancellationToken)
        {
            command = command with { ComplaintId = complaintId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Rejects a complaint")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPut("{complaintId:guid}/reject")]
        public async Task<ActionResult> RejectComplaint([FromRoute] Guid complaintId, [FromForm] DecisionRejectDto decision, 
            CancellationToken cancellationToken)
        {
            await _mediator.Send(new RejectComplaint(complaintId, decision), cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Updates a complaint's note")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [HttpPatch("{complaintId:guid}/note")]
        public async Task<ActionResult> SetNote([FromRoute] Guid complaintId, [FromForm] string note, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SetNote(complaintId, note), cancellationToken);
            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager, Employee")]
        [SwaggerOperation("Deletes a specified complaint")]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        [HttpDelete("{complaintId:guid}")]
        public async Task<ActionResult> DeleteComplaint([FromRoute]Guid complaintId, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteComplaint(complaintId), cancellationToken);
            return NoContent();
        }
        //Method is not ready yet. 
        //[SwaggerOperation("Edits a complaint's decision")]
        //[SwaggerResponse(StatusCodes.Status204NoContent)]
        //[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        //[HttpPut("{complaintId:guid}/decision")]
        //public async Task<ActionResult> EditDecision([FromRoute]Guid complaintId, [FromForm] EditDecision command, 
        //    CancellationToken cancellationToken = default)
        //{
        //    command = command with { ComplaintId=complaintId };
        //    await _mediator.Send(command, cancellationToken);
        //    return NoContent();
        //}
    }
}
