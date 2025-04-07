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

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Professional> Professionals { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=ConnectionStrings:ConnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.IdBooking).HasName("PK__Booking__3A710F7F9F5FD311");

            entity.ToTable("Booking");

            entity.HasIndex(e => e.UserId, "idxBookingsUser");

            entity.Property(e => e.IdBooking).HasColumnName("idBooking");
            entity.Property(e => e.BookingDate).HasColumnName("bookingDate");
            entity.Property(e => e.BookingStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending")
                .HasColumnName("bookingStatus");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdAt");
            entity.Property(e => e.ProfessionalId).HasColumnName("professionalId");
            entity.Property(e => e.ServiceId).HasColumnName("serviceId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Professional).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ProfessionalId)
                .HasConstraintName("FK__Booking__profess__75A278F5");

            entity.HasOne(d => d.Service).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__Booking__service__76969D2E");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Booking__userId__74AE54BC");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.IdClient).HasName("PK__Client__A6A610D45F915ABF");

            entity.ToTable("Client");

            entity.HasIndex(e => e.MemberId, "UQ__Client__7FD7CF174443A9CB").IsUnique();

            entity.Property(e => e.IdClient).HasColumnName("idClient");
            entity.Property(e => e.MemberId).HasColumnName("memberId");

            entity.HasOne(d => d.Member).WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Client__memberId__4F7CD00D");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Members__7FD7CF166C406A02");

            entity.HasIndex(e => e.Email, "UQ__Members__AB6E61647BC28417").IsUnique();

            entity.HasIndex(e => e.Email, "idxMembersEmail");

            entity.Property(e => e.MemberId).HasColumnName("memberId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.MemberAddress)
                .HasMaxLength(255)
                .HasColumnName("memberAddress");
            entity.Property(e => e.MemberType)
                .HasMaxLength(20)
                .HasColumnName("memberType");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("passwordHash");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Professional>(entity =>
        {
            entity.HasKey(e => e.IdProfessional).HasName("PK__Professi__0C78023047A29D19");

            entity.ToTable("Professional");

            entity.HasIndex(e => e.MemberId, "UQ__Professi__7FD7CF1725CADD85").IsUnique();

            entity.Property(e => e.IdProfessional).HasColumnName("idProfessional");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .HasColumnName("category");
            entity.Property(e => e.ExperienceYears).HasColumnName("experienceYears");
            entity.Property(e => e.MemberId).HasColumnName("memberId");
            entity.Property(e => e.Rating)
                .HasDefaultValue(0.0)
                .HasColumnName("rating");

            entity.HasOne(d => d.Member).WithOne(p => p.Professional)
                .HasForeignKey<Professional>(d => d.MemberId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Professio__membe__5441852A");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.IdReview).HasName("PK__Review__04F7FE10E392F044");

            entity.ToTable("Review");

            entity.HasIndex(e => e.ProfessionalId, "idxReviewsProfessional");

            entity.Property(e => e.IdReview).HasColumnName("idReview");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("createdAt");
            entity.Property(e => e.ProfessionalId).HasColumnName("professionalId");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Professional).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProfessionalId)
                .HasConstraintName("FK__Review__professi__02084FDA");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Review__userId__01142BA1");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("PK__Service__0E3EA45B0E6478B1");

            entity.ToTable("Service");

            entity.HasIndex(e => e.ProfessionalId, "idxServicesProfessional");

            entity.Property(e => e.IdService).HasColumnName("idService");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProfessionalId).HasColumnName("professionalId");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(100)
                .HasColumnName("serviceName");

            entity.HasOne(d => d.Professional).WithMany(p => p.Services)
                .HasForeignKey(d => d.ProfessionalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Service__profess__571DF1D5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
