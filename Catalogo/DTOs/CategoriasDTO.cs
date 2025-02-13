using Catalogo.Models;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Catalogo.DTOs
{
    public class CategoriasDTO
    {

        
        public int CategoriaId { get; set; }
        
        public string? Nome { get; set; }
        
        public string? ImagemUrl { get; set; }



    

    }
}
