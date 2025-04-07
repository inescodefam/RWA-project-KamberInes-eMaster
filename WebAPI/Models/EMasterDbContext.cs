using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class EMasterDbContext : DbContext
{
    public EMasterDbContext()
    {
    }

    public EMasterDbContext(DbContextOptions<EMasterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Professional> Professionals { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=ConnectionString:ConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.IdBooking).HasName("PK__Booking__3A710F7F2D4DC1BC");

            entity.Property(e => e.BookingStatus).HasDefaultValue("Pending");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Professional).WithMany(p => p.Bookings).HasConstraintName("FK_Professional_Booking");

            entity.HasOne(d => d.Service).WithMany(p => p.Bookings).HasConstraintName("FK_Service_Booking");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings).HasConstraintName("FK_User_Booking");
        });

        modelBuilder.Entity<Professional>(entity =>
        {
            entity.HasKey(e => e.IdProfessional).HasName("PK__Professi__0C78023040E10AC1");

            entity.Property(e => e.Rating).HasDefaultValue(0.0);

            entity.HasOne(d => d.User).WithMany(p => p.Professionals)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.IdReview).HasName("PK__Review__04F7FE103B43A98A");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Professional).WithMany(p => p.Reviews).HasConstraintName("FK_Professional_Review");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews).HasConstraintName("FK_User_Review");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("PK__Service__0E3EA45B4B84437C");

            entity.HasOne(d => d.Professional).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Professional");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DF1B05A0E5");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Role).HasDefaultValue("User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
