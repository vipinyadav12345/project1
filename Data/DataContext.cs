using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
      //  internal object characters;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
     protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
          modelBuilder.Entity<Skill>().HasData(
            new Skill{Id = 1 , Name ="Fireball", Damage = 30},
            new Skill{Id = 2 , Name ="Frenzy", Damage = 20},
             new Skill{Id = 3 , Name ="Blizzerd", Damage = 50}
          );
       }
 
  
    public DbSet<character> character { get; set; }
    public DbSet<User> users { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<Skill> Skills{ get; set; }

    }

}