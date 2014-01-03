using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RadarAPI.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage="Geen geldig emailadres")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Paswoord is verplicht")]
        [Display(Name = "Paswoord")]
        public string Password { get; set; }
    }

    public class Register
    {
        [Required(ErrorMessage="Naam is verplicht")]
        [Display(Name="Naam en voornaam *")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage="Geen geldig emailadres")]
        [Display(Name = "Email *")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Paswoord is verplicht")]
        [Display(Name = "Paswoord *")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Paswoord herhalen is verplicht")]
        [Compare("Password", ErrorMessage="Paswoorden zijn niet hetzelfde")]
        [Display(Name = "Paswoord herhalen *")]
        public string PasswordRepeat { get; set; }

        [Required(ErrorMessage = "Geslacht is verplicht")]
        [Display(Name = "Geslacht *")]
        public Boolean Gender { get; set; }

        [Required(ErrorMessage = "Geboortedatum is verplicht")]
        [Display(Name = "Geboortedatum *")]
        [DataType(DataType.Date,ErrorMessage="Geen geldige geboortedatum")]
        [DisplayFormat(DataFormatString="{0:dd/MM/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(500, ErrorMessage = "Bio mag maximum 500 karakters bevatten")]
        [Display(Name = "Bio")]
        public string Bio { get; set; }

        public LocationModel Location { get; set; }
    }

    public class LocationModel
    {
        public Decimal Latitude { get; set; }

        public Decimal Longitude { get; set; }

        [Required(ErrorMessage = "Gemeente is verplicht")]
        [MaxLength(255, ErrorMessage = "Gemeente mag maximum 255 karakters bevatten")]
        [Display(Name = "Straat *")]
        public String Street { get; set; }

        [MaxLength(255, ErrorMessage = "Huisnummer mag maximum 255 karakters bevatten")]
        [Required(ErrorMessage = "Huisnummer is verplicht")]
        [Display(Name = "Huisnummer *")]
        public String Number { get; set; }

        [MaxLength(255, ErrorMessage = "Bus mag maximum 255 karakters bevatten")]
        [Display(Name = "Bus")]
        public String Box { get; set; }

        [MaxLength(255, ErrorMessage = "Postcode mag maximum 255 karakters bevatten")]
        [Required(ErrorMessage = "Postcode is verplicht")]
        [Display(Name = "Postcode *")]
        public String Zipcode { get; set; }

        [MaxLength(255, ErrorMessage="Gemeente mag maximum 255 karakters bevatten")]
        [Required(ErrorMessage = "Gemeente is verplicht")]
        [Display(Name = "Gemeente *")]
        public String City { get; set; }

        [MaxLength(255, ErrorMessage = "Land mag maximum 255 karakters bevatten")]
        [Required(ErrorMessage = "Land is verplicht")]
        [Display(Name = "Land *")]
        public String Country { get; set; }
    }
}