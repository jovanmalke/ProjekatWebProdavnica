using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class ProizvodNarudzbinaSpoj
    {
        [Key]
        public int ID { get; set; }

        public int KolicinaProizvodaUNarudzbini { get; set; }

        public int Cena { get; set; }

        [JsonIgnore]
        public virtual Narudzbina NarudzbinaSpoj2 { get; set; }

        public virtual Proizvod ProizvodSpoj2 { get; set; }
    }
}
