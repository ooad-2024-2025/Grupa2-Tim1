using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PixelVrtic.Models
{
    public class Grupa
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Naziv grupe je obavezan.")]
        [StringLength(50, ErrorMessage = "Naziv grupe može sadržavati najviše 50 karaktera.")]
        public string naziv { get; set; }

        [ForeignKey("Korisnik")]
        [Required(ErrorMessage = "Vaspitač (korisnik) grupe je obavezan.")]
        public string idKorisnika { get; set; }

        [ValidateNever]
        public Korisnik Korisnik { get; set; }

        public Grupa() { }
    }
}
