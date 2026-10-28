using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.Models;

public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }

    [Required, StringLength(500)]
    public string Reason { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Status { get; set; } = "Pending";
}
