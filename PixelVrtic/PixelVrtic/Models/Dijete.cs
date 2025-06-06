using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PixelVrtic.Models
{
    public class Dijete
    {
        public Dijete() { }

        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Ime je obavezno.")]
        [StringLength(50, ErrorMessage = "Ime može sadržavati najviše 50 karaktera.")]
        public string ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno.")]
        [StringLength(50, ErrorMessage = "Prezime može sadržavati najviše 50 karaktera.")]
        public string prezime { get; set; }

        [Required(ErrorMessage = "Datum rođenja je obavezan.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Dijete), nameof(ValidirajDatumRodjenja))]
        public DateTime datumRodjenja { get; set; }

        [Required(ErrorMessage = "Mjesto rođenja je obavezno.")]
        [StringLength(100, ErrorMessage = "Mjesto rođenja može sadržavati najviše 100 karaktera.")]
        public string mjestoRodenja { get; set; }

        [Required(ErrorMessage = "JMBG je obavezan.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "JMBG mora sadržavati tačno 13 cifara.")]
        public string JMBG { get; set; }

        [ForeignKey("Grupa")]
        [Required(ErrorMessage = "Grupa je obavezna.")]
        public int grupaId { get; set; }

        [ValidateNever]
        public Grupa grupa { get; set; }

        [StringLength(500, ErrorMessage = "Zdravstvene napomene mogu sadržavati najviše 500 karaktera.")]
        public string zdravstveneNapomene { get; set; }

        [StringLength(255, ErrorMessage = "Putanja fotografije može sadržavati najviše 255 karaktera.")]
        public string fotografija { get; set; }

        [ForeignKey("Korisnik")]
        [Required(ErrorMessage = "Roditelj (korisnik) je obavezan.")]
        public string roditeljId { get; set; }

        [ValidateNever]
        public Korisnik Korisnik { get; set; }

        // Custom validator for birth date
        public static ValidationResult ValidirajDatumRodjenja(DateTime datumRodjenja, ValidationContext context)
        {
            if (datumRodjenja > DateTime.Today)
            {
                return new ValidationResult("Datum rođenja ne može biti u budućnosti.");
            }

            if (datumRodjenja < DateTime.Today.AddYears(-7))
            {
                return new ValidationResult("Dijete ne može biti starije od 7 godina.");
            }

            return ValidationResult.Success;
        }
    }
}
