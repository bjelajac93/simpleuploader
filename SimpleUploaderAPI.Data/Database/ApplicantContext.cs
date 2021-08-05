using Microsoft.EntityFrameworkCore;
using SimpleUploaderAPI.Domain.Entities;

namespace ApplicantsApi.Data.Database
{
    public class ApplicantContext : DbContext
    {
        public ApplicantContext(DbContextOptions<ApplicantContext> options) : base(options)
        {
        }

        public DbSet<FileData> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileData>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired();
                entity.Property(e => e.FileType).IsRequired();
                entity.Property(e => e.FileSize).IsRequired();
                entity.Property(e => e.UploadDate).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}