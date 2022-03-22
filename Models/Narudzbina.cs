using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Narudzbina")]
    public class Narudzbina
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Adresa { get; set; }

        [Required]
        [Phone]
        [RegularExpression("^\\s*\\+?\\s*([0-9][\\s-]*){9,}$")]
        public string BrTelefona { get; set; }

        public Prodavnica NarudzbinaProdavnica { get; set; }

        public Dostavljac NarudzbinaDostavljac { get; set; }

        [JsonIgnore]

        public virtual List<ProizvodNarudzbinaSpoj> NarudzbinaProizvod { get; set; }
    }
}