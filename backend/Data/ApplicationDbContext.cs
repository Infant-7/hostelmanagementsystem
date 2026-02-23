using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HostelManagement.API.Models;

namespace HostelManagement.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Warden> Wardens { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<GatePassRequest> GatePassRequests { get; set; }
    public DbSet<RoomChangeRequest> RoomChangeRequests { get; set; }
    public DbSet<Grievance> Grievances { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Student
        builder.Entity<Student>(entity =>
        {
            entity.HasIndex(e => e.RollNumber).IsUnique();
            entity.HasIndex(e => e.UserId).IsUnique();
        });

        // Configure Warden
        builder.Entity<Warden>(entity =>
        {
            entity.HasIndex(e => e.UserId).IsUnique();
        });

        // Configure Room
        builder.Entity<Room>(entity =>
        {
            entity.HasIndex(e => e.RoomNumber).IsUnique();
        });

        // Configure RoomChangeRequest relationships
        builder.Entity<RoomChangeRequest>(entity =>
        {
            // Configure relationship with CurrentRoom
            entity.HasOne(rcr => rcr.CurrentRoom)
                  .WithMany()
                  .HasForeignKey(rcr => rcr.CurrentRoomId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            // Configure relationship with RequestedRoom
            entity.HasOne(rcr => rcr.RequestedRoom)
                  .WithMany()
                  .HasForeignKey(rcr => rcr.RequestedRoomId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Attendance
        builder.Entity<Attendance>(entity =>
        {
            entity.HasIndex(e => new { e.StudentId, e.Date }).IsUnique();
        });
    }
}



