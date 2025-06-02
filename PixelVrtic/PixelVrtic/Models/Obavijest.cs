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
		public string naslov { get; set; }
		public string tekst { get; set; }
		public DateTime datum { get; set; }
		[ForeignKey("Korisnik")]
        [ValidateNever]

        public string idAutora { get; set; }

        [ValidateNever]	
        public Korisnik Korisnik { get; set; }
	}
}