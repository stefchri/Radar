using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RadarMVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email verplicht in te vullen.")]
        [Display(Name = "Email adres")]
        [StringLength(32, ErrorMessage = "Je email adres moet tenminste {2} karakters lang zijn.", MinimumLength = 4)]
        [EmailAddress(ErrorMessage = "Geen correct emailadres ingevuld.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Paswoord verplicht in te vullen.")]
        [DataType(DataType.Password)]
        [Display(Name = "Paswoord")]
        [StringLength(128, ErrorMessage = "Je paswoord moet tenminste {2} karakters lang zijn.", MinimumLength = 5)]
        public string Password { get; set; }

        [Display(Name = "Laat me aangemeld blijven")]
        public bool RememberMe { get; set; }
    }
    public class RegisterModel
    {
        [Required(ErrorMessage="Gebruikersnaam verplicht in te vullen.")]
        [Display(Name = "Gebruikersnaam")]
        [StringLength(255, ErrorMessage = "Je gebruikersnaam moet tenminste {2} karakters lang zijn.", MinimumLength = 4)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email verplicht in te vullen.")]
        [Display(Name = "Email adres")]
        [StringLength(32, ErrorMessage = "Je email adres moet tenminste {2} karakters lang zijn.", MinimumLength = 4)]
        [EmailAddress(ErrorMessage="Geen correct emailadres ingevuld.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Geboortedatum verplicht in te vullen.")]
        [Display(Name = "Geboortedatum")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Geslacht verplicht in te vullen.")]
        [Display(Name = "Geslacht")]
        public Gender Gender { get; set; }

        public string Avatar { get; set; }

        [Required(ErrorMessage = "Paswoord verplicht in te vullen.")]
        [Display(Name = "Paswoord")]
        [StringLength(32, ErrorMessage = "Je paswoord moet tenminste {2} karakters lang zijn.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Heraling van het paswoord verplicht in te vullen.")]
        [Display(Name = "Paswoord herhalen")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="De paswoorden komen niet overeen.")]
        public string PasswordRepeat { get; set; }

        [Display(Name = "Bio")]
        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }

        //LOCATION??
    }
    public enum Gender
    {
        Man,
        Vrouw
    }
}