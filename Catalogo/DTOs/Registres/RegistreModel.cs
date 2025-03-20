using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Catalogo.DTOs.Registres
{
    public class RegistreModel
    {
        [Required(ErrorMessage = "User Name is required!")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required!")]
        public string Passwaord { get; set; }
    }
}
