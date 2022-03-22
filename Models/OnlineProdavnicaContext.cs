using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class OnlineProdavnicaContext : DbContext
    {
        public DbSet<Prodavnica> Prodavnice { get; set; }

        public DbSet<Proizvod> Proizvodi { get; set; }

        public DbSet<Narudzbina> Narudzbine { get; set; }

        public DbSet<Dostavljac> Dostavljaci { get; set; }

        public DbSet<Nabavljac> Nabavljaci { get; set; }

        public DbSet<ProdavnicaProizvodSpoj> ProdavnicaProizvod { get; set; }

        public DbSet<ProizvodNarudzbinaSpoj> ProizvodNarudzbina { get; set; }

        public OnlineProdavnicaContext(DbContextOptions options) : base(options)
        {

        }
    }
}