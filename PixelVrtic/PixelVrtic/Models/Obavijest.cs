using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PixelVrtic.Models
{
    public class Obavijest
    {
        public Obavijest() { }

        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Naslov obavijesti je obavezan.")]
        [StringLength(100, ErrorMessage = "Naslov može sadržavati najviše 100 karaktera.")]
        [RegularExpression(@"^(?=.*[a-zA-ZčćžšđČĆŽŠĐ])[a-zA-Z0-9čćžšđČĆŽŠĐ\s.,!?-]*$", ErrorMessage = "Naslov mora sadržavati barem jedno slovo i može sadržavati samo slova, brojeve i osnovne interpunkcijske znakove.")]
        public string naslov { get; set; }

        [Required(ErrorMessage = "Tekst obavijesti je obavezan.")]
        [StringLength(2000, ErrorMessage = "Tekst može sadržavati najviše 2000 karaktera.")]
        [RegularExpression(@"^(?=.*[a-zA-ZčćžšđČĆŽŠĐ])[a-zA-Z0-9čćžšđČĆŽŠĐ\s.,!?;:'""()\-]*$", ErrorMessage = "Tekst mora sadržavati barem jedno slovo i može sadržavati samo slova, brojeve i osnovne interpunkcijske znakove.")]
        public string tekst { get; set; }

        [Required(ErrorMessage = "Datum objave je obavezan.")]
        [DataType(DataType.DateTime)]
        public DateTime datum { get; set; }

        [ForeignKey("Korisnik")]
        [Required(ErrorMessage = "Autor obavijesti je obavezan.")]
        public string idAutora { get; set; }

        [ValidateNever]
        public Korisnik Korisnik { get; set; }
    }
}
