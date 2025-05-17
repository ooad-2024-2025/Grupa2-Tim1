﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PixelVrtic.Models
{
    public class Korisnik
    {
        public Korisnik() { }
        [Key]
        public int id { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
        public string email { get; set; }
        public string lozinka { get; set; }
        public Uloga uloga { get; set; }
        public string brojTelefona { get; set; }
        public string? adresa { get; set; }
        public bool? uplaceno { get; set; }
        public int? idGrupe { get; set; }
        public DateTime? datumRodjenja { get; set; }
        public string? mjestoRodjenja { get; set; }
        public string? fotografija { get; set; }
        public string? univerzitet { get; set; }
        public string? diploma { get; set; }
        public DateTime? pocetakObrazovanja { get; set; }
        public DateTime? krajObrazovanja { get; set; }
        public string? grad { get; set; }
        public double? naknada { get; set; }
    }
}
