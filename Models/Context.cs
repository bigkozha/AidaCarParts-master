using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AidaCarParts.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        { }
        public DbSet<Part> Parts { get; set; }
        public DbSet<SectionsAndSubsections> SectionsAndSubsections { get; set; }
    }
}
