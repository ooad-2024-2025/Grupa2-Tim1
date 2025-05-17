using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public Grupa grupa { get; set; }
        public string zdravstveneNapomene { get; set; }
		public string fotografija { get; set; }
		[ForeignKey("Korisnik")]
        public int roditeljId { get; set; }
        public Korisnik Korisnik { get; set; }
    }
}
