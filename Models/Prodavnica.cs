using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Prodavnica")]
    public class Prodavnica
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        public string Naziv { get; set; }

        [Required]
        [MaxLength(50)]
        public string Adresa { get; set; }

        public virtual List<Dostavljac> ProdavnicaDostavljaci { get; set; }

        public virtual List<Narudzbina> ProdavnicaNarudzbine { get; set; }

        [JsonIgnore]
        public virtual List<ProdavnicaProizvodSpoj> ProdavnicaProizvod { get; set; }

        public virtual Nabavljac ProdavnicaNabavljac { get; set; }
    }
}