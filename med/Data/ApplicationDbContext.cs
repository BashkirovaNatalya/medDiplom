using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using med.Models;
using med.ViewModels;

namespace med.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Equipment>? Equipment { get; set; }
        public DbSet<EquipmentType>? EquipmentType { get; set; }
        public DbSet<Organization>? Organization { get; set; }
        public DbSet<Filial>? Filial { get; set; }
        public DbSet<Cabinet>? Cabinet { get; set; }
        public DbSet<Client>? Client { get; set; }
        public DbSet<ClientPosition>? ClientPosition { get; set; }
        public DbSet<EmployeePosition>? EmployeePosition { get; set; }
        public DbSet<Employee>? Employee { get; set; }
        public DbSet<Stage>? Stage { get; set; }
        public DbSet<JobType>? JobType { get; set; }
        public DbSet<Job>? Job { get; set; }
        public DbSet<ApplicationStatus>? ApplicationStatus { get; set; }
        public DbSet<JobImage>? JobImage { get; set; }
        public DbSet<License>? License { get; set; }





        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var foreignKey in builder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }







        public DbSet<med.Models.Application>? Application { get; set; }







    }
}