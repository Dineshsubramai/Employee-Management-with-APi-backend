using EmployeeApi.DTOs;
using EmployeeApi.Models;
using EmployeeApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/leave-requests")]
public class LeaveRequestsController(IEmployeeService service) : ControllerBase
{
    private static readonly string[] AllowedStatuses = ["Pending", "Approved", "Rejected"];

    [HttpGet]
    public async Task<ActionResult<List<LeaveRequest>>> GetAll([FromQuery] int? employeeId) =>
        Ok(await service.GetLeaveRequests(employeeId));

    [HttpPost]
    public async Task<ActionResult<LeaveRequest>> Create(LeaveRequestDto dto)
    {
        if (dto.EndDate < dto.StartDate) return BadRequest("End date must be on or after start date.");
        var request = await service.AddLeaveRequest(dto);
        if (request is null) return BadRequest("Employee was not found.");
        return Created($"/api/leave-requests?employeeId={request.EmployeeId}", request);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
    {
        if (!AllowedStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            return BadRequest("Status must be Pending, Approved, or Rejected.");
        var updated = await service.UpdateLeaveStatus(id, AllowedStatuses.First(s =>
            s.Equals(status, StringComparison.OrdinalIgnoreCase)));
        return updated ? NoContent() : NotFound();
    }
}
