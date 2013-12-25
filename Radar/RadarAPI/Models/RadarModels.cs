using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RadarAPI.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public Boolean RememberMe { get; set; }
    }

    public class Register
    {
        [Required(ErrorMessage="Naam is verplicht")]
        [Display(Name="Naam")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Paswoord is verplicht")]
        [Display(Name = "Paswoord")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Paswoord herhalen is verplicht")]
        [Compare("Password")]
        [Display(Name = "Paswoord herhalen")]
        public string PasswordRepeat { get; set; }

        [Required(ErrorMessage = "Geslacht is verplicht")]
        [Display(Name = "Geslacht")]
        public Boolean Gender { get; set; }

        [Required(ErrorMessage = "Geboortedatum is verplicht")]
        [Display(Name = "Geboortedatum")]
        [DataType(DataType.Date,ErrorMessage="Geen geldige geboortedatum")]
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

        [MaxLength(255)]
        public String Street { get; set; }

        [MaxLength(255)]
        public String Number { get; set; }

        [MaxLength(255)]
        public String Box { get; set; }

        [MaxLength(255)]
        public String Zipcode { get; set; }

        [MaxLength(255)]
        public String City { get; set; }

        [MaxLength(255)]
        public String Country { get; set; }
    }
}