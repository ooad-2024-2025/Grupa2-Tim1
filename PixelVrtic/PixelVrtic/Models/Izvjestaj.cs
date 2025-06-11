using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PixelVrtic.Models
{
    public class Izvjestaj
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Dijete je obavezno.")]
        [ForeignKey("Dijete")]
        public int DijeteId { get; set; }

        [ValidateNever]
        public Dijete Dijete { get; set; }

        [Required(ErrorMessage = "Period izvještaja je obavezan.")]
        public DateTime Period { get; set; }

        [Required(ErrorMessage = "Broj dana prisustva je obavezan.")]
        [Range(0, int.MaxValue, ErrorMessage = "Broj dana prisustva mora biti nenegativan broj.")]
        public int? BrojDanaPrisustva { get; set; }

        [StringLength(1000, ErrorMessage = "Komentar može sadržavati najviše 1000 karaktera.")]
        [RegularExpression(@"^$|^(?=.*[a-zA-ZčćžšđČĆŽŠĐ])[a-zA-Z0-9čćžšđČĆŽŠĐ\s.,!?;:'""()\-]*$", ErrorMessage = "Komentar može sadržavati samo slova, brojeve i interpunkcijske znakove i mora imati barem jedno slovo ako je unesen.")]
        public string? KomentarVaspitaca { get; set; }

        [Required(ErrorMessage = "Lista aktivnosti je obavezna.")]
        [StringLength(2000, ErrorMessage = "Lista aktivnosti može sadržavati najviše 2000 karaktera.")]
        [RegularExpression(@"^(?=.*[a-zA-ZčćžšđČĆŽŠĐ])[a-zA-Z0-9čćžšđČĆŽŠĐ\s.,!?;:'""()\-]*$", ErrorMessage = "Lista aktivnosti mora sadržavati barem jedno slovo i može sadržavati samo slova, brojeve i interpunkcijske znakove.")]
        public string? ListaAktivnosti { get; set; }
    }
}
