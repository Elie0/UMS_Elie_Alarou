using UniDb.Persistence.Models;

namespace UniDb.Persistence;

public class UniversityDbContext : DbContext
{
    public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<User> Users { get; set; }
    public DbSet<TeacherPerCourse> TeacherPerCourse { get; set; }
    public DbSet<ClassEnrollment>  ClassEnrollment  { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
