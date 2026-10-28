using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeApi.Models;

public class Employee
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string JobTitle { get; set; } = string.Empty;

    public DateTime DateHired { get; set; } = DateTime.UtcNow.Date;
    public bool IsActive { get; set; } = true;
    [JsonIgnore]
    public List<AttendanceRecord> AttendanceRecords { get; set; } = [];
    [JsonIgnore]
    public List<LeaveRequest> LeaveRequests { get; set; } = [];
}
