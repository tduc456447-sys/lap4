using lap4.Models;
using Microsoft.EntityFrameworkCore;

namespace lap4.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext (DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }
        public virtual DbSet<lap4.Models.Course> Course { get; set; }
        public virtual DbSet<lap4.Models.Enrollment> Enrollment { get; set; }
        public virtual DbSet<lap4.Models.Learner> Learner { get; set; }
        public virtual DbSet<lap4.Models.Major> Major { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Major>().ToTable(nameof(Major));
            modelBuilder.Entity<Learner>().ToTable(nameof(Learner));
            modelBuilder.Entity<Enrollment>().ToTable(nameof(Enrollment));
            modelBuilder.Entity<Course>().ToTable(nameof(Course));
        }

    }
}
