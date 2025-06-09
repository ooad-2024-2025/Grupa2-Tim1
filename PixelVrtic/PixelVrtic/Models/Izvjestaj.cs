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

        [Required]
        [ForeignKey("Dijete")]
        public int DijeteId { get; set; }

        [ValidateNever]
        public Dijete Dijete { get; set; }

        [Required]
        public DateTime Period { get; set; }

        [Required]
        public int? BrojDanaPrisustva { get; set; }

        [StringLength(1000)]
        public string? KomentarVaspitaca { get; set; }

        [Required]
        [StringLength(2000)]
        public string? ListaAktivnosti { get; set; } // Npr. "Igranje u parku, Čitanje priče, Likovno izražavanje"
    }
}
