using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Zza.Entities;

namespace Zza.Data
{
    public class ZzaDbContext : DbContext
    {
     
        public DbSet<Client> Clients { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Table names match entity names by default (don't pluralize)
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Globally disable the convention for cascading deletes
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Task>()
            //            .Property(c => c.taskID) // Client must set the ID.
            //            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //modelBuilder.Entity<Client>()
            //          .Property(c => c.ID) // Client must set the ID.
            //          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //modelBuilder.Entity<Customer>().Ignore(c => c.FullName);
        }
    }
}
