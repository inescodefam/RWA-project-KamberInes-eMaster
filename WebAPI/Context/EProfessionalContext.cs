using Microsoft.EntityFrameworkCore;
using Shared.BL.Models;

namespace WebAPI.Models;

public partial class EProfessionalContext : DbContext
{
    public EProfessionalContext()
    {
    }

    public EProfessionalContext(DbContextOptions<EProfessionalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Professional> Professionals { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceType> ServiceTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=ConnectionString:ConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Idcity).HasName("PK__City__36D350838117212F");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("PK__LOG__0C54DBC64FBA6AF8");
        });

        modelBuilder.Entity<Professional>(entity =>
        {
            entity.HasKey(e => e.IdProfessional).HasName("PK__Professi__0C7802305FAAA454");

            //entity.HasOne(d => d.City).WithMany(p => p.Professionals)
            //    .OnDelete(DeleteBehavior.Cascade)
            //    .HasConstraintName("FK_cityId");

            entity.HasOne(d => d.User).WithMany(p => p.Professionals)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrole).HasName("PK__Role__A1BA16C431AE4A96");

            entity.HasOne(d => d.User).WithMany(p => p.Roles)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_userId");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("PK__Service__0E3EA45BC34C5E9F");

            entity.HasOne(d => d.Professional).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Professional");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.Services)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ServiceType");
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.IdserviceType).HasName("PK__ServiceT__5E5E6A09F5269C3C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DFF46D070B");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
