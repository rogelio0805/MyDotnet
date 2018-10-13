using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalifornia.Models
{
    public class Special
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
    }

    public class SpecialDataContext: DbContext
    {
        public DbSet<Special> DbSpecials { get; set; }

        public SpecialDataContext(DbContextOptions<SpecialDataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public IEnumerable<Special> GetMonthlySpecials()
        {
            return DbSpecials.ToArray();            
        }
    }
}
