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
using Ecommerce.Shared.Infrastructure.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Modules.Orders.Api.Controllers
{
    internal class ComplaintController : BaseController
    {
        public ComplaintController(IMediator mediator) : base(mediator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>>>> BrowseOrders([FromQuery] BrowseComplaints query)
        {
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<CursorPagedResult<ComplaintBrowseDto, ComplaintCursorDto>>(HttpStatusCode.OK, result));
        }
        [HttpGet("{complaintId:guid}")]
        public async Task<ActionResult<ApiResponse<ComplaintDetailsDto>>> GetOrder([FromRoute] Guid complaintId)
            => OkOrNotFound<ComplaintDetailsDto, Complaint>(await _mediator.Send(new GetComplaint(complaintId)));
        [HttpPost("{complaintId:guid}/approve")]
        public async Task<ActionResult> ApproveComplaint([FromRoute] Guid complaintId, [FromForm] ApproveComplaint command)
        {
            command = command with { ComplaintId = complaintId };
            await _mediator.Send(command);
            return NoContent();
        }
        [HttpPost("{complaintId:guid}/reject")]
        public async Task<ActionResult> RejectComplaint([FromRoute] Guid complaintId, [FromForm] DecisionDto decision)
        {
            await _mediator.Send(new RejectComplaint(decision, complaintId));
            return NoContent();
        }
        [HttpPut("{complaintId:guid}/note")]
        public async Task<ActionResult> SetNote([FromRoute] Guid complaintId, [FromForm] string note)
        {
            await _mediator.Send(new SetNote(note, complaintId));
            return NoContent();
        }
        [HttpPut("{complaintId:guid}/decision")]
        public async Task<ActionResult> EditDecision([FromRoute]Guid complaintId, [FromForm] EditDecision command)
        {
            command = command with { ComplaintId=complaintId };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
