using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Dostavljac")]
    public class Dostavljac
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
        [Range(18, 60)]
        public int Godine { get; set; }

        [Required]
        [Phone]
        [RegularExpression("^\\s*\\+?\\s*([0-9][\\s-]*){9,}$")]
        public string BrTelefona { get; set; }

        [Required]
        [Range(100, 1000)]
        public int CenaUsluge { get; set; }

        public Prodavnica DostavljacProdavnica { get; set; }

        public List<Narudzbina> DostavljacNarudzbina { get; set; }
    }
}