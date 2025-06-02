using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PixelVrtic.Models
{
    public class Grupa
    {
        [Key]
        public int id { get; set; }
        public string naziv { get; set; }
        [ForeignKey("Korisnik")]
        public string idKorisnika { get; set; }
        [ValidateNever]
        public Korisnik Korisnik { get; set; }
        public Grupa() { }
    }
}
