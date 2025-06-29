using eProfessional.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace eProfessional.DAL.Context;

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

    public virtual DbSet<CityProfessional> CityProfessionals { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Professional> Professionals { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceType> ServiceTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("name=ConnectionStrings:ConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Idcity).HasName("PK__City__36D3508368875118");

            entity.ToTable("City");

            entity.Property(e => e.Idcity).HasColumnName("IDCity");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("City");
        });

        modelBuilder.Entity<CityProfessional>(entity =>
        {
            entity.HasKey(e => e.IdProfessionalCity).HasName("PK__CityProf__C104735C0F24E820");

            entity.ToTable("CityProfessional");

            entity.Property(e => e.IdProfessionalCity).HasColumnName("idProfessionalCity");
            entity.Property(e => e.CityId).HasColumnName("cityId");
            entity.Property(e => e.ProfessionalId).HasColumnName("professionalId");

            entity.HasOne(d => d.City).WithMany(p => p.CityProfessionals)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CityProfessional");

            entity.HasOne(d => d.Professional).WithMany(p => p.CityProfessionals)
                .HasForeignKey(d => d.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProfessionalCity");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("PK__LOG__0C54DBC6EC3D41B2");

            entity.ToTable("LOG");

            entity.Property(e => e.LogLevel).HasMaxLength(50);
            entity.Property(e => e.LogMessage).HasMaxLength(200);
        });

        modelBuilder.Entity<Professional>(entity =>
        {
            entity.HasKey(e => e.IdProfessional).HasName("PK__Professi__0C780230A5334413");

            entity.ToTable("Professional");

            entity.Property(e => e.IdProfessional).HasColumnName("idProfessional");
            entity.Property(e => e.ExperienceYears).HasColumnName("experienceYears");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Professionals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Idrole).HasName("PK__Role__A1BA16C4ED9628AE");

            entity.ToTable("Role");

            entity.Property(e => e.Idrole).HasColumnName("IDRole");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Roles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_userId");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("PK__Service__0E3EA45B0C0A1DB7");

            entity.ToTable("Service");

            entity.Property(e => e.IdService).HasColumnName("idService");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProfessionalId).HasColumnName("professionalId");
            entity.Property(e => e.ServiceTypeId).HasColumnName("serviceTypeID");

            entity.HasOne(d => d.Professional).WithMany(p => p.Services)
                .HasForeignKey(d => d.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Professional");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.Services)
                .HasForeignKey(d => d.ServiceTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ServiceType");
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.IdserviceType).HasName("PK__ServiceT__5E5E6A098D7154A1");

            entity.ToTable("ServiceType");

            entity.Property(e => e.IdserviceType).HasColumnName("IDServiceType");
            entity.Property(e => e.ServiceTypeName)
                .HasMaxLength(100)
                .HasColumnName("ServiceType");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DFAD3FDDE4");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E6164F1759908").IsUnique();

            entity.Property(e => e.Iduser).HasColumnName("IDUser");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("passwordHash");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(255)
                .HasColumnName("passwordSalt");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
