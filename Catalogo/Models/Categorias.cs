using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Catalogo.Models;
[Table("Categoria")]
public class Categorias
{
    [Key]
    public int CategoriaId { get; set; }
    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }
    [Required]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

   [JsonIgnore] 
    public ICollection<Produtos> Produtos { get; set; }

    public Categorias()
    {
        Produtos = new Collection<Produtos>();
    }

}
