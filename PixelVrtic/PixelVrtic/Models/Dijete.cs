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
		public string ime { get; set; }
		public string prezime { get; set; }
		public DateTime datumRodjenja { get; set; }
		public string mjestoRodenja { get; set; }
		public string JMBG { get; set; }
        [ForeignKey("Grupa")]
        public int grupaId { get; set; }
        [ValidateNever]
        public Grupa grupa { get; set; }
        public string zdravstveneNapomene { get; set; }
		public string fotografija { get; set; }
		[ForeignKey("Korisnik")]
        public string roditeljId { get; set; }
        [ValidateNever]

        public Korisnik Korisnik { get; set; }
    }
}
