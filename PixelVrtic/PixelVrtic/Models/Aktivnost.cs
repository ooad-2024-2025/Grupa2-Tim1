using System;
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

        [Required(ErrorMessage = "Naziv aktivnosti je obavezan.")]
        [StringLength(100, ErrorMessage = "Naziv aktivnosti može sadržavati najviše 100 karaktera.")]
        [RegularExpression(@"^[A-ZĆČŽŠĐa-zćčžšđ0-9 \-]+$", ErrorMessage = "Naziv aktivnosti može sadržavati samo slova, brojeve, razmake i crtice.")]
        public string nazivAktivnosti { get; set; }

        [Required(ErrorMessage = "Tip aktivnosti je obavezan.")]
        public TipAktivnosti tipAktivnosti { get; set; }

        [Required(ErrorMessage = "Datum početka je obavezan.")]
        [DataType(DataType.DateTime)]
        public DateTime datumPocetka { get; set; }

        [Required(ErrorMessage = "Datum završetka je obavezan.")]
        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(Aktivnost), nameof(ValidirajDatume))]
        public DateTime datumZavrsetka { get; set; }


        [StringLength(1000, ErrorMessage = "Opis može sadržavati najviše 1000 karaktera.")]
        [RegularExpression(@"^[A-ZĆČŽŠĐa-zćčžšđ0-9\s.,!?\-–()]*$", ErrorMessage = "Opis sadrži nedozvoljene karaktere.")]
        public string opis { get; set; }

        [ForeignKey("Grupa")]
        [Required(ErrorMessage = "Grupa aktivnosti je obavezna.")]
        public int idGrupe { get; set; }

        [ValidateNever]
        public Grupa Grupa { get; set; }

        [ForeignKey("Korisnik")]
        [Required(ErrorMessage = "Organizator aktivnosti je obavezan.")]
        public string idKorisnika { get; set; }

        [ValidateNever]
        public Korisnik Korisnik { get; set; }

        [StringLength(255, ErrorMessage = "Putanja fotografije može sadržavati najviše 255 karaktera.")]
        public string? fotografija { get; set; }

        public static ValidationResult ValidirajDatumPocetka(DateTime datumPocetka, ValidationContext context)
        {
            if (datumPocetka < DateTime.Today.AddYears(-1))
                return new ValidationResult("Datum početka ne može biti više od godinu dana u prošlosti.");

            return ValidationResult.Success;
        }


        public static ValidationResult ValidirajDatume(DateTime datumZavrsetka, ValidationContext context)
        {
            var instance = context.ObjectInstance as Aktivnost;
            if (instance == null)
                return ValidationResult.Success;

            if (datumZavrsetka < instance.datumPocetka)
                return new ValidationResult("Datum završetka ne može biti prije datuma početka.");

            if ((datumZavrsetka - instance.datumPocetka).TotalDays > 30)
                return new ValidationResult("Trajanje aktivnosti ne može biti duže od 30 dana.");

            return ValidationResult.Success;
        }
    }
}
