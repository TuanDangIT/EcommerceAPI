using Asp.Versioning;
using Ecommerce.Modules.Orders.Application.Complaints.DTO;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.ApproveComplaint;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.BrowseComplaints;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.EditDecision;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.GetComplaint;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.RejectComplaint;
using Ecommerce.Modules.Orders.Application.Complaints.Features.Complaint.SetNote;
using Ecommerce.Modules.Orders.Application.Orders.DTO;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.BrowseOrders;
using Ecommerce.Modules.Orders.Application.Orders.Features.Order.GetOrder;
using Ecommerce.Modules.Orders.Domain.Complaints.Entities;
using Ecommerce.Shared.Abstractions.Api;
using Ecommerce.Shared.Infrastructure.Pagination.CursorPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    [Authorize(Roles = "Admin, Manager, Employee")]
    [ApiVersion(1)]
    internal class ComplaintsController : BaseController
    {
        public ComplaintsController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>>>> BrowseComplaints([FromQuery] BrowseComplaints query, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(new ApiResponse<CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>>(HttpStatusCode.OK, result));
        }
        [AllowAnonymous]
        [HttpGet("{complaintId:guid}")]
        public async Task<ActionResult<ApiResponse<ComplaintDetailsDto>>> GetComplaint([FromRoute] Guid complaintId, CancellationToken cancellationToken = default)
            => OkOrNotFound<ComplaintDetailsDto, Complaint>(await _mediator.Send(new GetComplaint(complaintId), cancellationToken));
        [HttpPost("{complaintId:guid}/approve")]
        public async Task<ActionResult> ApproveComplaint([FromRoute] Guid complaintId, [FromForm] ApproveComplaint command, 
            CancellationToken cancellationToken = default)
        {
            command = command with { ComplaintId = complaintId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
        [HttpPost("{complaintId:guid}/reject")]
        public async Task<ActionResult> RejectComplaint([FromRoute] Guid complaintId, [FromForm] DecisionRejectDto decision, 
            CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new RejectComplaint(complaintId, decision), cancellationToken);
            return NoContent();
        }
        [HttpPut("{complaintId:guid}/note")]
        public async Task<ActionResult> SetNote([FromRoute] Guid complaintId, [FromForm] string note, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new SetNote(complaintId, note), cancellationToken);
            return NoContent();
        }
        [HttpPut("{complaintId:guid}/decision")]
        public async Task<ActionResult> EditDecision([FromRoute]Guid complaintId, [FromForm] EditDecision command, 
            CancellationToken cancellationToken = default)
        {
            command = command with { ComplaintId=complaintId };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
