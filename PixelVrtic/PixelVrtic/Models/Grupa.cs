using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelVrtic.Models
{
    public class Grupa
    {
        [Key]
        public int id { get; set; }
        public string naziv { get; set; }
        [ForeignKey("Korisnik")]
        public int idKorisnika { get; set; }
        public Korisnik Korisnik { get; set; }
        public Grupa() { }
    }
}
