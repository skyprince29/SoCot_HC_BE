using Microsoft.EntityFrameworkCore;
using SoCot_HC_BE.Model;

namespace SoCot_HC_BE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<VitalSign> VitalSigns { get; set; }
        public DbSet<Province> Provice {  get; set; }
        public DbSet<CityMunicipality> CityMunicipality { get; set; }
    }
}
