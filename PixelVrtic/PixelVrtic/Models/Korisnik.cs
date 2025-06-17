using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PixelVrtic.Models
{
    public class Korisnik : IdentityUser
    {
        public Korisnik() { }

        [StringLength(50, ErrorMessage = "Ime može sadržavati najviše 50 karaktera.")]
        [RegularExpression(@"^[A-ZĆČŽŠĐa-zćčžšđ\- ]+$", ErrorMessage = "Ime može sadržavati samo slova, razmake i crtice.")]
        public string? ime { get; set; }

        [StringLength(50, ErrorMessage = "Prezime može sadržavati najviše 50 karaktera.")]
        [RegularExpression(@"^[A-ZĆČŽŠĐa-zćčžšđ\- ]+$", ErrorMessage = "Prezime može sadržavati samo slova, razmake i crtice.")]
        public string? prezime { get; set; }

        public Uloga? uloga { get; set; }

        [Phone(ErrorMessage = "Unesite ispravan broj telefona.")]
        public string? brojTelefona { get; set; }

        [StringLength(100, ErrorMessage = "Adresa može sadržavati najviše 100 karaktera.")]
        public string? adresa { get; set; }

        public bool? uplaceno { get; set; }

        public int? idGrupe { get; set; }

        [DataType(DataType.Date)]
        [CustomValidation(typeof(Korisnik), nameof(ValidirajDatumRodjenja))]
        public DateTime? datumRodjenja { get; set; }

        [StringLength(100, ErrorMessage = "Mjesto rođenja može sadržavati najviše 100 karaktera.")]
        [RegularExpression(@"^[A-ZĆČŽŠĐa-zćčžšđ\- ]+$", ErrorMessage = "Mjesto rođenja može sadržavati samo slova, razmake i crtice.")]
        public string? mjestoRodjenja { get; set; }

        [StringLength(255, ErrorMessage = "Putanja fotografije može sadržavati najviše 255 karaktera.")]
        public string? fotografija { get; set; }

        [StringLength(100, ErrorMessage = "Naziv univerziteta može sadržavati najviše 100 karaktera.")]
        [RegularExpression(@"^[A-ZĆČŽŠĐa-zćčžšđ0-9\- ]+$", ErrorMessage = "Naziv univerziteta može sadržavati slova, brojeve, razmake i crtice.")]
        public string? univerzitet { get; set; }

        [StringLength(100, ErrorMessage = "Naziv diplome može sadržavati najviše 100 karaktera.")]
        [RegularExpression(@"^[A-ZĆČŽŠĐa-zćčžšđ0-9\- ]+$", ErrorMessage = "Diploma može sadržavati slova, brojeve, razmake i crtice.")]
        public string? diploma { get; set; }

        [DataType(DataType.Date)]
        public DateTime? pocetakObrazovanja { get; set; }

        [DataType(DataType.Date)]
        public DateTime? krajObrazovanja { get; set; }

        [StringLength(50, ErrorMessage = "Grad može sadržavati najviše 50 karaktera.")]
        [RegularExpression(@"^[A-ZĆČŽŠĐa-zćčžšđ\- ]+$", ErrorMessage = "Grad može sadržavati samo slova, razmake i crtice.")]
        public string? grad { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Naknada mora biti pozitivna vrijednost.")]
        public double? naknada { get; set; }

        public static ValidationResult? ValidirajDatumRodjenja(DateTime? datumRodjenja, ValidationContext context)
        {
            if (datumRodjenja == null)
                return ValidationResult.Success;

            if (datumRodjenja > DateTime.Today)
                return new ValidationResult("Datum rođenja ne može biti u budućnosti.");

            if (datumRodjenja < DateTime.Today.AddYears(-100))
                return new ValidationResult("Datum rođenja je predalek u prošlosti.");

            return ValidationResult.Success;
        }
    }
}