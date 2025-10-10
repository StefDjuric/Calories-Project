using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calories.Entities.Models
{
    public class CaloriesDbContext : IdentityDbContext<User>
    {
        public CaloriesDbContext(DbContextOptions<CaloriesDbContext> options) : base(options){ }

        public DbSet<Meal> Meals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Meal>()
                .HasOne(m => m.User)
                .WithMany(u => u.Meals)
                .HasForeignKey(m => m.UserId);
        }
    }
}
