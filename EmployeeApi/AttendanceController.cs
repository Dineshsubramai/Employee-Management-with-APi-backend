using EmployeeApi.DTOs;
using EmployeeApi.Models;
using EmployeeApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/attendance")]
public class AttendanceController(IEmployeeService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AttendanceRecord>>> GetAll([FromQuery] int? employeeId) =>
        Ok(await service.GetAttendance(employeeId));

    [HttpPost]
    public async Task<ActionResult<AttendanceRecord>> Create(AttendanceDto dto)
    {
        var record = await service.AddAttendance(dto);
        if (record is null) return BadRequest("Employee was not found.");
        return Created($"/api/attendance?employeeId={record.EmployeeId}", record);
    }
}
