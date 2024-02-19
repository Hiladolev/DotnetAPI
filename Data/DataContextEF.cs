using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class DataContextEF : DbContext
    {
      private readonly IConfiguration _config;
      public DataContextEF(IConfiguration config)  
      {
        _config = config;
      }
      public virtual DbSet<User> Users {get;set;}
      public virtual DbSet<UserSalary> UserSalary {get;set;}
      public virtual DbSet<UserJobInfo> UserJobInfo {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("TutorialAppSchema");
        }
    }
}