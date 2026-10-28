using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.Models;

public class AttendanceRecord
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public DateOnly WorkDate { get; set; }
    public TimeOnly? CheckIn { get; set; }
    public TimeOnly? CheckOut { get; set; }

    [StringLength(300)]
    public string? Notes { get; set; }
}
