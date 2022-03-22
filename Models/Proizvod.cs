using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Proizvod")]
    public class Proizvod
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        public string Ime { get; set; }

        public virtual List<ProdavnicaProizvodSpoj> ProizvodProdavnica { get; set; }

        public virtual List<ProizvodNarudzbinaSpoj> ProizvodNarudzbina { get; set; }
    }
}