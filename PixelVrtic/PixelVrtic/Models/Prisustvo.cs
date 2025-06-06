using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PixelVrtic.Models
{
    public class Prisustvo
    {
        public Prisustvo() { }

        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Datum prisustva je obavezan.")]
        [DataType(DataType.Date)]
        public DateTime datum { get; set; }

        [ForeignKey("Dijete")]
        [Required(ErrorMessage = "Dijete je obavezno.")]
        public int dijeteId { get; set; }

        public Dijete Dijete { get; set; }

        [Required(ErrorMessage = "Prisustvo je obavezno.")]
        public bool prisutan { get; set; }

        [StringLength(500, ErrorMessage = "Razlog odsutnosti može imati najviše 500 karaktera.")]
        public string? razlogOdsutnosti { get; set; }
    }
}
