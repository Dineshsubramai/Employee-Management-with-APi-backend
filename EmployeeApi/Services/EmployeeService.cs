using EmployeeApi.Data;
using EmployeeApi.DTOs;
using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Services;

public class EmployeeService(EmployeeDbContext db) : IEmployeeService
{
    public Task<List<Employee>> GetEmployees() => db.Employees.AsNoTracking().ToListAsync();
    public Task<Employee?> GetEmployee(int id) => db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);

    public async Task<Employee> AddEmployee(EmployeeDto dto)
    {
        var employee = new Employee();
        Copy(dto, employee);
        db.Employees.Add(employee);
        await db.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> UpdateEmployee(int id, EmployeeDto dto)
    {
        var employee = await db.Employees.FindAsync(id);
        if (employee is null) return false;
        Copy(dto, employee);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        var employee = await db.Employees.FindAsync(id);
        if (employee is null) return false;
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<AttendanceRecord>> GetAttendance(int? employeeId)
    {
        var query = db.Attendance.AsNoTracking().Include(a => a.Employee).AsQueryable();
        if (employeeId.HasValue) query = query.Where(a => a.EmployeeId == employeeId.Value);
        return await query.OrderByDescending(a => a.WorkDate).ToListAsync();
    }

    public async Task<AttendanceRecord?> AddAttendance(AttendanceDto dto)
    {
        if (!await db.Employees.AnyAsync(e => e.Id == dto.EmployeeId)) return null;
        var record = new AttendanceRecord { EmployeeId = dto.EmployeeId, WorkDate = dto.WorkDate,
            CheckIn = dto.CheckIn, CheckOut = dto.CheckOut, Notes = dto.Notes };
        db.Attendance.Add(record);
        await db.SaveChangesAsync();
        return record;
    }

    public async Task<List<LeaveRequest>> GetLeaveRequests(int? employeeId)
    {
        var query = db.LeaveRequests.AsNoTracking().Include(l => l.Employee).AsQueryable();
        if (employeeId.HasValue) query = query.Where(l => l.EmployeeId == employeeId.Value);
        return await query.OrderByDescending(l => l.StartDate).ToListAsync();
    }

    public async Task<LeaveRequest?> AddLeaveRequest(LeaveRequestDto dto)
    {
        if (!await db.Employees.AnyAsync(e => e.Id == dto.EmployeeId)) return null;
        var request = new LeaveRequest { EmployeeId = dto.EmployeeId, StartDate = dto.StartDate,
            EndDate = dto.EndDate, Reason = dto.Reason };
        db.LeaveRequests.Add(request);
        await db.SaveChangesAsync();
        return request;
    }

    public async Task<bool> UpdateLeaveStatus(int id, string status)
    {
        var request = await db.LeaveRequests.FindAsync(id);
        if (request is null) return false;
        request.Status = status;
        await db.SaveChangesAsync();
        return true;
    }

    private static void Copy(EmployeeDto dto, Employee employee)
    {
        employee.Name = dto.Name;
        employee.Email = dto.Email;
        employee.Department = dto.Department;
        employee.JobTitle = dto.JobTitle;
        employee.DateHired = dto.DateHired;
    }

}
