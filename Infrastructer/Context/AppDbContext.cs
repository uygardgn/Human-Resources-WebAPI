using Domain.Entities.Concrete;
using Infrastructer.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructer.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("[Connection String Here]");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration<AppRole>(new AppRoleCFG());
            builder.ApplyConfiguration<Department>(new DepartmentCFG());
            builder.ApplyConfiguration<Company>(new CompanyCFG());
            builder.ApplyConfiguration<Leave>(new LeaveCFG());

            base.OnModelCreating(builder);
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveAppUser> LeaveAppUsers { get; set; }
    }
}
