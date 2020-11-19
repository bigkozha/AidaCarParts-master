using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AidaCarParts.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Part>(part =>
            {
                part
                .HasKey(p => p.Id);

                part
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

                part
                .Property(p => p.Numerate)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            });
        }

        public DbSet<Part> Parts { get; set; }
        public DbSet<SectionsAndSubsections> SectionsAndSubsections { get; set; }


    }
}
