using Microsoft.EntityFrameworkCore;
using IhtiyacMolasi.Models;

namespace IhtiyacMolasi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ── DbSet'ler ──────────────────────────────────────────
        public DbSet<Category>            Categories            { get; set; }
        public DbSet<Neighborhood>        Neighborhoods         { get; set; }
        public DbSet<User>                Users                 { get; set; }
        public DbSet<HelpRequest>         HelpRequests          { get; set; }
        public DbSet<VolunteerAssignment> VolunteerAssignments  { get; set; }
        public DbSet<Comment>             Comments              { get; set; }
        public DbSet<Report>              Reports               { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Category ──────────────────────────────────────
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Categories");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.Icon).HasMaxLength(50).HasDefaultValue("bi-question-circle");
                e.Property(x => x.ColorHex).HasMaxLength(10).HasDefaultValue("#6C757D");
                e.Property(x => x.IsActive).HasDefaultValue(true);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            // ── Neighborhood ──────────────────────────────────
            modelBuilder.Entity<Neighborhood>(e =>
            {
                e.ToTable("Neighborhoods");
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(150);
                e.Property(x => x.City).IsRequired().HasMaxLength(100);
                e.Property(x => x.District).IsRequired().HasMaxLength(100);
                e.Property(x => x.IsActive).HasDefaultValue(true);
                e.Ignore(x => x.FullName);  // NotMapped
            });

            // ── User ──────────────────────────────────────────
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(x => x.Id);
                e.Property(x => x.Alias).IsRequired().HasMaxLength(80);
                e.Property(x => x.SessionToken).IsRequired().HasMaxLength(200);
                e.HasIndex(x => x.SessionToken).IsUnique();
                e.Property(x => x.IsVolunteer).HasDefaultValue(false);
                e.Property(x => x.VolunteerScore).HasDefaultValue(0);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
                e.Property(x => x.LastSeen).HasDefaultValueSql("GETDATE()");

                e.HasOne(x => x.Neighborhood)
                 .WithMany(n => n.Users)
                 .HasForeignKey(x => x.NeighborhoodId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── HelpRequest ───────────────────────────────────
            modelBuilder.Entity<HelpRequest>(e =>
            {
                e.ToTable("HelpRequests");
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).IsRequired().HasMaxLength(200);
                e.Property(x => x.Description).IsRequired().HasMaxLength(1000);
                e.Property(x => x.RequesterAlias).HasMaxLength(80).HasDefaultValue("Anonim");
                e.Property(x => x.Status).HasConversion<byte>();
                e.Property(x => x.UrgencyLevel).HasConversion<byte>();
                e.Property(x => x.ViewCount).HasDefaultValue(0);
                e.Property(x => x.IsAnonymous).HasDefaultValue(true);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");
                e.Property(x => x.UpdatedAt).HasDefaultValueSql("GETDATE()");

                // Hesaplanan alanlar ignore
                e.Ignore(x => x.IsExpired);
                e.Ignore(x => x.TimeLeft);
                e.Ignore(x => x.TimeLeftDisplay);
                e.Ignore(x => x.UrgencyBadgeClass);
                e.Ignore(x => x.StatusBadgeClass);

                e.HasOne(x => x.Category)
                 .WithMany(c => c.HelpRequests)
                 .HasForeignKey(x => x.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Neighborhood)
                 .WithMany(n => n.HelpRequests)
                 .HasForeignKey(x => x.NeighborhoodId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.CreatedBy)
                 .WithMany(u => u.CreatedRequests)
                 .HasForeignKey(x => x.CreatedByUserId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── VolunteerAssignment ───────────────────────────
            modelBuilder.Entity<VolunteerAssignment>(e =>
            {
                e.ToTable("VolunteerAssignments");
                e.HasKey(x => x.Id);
                e.Property(x => x.VolunteerAlias).IsRequired().HasMaxLength(80);
                e.Property(x => x.Status).HasConversion<byte>();
                e.Property(x => x.Note).HasMaxLength(500);
                e.Property(x => x.AssignedAt).HasDefaultValueSql("GETDATE()");

                e.HasOne(x => x.HelpRequest)
                 .WithMany(hr => hr.Assignments)
                 .HasForeignKey(x => x.HelpRequestId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Volunteer)
                 .WithMany(u => u.Assignments)
                 .HasForeignKey(x => x.VolunteerUserId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── Comment ───────────────────────────────────────
            modelBuilder.Entity<Comment>(e =>
            {
                e.ToTable("Comments");
                e.HasKey(x => x.Id);
                e.Property(x => x.AuthorAlias).HasMaxLength(80).HasDefaultValue("Anonim");
                e.Property(x => x.Content).IsRequired().HasMaxLength(500);
                e.Property(x => x.IsDeleted).HasDefaultValue(false);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");

                e.HasOne(x => x.HelpRequest)
                 .WithMany(hr => hr.Comments)
                 .HasForeignKey(x => x.HelpRequestId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Report ────────────────────────────────────────
            modelBuilder.Entity<Report>(e =>
            {
                e.ToTable("Reports");
                e.HasKey(x => x.Id);
                e.Property(x => x.ReporterAlias).HasMaxLength(80);
                e.Property(x => x.Reason).IsRequired().HasMaxLength(300);
                e.Property(x => x.IsReviewed).HasDefaultValue(false);
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETDATE()");

                e.HasOne(x => x.HelpRequest)
                 .WithMany(hr => hr.Reports)
                 .HasForeignKey(x => x.HelpRequestId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
