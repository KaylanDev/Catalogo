namespace Catalogo.Paginations
{
    public class ProdutosFiltroPrecos : QueryStringParameters
    {
        public int? Preco { get; set; }
        public string? PrecoCriterio { get; set; }
    }
}
