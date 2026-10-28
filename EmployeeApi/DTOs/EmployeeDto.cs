using System.ComponentModel.DataAnnotations;

namespace EmployeeApi.DTOs;

public class EmployeeDto
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string Department { get; set; } = string.Empty;
    [Required, StringLength(100)]
    public string JobTitle { get; set; } = string.Empty;
    public DateTime DateHired { get; set; } = DateTime.UtcNow.Date;
}
