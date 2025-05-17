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
        public DateTime datum { get; set; }
        [ForeignKey("Dijete")]
        public int dijeteId { get; set; }
        public Dijete Dijete { get; set; }
        public bool prisutan { get; set; }
        public string? razlogOdsutnosti { get; set; }
    }
}