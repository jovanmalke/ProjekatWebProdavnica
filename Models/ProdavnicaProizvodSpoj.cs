using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class ProdavnicaProizvodSpoj
    {
        [Key]
        public int ID { get; set; }

        [Range(0, 100)]
        public int Kolicina { get; set; }

        [Range(1, 10000)]
        public int Cena { get; set; }

        [JsonIgnore]
        public virtual Prodavnica ProdavnicaSpoj1 { get; set; }

        public virtual Proizvod ProizvodSpoj1 { get; set; }
    }
}
