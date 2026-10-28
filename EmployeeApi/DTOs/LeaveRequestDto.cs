using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.DTOs;

public class LeaveRequestDto
{
    [Range(1, int.MaxValue)]
    public int EmployeeId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    [Required, StringLength(500)]
    public string Reason { get; set; } = string.Empty;
}
