using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PixelVrtic.Models
{
    public class Aktivnost
    {
        public Aktivnost() { }
        [Key]
        public int id { get; set; }
        public string nazivAktivnosti { get; set; }
        public TipAktivnosti tipAktivnosti { get; set; }
        public DateTime datumPocetka { get; set; }
        public DateTime datumZavrsetka { get; set; }
        public string opis { get; set; }
        [ForeignKey("Grupa")]
        public int idGrupe { get; set; }
        [ValidateNever]
        public Grupa Grupa { get; set; }
        [ForeignKey("Korisnik")]
        public string idKorisnika { get; set; }

        [ValidateNever]

        public Korisnik Korisnik { get; set; }
        public string? fotografija { get; set; }


    }
}
