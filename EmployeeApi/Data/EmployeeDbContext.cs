using EmployeeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Data;

public class EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<AttendanceRecord> Attendance => Set<AttendanceRecord>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
        modelBuilder.Entity<Employee>().HasMany(e => e.AttendanceRecords)
            .WithOne(a => a.Employee).HasForeignKey(a => a.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Employee>().HasMany(e => e.LeaveRequests)
            .WithOne(l => l.Employee).HasForeignKey(l => l.EmployeeId).OnDelete(DeleteBehavior.Cascade);
    }
}

