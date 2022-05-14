using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MultipleBlazorApps.Shared.Entities;


namespace MultipleBlazorApps.Server
{
    public class ApplicationDbContext : DbContext
    {
        // the following is an empty constructor method for this class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        // the following defines the primary key relationships in the joiner classes 
        //(allowing 2 classes to be joined via the joiner class)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<MoviesPersons>().HasKey(x => new {
            //    x.MovieId,
            //    x.PersonId
            //});


            base.OnModelCreating(modelBuilder);
        }
        // the following defines the classes / tables to be created

        public DbSet<People> People { get; set; }


    }

}
