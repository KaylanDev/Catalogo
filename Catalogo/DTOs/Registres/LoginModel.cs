using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.ComponentModel.DataAnnotations;

namespace Catalogo.DTOs.Registres
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        public string Passwaord { get; set; }
    }
}
