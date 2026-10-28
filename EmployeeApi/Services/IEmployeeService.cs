using EmployeeApi.DTOs;
using EmployeeApi.Models;

namespace EmployeeApi.Services;

public interface IEmployeeService
{
    Task<List<Employee>> GetEmployees();
    Task<Employee?> GetEmployee(int id);
    Task<Employee> AddEmployee(EmployeeDto dto);
    Task<bool> UpdateEmployee(int id, EmployeeDto dto);
    Task<bool> DeleteEmployee(int id);
    Task<List<AttendanceRecord>> GetAttendance(int? employeeId);
    Task<AttendanceRecord?> AddAttendance(AttendanceDto dto);
    Task<List<LeaveRequest>> GetLeaveRequests(int? employeeId);
    Task<LeaveRequest?> AddLeaveRequest(LeaveRequestDto dto);
    Task<bool> UpdateLeaveStatus(int id, string status);
}
