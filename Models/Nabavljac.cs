using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    [Table("Nabavljac")]
    public class Nabavljac
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(20)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(20)]
        public string Prezime { get; set; }

        [Required]
        [Phone]
        [RegularExpression("^\\s*\\+?\\s*([0-9][\\s-]*){9,}$")]
        public string BrTelefona { get; set; }

        [Required]
        [MaxLength(50)]
        public string Sifra { get; set; }

        [JsonIgnore]
        public virtual List<Prodavnica> NabavljacProdavnica { get; set; }
    }
}