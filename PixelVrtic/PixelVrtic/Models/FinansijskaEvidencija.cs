using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PixelVrtic.Models
{
    public class FinansijskaEvidencija
    {
        [Key]
        public int id { get; set; }

        [Required]
        [ForeignKey("Roditelj")]
        public string roditeljId { get; set; }

        [ValidateNever]
        public Korisnik Roditelj { get; set; }

        [Required]
        public int mjesec { get; set; }

        [Required]
        public int godina { get; set; }

        [Required]
        public double iznos { get; set; }

        [Required]
        public bool uplaceno { get; set; }
    }
}
