using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class EMasterContext : DbContext
{
    public EMasterContext()
    {
    }

    public EMasterContext(DbContextOptions<EMasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Professional> Professionals { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=ConnectionString:ConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.IdBooking).HasName("PK__Booking__3A710F7F8A2FC5A1");

            entity.Property(e => e.BookingStatus).HasDefaultValue("Pending");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Professional).WithMany(p => p.Bookings).HasConstraintName("FK_Professional_Booking");

            entity.HasOne(d => d.Service).WithMany(p => p.Bookings).HasConstraintName("FK_Service_Booking");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings).HasConstraintName("FK_User_Booking");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("PK__LOG__0C54DBC6A4AD9BC3");
        });

        modelBuilder.Entity<Professional>(entity =>
        {
            entity.HasKey(e => e.IdProfessional).HasName("PK__Professi__0C7802302660FD4E");

            entity.HasOne(d => d.User).WithMany(p => p.Professionals)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasOne(d => d.Professional).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfessionalRating");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRating");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.IdReview).HasName("PK__Review__04F7FE1059B6B3E0");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Professional).WithMany(p => p.Reviews).HasConstraintName("FK_Professional_Review");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews).HasConstraintName("FK_User_Review");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrole).HasName("PK__Role__A1BA16C4397B56A4");

            entity.HasOne(d => d.User).WithMany(p => p.Roles)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_userId");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("PK__Service__0E3EA45BB6D56C45");

            entity.HasOne(d => d.Professional).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Professional");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DF7278BDCD");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
