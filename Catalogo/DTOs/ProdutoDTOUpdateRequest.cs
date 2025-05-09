using Catalogo.Migrations;
using System.ComponentModel.DataAnnotations;

namespace Catalogo.DTOs
{
    public class ProdutoDTOUpdateRequest : IValidatableObject
    {
        [Range(1,9999,ErrorMessage ="O valor precisa estar entre 1 e 9999")]
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }
        public int CategoriaId { get; set; } = 0;
        public decimal preco { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DataCadastro.Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("a data deve ser maior que a data atual", new[] { nameof(this.DataCadastro)});
            }
        }
    }
}
